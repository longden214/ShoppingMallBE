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
    
    public partial class AdminRole
    {
        public int id { get; set; }
        public int Role_id { get; set; }
        public int Admin_id { get; set; }
    
        public virtual Admin Admin { get; set; }
        public virtual Role Role { get; set; }
    }
}
