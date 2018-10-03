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
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                using (UserContext db = new UserContext())
                {
                    List<Role> roleList = GetRoles();
                    //List<Role> list = db.Roles.ToList();
                    ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
                    List<Course> courseList = db.Courses.ToList();
                    ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                    //List<Country> countryList = db.Country.ToList();
                    //ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
                   CountryBind();
                    //StateBind(countryId);
                   

                    //List<State> stateList = GetStates(Country.CountryId);
                    ////List<State> stateList = stateList.Where(x => stateList.Any(p => x.CountryId == p.CountryId));
                    //ViewBag.StateList = new SelectList(stateList, "StateId", "StateName");
                    //List<City> cityList = GetCity(stateId);
                    //ViewBag.CityList = new SelectList(cityList, "CityId", "CityName");

                    User obj = new User();
                    Address address = new Address();
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
                   //obj.DateCreated = DateTime.Now;
                    //obj.DateModified = DateTime.Now;

                    obj.DateCreated = user.DateCreated;
                    obj.DateModified = user.DateModified;
                    obj.Address.CountryId = address.CountryId;
                    obj.Address.StateId = address.StateId;
                    obj.Address.CityId = address.CityId;
                    obj.RoleId = user.RoleId;
                    obj.CourseId = user.CourseId;
                   
                    obj.IsActive = user.IsActive;
                    obj.AddressId = user.AddressId;
                   
                    db.Users.Add(obj);
                    db.SaveChanges();

                    int NewAddressId = obj.AddressId;
                    
                    address.CountryId = address.CountryId;
                    address.StateId = address.StateId;
                    address.CityId = address.CityId;
                    address.AddressLine1 = address.AddressLine1;
                    address.AddressLine2 = address.AddressLine2;
                    address.ZipCode = address.ZipCode;
                    address.AddressId = NewAddressId;
                    db.Addresses.Add(address);
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

        public static List<Role> GetRoles()
        {
            using (var db = new UserContext())
            {
                var k = db.Roles.Where(x => x.RoleId != 1 && x.RoleId != 2);
                return k.ToList();
            }
        }
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