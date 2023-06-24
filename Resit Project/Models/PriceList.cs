using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using Owin;

namespace Resit_Project.Models
{
    public partial class PriceList
    {
        public PriceList()
        {
            Categories = new HashSet<Category>();
        }
        [Key]
        public int PricelistId { get; set; }
        [Required]
        public string Stage { get; set; }
        public Machine Machine { get; set; }
        [Required]
        public int Price { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
    public enum Machine
    {
        VS,
        TS,
        AK,
        TK
    }
}