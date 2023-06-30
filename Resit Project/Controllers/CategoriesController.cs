using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Resit_Project.Models;

namespace Resit_Project.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Index(string searchString, string categoryMonth, int? categoryYear)
        {
            var categories = from c in db.Categories select c;

            // Lọc theo category name
            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(c => c.Name.Contains(searchString));
            }

            // Lọc theo tháng
            if (!String.IsNullOrEmpty(categoryMonth))
            {
                categories = categories.Where(c => c.Month.ToString() == categoryMonth);
            }

            // Lọc theo năm
            if (categoryYear != null)
            {
                categories = categories.Where(c => c.Year == categoryYear);
            }

            return View(categories.ToList());
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Filter(int year, Month month)
        {
            var categories = db.Categories
                                .Where(c => c.Year == year && c.Month == month)
                                .Include(c => c.Pricelist)
                                .ToList();
            return View(categories);
        }

        [Authorize(Roles = "Admin, Staff")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = await db.Categories.Include(c => c.Pricelist).FirstOrDefaultAsync(m => m.CateId == id);
            int[] PriceIdList = category.PricelistIdList.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            List<PriceList> priceLists = new List<PriceList>();

            foreach (var PriceId in PriceIdList)
            {
                var PriceDetails = db.PriceLists.Where(c => c.PricelistId == PriceId).FirstOrDefault();
                priceLists.Add(PriceDetails);
            }

            ViewBag.priceLists = priceLists;

            ViewBag.QuantityThreshold = category.Quantity;
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var priceLists = db.PriceLists.ToList();

            List<SelectListItem> myList = new List<SelectListItem>();
            foreach (var price in priceLists)
            {
                myList.Add(new SelectListItem { Text = price.Stage, Value = price.PricelistId.ToString() });
            }

            ViewBag.PricelistId = myList;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CateId,Name,Month,Year,PricelistId,Quantity,Description,Price,TotalPrice")] Category category, int[] PricelistId)
        {
            if (PricelistId == null || PricelistId.Length == 0)
            {
                ModelState.AddModelError("PricelistId", "Please choose a stage");
            }

            var stage = "";
            PriceList priceList = null;
            foreach (var id in PricelistId)
            {
                var list = db.PriceLists.Where(m => m.PricelistId == id).FirstOrDefault();
                if (list != null)
                {
                    stage = list.Stage.ToString();
                    priceList = list;
                    break;
                }
            }

            if (priceList != null)
            {
                category.Pricelist = priceList;
                category.PricelistIdList = string.Join(",", PricelistId);

                // Calculate the total price of stages in the PriceList
                var totalStagesPrice = db.PriceLists
                    .Where(m => PricelistId.Contains(m.PricelistId))
                    .Sum(m => m.Price * category.Quantity);

                // Calculate the total price for the category
                category.Price = totalStagesPrice;
            }

            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Pass the SelectList as ViewBag to the view
            var priceLists = db.PriceLists.ToList();
            List<SelectListItem> myList = new List<SelectListItem>();
            foreach (var price in priceLists)
            {
                myList.Add(new SelectListItem { Text = price.Stage, Value = price.PricelistId.ToString() });
            }
            ViewBag.PricelistId = myList;

            return View(category);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);

            var priceLists = db.PriceLists.ToList();
            List<SelectListItem> myList = new List<SelectListItem>();
            foreach (var price in priceLists)
            {
                myList.Add(new SelectListItem { Text = price.Stage, Value = price.PricelistId.ToString() });
            }

            ViewBag.PricelistId = myList;
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = db.Categories.Find(category.CateId);
                existingCategory.Name = category.Name;
                existingCategory.Month = category.Month;
                existingCategory.Year = category.Year;
                existingCategory.Description = category.Description;

                db.Entry(existingCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var priceLists = db.PriceLists.ToList();
            List<SelectListItem> myList = new List<SelectListItem>();
            foreach (var price in priceLists)
            {
                myList.Add(new SelectListItem { Text = price.Stage, Value = price.PricelistId.ToString() });
            }

            ViewBag.PricelistId = myList;
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddStage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var priceLists = db.PriceLists.ToList();

            var priceListIds = category.PricelistIdList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var availablePriceLists = priceLists.Where(m => !priceListIds.Contains(m.PricelistId)).ToList();

            ViewBag.PricelistId = new SelectList(availablePriceLists, "PricelistId", "Stage");

            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult AddStage(int id, int[] PricelistId, int Quantity)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            // Convert the PricelistIdList to an array of integers
            int[] existingPriceIdList = category.PricelistIdList.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            // Get the PriceList for the selected PricelistId values
            List<PriceList> priceLists = null;
            if (PricelistId != null)
            {
                priceLists = db.PriceLists.Where(p => PricelistId.Contains(p.PricelistId)).ToList();
            }

            // Calculate the total price of the existing PricelistId items
            var totalExistingPrice = db.PriceLists.Where(m => existingPriceIdList.ToList().Contains(m.PricelistId)).Sum(m => m.Price);

            // Add new PricelistId to the array
            var newPriceIdList = existingPriceIdList.Concat(PricelistId ?? new int[0]).Distinct().ToArray();

            // Check if the PricelistIdList has been modified
            bool isModified = !existingPriceIdList.SequenceEqual(newPriceIdList);

            // Calculate the total price of all the PricelistId items
            var totalNewPrice = db.PriceLists.Where(m => newPriceIdList.Contains(m.PricelistId)).Sum(m => m.Price);

            // Update the Category's Price and Quantity properties
            category.Quantity = Quantity;
            if (isModified)
            {
                category.Price = totalNewPrice * Quantity;
                category.PricelistIdList = string.Join(",", newPriceIdList);
            }
            else
            {
                category.Price = totalExistingPrice * Quantity;
            }

            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = category.CateId });
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePrice(int priceId, int cateId)
        {
            var category = await db.Categories.Include(c => c.Pricelist).FirstOrDefaultAsync(m => m.CateId == cateId);
            if (category == null)
            {
                return HttpNotFound();
            }

            // Remove the PriceId from the PricelistIdList
            category.PricelistIdList = String.Join(",", category.PricelistIdList.Split(',').Where(x => x != priceId.ToString()));

            // Calculate the total price of stages in the updated PricelistIdList
            int[] updatedPriceIdList = category.PricelistIdList.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var totalStagesPrice = db.PriceLists.Where(m => updatedPriceIdList.Contains(m.PricelistId)).Sum(m => m.Price);

            // Calculate the new Price for the Category
            category.Price = category.Quantity * totalStagesPrice;

            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = category.CateId });
        }


    }
}
