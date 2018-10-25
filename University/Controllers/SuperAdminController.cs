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
    public class SuperAdminController : Controller
    {
        // Object of Context class is made.
        private UserContext db = new UserContext();

        public ActionResult Index(string searching)
        {
            var users = from s in db.Users select s;
            if (!String.IsNullOrEmpty(searching))
            {
                users = users.Where(s => s.Role.RoleName.Contains(searching) && s.RoleId != 1 && s.RoleId != 2 || s.FirstName.Contains(searching) ||
                 s.LastName.Contains(searching) || s.course.CourseName.Contains(searching));

            }
            return View(users.ToList());
        }

        /// <summary>
        /// To show List of all Users and Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUsers(string searching)
        {
            var users = from s in db.Users where s.RoleId !=1 select s;
            if (!String.IsNullOrEmpty(searching))
            {
                users = users.Where(s => s.Role.RoleName.Contains(searching) && s.RoleId != 1);

            }
            //  var returnedUserList = db.Users.Where(x => x.RoleId != 1).ToList();
            // return View(returnedUserList);
            return View(users.ToList());
        }

        /// <summary>
        ///  GET: To Show the details of the user
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
                //  select p;
                // var tempUserList = db.Users.ToList();

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
                    IsVerified = user.IsVerified,
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
        /// GET To Create new User Record
        /// </summary>
        /// <returns></returns>

        public ActionResult CreateUser()
        {
            // Code to show DropDown for Role.
            List<Role> roleList = GetRoles();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            // Code to show DropDown for Course.
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            //Code to show DropDown for Country.
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");


            return View();
        }
        /// <summary>
        /// Post method : To Create new user Record
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

            //objUserModel.UserId = 1;
            //objUserModel.AddressId = 1;

            /* Create the TransactionScope to execute the commands, guaranteeing
              
             that both commands can commit or roll back as a single unit of work.*/
            
            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    Address address = new Address
                    {

                        //address.AddressId = objUserModel.AddressId;
                        AddressLine1 = objUserModel.AddressLine1,
                        AddressLine2 = objUserModel.AddressLine2,
                        CountryId = objUserModel.CountryId,
                        StateId = objUserModel.StateId,
                        CityId = objUserModel.CityId,
                        ZipCode = objUserModel.ZipCode
                    };

                    db.Addresses.Add(address); //Address of the user is stored in the DataBase.
                    db.SaveChanges();

                    //Data is saved in the User Table.
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
                    db.Users.Add(obj);      
                    db.SaveChanges();

                    // User and their Roles are saved in the UserInRole Table.
                    int latestUserId = obj.UserId;
                    UserInRole userInRole = new UserInRole
                    {
                        RoleId = objUserModel.RoleId,
                        UserId = latestUserId
                    };
                    db.UserInRoles.Add(userInRole);

                    db.SaveChanges();
                    transaction.Commit();
                    return RedirectToAction("GetAllUsers");
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
            objUserViewModel.IsVerified = objUser.IsVerified;
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
        ///  To Edit User Record
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
                    objUser.IsVerified = objUserViewModel.IsVerified;
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
                    objUser.IsActive = objUserViewModel.IsActive;
                    objUser.DateModified = DateTime.Now;
                    //User Data is saved in the user table

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
        ///  GET: To Delete User from User table
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

            UserViewModel objUserViewModel = new UserViewModel();
            objUserViewModel.FirstName = objUser.FirstName;
            objUserViewModel.LastName = objUser.LastName;
            objUserViewModel.Gender = objUser.Gender;
            objUserViewModel.Hobbies = objUser.Hobbies;
            objUserViewModel.Email = objUser.Email;
            objUserViewModel.Password = objUser.Password;
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
        ///  POST Method: To Delete User from User table
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
        /// Function to get list of Roles
        /// </summary>
        /// <returns></returns>

        public static List<Role> GetRoles()
        {
            using (var db = new UserContext())
            {
                // condition not to Display SuperAdmin
                var roleList = db.Roles.Where(x => x.RoleId != 1);
                return roleList.ToList();
            }
        }


        /// <summary>
        /// Get all states from state table 
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
        /// Get all Cities from City table
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
