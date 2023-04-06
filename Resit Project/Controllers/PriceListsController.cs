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
    public class PriceListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PriceLists
        public ActionResult Index()
        {
            return View(db.PriceLists.ToList());
        }

        // GET: PriceLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = db.PriceLists.Find(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceList);
        }

        // GET: PriceLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PriceLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PricelistId,Machine,Stage,Price")] PriceList priceList, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                priceList.Image = new byte[image.ContentLength];
                image.InputStream.Read(priceList.Image, 0, image.ContentLength);
                string fileName = System.IO.Path.GetFileName(image.FileName);
                string urlImage = Server.MapPath("~/Image/" + fileName);

                priceList.UrlImage = "Image/" + fileName;
            }
                
            if (ModelState.IsValid)
            {
                db.PriceLists.Add(priceList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(priceList);
        }

        // GET: PriceLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = db.PriceLists.Find(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceList);
        }

        // POST: PriceLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PricelistId,Machine,Stage,Price")] PriceList priceList, HttpPostedFileBase editImage)
        {
            if (ModelState.IsValid)
            {
                PriceList modifyPriceList = db.PriceLists.Find(priceList.PricelistId);
                if (modifyPriceList != null)
                {
                    if (editImage != null && editImage.ContentLength > 0)
                    {
                        modifyPriceList.Image = new byte[editImage.ContentLength];
                        editImage.InputStream.Read(modifyPriceList.Image, 0, editImage.ContentLength);
                        string fileName = System.IO.Path.GetFileName(editImage.FileName);
                        string urlImage = Server.MapPath("~/Image/" + fileName);
                        editImage.SaveAs(urlImage);

                        modifyPriceList.UrlImage = "Image/" + fileName;
                    }
                }

                db.Entry(priceList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(priceList);
        }

        // GET: PriceLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceList priceList = db.PriceLists.Find(id);
            if (priceList == null)
            {
                return HttpNotFound();
            }
            return View(priceList);
        }

        // POST: PriceLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PriceList priceList = db.PriceLists.Find(id);
            db.PriceLists.Remove(priceList);
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
