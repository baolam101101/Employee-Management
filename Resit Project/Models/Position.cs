using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Resit_Project.Models
{
    public partial class Position
    {
        public Position()
        {
            Staffs = new HashSet<Staff>();
        }
        [Key]
        public int PositionId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Salary { get; set; }

        public virtual ICollection<Staff> Staffs { get; set; }
    }
}