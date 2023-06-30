using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Resit_Project.Models;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace Resit_Project.Controllers
{
    [Authorize(Roles = "Admin, Staff")]
    public class WorksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Index()
        {
            var works = db.Works.ToList();
            return View(works);
        }


        [Authorize(Roles = "Admin, Staff")]
        public async Task<ActionResult> Details(int? id, int? priceListId)
        {
            if (id == null || priceListId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var works = await db.Works
                .Include(w => w.Category)
                .Include(w => w.Category.Pricelist)
                .Include(w => w.Staff)
                .Where(w => w.CateId == id && w.PriceListId == priceListId)
                .ToListAsync();

            if (works == null || works.Count == 0)
            {
                ViewBag.WorksExist = false;
            }
            else
            {
                ViewBag.WorksExist = true;
            }

            var totalQuantity = works.Sum(w => w.Quantity);

            ViewBag.Works = works;
            ViewBag.PriceListId = priceListId;
            ViewBag.CategoryId = id;
            ViewBag.TotalQuantity = totalQuantity;


            return View(works);
        }


        [Authorize(Roles = "Admin, Staff")]
        private bool IsDuplicate(Work work)
        {
            return db.Works.Any(w => w.WorkId != work.WorkId && w.StaffId == work.StaffId &&
            w.CateId == work.CateId && w.PriceListId == work.PriceListId);
        }

        [Authorize(Roles = "Admin, Staff, Staff")]
        public ActionResult Create()
        {
            ViewBag.CateList = new SelectList(db.Categories, "CateId", "Name");
            ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName");
            ViewBag.PriceList = new SelectList(db.PriceLists, "PricelistId", "Stage");
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Staff, Staff")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkId,StaffId,CateId,Quantity,Price,PriceListId")] Work work)
        {
            if (IsDuplicate(work))
            {
                ModelState.AddModelError("", "A staff already exists!");
                /*ViewBag.DuplicateStaffError = "A staff already exists!";*/
            }

            if (ModelState.IsValid)
            {
                var category = db.Categories.Find(work.CateId);

                // Chuyển chuỗi PricelistIdList thành mảng các ID.
                var priceListIds = category.PricelistIdList.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                var totalQuantity = db.Works
                    .Where(w => w.CateId == work.CateId && w.PriceListId == work.PriceListId)
                    .Sum(w => (int?)w.Quantity) ?? 0;
                if (totalQuantity + work.Quantity > category.Quantity)
                {
                    ModelState.AddModelError("", "Total quantity exceeds category limit!");
                    ViewBag.CateList = new SelectList(db.Categories, "CateId", "Name", work.CateId);
                    ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName", work.StaffId);
                    ViewBag.PriceList = new SelectList(priceListIds, work.PriceListId);
                    return View(work);
                }

                // Lấy PriceList tương ứng với ID được chọn.
                var selectedPriceList = db.PriceLists
                    .FirstOrDefault(p => p.PricelistId == work.PriceListId && priceListIds.Contains(p.PricelistId));

                if (selectedPriceList == null)
                {
                    ModelState.AddModelError("", "Selected PriceList is invalid!");
                    ViewBag.CateList = new SelectList(db.Categories, "CateId", "Name", work.CateId);
                    ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName", work.StaffId);
                    ViewBag.PriceList = new SelectList(priceListIds, work.PriceListId);
                    return View(work);
                }

                // Tính giá tiền của PriceList được chọn.
                var quantity = work.Quantity;
                var stagePrice = selectedPriceList.Price * quantity;
                work.Price = stagePrice;

                if (selectedPriceList != null)
                {
                    work.PriceListName = selectedPriceList.Stage;
                }

                db.Works.Add(work);
                db.SaveChanges();
                return RedirectToAction("Index", "Categories");
            }

            ViewBag.CateList = new SelectList(db.Categories, "CateId", "Name", work.CateId);
            ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName", work.StaffId);
            ViewBag.PriceList = new SelectList(db.PriceLists, "PricelistId", "Stage", work.PriceListId);

            return View(work);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult GetStages(int categoryId)
        {
            var category = db.Categories.Find(categoryId);
            if (category == null)
            {
                return HttpNotFound();
            }

            var priceListIds = category.PricelistIdList.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            var stages = db.PriceLists
                .Where(p => priceListIds.Contains(p.PricelistId))
                .Select(p => new { p.PricelistId, p.Stage, p.Price })
                .ToList();

            return Json(stages, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }

            ViewBag.CateList = new SelectList(db.Categories, "CateId", "Name", work.CateId);
            ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName", work.StaffId);
            ViewBag.PriceList = new SelectList(db.PriceLists, "PricelistId", "Stage", work.PriceListId);

            return View(work);

        }

        [HttpPost]
        [Authorize(Roles = "Admin, Staff")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkId,StaffId,CateId,Quantity")] Work work)
        {
            if (IsDuplicate(work))
            {
                ModelState.AddModelError("", "A staff already exists!");
            }

            if (ModelState.IsValid)
            {
                var existingWork = db.Works.Find(work.WorkId);
                if (existingWork == null)
                {
                    return HttpNotFound();
                }

                // Keep the values that should not be modified
                work.PriceListId = existingWork.PriceListId;
                work.Price = existingWork.Price;
                work.PriceListName = existingWork.PriceListName;

                // Get the category to recalculate the price and quantity
                var category = db.Categories.Find(existingWork.CateId);
                if (category == null)
                {
                    ModelState.AddModelError("", "Category not found!");
                    return View(work);
                }

                // Calculate the total quantity for the category by summing the quantities of all works with the same category
                var totalQuantity = db.Works
                                    .Where(w => w.CateId == work.CateId && w.PriceListId == work.PriceListId)
                                    .Sum(w => (int?)w.Quantity) ?? 0;
                if (totalQuantity + work.Quantity > category.Quantity)
                {
                    ModelState.AddModelError("", "Total quantity exceeds the category limit!");
                    ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName", work.StaffId);
                    return View(work);
                }

                // Convert the PriceListIdList string to an array of IDs
                var priceListIds = category.PricelistIdList.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                // Get the selected PriceList based on the chosen ID
                var selectedPriceList = db.PriceLists.FirstOrDefault(p => p.PricelistId == existingWork.PriceListId && priceListIds.Contains(p.PricelistId));

                if (selectedPriceList == null)
                {
                    ModelState.AddModelError("", "Selected stage is invalid!");
                    return View(work);
                }

                // Update the Quantity and calculate the price
                existingWork.StaffId = work.StaffId;
                existingWork.Quantity = work.Quantity;
                existingWork.Price = selectedPriceList.Price * work.Quantity;

                db.Entry(existingWork).State = EntityState.Modified;
                db.SaveChanges();

                int? priceListId = work.PriceListId;

                string returnUrl = Url.Action("Details", "Works", new { id = existingWork.CateId, priceListId });
                return Redirect(returnUrl);
            }

            ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName", work.StaffId);
            return View(work);
        }



        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }

            int? priceListId = work.PriceListId;
            int? categoryId = work.CateId;

            db.Works.Remove(work);
            db.SaveChanges();

            string returnUrl = Url.Action("Details", "Works", new { id = categoryId, priceListId });
            return Redirect(returnUrl);
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
