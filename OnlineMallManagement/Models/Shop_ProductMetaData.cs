using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Models
{
    [MetadataType(typeof(Shop_ProductMetaData))]
    public partial class Shop_Product
    {

    }
    public class Shop_ProductMetaData
    {
        [Display(Name = "Pro_name")]
        [Required(ErrorMessage = "The Pro_name field is required")]
        public string Pro_name { get; set; }
        [Display(Name = "Shop_ShoId")]
        [Required(ErrorMessage = "The Shop_ShoId field is required")]
        public int Shop_ShoId { get; set; }
        [Display(Name = "Images")]
        [Required(ErrorMessage = "The Images field is required")]
        public string Images { get; set; }

    }
}