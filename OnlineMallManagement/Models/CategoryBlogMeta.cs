using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Models
{
    [MetadataType(typeof(CategoryBlogMeta))]
    public partial class Category_blog
    {

    }
    public class CategoryBlogMeta
    {
        [Display(Name = "name")]
        [Required(ErrorMessage = "The Name field is required")]
        public string name { get; set; }
        [Display(Name = "slug")]
        [Required(ErrorMessage = "The slug field is required")]
        public string slug { get; set; }
        [Display(Name = "Meta_title")]
        [Required(ErrorMessage = "The Meta_title field is required")]
        public string Meta_title { get; set; }
        [Display(Name = "Meta_keyword")]
        [Required(ErrorMessage = "The Meta_keyword field is required")]
        public string Meta_keyword { get; set; }
        [Display(Name = "Meta_description")]
        [Required(ErrorMessage = "The Meta_description field is required")]
        public string Meta_description { get; set; }
    }
}