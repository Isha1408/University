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
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
            
            //CountryBind();
            
            return View();
        }
        
        [HttpPost]
        public ActionResult UserReg(UserModel ObjuserModel)
        {
            List<Role> roleList = GetRoles();

            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            //CountryBind();
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");

            ObjuserModel.UserId = 1;
            ObjuserModel.AddressId = 1;

            try
            {
                    Address address = new Address();

                    address.AddressId = ObjuserModel.AddressId;
                    address.AddressLine1 = ObjuserModel.AddressLine1;
                    address.AddressLine2 = ObjuserModel.AddressLine2;

                    address.CountryId = ObjuserModel.CountryId;
                    address.StateId = ObjuserModel.StateId;
                    address.CityId = ObjuserModel.CityId;

                    db.Addresses.Add(address);
                    db.SaveChanges();

                    int latestAddressId = address.AddressId;
                    User obj = new User();


                    obj.UserId = ObjuserModel.UserId;
                    obj.FirstName = ObjuserModel.FirstName;
                    obj.LastName = ObjuserModel.LastName;
                    obj.Gender = ObjuserModel.Gender;
                    obj.Hobbies = ObjuserModel.Hobbies;
                    obj.Password = ObjuserModel.Password;
                    obj.ConfirmPassword = ObjuserModel.ConfirmPassword;
                    obj.IsVerified = ObjuserModel.IsVerified;
                    obj.Email = ObjuserModel.Email;
                    obj.DateOfBirth = ObjuserModel.DateOfBirth;

                    obj.IsActive = ObjuserModel.IsActive;

                    obj.DateCreated = ObjuserModel.DateCreated;
                    obj.DateModified = ObjuserModel.DateModified;
                    obj.RoleId = ObjuserModel.RoleId;
                    obj.CourseId = ObjuserModel.CourseId;
                    obj.AddressId = ObjuserModel.AddressId;
                    db.Users.Add(obj);
                    db.SaveChanges();


                    int latestUserId = obj.UserId;
                    UserInRole userInRole = new UserInRole();
                    userInRole.RoleId = ObjuserModel.RoleId;
                    userInRole.UserId = latestUserId;
                    db.UserInRoles.Add(userInRole);
                    db.SaveChanges();
                }
            catch(Exception ex)
            {
                return View(ex);
            }
            return View();
        

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

