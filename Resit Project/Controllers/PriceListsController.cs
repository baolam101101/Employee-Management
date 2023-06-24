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

        private bool PricelistDuplicate(PriceList priceList)
        {
            return db.PriceLists.Any(p => p.Stage == priceList.Stage && p.Machine == priceList.Machine);
        }

        // GET: PriceLists/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PricelistId,Machine,Stage,Price")] PriceList priceList, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                priceList.Image = new byte[image.ContentLength];
                image.InputStream.Read(priceList.Image, 0, image.ContentLength);
            }

            if (ModelState.IsValid)
            {
                if (PricelistDuplicate(priceList))
                {
                    ModelState.AddModelError("", "A stage of using this machine already exists!");
                    return View(priceList);
                }

                db.PriceLists.Add(priceList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(priceList);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [Bind(Include = "PricelistId,Machine,Stage,Price")] PriceList priceList, HttpPostedFileBase image)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PriceList priceListToUpdate = db.PriceLists.Find(id);
            if (priceListToUpdate == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (PricelistDuplicate(priceList))
                {
                    ModelState.AddModelError("", "A stage of using this machine already exists!");
                    return View(priceList);
                }
                // Update existing properties
                priceListToUpdate.Machine = priceList.Machine;
                priceListToUpdate.Stage = priceList.Stage;
                priceListToUpdate.Price = priceList.Price;

                // Update image if it was changed
                if (image != null && image.ContentLength > 0)
                {
                    priceListToUpdate.Image = new byte[image.ContentLength];
                    image.InputStream.Read(priceListToUpdate.Image, 0, image.ContentLength);
                }

                db.Entry(priceListToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(priceListToUpdate); // Pass the PriceList object to the view
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
