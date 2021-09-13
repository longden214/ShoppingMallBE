using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Models
{
    [MetadataType(typeof(SalesMetaData))]
    public partial class Sale
    {

    }
    public class SalesMetaData
    {
        [Display(Name = "Shop_ShoId")]
        [Required(ErrorMessage = "The Shop_ShoId field is required")]
        public int Shop_ShoId { get; set; }

        [Display(Name = "StartDate")]
        [Required(ErrorMessage = "Please enter StartDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public System.DateTime StartDate { get; set; }
        [Display(Name = "EndDate")]
        [Required(ErrorMessage = "Please enter EndDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public System.DateTime EndDate { get; set; }
        [Display(Name = "Sale")]
        [Range(0, 100)]
        [Required(ErrorMessage = "The Sale field is required")]
        public int Sale1 { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The slug field is required")]
        [MinLength(1, ErrorMessage = "The title field is required")]
        public string slug { get; set; }
        [Display(Name = "Meta_title")]
        [Required(ErrorMessage = "The Meta_title field is required")]
        [MinLength(1, ErrorMessage = "The Meta_title field is required")]
        public string Meta_title { get; set; }
        [Display(Name = "Meta_keyword")]
        [Required(ErrorMessage = "The Meta_keyword field is required")]
        [MinLength(1, ErrorMessage = "The Meta_keyword field is required")]
        public string Meta_keyword { get; set; }
        [Display(Name = "Meta_description")]
        [Required(ErrorMessage = "The Meta_description field is required")]
        [MinLength(1, ErrorMessage = "The Meta_description field is required")]
        public string Meta_description { get; set; }
    }
}