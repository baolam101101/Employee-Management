using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.Ajax.Utilities;

namespace Resit_Project.Models
{
    public class Category
    {
        public Category()
        {
            Works = new HashSet<Work>();
            Created = DateTime.Now;
        }
        [Key]
        public int CateId { get; set; }
        [Required(ErrorMessage = "Please enter the category")]
        public string Name { get; set; }
        [Required]
        public Month Month { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Year is invalid")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Please choose at least one stage")]
        public int PricelistId { get; set; }
        public string PricelistIdList { get; set; }
        [Required(ErrorMessage = "Please enter quantity")]
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public virtual PriceList Pricelist { get; set; }
        public virtual ICollection<Work> Works { get; set; }
        public virtual ICollection<Dashboard> Dashboards { get; set; }

        public DateTime Created { get; set; }
        
    }

    public enum Month
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        Octocber,
        November,
        December
    }
}