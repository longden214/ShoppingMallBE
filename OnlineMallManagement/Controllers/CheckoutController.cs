using OnlineMallManagement.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace OnlineMallManagement.Controllers
{
    public class CheckoutController : Controller
    {

        private const string CartSession = "CartSession";
        private DBOnlineMallEntities dbContext = new DBOnlineMallEntities();
        public JsonResult AddCart(List<CartItem> model)
        {
            var list = new List<CartItem>();
            foreach (var item in model)
            {
                var seat = new CartItem();

                seat.SeatId = item.SeatId;
                seat.SeatName = item.SeatName;
                seat.SeatPrice = item.SeatPrice;
                seat.ScreeningId = item.ScreeningId;

                list.Add(seat);
            }

            //Gán vào session
            Session[CartSession] = list;

            return Json(new { newUrl = Url.Action("MovieCheckout", "Movie", new { showId = model.FirstOrDefault().ScreeningId }) }, JsonRequestBehavior.AllowGet);
        }

        private Payment payment;

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var listItems = new ItemList() { items = new List<Item>() };

            List<CartItem> listCarts = (List<CartItem>)Session[CartSession];

            foreach (var cart in listCarts)
            {
                var movieId = dbContext.Screenings.Find(cart.ScreeningId);
                var movieName = dbContext.Movies.Find(movieId.Movie_Id);
                listItems.items.Add(new Item()
                {
                    //Adding Item Details like name, currency, price etc
                    name = movieName.MoviveName,
                    currency = "USD",
                    price = cart.SeatPrice.ToString(),
                    quantity = "1",
                    sku = "Seats: " + cart.SeatName
                });
            }

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = listCarts.Sum(x => x.SeatPrice).ToString()
            };

            //Final amount with details
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(), // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Convert.ToString((new Random()).Next(100000)), //mã hóa đơn của bạn
                amount = amount,
                item_list = listItems
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Checkout/PaymentWithPaypal?";

                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        Session["Customer"] = null;
                        Session[CartSession] = null;
                        return RedirectToAction("Failure");
                    }
                }
            }
            catch (Exception ex)
            {
                Session["Customer"] = null;
                Session[CartSession] = null;
                PaypalLogger.Log("Error: " + ex.Message);
                return RedirectToAction("Failure");
            }


            //on successful payment, show success page to user.
            return RedirectToAction("Success");
        }

        public ActionResult Failure()
        {
            return View();
        }

        public ActionResult Success()
        {
            //add new Customer to database
            Customer cus = (Customer)Session["Customer"];
            cus.CreatedDate = DateTime.Now;
            cus.ModifiedDate = DateTime.Now;

            dbContext.Customers.Add(cus);
            dbContext.SaveChanges();

            //add Order to database
            List<CartItem> listCarts = (List<CartItem>)Session[CartSession];

            Models.Order o = new Models.Order();
            o.CustomerId = cus.CustomerId;
            o.Discount = 0;
            o.Total = Convert.ToDouble(listCarts.Sum(x => x.SeatPrice).ToString()) + 3;
            o.Screening_Id = listCarts.FirstOrDefault().ScreeningId;
            o.OrderDate = DateTime.Now;
            o.NumberOfSeats = listCarts.Count();
            o.Status = true;

            dbContext.Orders.Add(o);
            dbContext.SaveChanges();

            //add Screening Seat to database


            foreach (var item in listCarts)
            {
                Screening_seat ss = new Screening_seat();
                ss.Seat_IdSeat = item.SeatId;
                ss.Screening_Id = item.ScreeningId;
                ss.Order_OrderId = o.OrderId;
                ss.Status = true;
                ss.Price = item.SeatPrice;
                ss.CreatedDate = DateTime.Now;
                ss.ModifiedDate = DateTime.Now;

                dbContext.Screening_seat.Add(ss);
                dbContext.SaveChanges();
            }

            SendRegisterEmail(o.OrderId,(DateTime) o.OrderDate);

            Session[CartSession] = null;
            Session["Customer"] = null;
            return View();
        }

        public JsonResult SendInfo(Customer cus)
        {
            Customer customer = new Customer();
            customer.FullName = cus.FullName;
            customer.Phone = cus.Phone;
            customer.Email = cus.Email;

            Session["Customer"] = customer;
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }

        public void SendRegisterEmail(int orderId,DateTime orderDate)
        {
            Customer cus = (Customer)Session["Customer"];
            if (cus.Email.Length > 0)
            {
                var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                var fromPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
                var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
                bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

                var fromEmail = new MailAddress(fromEmailAddress, fromEmailDisplayName);
                var fromEmailPassword = fromPassword;

                var toEmail = new MailAddress(cus.Email); 

                string subject = "New message from Cinema";

                string body = System.IO.File.ReadAllText(Server.MapPath("~/Content/client/template/mail-invoice.html"));

                body = body.Replace("{{name}}", cus.FullName);

                List<CartItem> listCarts = (List<CartItem>)Session[CartSession];
                var screening = dbContext.Screenings.Find(listCarts.FirstOrDefault().ScreeningId);
                var movie = dbContext.Movies.Find(screening.Movie_Id);
                body = body.Replace("{{movie}}", movie.MoviveName);

                var room = dbContext.Rooms.Find(screening.Room_IdRoom);
                body = body.Replace("{{cinemaHall}}", room.RoomName);

                string convertDay = screening.ScreeningDate.ToString("yyyy-MM-dd");
                body = body.Replace("{{day}}", convertDay);

                string convertTime = screening.StartTime.ToString(@"hh\:mm");
                body = body.Replace("{{time}}", convertTime);

                body = body.Replace("{{orderId}}", "OrderId :" + orderId.ToString());

                string convert = orderDate.ToString("yyyy-MM-dd");
                body = body.Replace("{{orderDate}}","Order Date :" + convert);

                var html = "";

                foreach (var item in listCarts)
                {
                    html += "<tr>";
                    html += "<td width='80%' class='purchase_item'>";
                    html += "<span class='f-fallback' >"+item.SeatName+"</span>";
                    html += "</td>";
                    html += "<td class='align-right' width ='20%' class='purchase_item' > ";
                    html += "<span class='f-fallback' >"+item.SeatPrice+"</span>";
                    html += "</td>";
                    html += "</tr>";
                }

                body = body.Replace("{{seatList}}", html);

                var total = (listCarts.Sum(x => x.SeatPrice) + 3).ToString();
                body = body.Replace("{{total}}", total);

                var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0,
                    EnableSsl = enabledSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })

                    smtp.Send(message);
            }
        }
    }
}