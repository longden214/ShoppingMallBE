using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Areas.Admin.Data
{
    public class MovieViewModel
    {
        public int _IdMovie { set; get; }
        public string _MoviveName { set; get; }
        public string _Image { set; get; }
        public int _Duration { set; get; }
        public int _AgeRestriction { set; get; }
        public string _ReleaseDate { set; get; }
        public string _Actors { set; get; }
        public string _Description { set; get; }
        public string _Language { set; get; }
        public string _Country { set; get; }
        public string _Banner { set; get; }
        public string _Photos { set; get; }
        public int? _Count { get; set; }
        public bool _Status { set; get; }
    }
}