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
    public class UserViewController : Controller
    {
        UserContext db = new UserContext();
       
        public ActionResult Index()
        {
            return View();
        }
        // GET: UserView
        public ActionResult UserReg()
        {
            List<Role> roleList = GetRoles();

            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            //List<Country> countryList = db.Country.ToList();
            //ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
            
            CountryBind();


            return View();
        }
        
        [HttpPost]

        public ActionResult UserReg(UserModel userModel)
        {
            if (ModelState.IsValid)
            {


                List<Role> roleList = GetRoles();

                ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
                List<Course> courseList = db.Courses.ToList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
               
                CountryBind();

                User obj = new User();


                obj.UserId = userModel.UserId;
                obj.FirstName = userModel.FirstName;
                obj.LastName = userModel.LastName;
                obj.Gender = userModel.Gender;
                obj.Hobbies = userModel.Hobbies;
                obj.Password = userModel.Password;
                obj.ConfirmPassword = userModel.ConfirmPassword;
                obj.IsVerified = userModel.IsVerified;
                obj.Email = userModel.Email;
                obj.DateOfBirth = userModel.DateOfBirth;

                obj.IsActive = userModel.IsActive;
                obj.AddressId = userModel.AddressId;
                obj.DateCreated = userModel.DateCreated;
                obj.DateModified = userModel.DateModified;
                obj.RoleId = userModel.RoleId;
                obj.CourseId = userModel.CourseId;

                db.Users.Add(obj);
                db.SaveChanges();

                Address address = new Address();

                address.AddressId = userModel.AddressId;
                address.AddressLine1 = userModel.AddressLine1;
                address.AddressLine2 = userModel.AddressLine2;

                address.CountryId = userModel.CountryId;
                address.StateId = userModel.StateId;
                address.CityId = userModel.CityId;

                db.Addresses.Add(address);
                db.SaveChanges();

                int latestUserId = obj.UserId;
                UserInRole userInRole = new UserInRole();
                userInRole.RoleId = userModel.RoleId;
                userInRole.UserId = latestUserId;
                db.UserInRoles.Add(userInRole);
                db.SaveChanges();
            }
            return View(userModel);
        

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
            
          // return (ViewBag.Country);
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

