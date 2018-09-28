using System;
using System.Collections.Generic;
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
                //List<Role> list = db.Roles.ToList();
                ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
                List<Course> courseList = db.Courses.ToList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                List<Country> countryList = db.Country.ToList();
                ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
                int k = ViewBag.CountryList;
                List<State> stateList = GetStates(k);
               ViewBag.StateList = new SelectList(stateList, "StateId", "StateName");
               List<City> cityList = GetCity(ViewBag.StateList.StateId);
               ViewBag.CityList = new SelectList(cityList, "CityId", "CityName");


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
                   // List<Role> list = db.Roles.ToList();
                    ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
                    List<Course> courseList = db.Courses.ToList();
                    ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                    List<Country> countryList = db.Country.ToList();
                    ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");

                    List<State> stateList = GetStates(ViewBag.CountryList.CountryId);
                    ViewBag.StateList = new SelectList(stateList, "StateId", "StateName");
                    List<City> cityList = GetCity(ViewBag.StateList.StateId);
                    ViewBag.CityList = new SelectList(cityList, "CityId", "CityName");

                    User obj = new User();
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
                    obj.DateCreated = user.DateCreated;
                    obj.DateModified = user.DateModified;

                    obj.RoleId = user.RoleId;
                    obj.CourseId = user.CourseId;
                    obj.Address = user.Address;
                    obj.IsActive = user.IsActive;
                    obj.AddressId = user.AddressId;
                    db.Users.Add(obj);
                    db.SaveChanges();

                   
                    Address address = new Address();
                    address.AddressId = address.AddressId;
                    address.CountryId = address.CountryId;
                    address.StateId = address.StateId;
                    address.CityId = address.CityId;
                    address.AddressLine1 = address.AddressLine1;
                    address.AddressLine2 = address.AddressLine2;
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
        
        public static List<State> GetStates(int countryId)
        {
            using (var db = new UserContext())
            {
                var k = db.States.Where(x => x.CountryId == countryId);
                return k.ToList();
            }
        }
        public static List<City> GetCity(int stateId)
        {
            using (var db = new UserContext())
            {
                var k = db.City.Where(x => x.StateId == stateId);
                return k.ToList();
            }
        }
    }
}