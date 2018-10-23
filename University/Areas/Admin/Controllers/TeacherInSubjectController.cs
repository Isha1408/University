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
    public class TeacherInSubjectController : Controller
    {
        private UserContext db = new UserContext();

        // GET: Admin/TeacherInSubject
        public ActionResult Index()
        {
            var teacherInSubjects = db.TeacherInSubjects.Include(t => t.Subject).Include(t => t.User);
            return View(teacherInSubjects.ToList());
        }

        // GET: Admin/TeacherInSubject/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherInSubject teacherInSubject = db.TeacherInSubjects.Find(id);
            if (teacherInSubject == null)
            {
                return HttpNotFound();
            }
            return View(teacherInSubject);
        }

        // GET: Admin/TeacherInSubject/Create
        public ActionResult Create()
        {
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            return View();
        }

        // POST: Admin/TeacherInSubject/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,SubjectId")] TeacherInSubject teacherInSubject)
        {
            if (ModelState.IsValid)
            {
                db.TeacherInSubjects.Add(teacherInSubject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", teacherInSubject.SubjectId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", teacherInSubject.UserId);
            return View(teacherInSubject);
        }

        // GET: Admin/TeacherInSubject/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherInSubject teacherInSubject = db.TeacherInSubjects.Find(id);
            if (teacherInSubject == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", teacherInSubject.SubjectId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", teacherInSubject.UserId);
            return View(teacherInSubject);
        }

        // POST: Admin/TeacherInSubject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,SubjectId")] TeacherInSubject teacherInSubject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacherInSubject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name", teacherInSubject.SubjectId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", teacherInSubject.UserId);
            return View(teacherInSubject);
        }

        // GET: Admin/TeacherInSubject/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherInSubject teacherInSubject = db.TeacherInSubjects.Find(id);
            if (teacherInSubject == null)
            {
                return HttpNotFound();
            }
            return View(teacherInSubject);
        }

        // POST: Admin/TeacherInSubject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TeacherInSubject teacherInSubject = db.TeacherInSubjects.Find(id);
            db.TeacherInSubjects.Remove(teacherInSubject);
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
