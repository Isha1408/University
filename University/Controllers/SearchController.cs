using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Entities;
using University.Models;

namespace University.Controllers
{
    public class SearchController : Controller
    {
        UserContext db = new UserContext();

        /// <summary>
        /// To search User record
        /// </summary>
        /// <param name="searching"></param>
        /// <returns></returns>
        public ActionResult Index(string searching)
        {
            var users = from s in db.Users select s;
            if (!String.IsNullOrEmpty(searching))
            {
                users = users.Where(s => s.FirstName.Contains(searching));

            }
            return View(users.ToList());
        }
        [HttpGet]
        /// <summary>
        /// Get Method : To Search User on the basis of filters.
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchUserRecord()
        {

            var model = new FilterViewModel();
           SearchViewModel objSearchViewModel = new SearchViewModel();

            List<Country> countriesList = new List<Country>();
            List<Course> courseList = new List<Course>();
            List<Role> roleList = new List<Role>();

            var tempCountryList = db.Country.ToList();
            var tempCourseList = db.Courses.ToList();
            var tempRoleList = db.Roles.ToList();
            //foreach (var item in cList)
            //{
            //    Country testCountry = new Country
            //    {
            //        CountryId = item.Value.ToString(),
            //        Name = item.Text.ToString()
            //    };
            //    countriesList.Add(testCountry);
            //}

            objSearchViewModel.CountryList = tempCountryList;
            objSearchViewModel.CourseList = tempCourseList;
            objSearchViewModel.RoleList = tempRoleList;

         
            return View(objSearchViewModel);
        }
        /// <summary>
        /// To search record from the data base
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public ActionResult SearchUserRecord(FilterViewModel objFilterViewModel)
        {
               //query to fetch record from multiple tables on the basis of filters.
               
            var resultedRecord = (from ux in db.Users
                               join ad in db.Addresses on ux.AddressId equals ad.AddressId
                                 
                                  
                                  where (ux.FirstName==(objFilterViewModel.FirstName) || objFilterViewModel.FirstName=="") &&
                                   (ux.LastName==(objFilterViewModel.LastName)|| objFilterViewModel.LastName=="") &&
                                    (ux.Gender==(objFilterViewModel.Gender)|| objFilterViewModel.Gender=="") &&
                                   (ux.Email==(objFilterViewModel.Email) || objFilterViewModel.Email == "") &&
                                      (ux.CourseId==(objFilterViewModel.CourseId) || objFilterViewModel.CourseId == 0 ) &&
                                         (ux.RoleId.Equals(objFilterViewModel.RoleId) || objFilterViewModel.RoleId == 0) &&
                                    (ad.AddressLine1==(objFilterViewModel.AddressLine1) || objFilterViewModel.AddressLine1 == "") &&
                                    (ad.AddressLine2==(objFilterViewModel.AddressLine2) || objFilterViewModel.AddressLine2 == "") ||
                                      (ad.CountryId==(objFilterViewModel.CountryId) || objFilterViewModel.CountryId == 0) 


                                  select new SearchViewModel
                                  {
                                      FirstName = ux.FirstName,
                                      LastName = ux.LastName,
                                      Gender = ux.Gender,
                                      DateOfBirth = ux.DateOfBirth,

                                      Email = ux.Email,
                                      AddressLine1 = ad.AddressLine1,
                                      AddressLine2 = ad.AddressLine2,
                                      CountryId = ad.CountryId,
                                      IsActive = ux.IsActive,
                                      CourseId = ux.CourseId,
                                      RoleId = ux.RoleId,

                                  }).ToList();


            objFilterViewModel.SearchList = resultedRecord;
            return View(objFilterViewModel);
        }

        public ActionResult _SearchResultGrid()

        {
            var userList = db.Users.ToList();
            return View(userList);
            //FilterViewModel objFilterViewModel = new FilterViewModel();

            //// Get user roles from DB
            //SearchViewModel objViewModel = new SearchViewModel();

            //List<Country> countriesList = new List<Country>();
            //List<Course> courseList = new List<Course>();
            //List<Role> roleList = new List<Role>();

            //var countryList = db.Country.ToList();
            //var tempCourseList = db.Courses.ToList();
            //var tempRoleList = db.Roles.ToList();

            //objViewModel.CountryList = countryList;
            //objViewModel.CourseList = tempCourseList;
            //objViewModel.RoleList = tempRoleList;

            //List<SearchViewModel> searchViewModelList = new List<SearchViewModel>();

            ////Get List of users from DB
            //var userData = (from p in db.Users select p).ToList();
            ////  to get address
            //var Useraddress = (from a in db.Addresses select a);
            //foreach (var item in userData)
            //{

            //    var userAddress = (from a in db.Addresses where a.AddressId == item.AddressId select a).FirstOrDefault();
            //    SearchViewModel objModel = new SearchViewModel
            //    {

            //        FirstName = item.FirstName,
            //        LastName = item.LastName,
            //        Gender = item.Gender,
            //        Email = item.Email,
            //        DateOfBirth = item.DateOfBirth,
            //        IsActive = item.IsActive,
            //        AddressLine1 = userAddress.AddressLine1,
            //        AddressLine2 = userAddress.AddressLine1,
            //        CountryId = userAddress.CountryId,
            //        ZipCode = userAddress.ZipCode
            //    };
            //    searchViewModelList.Add(objModel);
            //}

            //objFilterViewModel.SearchList = searchViewModelList;
            //return PartialView(objFilterViewModel);
            
        }

    }
}
     


