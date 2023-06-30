using Resit_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Resit_Project.Models
{
    public class Work
    {
        public Work()
        {
            DateCompleted = DateTime.Now;
        }
        public int WorkId { get; set; }
        [Display(Name = "Staff")]
        [Required(ErrorMessage = "Please select a staff.")]
        public int StaffId { get; set; }
        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public int CateId { get; set; }
        public string PriceListName { get; set; }

        [Required(ErrorMessage = "Please select a stage")]
        [Display(Name = "Stage")]
        public int PriceListId { get; set; }

        [Required(ErrorMessage = "Please enter a quantity.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid quantity.")]
        public int Quantity { get; set; }

        public int Price { get; set; }

        public DateTime DateCompleted { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Category Category { get; set; }

    }
}