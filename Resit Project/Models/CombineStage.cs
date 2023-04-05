using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Resit_Project.Models
{
    public class CombineStage
    {
        [Key]
        public int StageId { get; set; }
        public int CateId { get; set; }
        [Required]
        public int StageName { get; set; }
        public int PricelistId { get; set; }
        public byte[] Picture { get; set; }
        public int Price { get; set; }
        public string PricelistIdList { get; set; }

        public virtual Category Cate { get; set; }
        public virtual PriceList Pricelist { get; set; }

    }
}