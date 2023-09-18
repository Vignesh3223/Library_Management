//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Fine
    {
        public int FineId { get; set; }
        public Nullable<int> TakeId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Username { get; set; }
        public Nullable<int> BookId { get; set; }
        public string BookName { get; set; }
        public Nullable<int> ExceededDays { get; set; }
        public Nullable<int> FineAmount { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public string Email { get; set; }
    
        public virtual Book_Taken Book_Taken { get; set; }
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
