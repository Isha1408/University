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
    public class SuperAdminController : Controller
    {
        private UserContext db=new UserContext();
        // GET: SuperAdmin
        public ActionResult Index()
        {

            var returnedResult = db.Users.Where(x => x.RoleId != 1).ToList();
            return View(returnedResult);

        }

        // GET: SuperAdmin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SuperAdmin/Create
        public ActionResult CreateUser()
        {
            List<Role> roleList = GetRoles();

            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(UserViewModel objUserModel)
        {
            List<Role> roleList = GetRoles();

            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            //CountryBind();
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");

            objUserModel.UserId = 1;
            objUserModel.AddressId = 1;

            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                // using (IDbTransaction tran = conn.BeginTransaction())
                //{
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
                obj.Password = objUserModel.Password;
                obj.ConfirmPassword = objUserModel.ConfirmPassword;
                obj.IsVerified = objUserModel.IsVerified;
                obj.Email = objUserModel.Email;
                obj.DateOfBirth = objUserModel.DateOfBirth;
                obj.IsActive = objUserModel.IsActive;
                obj.DateCreated = DateTime.Now;
                obj.DateModified = DateTime.Now;
                obj.RoleId = objUserModel.RoleId;
                obj.CourseId = objUserModel.CourseId;
                obj.AddressId = objUserModel.AddressId;
                db.Users.Add(obj);//Data is saved in the User Table.
                db.SaveChanges();


                int latestUserId = obj.UserId;
                UserInRole userInRole = new UserInRole();
                userInRole.RoleId = objUserModel.RoleId;
                userInRole.UserId = latestUserId;
                db.UserInRoles.Add(userInRole);// User and their Roles are saved in the UserInRole Table.
                db.SaveChanges();

                return RedirectToAction("Index");

                // tran.Commit();


            }

            catch (Exception ex)
            {
                // tran.Rollback();
                //throw;
                return View(ex);
            }

          

        }

        // GET: SuperAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SuperAdmin/Edit/5
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

        // GET: SuperAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SuperAdmin/Delete/5
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


        public static List<Role> GetRoles()
        {
            using (var db = new UserContext())
            {
                var k = db.Roles.Where(x => x.RoleId != 1 );
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
