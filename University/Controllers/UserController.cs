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

        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                using (UserContext db = new UserContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                ModelState.Clear();
              //  ViewBag.Message = user.FirstName + "" + user.LastName + "" + user.EmailId + "" + user.Gender + "" + user.Password + "" + user.ConfirmPassword + "" + user.Roles + "" + user.UserId + "" + user.UserName + "" + user.Courses + "Succesfully Registered.";

            }
            return View();
        }


    }
}