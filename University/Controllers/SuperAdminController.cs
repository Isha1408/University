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
        private UserContext db=new UserContext();
        // GET: SuperAdmin
        public ActionResult Index()
        {

            var returnedResult = db.Users.Where(x => x.RoleId != 1).ToList();
            return View(returnedResult);

        }

        // GET: SuperAdmin/Details/5
        public ActionResult Details(int? id)
        {
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User user = db.Users.Find(id);
                var data = from p in db.Users
                           where p.UserId == id
                           select p;
                var TempList = db.Users.ToList();

                UserViewModel objUserViewModel = new UserViewModel();

                objUserViewModel.UserId = user.UserId;
                objUserViewModel.FirstName = user.FirstName;
                objUserViewModel.LastName = user.LastName;
                objUserViewModel.Gender = user.Gender;
                objUserViewModel.Hobbies = user.Hobbies;
                objUserViewModel.Email = user.Email;
                objUserViewModel.Password = user.Password;
                objUserViewModel.DateOfBirth = user.DateOfBirth;
                objUserViewModel.RoleId = user.RoleId;
                objUserViewModel.CourseId = user.CourseId;
                objUserViewModel.AddressId = user.AddressId;
                objUserViewModel.IsActive = user.IsActive;
                objUserViewModel.DateCreated = user.DateCreated;
                objUserViewModel.DateModified = user.DateModified;
                objUserViewModel.AddressLine1 = user.Address.AddressLine1;
                objUserViewModel.AddressLine2 = user.Address.AddressLine2;
                objUserViewModel.CountryId = user.Address.CountryId;
                objUserViewModel.StateId = user.Address.StateId;
                objUserViewModel.CityId = user.Address.CityId;
                objUserViewModel.ZipCode = user.Address.ZipCode;


                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(objUserViewModel);
            }
        }
        /// <summary>
        /// GET: SuperAdmin/Create
        /// </summary>
        /// <returns></returns>
        
        public ActionResult Create()
        {
            List<Role> roleList = GetRoles();

            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            List<Course> courseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
            return View();
        }
        /// <summary>
        /// Post method 
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Create(UserViewModel objUserModel)
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
                obj.AddressId = latestAddressId;
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
        /// <summary>
        ///  GET: SuperAdmin/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        
        public ActionResult Edit(int id)
        {
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = objRoleList;
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");

            if (id == 0 )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User objUser = db.Users.Find(id);

          //Address objAddress = db.Addresses.Find(addressId);
            
            var data = from p in db.Users where p.UserId == id select p;
            //var addressdata = from v in db.Addresses where v.AddressId == addressId select v;

            var tempList = db.Users.ToList();
           

            UserViewModel objUserViewModel = new UserViewModel();

            objUserViewModel.UserId= objUser.UserId;
            objUserViewModel.FirstName =objUser.FirstName ;
            objUserViewModel.LastName = objUser.LastName;
            objUserViewModel.Gender = objUser.Gender;
            objUserViewModel.Hobbies = objUser.Hobbies;
            objUserViewModel.Email = objUser.Email;
            objUserViewModel.Password = objUser.Password;
            objUserViewModel.DateOfBirth = objUser.DateOfBirth;
            objUserViewModel.RoleId = objUser.RoleId;
            objUserViewModel.CourseId = objUser.CourseId;
            objUserViewModel.AddressId = objUser.AddressId;
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
        ///  Action for Edit Post method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>

        // POST: SuperAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id,UserViewModel objUserViewModel)
        {
            List<Role> objRoleList = GetRoles();
            ViewBag.Role = new SelectList(db.Users.ToList(), "RoleId", "RoleName");
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            List<Country> countryList = db.Country.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
            //List<State> statesList = db.States.Where(x => x.CountryId == Address.CountryId).ToList();
          //  ViewBag.StateList = new SelectList(statesList, "StateId", "StateName");

          //  List<City> citiesList = db.Cities.Where(x => x.StateId == objUserRegistration.Address.StateId).ToList();
          //  ViewBag.CityList = new SelectList(citiesList, "CityId", "CityName");

            try
            {
                User userData = db.Users.Find(objUserViewModel.UserId);
                Address addressData = db.Addresses.Find(objUserViewModel.AddressId);
                var data = from p in db.Users where p.UserId == objUserViewModel.UserId select p;
                var tempData = from v in db.Addresses where v.AddressId == objUserViewModel.AddressId select v;


                var TempList = db.Users.FirstOrDefault();

                if (ModelState.IsValid)
                {

                    userData.UserId = objUserViewModel.UserId;
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
                    userData.AddressId = objUserViewModel.AddressId;
                    userData.Address.AddressLine1 = objUserViewModel.AddressLine1;
                    userData.Address.AddressLine2 = objUserViewModel.AddressLine2;
                    userData.Address.CountryId = objUserViewModel.CountryId;
                    userData.Address.StateId = objUserViewModel.StateId;
                    userData.Address.CityId = objUserViewModel.CityId;
                    userData.Address.ZipCode = objUserViewModel.ZipCode;

                    userData.IsActive = objUserViewModel.IsActive;
                    userData.DateModified = DateTime.Now;

                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View(objUserViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        ///  GET: SuperAdmin/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        public ActionResult Delete(int? id)
        {
            
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User objUser = db.Users.Find(id);
                var data = from p in db.Users
                           where p.UserId == id
                           select p;
                var TempList = db.Users.ToList();


                UserViewModel objUserViewModel = new UserViewModel();

                objUserViewModel.UserId = objUser.UserId;
                objUserViewModel.FirstName = objUser.FirstName;
                objUserViewModel.LastName = objUser.LastName;
                objUserViewModel.Gender = objUser.Gender;
                objUserViewModel.Hobbies = objUser.Hobbies;
                objUserViewModel.Email = objUser.Email;
                objUserViewModel.Password = objUser.Password;
                objUserViewModel.DateOfBirth = objUser.DateOfBirth;
                objUserViewModel.RoleId = objUser.RoleId;
                objUserViewModel.CourseId = objUser.CourseId;
                objUserViewModel.AddressId = objUser.AddressId;
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
        ///  POST: SuperAdmin/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
       
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    User objUser = db.Users.Find(id);
                    db.Users.Remove(objUser);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult CreateCourse ()
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
                // condition not to Display SuperAdmin
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
