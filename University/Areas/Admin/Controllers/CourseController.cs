using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University.Entities;
using University.Models;

namespace University.Areas.Admin.Controllers
{
    
    public class CourseController : Controller
    {
        private UserContext db = new UserContext();
        /// <summary>
        /// To get List of courses
        /// </summary>
        /// <returns></returns>
     
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        /// <summary>
        /// To get details of available Courses
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CourseDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

      /// <summary>
      /// GET Method to create new Course
      /// </summary>
      /// <returns></returns>
        public ActionResult CreateCourse()
        {
            return View();
        }
        /// <summary>
        /// POST:Method To Create new Course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse([Bind(Include = "CourseId,CourseName,IsActive")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

      /// <summary>
      /// GET method: To Edit Course
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        public ActionResult EditCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        /// <summary>
        /// Post Method: To Edit Course.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse([Bind(Include = "CourseId,CourseName,IsActive")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

       /// <summary>
       /// GET Method: To delete Course from database.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult DeleteCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

       /// <summary>
       /// Post Method: To delete Course from Database.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        [HttpPost, ActionName("DeleteCourse")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourse(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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
