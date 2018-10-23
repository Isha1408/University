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

        // GET: Admin/SubjectInCourse
        public ActionResult Index()
        {
            var subjectInCourses = db.SubjectInCourses.Include(s => s.Course).Include(s => s.Subject);
            return View(subjectInCourses.ToList());
        }

        // GET: Admin/SubjectInCourse/Details/5
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

        // GET: Admin/SubjectInCourse/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name");
            return View();
        }

        // POST: Admin/SubjectInCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Admin/SubjectInCourse/Edit/5
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

        // POST: Admin/SubjectInCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Admin/SubjectInCourse/Delete/5
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

        // POST: Admin/SubjectInCourse/Delete/5
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
