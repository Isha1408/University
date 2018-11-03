﻿using System;
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
       
        /// <summary>
        /// To Show Profile Of Logged in User
        /// </summary>
        /// <returns></returns>
        public ActionResult MyProfile()
        { 
            //Code to get Details of Logged In Teacher.
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
        /// To edit Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditUser(int id)
        {
            // Code to show Roles in DropDown
           List<Role> objRoleList = GetRoles();
            ViewBag.Role = objRoleList;
            // Code to show Courses in DropDown
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            // Code to show Countries in DropDown
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");

            //Code to Show State Dropdown
            List<State> statesList = db.States.ToList();
            ViewBag.StateList = new SelectList(statesList, "StateId", "Name");
            //Code to show City dropDown
            List<City> citiesList = db.City.ToList();
            ViewBag.CityList = new SelectList(citiesList, "CityId", "Name");


            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                User objUser = db.Users.Find(id);
                UserViewModel objUserViewModel = new UserViewModel();
                if (ModelState.IsValid)
                {
                    
                    objUserViewModel.FirstName = objUser.FirstName;
                    objUserViewModel.LastName = objUser.LastName;
                    objUserViewModel.Gender = objUser.Gender;
                    objUserViewModel.Hobbies = objUser.Hobbies;
                    objUserViewModel.Email = objUser.Email;
                    objUserViewModel.Password = objUser.Password;
                    objUserViewModel.ConfirmPassword = objUser.ConfirmPassword;
                    objUserViewModel.DateOfBirth = objUser.DateOfBirth;
                    objUserViewModel.RoleId = objUser.RoleId;
                    objUserViewModel.CourseId = objUser.CourseId;
                    objUserViewModel.IsActive = true;
                    objUserViewModel.DateCreated = objUser.DateCreated;
                    objUserViewModel.DateModified = objUser.DateModified;
                    objUserViewModel.AddressLine1 = objUser.Address.AddressLine1;
                    objUserViewModel.AddressLine2 = objUser.Address.AddressLine2;
                    objUserViewModel.CountryId = objUser.Address.CountryId;
                    objUserViewModel.StateId = objUser.Address.StateId;
                    objUserViewModel.CityId = objUser.Address.CityId;
                    objUserViewModel.ZipCode = objUser.Address.ZipCode;
                  
                   
                }
                return View(objUserViewModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        /// <summary>
        ///  To Edit User Record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUser(int id, UserViewModel objUserViewModel)
        {
            // Code to show Roles in DropDown
           //List<Role> objRoleList = GetRoles();
           //ViewBag.Role = new SelectList(db.Users.ToList(), "RoleId", "RoleName");
           // // Code to show Courses in DropDown
           // List<Course> objCourseList = db.Courses.ToList();
           // ViewBag.Course = objCourseList;
           // // Code to show Countries in DropDown
           // List<Country> countryList = db.Country.ToList();
           // ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
           // //Code to Show State Dropdown
           // List<State> statesList = db.States.ToList();
           // ViewBag.StateList = new SelectList(statesList, "StateId", "Name");
           // //Code to show City dropDown
           // List<City> citiesList = db.City.ToList();
           // ViewBag.CityList = new SelectList(citiesList, "CityId", "Name");
            try
            {
                User objUser = db.Users.Find(id);

                if (ModelState.IsValid)
                {
                    objUser.FirstName = objUserViewModel.FirstName;
                    objUser.LastName = objUserViewModel.LastName;
                    objUser.Gender = objUserViewModel.Gender;
                    objUser.Hobbies = objUserViewModel.Hobbies;
                    objUser.Email = objUserViewModel.Email;
                    objUser.IsVerified = objUserViewModel.IsVerified;
                    objUser.Password = objUserViewModel.Password;
                    objUser.ConfirmPassword = objUserViewModel.ConfirmPassword;
                    objUser.DateOfBirth = objUserViewModel.DateOfBirth;
                    objUser.CourseId = objUserViewModel.CourseId;
                    objUser.RoleId = objUserViewModel.RoleId;
                    objUser.Address.AddressLine1 = objUserViewModel.AddressLine1;
                    objUser.Address.AddressLine2 = objUserViewModel.AddressLine2;
                    objUser.Address.CountryId = objUserViewModel.CountryId;
                    objUser.Address.StateId = objUserViewModel.StateId;
                    objUser.Address.CityId = objUserViewModel.CityId;
                    objUser.Address.ZipCode = objUserViewModel.ZipCode;
                    objUser.IsActive = true;
                    objUser.DateModified = DateTime.Now;
                    //User Data is saved in the user table

                    db.SaveChanges();
                    return RedirectToAction("MyProfile");

                }
                return View(objUserViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// To Show List of Students
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllStudents(int id)
        {
            var studentList = db.Users.Where(x => x.RoleId == 4 && x.IsActive==true && x.CourseId==id).ToList();
            return View(studentList);
        }


        /// <summary>
        ///  GET: To Show the details of the Students
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult StudentDetails(int id)
        {
            // Code to show Roles in DropDown
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = objRoleList;
            // Code to show Courses in DropDown
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            // Code to show Countries in DropDown
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
            //Code to Show State Dropdown
            List<State> statesList = db.States.ToList();
            ViewBag.StateList = new SelectList(statesList, "StateId", "Name");
            //Code to show City dropDown
            List<City> citiesList = db.City.ToList();
            ViewBag.CityList = new SelectList(citiesList, "CityId", "Name");

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User objUser = db.Users.Find(id);

            UserViewModel objUserViewModel = new UserViewModel();
            objUserViewModel.FirstName = objUser.FirstName;
            objUserViewModel.LastName = objUser.LastName;
            objUserViewModel.Gender = objUser.Gender;
            objUserViewModel.Hobbies = objUser.Hobbies;
            objUserViewModel.Email = objUser.Email;
            objUserViewModel.Password = objUser.Password;
            objUserViewModel.ConfirmPassword = objUser.ConfirmPassword;
            objUserViewModel.DateOfBirth = objUser.DateOfBirth;
            objUserViewModel.RoleId = objUser.RoleId;
            objUserViewModel.CourseId = objUser.CourseId;
            objUserViewModel.IsActive = objUser.IsActive;
            objUserViewModel.DateCreated = objUser.DateCreated;
            objUserViewModel.DateModified = objUser.DateModified;
            objUserViewModel.AddressLine1 = objUser.Address.AddressLine1;
            objUserViewModel.AddressLine2 = objUser.Address.AddressLine2;
            objUserViewModel.CountryId = objUser.Address.CountryId;
            objUserViewModel.StateId = objUser.Address.StateId;
            objUserViewModel.CityId = objUser.Address.CityId;
            objUserViewModel.ZipCode = objUser.Address.ZipCode;

            if (objUser == null)
            {
                return HttpNotFound();
            }
            return View(objUserViewModel);
        }


     

        public static List<Role> GetRoles()
        {
            using (var db = new UserContext())
            {
                // condition not to Display SuperAdmin
                var roleList = db.Roles.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 4);
                return roleList.ToList();
            }
        }
    }
}
