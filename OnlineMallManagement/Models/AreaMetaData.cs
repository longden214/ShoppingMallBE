using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Models
{
    [MetadataType(typeof(AreaMetaData))]
    public partial class Area
    {

    }
    public class AreaMetaData
    {
        [Display(Name = "AreaName")]
        [Required(ErrorMessage = "The AreaName field is required")]
        [MinLength(1)]
        public string AreaName { get; set; }
    }
}