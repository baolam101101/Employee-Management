using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Resit_Project.Models
{
    public class Category
    {
        public Category()
        {
            Works = new HashSet<Work>();
        }
        [Key]
        public int CateId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please choose at least one stage")]
        public int PricelistId { get; set; }
        public string PricelistIdList { get; set; }

        public virtual PriceList Pricelist { get; set; }
        public virtual ICollection<Work> Works { get; set; }

    }
}