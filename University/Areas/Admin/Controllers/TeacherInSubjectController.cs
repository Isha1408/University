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

      /// <summary>
      /// To Show Teachewr List with their Subjects
      /// </summary>
      /// <returns></returns>
        public ActionResult Index()
        {
            var teacherInSubjects = db.TeacherInSubjects.Include(t => t.Subject).Include(t => t.User);
            return View(teacherInSubjects.ToList());
        }
       /// <summary>
       /// To show Details of Each teacher with their Subject
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
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

       /// <summary>
       /// GET Method: To assign Subject To teachers
       /// </summary>
       /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users.Where(x=>x.RoleId==3), "UserId", "FirstName");
            return View();
        }

        /// <summary>
        /// POST Method: To assign Subject To teachers
        /// </summary>
        /// <param name="teacherInSubject"></param>
        /// <returns></returns>
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
           // List<User> List = db.Users.Where(u => u.RoleId != 1 && u.RoleId != 2 && u.RoleId != 4).ToList();
            ViewBag.UserId = new SelectList(db.Users.Where(x=>x.RoleId == 3 ), "UserId", "FirstName", teacherInSubject.UserId);
            return View(teacherInSubject);
        }

        /// <summary>
        /// GET Method: To edit Subjectand Teachers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            ViewBag.UserId = new SelectList(db.Users.Where(x => x.RoleId == 3), "UserId", "FirstName", teacherInSubject.UserId);
            return View(teacherInSubject);
        }
        /// <summary>
        /// POST Method: To edit Subject and Teachers,.
        /// </summary>
        /// <param name="teacherInSubject"></param>
        /// <returns></returns>
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
            ViewBag.UserId = new SelectList(db.Users.Where(x => x.RoleId == 3), "UserId", "FirstName", teacherInSubject.UserId);
            return View(teacherInSubject);
        }

       /// <summary>
       /// GET Method: To delete subject and teacher.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
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

        /// <summary>
        /// POST  Method: To delete subject and teacher.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
