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
    public class SubjectController : Controller
    {
        private UserContext db = new UserContext();

       /// <summary>
       /// To get List of Subjects.
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        {
            return View(db.Subjects.ToList());
        }

      /// <summary>
      /// To get details of each subject.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        public ActionResult SubjectDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

       /// <summary>
       /// GET Method to create new Subject.
       /// </summary>
       /// <returns></returns>
        public ActionResult CreateSubject()
        {
            return View();
        }

       /// <summary>
       /// POST Method To create new subject.
       /// </summary>
       /// <param name="subject"></param>
       /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubject([Bind(Include = "Id,Name,IsActive")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subject);
        }

      /// <summary>
      /// GET Method: To edit available Subjects..
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        public ActionResult EditSubject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

       /// <summary>
       /// Post Method To edit Subject in the database.
       /// </summary>
       /// <param name="subject"></param>
       /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubject([Bind(Include = "Id,Name,IsActive")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

      /// <summary>
      /// GET method to delete Subjects.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        public ActionResult DeleteSubject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }
          /// <summary>
          /// POST method to edit Subjects.
          /// </summary>
          /// <param name="id"></param>
          /// <returns></returns>
        [HttpPost, ActionName("DeleteSubject")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubject(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
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
