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

        // GET: Categories
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.Pricelist);
            return View(categories.ToList());
        }

        // GET: Categories/Details/5
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
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            /*ViewBag.PricelistId = new SelectList(db.PriceLists, "PricelistId", "Stage");*/

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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CateId,Name,PricelistId,PricelistIdList")] Category category, int[] PricelistId)
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


        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var priceLists = db.PriceLists.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var price in priceLists)
            {
                list.Add(new SelectListItem { Text = price.Stage, Value = price.PricelistId.ToString() });
            }
            ViewBag.PricelistsId = list;

            var category = await db.Categories.Include(c => c.Pricelist).FirstOrDefaultAsync(m => m.CateId == id);
            int[] PriceIdList = category.PricelistIdList.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            List<PriceList> priceList = new List<PriceList>();

            foreach (var PriceId in PriceIdList)
            {
                var PriceDetails = db.PriceLists.Where(c => c.PricelistId == PriceId).FirstOrDefault();
                priceList.Add(PriceDetails);
            }
            ViewBag.priceLists = priceList;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.PricelistId = new SelectList(db.PriceLists, "PricelistId", "Stage", category.PricelistId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CateId,Name,PricelistId,PricelistIdList")] Category category, int[] PricelistId)
        {
            var stage = "";
            PriceList priceList = new PriceList();
            foreach (var id in PricelistId)
            {
                stage = db.PriceLists.Where(m => m.PricelistId == id).FirstOrDefault().Stage.ToString();
                priceList = db.PriceLists.Where(m => m.PricelistId == id).FirstOrDefault();

            }
            category.Pricelist = priceList;
            category.PricelistIdList = string.Join(",", PricelistId);

            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PricelistId = new SelectList(db.PriceLists, "PricelistId", "Stage", category.PricelistId);
            return View(category);
        }

        // GET: Categories/Delete/5
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

        // POST: Categories/Delete/5
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
    }
}
