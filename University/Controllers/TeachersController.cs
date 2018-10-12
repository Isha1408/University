using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Models;

namespace University.Controllers
{
    public class TeachersController : Controller
    {
        private UserContext db = new UserContext();

        /// <summary>
        /// To Show List of Students
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllStudents()
        {
            var studentList = db.Users.Where(x => x.RoleId == 4).ToList();
            return View(studentList);
        }

        
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Teachers/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Teachers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Teachers/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Teachers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Teachers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
