//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineMallManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> BlogTime { get; set; }
        public int Category_blog_id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<bool> Status { get; set; }
        public string slug { get; set; }
        public string Meta_title { get; set; }
        public string Meta_keyword { get; set; }
        public string Meta_description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual Category_blog Category_blog { get; set; }
    }
}
