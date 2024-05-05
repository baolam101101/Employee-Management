using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Resit_Project.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 10)]
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        [Required]
        public string Address { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        public int TotalPrice { get; set; }
        public int Phone { get; set; }
        public int PositionId { get; set; }
        public byte[] Image { get; set; }
        public virtual Position Positions { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}