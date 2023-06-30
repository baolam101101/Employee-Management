using Resit_Project.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Resit_Project.Controllers
{
    public class SalaryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public FileContentResult ExportExcel()
        {
            string csv = "\"FullName\",\"Genber\",\"Birthday\",\"Salary\" \n";
            var worksList = db.Works.ToList(); //get this list from database 
            foreach (Work item in worksList)
            {
                csv += String.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\" \n",
                                           item.Staff.FullName,
                                           item.Staff.Gender,
                                           item.Staff.Birthday,
                                           item.Price);
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "salary.csv");
        }
    }
}
