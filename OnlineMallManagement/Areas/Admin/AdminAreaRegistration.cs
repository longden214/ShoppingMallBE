using System.Web.Mvc;

namespace OnlineMallManagement.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index",controller = "Dashboard", id = UrlParameter.Optional },
                namespaces: new[] { "OnlineMallManagement.Areas.Admin.Controllers" }
            );
        }
    }
}