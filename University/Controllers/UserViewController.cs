﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using University.Entities;
using University.Models;

namespace University.Controllers
{
    public class UserViewController : Controller
    {
        // Object of Context class is made.

        UserContext db = new UserContext();

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// GET Method: For Creating User
        /// </summary>
        /// <returns></returns>
        public ActionResult UserRegistration()
        {
            List<Role> roleList = GetRoles();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
            return View();
        }
        /// <summary>
        /// Post Method: To Post the data of User in User table.
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserRegistration(UserViewModel objUserModel)
        {
            //Code to show DropDown of Role
            List<Role> roleList = GetRoles();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            //Code to show Dropdown of Cousre
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            // Code to show DropDown of Country
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");

            objUserModel.UserId = 1;
            objUserModel.AddressId = 1;

            // Create the TransactionScope to execute the commands, guaranteeing
            // that both commands can commit or roll back as a single unit of work.

            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    Address address = new Address();

                    address.AddressId = objUserModel.AddressId;
                    address.AddressLine1 = objUserModel.AddressLine1;
                    address.AddressLine2 = objUserModel.AddressLine2;
                    address.CountryId = objUserModel.CountryId;
                    address.StateId = objUserModel.StateId;
                    address.CityId = objUserModel.CityId;

                    db.Addresses.Add(address); //Address of the user is stored in the DataBase.
                    db.SaveChanges();

                    int latestAddressId = address.AddressId;
                    User obj = new User();
                    obj.UserId = objUserModel.UserId;
                    obj.FirstName = objUserModel.FirstName;
                    obj.LastName = objUserModel.LastName;
                    obj.Gender = objUserModel.Gender;
                    obj.Hobbies = objUserModel.Hobbies;
                    obj.Password = objUserModel.Password.GetHashCode().ToString();
                    obj.ConfirmPassword = objUserModel.ConfirmPassword.GetHashCode().ToString();
                    obj.IsVerified = objUserModel.IsVerified;
                    obj.Email = objUserModel.Email;
                    obj.DateOfBirth = objUserModel.DateOfBirth;
                    obj.IsActive = objUserModel.IsActive;
                    obj.DateCreated = DateTime.Now;
                    obj.DateModified = DateTime.Now;
                    obj.RoleId = objUserModel.RoleId;
                    obj.CourseId = objUserModel.CourseId;
                    obj.AddressId = latestAddressId;
                    db.Users.Add(obj);//Data is saved in the User Table.
                    db.SaveChanges();


                    int latestUserId = obj.UserId;
                    UserInRole userInRole = new UserInRole();
                    userInRole.RoleId = objUserModel.RoleId;
                    userInRole.UserId = latestUserId;
                    db.UserInRoles.Add(userInRole);// User and their Roles are saved in the UserInRole Table.
                    db.SaveChanges();
                    transaction.Commit();
                    return RedirectToAction("ThankYou");
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
                    return View(ex);
                }

            }
        }

        /// <summary>
        /// GET Method: For User Login
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Post Method: To Authenticate User Identity
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            var userDetails = db.Users.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();

            //Code to Authenticate Identity Of user.
            if (userDetails != null)
            {
                Session["UserId"] = userDetails.UserId.ToString();
                Session["UserName"] = userDetails.Email.ToString();

                if (userDetails.RoleId == 1)
                {

                    return RedirectToAction("Index", "SuperAdmin");

                }
                else if (userDetails.RoleId == 2)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.Email.ToString();

                    return RedirectToAction("Index", "Admin");
                }
                else if (userDetails.RoleId == 3)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.Email.ToString();
                    return RedirectToAction("Index", "Teacher");
                }
                else if (userDetails.RoleId == 4)
                {
                    Session["UserId"] = userDetails.UserId.ToString();
                    Session["UserName"] = userDetails.Email.ToString();

                    return RedirectToAction("Index", "student");
                }
            }
            else
            {
                ModelState.AddModelError("", "UserName or Password is wrong");

            }


            return View();
        }
        public ActionResult ThankYou()
        {
            return View();
        }

        /// <summary>
        /// Action method for LogOut
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // Get All Roles

        public static List<Role> GetRoles()
        {
            using (var db = new UserContext())
            {
                var k = db.Roles.Where(x => x.RoleId != 1 && x.RoleId != 2);
                return k.ToList();
            }
        }

        /// <summary>
        /// Get all states
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>

        public DataSet GetStates(string countryId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString);

            SqlCommand com = new SqlCommand("Select * from State where CountryId=@catid", con);
            com.Parameters.AddWithValue("@catid", countryId);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;

        }
        /// <summary>
        /// Code to bind States.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public JsonResult StateBind(string countryId)
        {

            DataSet ds = GetStates(countryId);
            List<SelectListItem> stateList = new List<SelectListItem>();


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                stateList.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["StateId"].ToString() });

            }

            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get all Cities
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public DataSet GetCity(string stateId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString);

            SqlCommand com = new SqlCommand("Select * from City where StateId=@staid", con);
            com.Parameters.AddWithValue("@staid", stateId);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;

        }
        /// <summary>
        /// Code To bind City
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public JsonResult CityBind(string stateId)
        {

            DataSet ds = GetCity(stateId);

            List<SelectListItem> cityList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cityList.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["CityId"].ToString() });
            }

            return Json(cityList, JsonRequestBehavior.AllowGet);
        }
    }
}

