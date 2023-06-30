using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resit_Project.Models
{
    public class DashboardSearchViewModel
    {
        public int TotalStaff { get; set; }
        public int TotalCategories { get; set; }
        public int TotalPrice { get; set; }
        public List<Category> Categories { get; set; }
        public List<Work> Works { get; internal set; }
    }

}