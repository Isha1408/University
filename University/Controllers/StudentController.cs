using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Entities;
using University.Models;

namespace University.Controllers
{
    public class StudentController : Controller
    {
        UserContext db = new UserContext();

        // GET: Student
        public ActionResult MyProfile(int? id)
        {

           // User user = (User)Session["UserId"];
            var usr = db.Users.Find(id);
            if (Session["User"] != null)
            {
                var userDetails = db.Users.Where(u => u.UserId == id);
                if (usr != null)
                return View(userDetails);
            }
            return View(usr);
        }


        public ActionResult Index()
        {
            var subjectList = db.SubjectInCourses.ToList();

            return View(subjectList.ToList());
           
        }
        public ActionResult TeachersList()
        {
            var teachersList = db.TeacherInSubjects.ToList();
            return View(teachersList);
        }
        public ActionResult SubjectList(User user )
        {
           
            var subjectList = db.SubjectInCourses.Where(x => user.CourseId == 1 || user.CourseId == 2||user.CourseId==3||user.CourseId==4).ToList();
            
            return View(subjectList.ToList());
        }
    }
}