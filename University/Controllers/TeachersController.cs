using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University.Entities;
using University.Models;

namespace University.Controllers
{
    public class TeachersController : Controller
    {
        private UserContext db = new UserContext();

        public ActionResult MyProfile()
        {

            //if (Session["UserId"] != null)
            //{
            //    var usr = db.Users.Where(m => m.UserId.Equals(Session["UserId"].ToString())).ToList();
            //    if (usr != null)
            //        return View(usr);
            //}
            //return View();

            User user = (User)Session["User"];
            var usr = db.Users.Find(user.UserId);
            if (Session["User"] != null)
            {
                var userDetails = db.Users.Where(u => u.UserId == user.UserId);
                if (usr != null)
                    return View(userDetails);
            }
            return View(usr);
        }

        /// <summary>
        /// To Show List of Students
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllStudents()
        {
            var studentList = db.Users.Where(x => x.RoleId == 4).ToList();
            return View(studentList);
        }


        /// <summary>
        ///  GET: To Show the details of the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult UserDetails(int? id)
        {
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User user = db.Users.Find(id);
                // var userData = from p in db.Users
                //  where p.UserId == id
                //  select p;
                // var tempUserList = db.Users.ToList();

                UserViewModel objUserViewModel = new UserViewModel();

                //objUserViewModel.UserId = user.UserId;
                objUserViewModel.FirstName = user.FirstName;
                objUserViewModel.LastName = user.LastName;
                objUserViewModel.Gender = user.Gender;
                objUserViewModel.Hobbies = user.Hobbies;
                objUserViewModel.Email = user.Email;
                objUserViewModel.Password = user.Password;
                objUserViewModel.DateOfBirth = user.DateOfBirth;
                objUserViewModel.RoleId = user.RoleId;
                objUserViewModel.CourseId = user.CourseId;
                //objUserViewModel.AddressId = user.AddressId;
                objUserViewModel.IsActive = user.IsActive;
                objUserViewModel.DateCreated = user.DateCreated;
                objUserViewModel.DateModified = user.DateModified;
                objUserViewModel.AddressLine1 = user.Address.AddressLine1;
                objUserViewModel.AddressLine2 = user.Address.AddressLine2;
                objUserViewModel.CountryId = user.Address.CountryId;
                objUserViewModel.StateId = user.Address.StateId;
                objUserViewModel.CityId = user.Address.CityId;
                objUserViewModel.ZipCode = user.Address.ZipCode;


                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(objUserViewModel);
            }
        }

        // GET: Admin/Teachers/Create
        public ActionResult Create()
        {
            return View();
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
