using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Resit_Project.Models;

namespace Resit_Project.Controllers
{
    public class StaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Staffs
        public ActionResult Index()
        {
            var staffs = db.Staffs.ToList();

            foreach (var staff in staffs)
            {
                int totalSalary = staff.Works.Sum(w => w.Price);
                staff.TotalSalary = totalSalary;

                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
            }

            return View(staffs);
        }


        public ActionResult Details(int? id, string name, int? month, int? year)
        {
            var works = db.Works
                .Include(w => w.Category)
                .Where(w => w.StaffId == id
                    && (string.IsNullOrEmpty(name) || w.Category.Name.Contains(name))
                    && (!month.HasValue || w.DateCompleted.Month == month)
                    && (!year.HasValue || w.DateCompleted.Year == year))
                .ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            /*var works = db.Works.Include(w => w.Category).Where(w => w.StaffId == id).ToList();*/

            int totalPrice = works.Sum(w => w.Price);
            staff.TotalPrice = totalPrice;

            ViewBag.Name = name;
            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.Works = works;

            return View(staff);
        }

        public ActionResult Filter(int id, string name, int? month, int? year)
        {
            var works = db.Works
                .Include(w => w.Category)
                .Where(w => w.StaffId == id
                    && (string.IsNullOrEmpty(name) || w.Category.Name.Contains(name))
                    && (!month.HasValue || w.DateCompleted.Month == month)
                    && (!year.HasValue || w.DateCompleted.Year == year))
                .ToList();
            return View(works);
        }

        private bool IsDuplicateStaff(Staff staff)
        {
            return db.Staffs.Any(s => s.StaffId != staff.StaffId && s.FullName == staff.FullName && s.Birthday == staff.Birthday);
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
                if (staff.Image == null)
                {
                    ModelState.AddModelError("", "Please upload an image");
                    return View(staff);
                }
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
