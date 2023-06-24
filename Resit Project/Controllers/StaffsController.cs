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
    public class StaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Staffs
        public ActionResult Index()
        {
            return View(db.Staffs.ToList());
        }

        // GET: Staffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        private bool IsDuplicateStaff(Staff staff)
        {
            return db.Staffs.Any(s => s.FullName == staff.FullName && s.Birthday == staff.Birthday);
        }

        // GET: Staffs/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffId,FullName,Gender,Birthday,Address,StartDate")] Staff staff, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                staff.Image = new byte[image.ContentLength];
                image.InputStream.Read(staff.Image, 0, image.ContentLength);
            }

            if (ModelState.IsValid)
            {
                if (IsDuplicateStaff(staff))
                {
                    ModelState.AddModelError("", "A staff with the same name and birthday already exists!");
                    return View(staff);
                }

                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staff);
        }
        // GET: Staffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [Bind(Include = "StaffId,FullName,Gender,Birthday,Address,StartDate")] Staff staff, HttpPostedFileBase image)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Staff staffToUpdate = db.Staffs.Find(id);
            if (staffToUpdate == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (IsDuplicateStaff(staff))
                {
                    ModelState.AddModelError("", "A staff with the same name and birthday already exists!");
                    return View(staff);
                }
                staffToUpdate.FullName = staff.FullName;
                staffToUpdate.Gender = staff.Gender;
                staffToUpdate.Birthday = staff.Birthday;
                staffToUpdate.Address = staff.Address;
                staffToUpdate.StartDate = staff.StartDate;

                if (image != null && image.ContentLength > 0)
                {
                    staffToUpdate.Image = new byte[image.ContentLength];
                    image.InputStream.Read(staffToUpdate.Image, 0, image.ContentLength);
                }

                db.Entry(staffToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staffToUpdate);
        }


        // GET: Staffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
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
