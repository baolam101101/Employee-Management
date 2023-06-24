using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Resit_Project.Models;

namespace Resit_Project.Controllers
{
    public class WorksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Works
        public ActionResult Index()
        {
            var works = db.Works.Include(w => w.Category).Include(w => w.Staff);
            return View(works.ToList());
        }

        // GET: Works/Details/5
        public ActionResult Details(int? id)
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

        // GET: Works/Create
        public ActionResult Create()
        {
            ViewBag.CateList = new SelectList(db.Categories, "CateId", "Name");
            ViewBag.PriceList = new SelectList(db.PriceLists, "PriceListId", "Stage");
            ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkId,StaffId,CateId,Quantity,Price")] Work work, int[] StaffId, int[] CateId)
        {
           

            if (ModelState.IsValid)
            {
                db.Works.Add(work);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CateList = new SelectList(db.Categories, "CateId", "Name", work.CateId);
            ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName", work.StaffId);
            ViewBag.PriceList = new SelectList(db.PriceLists, "PriceListId", "Name", work.Category.PricelistId);
            return View(work);
        }

        // GET: Works/Edit/5
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
            ViewBag.PriceList = new SelectList(db.PriceLists, "PriceListId", "Name", work.Category.PricelistId);
            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkId,StaffId,CateId,Quantity,Price")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CateList = new SelectList(db.Categories, "CateId", "Name", work.CateId);
            ViewBag.StaffList = new SelectList(db.Staffs, "StaffId", "FullName", work.StaffId);
            ViewBag.PriceList = new SelectList(db.PriceLists, "PriceListId", "Name", work.Category.PricelistId);
            return View(work);
        }

        // GET: Works/Delete/5
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

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work work = db.Works.Find(id);
            db.Works.Remove(work);
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
