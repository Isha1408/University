using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
            //UserViewModel objUserViewModel = new UserViewModel();
            //var queryRole = (from role in db.Roles select role).ToList();

            //objUserViewModel.RoleList = queryRole;

            //Code to show DropDown of Role
            List<Role> roleList = GetRoles();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            //Code to show Dropdown of Cousre
            List<Course> courseList = db.Courses.Where(x=>x.IsActive==true).ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            // Code to show DropDown of Country
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
            List<Course> courseList = db.Courses.Where(x => x.IsActive == true).ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            // Code to show DropDown of Country
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
         
            // Create the TransactionScope to execute the commands, guaranteeing
            // that both commands can commit or roll back as a single unit of work.

            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        //Address of the user is stored in the DataBase.
                        Address address = new Address();

                        //address.AddressId = objUserModel.AddressId;
                        address.AddressLine1 = objUserModel.AddressLine1;
                        address.AddressLine2 = objUserModel.AddressLine2;
                        address.CountryId = objUserModel.CountryId;
                        address.StateId = objUserModel.StateId;
                        address.CityId = objUserModel.CityId;
                        address.ZipCode = objUserModel.ZipCode;
                        db.Addresses.Add(address);
                        db.SaveChanges();

                        //Data is saved in the User Table.
                        User objUser = new User();
                        int latestAddressId = address.AddressId;
                        //obj.UserId = objUserModel.UserId;
                        objUser.FirstName = objUserModel.FirstName;
                        objUser.LastName = objUserModel.LastName;
                        objUser.Gender = objUserModel.Gender;
                        objUser.Hobbies = objUserModel.Hobbies;
                        objUser.Password = objUserModel.Password;
                        objUser.ConfirmPassword = objUserModel.ConfirmPassword;
                        objUser.IsVerified = objUserModel.IsVerified;
                        objUser.Email = objUserModel.Email;
                        objUser.DateOfBirth = objUserModel.DateOfBirth;
                        objUser.IsActive = objUserModel.IsActive;
                        objUser.DateCreated = DateTime.Now;
                        objUser.DateModified = DateTime.Now;
                        objUser.RoleId = objUserModel.RoleId;
                        objUser.CourseId = objUserModel.CourseId;
                        objUser.AddressId = latestAddressId;

                        db.Users.Add(objUser);
                        db.SaveChanges();

                        // User and their Roles are saved in the UserInRole Table.
                        int latestUserId = objUser.UserId;
                        UserInRole userInRole = new UserInRole();
                        userInRole.RoleId = objUserModel.RoleId;
                        userInRole.UserId = latestUserId;

                        db.UserInRoles.Add(userInRole);
                        db.SaveChanges();

                        transaction.Commit();
                        return RedirectToAction("Login");
                    }

                    return View(objUserModel);
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
                    throw ex;
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

           
            var userDetails = db.Users.Where(x => x.Email == user.Email && x.Password == user.Password ).FirstOrDefault();

           
            //Code to Authenticate Identity Of user.

            if (userDetails != null && userDetails.IsActive == true && userDetails.IsVerified==true)
            {


                Session["UserId"] = userDetails.UserId.ToString();
                Session["UserName"] = userDetails.Email.ToString();
                //For SuperAdmin
                if (userDetails.RoleId == 1)
                {
                    return RedirectToAction("GetAllUsers", "SuperAdmin");
                }
                //For Admin
                else if (userDetails.RoleId == 2)
                {
                    // return RedirectToAction("GetAllUsers", "Admin");
                    return RedirectToAction("GetAllUsers", "Admin", new { area = "Admin" });
                }
                //For Teacher
                else if (userDetails.RoleId == 3)
                {
                    Session["User"] = userDetails;

                    return RedirectToAction("MyProfile", "Teachers");
                }
                //For Student
                else if (userDetails.RoleId == 4)
                {
                    Session["User"] = userDetails;

                    return RedirectToAction("MyProfile", "Student");
                }
            }

            else if (userDetails == null)
            {
                ModelState.AddModelError("", "UserName or Password is wrong");

            }
           
            else if (userDetails.IsVerified != true)
            {
                ModelState.AddModelError("", "Please Verify Your Email");
            }
            else if (userDetails.IsActive != true)
            {
                ModelState.AddModelError("", "Your Account has not been Activated By Admin");
            }
            return View();
        }
        //To display ThankYou page after Registration.
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

            ////back button 
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            //return RedirectToAction("ThankYou");
            //    if (Session["UserId"] !=null && Session["UserName"] == null)
            //{
            //    Session.Abandon();
            //    Session.RemoveAll();
            //    Session.Remove("UserId");
            //    return RedirectToAction("Login");
            //}
            //else
            //{

            //    return RedirectToAction("Login");

            //}
            Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();
            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["User"] = null;
          
            return RedirectToAction("Login");
        }

        // Get All Roles

        public static List<Role> GetRoles()
        {
            using (var db = new UserContext())
            {
                var roleList = db.Roles.Where(x => x.RoleId != 1 && x.RoleId != 2);
                return roleList.ToList();
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

