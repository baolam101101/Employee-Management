using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Resit_Project.Models
{
    public class Dashboard
    {
        [Key]
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int CateId { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public int TotalStaff { get; set; }
        public int TotalCategories { get; set; }
        public int TotalPrice { get; set; }
        public virtual Staff Staff { get; set;}
        public virtual Category Category { get; set; } 
    }
}