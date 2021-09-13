using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMallManagement.Areas.Admin.Data
{
    public class BlogViewModel
    {
        public int BlogId { get; set; }
        public string Description { get; set; }
        public string BlogTime { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public string CategoryBlog { get; set; }
        public int Category_blog_id { get; set; }
        public string Content { get; set; }
        public string slug { get; set; }
        public string Meta_title { get; set; }
        public string Meta_keyword { get; set; }
        public string Meta_description { get; set; }
        public string Images { get; set; }
    }
}