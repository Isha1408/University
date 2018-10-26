using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using University.Entities;
using University.Models;

namespace University.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // Object of Context class is made. 
        private UserContext db = new UserContext();


        /// <summary>
        /// To Show List of Teacher and Student
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUsers(string searching )
        {
            var users = from s in db.Users where s.RoleId != 1 && s.RoleId !=2 select s;
            if (!String.IsNullOrEmpty(searching))
            {
                users = users.Where(s => s.Role.RoleName.Contains(searching) && s.RoleId != 1 && s.RoleId != 2 ||s.FirstName.Contains(searching)||
                s.LastName.Contains(searching)|| s.course.CourseName.Contains(searching));

            }
            
            return View(users.ToList());
           // var returnedResult = db.Users.Where(x => x.RoleId != 1 && x.RoleId != 2).ToList();
           // return View(returnedResult);

        }
        /// <summary>
        ///  GET: To Show the details of the Students
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserDetails(int id)
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
        /// <summary>
        /// GET: To create Teacher and Student
        /// </summary>
        /// <returns></returns>

        public ActionResult CreateUser()
        {
            // Code to show Roles in DropDown
            List<Role> roleList = GetRoles();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            // Code to show Courses in DropDown
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            // Code to show Countries in DropDown
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
            return View();
        }
        /// <summary>
        /// Post:To create Teacher and Student 
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUser(UserViewModel objUserModel)
        {
            // Code to show Roles in DropDown
            List<Role> roleList = GetRoles();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            // Code to show Courses in DropDown
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            // Code to show Countries in DropDown
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");

          //  objUserModel.UserId = 1;
            //objUserModel.AddressId = 1;

            // Create the TransactionScope to execute the commands, guaranteeing
            // that both commands can commit or roll back as a single unit of work.

            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    if (ModelState.IsValid)
                    {

                        Address address = new Address
                        {
                            AddressLine1 = objUserModel.AddressLine1,
                            AddressLine2 = objUserModel.AddressLine2,
                            CountryId = objUserModel.CountryId,
                            StateId = objUserModel.StateId,
                            CityId = objUserModel.CityId
                        };
                        db.Addresses.Add(address); //Address of the user is stored in the DataBase.
                        db.SaveChanges();

                        int latestAddressId = address.AddressId;
                        User obj = new User
                        {
                            FirstName = objUserModel.FirstName,
                            LastName = objUserModel.LastName,
                            Gender = objUserModel.Gender,
                            Hobbies = objUserModel.Hobbies,
                            Password = objUserModel.Password,
                            ConfirmPassword = objUserModel.ConfirmPassword,
                            IsVerified = objUserModel.IsVerified,
                            Email = objUserModel.Email,
                            DateOfBirth = objUserModel.DateOfBirth,
                            IsActive = objUserModel.IsActive,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            RoleId = objUserModel.RoleId,
                            CourseId = objUserModel.CourseId,
                            AddressId = latestAddressId
                        };
                      db.Users.Add(obj);//Data is saved in the User Table.
                        db.SaveChanges();


                        int latestUserId = obj.UserId;

                        UserInRole userInRole = new UserInRole();
                        userInRole.RoleId = objUserModel.RoleId;
                        userInRole.UserId = latestUserId;
                        db.UserInRoles.Add(userInRole);// User and their Roles are saved in the UserInRole Table.
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("GetAllUsers");
                    }
                    return View(objUserModel);
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
        ///  GET: To Edit User Record
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
            List<State> statesList = db.States.ToList();
            ViewBag.StateList = new SelectList(statesList, "StateId", "Name");
            List<City> citiesList = db.City.ToList();
            ViewBag.CityList = new SelectList(citiesList, "CityId", "Name");

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User objUser = db.Users.Find(id);

            UserViewModel objUserViewModel = new UserViewModel
            {
                FirstName = objUser.FirstName,
                LastName = objUser.LastName,
                Gender = objUser.Gender,
                Hobbies = objUser.Hobbies,
                Email = objUser.Email,
                Password = objUser.Password,
                ConfirmPassword = objUser.ConfirmPassword,
                DateOfBirth = objUser.DateOfBirth,
                RoleId = objUser.RoleId,
                CourseId = objUser.CourseId,
                IsActive = objUser.IsActive,
                DateCreated = objUser.DateCreated,
                DateModified = objUser.DateModified,
                AddressLine1 = objUser.Address.AddressLine1,
                AddressLine2 = objUser.Address.AddressLine2,
                CountryId = objUser.Address.CountryId,
                StateId = objUser.Address.StateId,
                CityId = objUser.Address.CityId,
                ZipCode = objUser.Address.ZipCode
            };


            if (objUser == null)
            {
                return HttpNotFound();
            }
            return View(objUserViewModel);
        }

        /// <summary>
        ///  POST:  To Edit User Record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUser(int id, UserViewModel objUserViewModel)
        {
            // Code to show Roles in DropDown
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = new SelectList(db.Users.ToList(), "RoleId", "RoleName");
            // Code to show Courses in DropDown
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            // Code to show Countries in DropDown
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
            // Code to show States in DropDown
            List<State> statesList = db.States.ToList();
            ViewBag.StateList = new SelectList(statesList, "StateId", "Name");
            // Code to show Cities in DropDown
            List<City> citiesList = db.City.ToList();
            ViewBag.CityList = new SelectList(citiesList, "CityId", "Name");
            try
            {
                User userData = db.Users.Find(id);
                            
                if (ModelState.IsValid)
                {
                    userData.FirstName = objUserViewModel.FirstName;
                    userData.LastName = objUserViewModel.LastName;
                    userData.Gender = objUserViewModel.Gender;
                    userData.Hobbies = objUserViewModel.Hobbies;
                    userData.Email = objUserViewModel.Email;
                    userData.IsVerified = objUserViewModel.IsVerified;
                    userData.Password = objUserViewModel.Password;
                    userData.ConfirmPassword = objUserViewModel.ConfirmPassword;
                    userData.DateOfBirth = objUserViewModel.DateOfBirth;
                    userData.CourseId = objUserViewModel.CourseId;
                    userData.RoleId = objUserViewModel.RoleId;                   
                    userData.Address.AddressLine1 = objUserViewModel.AddressLine1;
                    userData.Address.AddressLine2 = objUserViewModel.AddressLine2;
                    userData.Address.CountryId = objUserViewModel.CountryId;
                    userData.Address.StateId = objUserViewModel.StateId;
                    userData.Address.CityId = objUserViewModel.CityId;
                    userData.Address.ZipCode = objUserViewModel.ZipCode;
                    userData.IsActive = objUserViewModel.IsActive;
                    userData.DateModified = DateTime.Now;
                    // Updated Data is saved in User table
                    db.SaveChanges();
                    return RedirectToAction("GetAllUsers");

                }
                return View(objUserViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        ///  GET: To Delete User Record from User table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult DeleteUser(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User objUser = db.Users.Find(id);

            UserViewModel objUserViewModel = new UserViewModel
            {
                FirstName = objUser.FirstName,
                LastName = objUser.LastName,
                Gender = objUser.Gender,
                Hobbies = objUser.Hobbies,
                Email = objUser.Email,
                Password = objUser.Password,
                DateOfBirth = objUser.DateOfBirth,
                RoleId = objUser.RoleId,
                CourseId = objUser.CourseId,
                IsActive = objUser.IsActive,
                DateCreated = objUser.DateCreated,
                DateModified = objUser.DateModified,
                AddressLine1 = objUser.Address.AddressLine1,
                AddressLine2 = objUser.Address.AddressLine2,
                CountryId = objUser.Address.CountryId,
                StateId = objUser.Address.StateId,
                CityId = objUser.Address.CityId,
                ZipCode = objUser.Address.ZipCode
            };

            if (objUser == null)
            {
                return HttpNotFound();
            }
            return View(objUserViewModel);

        }
        /// <summary>
        ///  POST: To Delete User Record from User table
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    UserInRole objUserInRole = db.UserInRoles.Where(m => m.UserId == id).FirstOrDefault();
                    User objUser = db.Users.Where(m => m.UserId == id).FirstOrDefault();
                    Address objAddress = db.Addresses.Where(m => m.AddressId == objUser.AddressId).FirstOrDefault(); 
                  
                        //To remove address of user from address table
                        db.Addresses.Remove(objAddress);
                        //To Remove User from User Table
                        db.Users.Remove(objUser);
                       
                        // To remove User from UserInRole table.
                        db.UserInRoles.Remove(objUserInRole);

                        db.SaveChanges();
                    
                }
                return RedirectToAction("GetAllUsers");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// For logging out the session
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("ThankYou");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Admin will be redirected to this page after Logout
        /// </summary>
        /// <returns></returns>
        public ActionResult ThankYou()
        {
            return View();
        }


        public ActionResult AddSubject()
        {
          
            List<Subject> Lists = db.Subjects.ToList();
            ViewBag.SubjectList = new SelectList(Lists, "SubjectId", "SubjectName");

            return View();
        }

        /// <summary>
        /// Function to get list of Roles
        /// </summary>
        /// <returns></returns>

        public static List<Role> GetRoles()
        {
            using (var db = new UserContext())
            {
                // condition not to Display SuperAdmin and Admin
                var roleList = db.Roles.Where(x => x.RoleId != 1 && x.RoleId!=2);
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