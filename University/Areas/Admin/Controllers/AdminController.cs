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
                users = users.Where(s => s.Role.RoleName.Contains(searching) && s.RoleId != 1 && s.RoleId != 2);

            }
            
            return View(users.ToList());
           // var returnedResult = db.Users.Where(x => x.RoleId != 1 && x.RoleId != 2).ToList();
           // return View(returnedResult);

        }
        /// <summary>
        /// To show UserDetails
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserDetails(int? id)
        {
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User user = db.Users.Find(id);
                // var userData = from p in db.Users
                //  where p.UserId == id
                // select p;
                //  var tempUserList = db.Users.ToList();

                UserViewModel objUserViewModel = new UserViewModel
                {

                    //objUserViewModel.UserId = user.UserId;
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Hobbies = user.Hobbies,
                    Email = user.Email,
                    Password = user.Password,
                    DateOfBirth = user.DateOfBirth,
                    RoleId = user.RoleId,
                    CourseId = user.CourseId,
                    //objUserViewModel.AddressId = user.AddressId;
                    IsActive = user.IsActive,
                    DateCreated = user.DateCreated,
                    DateModified = user.DateModified,
                    AddressLine1 = user.Address.AddressLine1,
                    AddressLine2 = user.Address.AddressLine2,
                    CountryId = user.Address.CountryId,
                    StateId = user.Address.StateId,
                    CityId = user.Address.CityId,
                    ZipCode = user.Address.ZipCode
                };


                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(objUserViewModel);
            }
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
                            Password = objUserModel.Password.GetHashCode().ToString()
                        };
                        ;
                        obj.ConfirmPassword = objUserModel.ConfirmPassword.GetHashCode().ToString(); ;
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

                    UserInRole objUserInRole = db.UserInRoles.Find(id);
                    User objUser = db.Users.Find(id);
                    Address objAddress = db.Addresses.Find(objUser.AddressId);
                  
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

        /// <summary>
        /// Function to get list of Roles
        /// </summary>
        /// <returns></returns>

        public static List<Role> GetRoles()
        {
            using (var db = new UserContext())
            {
                // condition not to Display SuperAdmin and Admin
                var roleList = db.Roles.Where(x => x.RoleId != 1);
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