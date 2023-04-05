using Resit_Project.CustomFilters;
using Resit_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Resit_Project.Controllers
{
    [AuthLog(Roles = "Admin")]
    public class UserController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin/ManageUser
        public ActionResult Index()
        {
            IEnumerable<ApplicationUser> model = context.Users.AsEnumerable();
            return View(model);
        }
        public ActionResult Edit(string Id)
        {
            ApplicationUser model = context.Users.Find(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser model)
        {
            try
            {
                context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public ActionResult Delete(string Id)
        {
            var model = context.Users.Find(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string Id)
        {
            ApplicationUser model = null;

            try
            {
                model = context.Users.Find(Id);
                context.Users.Remove(model);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Delete", model);
            }
        }
    }
}