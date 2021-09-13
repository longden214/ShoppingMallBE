using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Models
{
    [MetadataType(typeof(ShopMetaData))]
    public partial class Shop
    {

    }
    public class ShopMetaData
    {
        public int ShoId { get; set; }
        [Display(Name = "ShopName")]
        [Required(ErrorMessage = "The ShopName field is required")]
        [MinLength(1, ErrorMessage = "The ShopName field is required")]
        public string ShopName { get; set; }
        [Display(Name = "PhoTo")]
        [Required(ErrorMessage = "The PhoTo field is required")]
        public string Photo { get; set; }
        [Display(Name = "ServiceId")]
        [Required(ErrorMessage = "The ServiceId field is required")]
        public int ServiceId { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "The Description field is required")]
        public string Description { get; set; }
        [Display(Name = "Address")]
        [Required(ErrorMessage = "The Address field is required")]
        public string Address { get; set; }
        [Display(Name = "Phone")]
        [Required(ErrorMessage = "The Phone field is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Not a valid phone number")]

        public string Phone { get; set; }
        [Display(Name = "Status")]
        [Required(ErrorMessage = "The Status field is required")]
        public Nullable<bool> Status { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "The Email field is required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
                   ErrorMessage = "Entered Email format is not valid.")]
        public string Email { get; set; }
        [Display(Name = "Url_web")]
        [Required(ErrorMessage = "The Url_web field is required")]
        public string Url_web { get; set; }
        [Display(Name = "Logo")]
        [Required(ErrorMessage = "The Logo field is required")]
        public string Logo { get; set; }
        [Display(Name = "Meta_title")]
        [Required(ErrorMessage = "The Meta_title field is required")]
        public string Meta_title { get; set; }
        [Display(Name = "Meta_keyword")]
        [Required(ErrorMessage = "The Meta_keyword field is required")]
        public string Meta_keyword { get; set; }
        [Display(Name = "Meta_description")]
        [Required(ErrorMessage = "The Meta_description field is required")]
        public string Meta_description { get; set; }
        [Display(Name = "slug")]
        [Required(ErrorMessage = "The Slug field is required")]
        public string slug { get; set; }
    }
}