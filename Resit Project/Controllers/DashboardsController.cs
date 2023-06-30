using Resit_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Resit_Project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index(int? id, string name, int? month, int? year)
        {
            var staffCount = _context.Staffs.Count();
            var prices = _context.Categories.Sum(c => c.Price);

            var categories = from c in _context.Categories select c;

            var works = _context.Works
                .Include(w => w.Category)
                .Include(w => w.Staff)
                .ToList();

            var model = new Dashboard
            {
                TotalStaff = staffCount,
                TotalCategories = categories.Count(),
                TotalPrice = prices
            };

            ViewBag.Name = name;
            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.Works = works;

            return View("Index", model);
        }

    }
}
