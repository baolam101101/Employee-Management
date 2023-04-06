using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using Owin;

namespace Resit_Project.Models
{
    public class PriceList
    {
        public PriceList()
        {
            CombineStages = new HashSet<CombineStage>();
        }
        [Key]
        public int PricelistId { get; set; }
        public string Stage { get; set; }
        public Machine Machine { get; set; }
        public int Price { get; set; }
        public byte[] Image { get; set; }
        public string UrlImage { get; set; }

        public virtual ICollection<CombineStage> CombineStages { get; set; }

    }
    public enum Machine
    {
        VS,
        TS,
        AK,
        TK
    }
}