using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Entities;
using University.Models;

namespace University.Controllers
{
    public class UserController : Controller
    {
        UserContext db = new UserContext();
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
                List<Role> roleList = GetRoles();
                
                ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
                List<Course> courseList = db.Courses.ToList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                
                CountryBind();
          
            }
            return View();
         }
        [HttpPost]
        public ActionResult Registration(UserViewModel user)
        {
            if (ModelState.IsValid)
            {


                List<Role> roleList = GetRoles();

                ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
                List<Course> courseList = db.Courses.ToList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");

                CountryBind();

             //  User obj = new User();
                

            //    obj.UserId = user.UserId;
            //    obj.FirstName = user.FirstName;
            //    obj.LastName = user.LastName;
            //    obj.Gender = user.Gender;
            //    obj.Hobbies = user.Hobbies;
            //    obj.Password = user.Password;
            //    obj.ConfirmPassword = user.ConfirmPassword;
            //    obj.IsVerified = user.IsVerified;
            //    obj.Email = user.Email;
            //    obj.DateOfBirth = user.DateOfBirth;

            //    obj.IsActive = user.IsActive;
            //    obj.AddressId = user.AddressId;
            //    obj.DateCreated = user.DateCreated;
            //    obj.DateModified = user.DateModified;
            //    obj.RoleId = user.RoleId;
            //    obj.CourseId = user.CourseId;
           
            //db.Users.Add(obj);
            //db.SaveChanges();

            //   Model.Address address = new Address();
            
            //address.AddressId = userModel.AddressId;
            //address.AddressLine1 = userModel.AddressLine1;
            //address.AddressLine2 = userModel.AddressLine2;

            //address.CountryId = userModel.CountryId;
            //address.StateId = userModel.StateId;
            //address.CityId = userModel.CityId;
       
                   
                   
                   
             //db.Addresses.Add(address);
             db.SaveChanges();

                    //int NewAddressId = obj.AddressId;
                    
                    //address.CountryId = address.CountryId;
                    //address.StateId = address.StateId;
                    //address.CityId = address.CityId;
                    //address.AddressLine1 = address.AddressLine1;
                    //address.AddressLine2 = address.AddressLine2;
                    //address.ZipCode = address.ZipCode;
                    //address.AddressId = NewAddressId;
                    //db.Addresses.Add(address);
                    //db.SaveChanges();


                   // int latestUserId = obj.UserId;
                   // UserInRole userInRole = new UserInRole();
                   // userInRole.RoleId = user.RoleId;
                   // userInRole.UserId = latestUserId;
                   // db.UserInRoles.Add(userInRole);
                   // db.SaveChanges();
        }
    
            return View(user);

        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {

            using (UserContext db = new UserContext())
            {
                var userDetails = db.Users.Where(x => x.Email== user.Email && x.Password == user.Password).FirstOrDefault();
                //Code to Authenticate Identity Of user.
                if (userDetails != null)
                {

                    if (userDetails.RoleId == 1)
                    {
                        Session["UserId"] = userDetails.UserId.ToString();
                        Session["UserName"] = userDetails.Email.ToString();
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (userDetails.RoleId == 2)
                    {
                        Session["User"] = userDetails;

                        return RedirectToAction("Index", "Student");
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
                        return RedirectToAction("Index", "Teacher");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password is wrong");

                }

            }
            return View();
        }

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
        /// Get All Countries
        /// </summary>
        /// <returns></returns>
        
        public DataSet GetCountry()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString);

            SqlCommand com = new SqlCommand("Select * from Country", con);
          

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;

        }
      
        public void CountryBind()
        {

            DataSet ds = GetCountry();
            List<SelectListItem> countryList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                countryList.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["CountryId"].ToString() });

            }
            ViewBag.Country = countryList;
            
        }
        /// <summary>
        /// Get all states
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public DataSet GetStates(int countryId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString);

            SqlCommand com = new SqlCommand("Select * from State where CountryId=@catid",con);
            com.Parameters.AddWithValue("@catid", countryId);

                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds);

                return ds;
             
        }
        public JsonResult StateBind(int countryId)
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
        public DataSet GetCity(int stateId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString);

            SqlCommand com = new SqlCommand("Select * from City where StateId=@staid", con);
            com.Parameters.AddWithValue("@staid", stateId);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;

        }
        public JsonResult CityBind(int stateId)
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