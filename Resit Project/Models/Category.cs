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
            CombineStages = new HashSet<CombineStage>();
        }
        [Key]
        public int CateId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CombineStage> CombineStages { get; set; }
    }
}