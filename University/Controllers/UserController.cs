using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Entities;
using University.Models;

namespace University.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        // Get :User
        public ActionResult Registration()
        {
            using (UserContext db = new UserContext())
            {
                List<Role> list = db.Roles.ToList();
                ViewBag.RoleList = new SelectList(list, "RoleId", "RoleName");
                List<Course> courseList = db.Courses.ToList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");

            }
            return View();
        }
        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                using (UserContext db = new UserContext())
                {

                    List<Role> list = db.Roles.ToList();
                    ViewBag.RoleList = new SelectList(list, "RoleId", "RoleName");
                    List<Course> courseList = db.Courses.ToList();
                    ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");

                    User obj = new User();
                    obj.UserId = user.UserId;
                    obj.FirstName = user.FirstName;
                    obj.LastName = user.LastName;
                    obj.Gender = user.Gender;
                    obj.Hobbies = user.Hobbies;
                    obj.Password = user.Password;
                    obj.ConfirmPassword = user.ConfirmPassword;
                    obj.IsVerified = user.IsVerified;
                    obj.Email = user.Email;
                    obj.DateOfBirth = user.DateOfBirth;
                    obj.DateCreated = user.DateCreated;
                    obj.DateModified = user.DateModified;

                    obj.RoleId = user.RoleId;
                    obj.CourseId = user.CourseId;
                    obj.Address = user.Address;
                    obj.IsActive = user.IsActive;

                    db.Users.Add(obj);
                    db.SaveChanges();

                    int latestUserId = obj.UserId;
                    UserInRole userInRole = new UserInRole();
                    userInRole.RoleId = user.RoleId;
                    userInRole.UserId = latestUserId;
                    db.UserInRoles.Add(userInRole);
                    db.SaveChanges();
                }
            }
            return View(user);

        }


    }
}