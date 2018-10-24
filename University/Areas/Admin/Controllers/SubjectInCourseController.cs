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
    public class SubjectInCourseController : Controller
    {
        private UserContext db = new UserContext();

       // To get List of Course with their Subjects.
        public ActionResult Index()
        {
            var subjectInCourses = db.SubjectInCourses.Include(s => s.Course).Include(s => s.Subject);
            return View(subjectInCourses.ToList());
        }

       /// <summary>
       /// To show details of each Course and Subjects
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInCourse subjectInCourse = db.SubjectInCourses.Find(id);
            if (subjectInCourse == null)
            {
                return HttpNotFound();
            }
            return View(subjectInCourse);
        }

      /// <summary>
      /// GET Method :-To Create new Subject and new Course
      /// </summary>
      /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name");
            return View();
        }

        /// <summary>
        /// POST  Method :-To Create new Subject and new Course
        /// </summary>
        /// <param name="subjectInCourse"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubjectId,CourseId")] SubjectInCourse subjectInCourse)
        {
            if (ModelState.IsValid)
            {
                db.SubjectInCourses.Add(subjectInCourse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", subjectInCourse.SubjectId);
            return View(subjectInCourse);
        }
        /// <summary>
        /// GET :-To Edit Subject In Course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
    
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInCourse subjectInCourse = db.SubjectInCourses.Find(id);
            if (subjectInCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", subjectInCourse.SubjectId);
            return View(subjectInCourse);
        }

        /// <summary>
        /// POST Method:- To edit  Subject In Course
        /// </summary>
        /// <param name="subjectInCourse"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubjectId,CourseId")] SubjectInCourse subjectInCourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjectInCourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", subjectInCourse.SubjectId);
            return View(subjectInCourse);
        }

     /// <summary>
     /// GET Method:To delete subject from any Course
     /// </summary>
     /// <param name="id"></param>
     /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInCourse subjectInCourse = db.SubjectInCourses.Find(id);
            if (subjectInCourse == null)
            {
                return HttpNotFound();
            }
            return View(subjectInCourse);
        }
        /// <summary>
        /// POST Method:To delete subject from any Course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubjectInCourse subjectInCourse = db.SubjectInCourses.Find(id);
            db.SubjectInCourses.Remove(subjectInCourse);
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
