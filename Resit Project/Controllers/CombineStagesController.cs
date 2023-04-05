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
    public class CombineStagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CombineStages
        public ActionResult Index()
        {
            var combineStages = db.CombineStages.Include(c => c.Cate).Include(c => c.Pricelist);
            return View(combineStages.ToList());
        }

        // GET: CombineStages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CombineStage combineStage = db.CombineStages.Find(id);
            if (combineStage == null)
            {
                return HttpNotFound();
            }
            return View(combineStage);
        }

        // GET: CombineStages/Create
        public ActionResult Create()
        {
            ViewBag.CateId = new SelectList(db.Categories, "CateId", "Name");
            ViewBag.PricelistId = new SelectList(db.PriceLists, "PricelistId", "Stage");
            return View();
        }

        // POST: CombineStages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StageId,CateId,StageName,PricelistId,Picture,Price,PricelistIdList")] CombineStage combineStage)
        {
            if (ModelState.IsValid)
            {
                db.CombineStages.Add(combineStage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CateId = new SelectList(db.Categories, "CateId", "Name", combineStage.CateId);
            ViewBag.PricelistId = new SelectList(db.PriceLists, "PricelistId", "Stage", combineStage.PricelistId);
            return View(combineStage);
        }

        // GET: CombineStages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CombineStage combineStage = db.CombineStages.Find(id);
            if (combineStage == null)
            {
                return HttpNotFound();
            }
            ViewBag.CateId = new SelectList(db.Categories, "CateId", "Name", combineStage.CateId);
            ViewBag.PricelistId = new SelectList(db.PriceLists, "PricelistId", "Stage", combineStage.PricelistId);
            return View(combineStage);
        }

        // POST: CombineStages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StageId,CateId,StageName,PricelistId,Picture,Price,PricelistIdList")] CombineStage combineStage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(combineStage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CateId = new SelectList(db.Categories, "CateId", "Name", combineStage.CateId);
            ViewBag.PricelistId = new SelectList(db.PriceLists, "PricelistId", "Stage", combineStage.PricelistId);
            return View(combineStage);
        }

        // GET: CombineStages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CombineStage combineStage = db.CombineStages.Find(id);
            if (combineStage == null)
            {
                return HttpNotFound();
            }
            return View(combineStage);
        }

        // POST: CombineStages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CombineStage combineStage = db.CombineStages.Find(id);
            db.CombineStages.Remove(combineStage);
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
