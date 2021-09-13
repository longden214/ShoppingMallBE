using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Models
{
    [MetadataType(typeof(ServicesMetaData))]
    public partial class Service
    {

    }
    public class ServicesMetaData
    {
        [Display(Name = "ServiceName")]
        [Required(ErrorMessage = "The ServiceName field is required")]
        [MinLength(1)]
        public string ServiceName { get; set; }
        public int Service_area { get; set; }
    }
}