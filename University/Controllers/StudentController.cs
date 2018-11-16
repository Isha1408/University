using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University.Entities;
using University.Models;

namespace University.Controllers
{
   
    public class StudentController : Controller
    {
        UserContext db = new UserContext();

        /// <summary>
        /// Method To get details of Logged in User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MyProfile()
        {
            if (Session["UserId"] == null && Session["UserName"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "UserView");
            }
            //Code to show details opf Logged in Student
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
        /// Method To display List of Subjects with Course of Student
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectList(int id)
        {
            if (Session["UserId"] == null && Session["UserName"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "UserView");
            }
            var subjectList = db.SubjectInCourses.Where(m => m.CourseId == id).ToList();

            return View(subjectList.ToList());

        }
        /// <summary>
        /// Method To show List of teachers Along with subjects
        /// </summary>
        /// <returns></returns>
        public ActionResult TeachersList(int id)
        {
            if (Session["UserId"] == null && Session["UserName"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "UserView");
            }

            var teachersList = db.TeacherInSubjects.Where(x => x.UserId == id).ToList();

            return View(teachersList);
        }
        public ActionResult TeachersCourseList(int id)
        {
            if (Session["UserId"] == null && Session["UserName"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "UserView");
            }

            var teachersList = db.Users.Where(x => x.CourseId == id && x.RoleId == 3 && x.IsActive == true).ToList();

            return View(teachersList);
        }
        /// <summary>
        /// To Edit Teacher Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditUser(int id)
        {
            if (Session["UserId"] == null && Session["UserName"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "UserView");
            }
            // Code to show Roles in DropDown
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = objRoleList;
            // Code to show Courses in DropDown
            List<Course> objCourseList = db.Courses.Where(x => x.IsActive == true).ToList();
            ViewBag.Course = objCourseList;
            // Code to show Countries in DropDown
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");

            


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
                    objUserViewModel.IsVerified = true;
                    objUserViewModel.DateCreated = objUser.DateCreated;
                    objUserViewModel.DateModified = objUser.DateModified;
                    objUserViewModel.AddressLine1 = objUser.Address.AddressLine1;
                    objUserViewModel.AddressLine2 = objUser.Address.AddressLine2;
                    objUserViewModel.CountryId = objUser.Address.CountryId;
                    objUserViewModel.StateId = objUser.Address.StateId;
                    objUserViewModel.CityId = objUser.Address.CityId;
                    objUserViewModel.ZipCode = objUser.Address.ZipCode;


                }
                //Code to Show State Dropdown
                List<State> statesList = db.States.Where(x => x.CountryId == objUserViewModel.CountryId).ToList();
                ViewBag.StateList = new SelectList(statesList, "StateId", "Name");
                //Code to show City dropDown
                List<City> citiesList = db.City.Where(x => x.StateId == objUserViewModel.StateId).ToList();
                ViewBag.CityList = new SelectList(citiesList, "CityId", "Name");
                return View(objUserViewModel);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
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
            if (Session["UserId"] == null && Session["UserName"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "UserView");
            }
            /// Code to show Roles in DropDown
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = objRoleList;
            // Code to show Courses in DropDown
            List<Course> objCourseList = db.Courses.Where(x => x.IsActive == true).ToList();
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
                    objUser.IsVerified = true;
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
                ModelState.AddModelError(string.Empty, ex.Message);
                throw ex;
            }
        }
    
        public static List<Role> GetRoles()
        {
            using (var db = new UserContext())
            {
                //condition to display only teachers
                var roleList = db.Roles.Where(x => x.RoleId != 1 && x.RoleId !=2 && x.RoleId!=3);
                return roleList.ToList();
            }
        }
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