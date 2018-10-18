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
        /// <summary>
        /// Get Method : To Search User
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchUserRecord()
        {
            SearchViewModel objSearchViewModel = new SearchViewModel();

            List<Country> countriesList = new List<Country>();
            List<Course> courseList = new List<Course>();
            List<Role> roleList = new List<Role>();

            var countryList = db.Country.ToList();
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

            objSearchViewModel.CountryList = countryList;
            objSearchViewModel.CourseList = tempCourseList;
            objSearchViewModel.RoleList = tempRoleList;


            //var courseList = (from x in db.Courses select new Course
            //{
            //    CourseName = x.CourseName,
            //    CourseId = x.CourseId
            //}).ToList();

            return View(objSearchViewModel);
        }
        /// <summary>
        /// To search record from the data base
        /// </summary>
        /// <returns></returns>


        public ActionResult SearchUserRecord(SearchViewModel objSearchViewModel)
        {
            // query to fetch record from multiple tables on the basis of filters.

            var resultedRecord = (from ux in db.Users
                                  join ad in db.Addresses on ux.AddressId equals ad.AddressId
                                  where (ux.FirstName.Equals(objSearchViewModel.FirstName) || ux.FirstName == null) &&
                                   (ux.LastName.Equals(objSearchViewModel.LastName) || ux.LastName == null) &&
                                    (ux.Gender.Equals(objSearchViewModel.Gender) || ux.Gender == null) &&
                                   (ux.Email.Equals(objSearchViewModel.Email) || ux.Email == null) &&
                                      (ux.CourseId.Equals(objSearchViewModel.CourseId) || ux.CourseId == 0) &&
                                         (ux.RoleId.Equals(objSearchViewModel.RoleId) || ux.RoleId == 0) &&
                                    (ad.AddressLine1.Equals(objSearchViewModel.AddressLine1) || ad.AddressLine1 == null) &&
                                    (ad.AddressLine2.Equals(objSearchViewModel.AddressLine2) || ad.AddressLine2 == null) &&
                                      (ad.CountryId.Equals(objSearchViewModel.CountryId) || ad.CountryId == 0) &&
                                       (ux.FirstName.Contains(objSearchViewModel.FirstName) || ux.FirstName == null) &&
                                        (ux.LastName.Contains(objSearchViewModel.LastName) || ux.LastName == null) &&
                                         (ux.Gender.Contains(objSearchViewModel.Gender) || ux.Gender == null) &&
                                         (ux.Email.Contains(objSearchViewModel.Email) || ux.Email == null) &&

                                            (ad.AddressLine1.Contains(objSearchViewModel.AddressLine1) || ad.AddressLine1 == null) &&
                                             (ad.AddressLine2.Contains(objSearchViewModel.AddressLine2) || ad.AddressLine2 == null)


                                  select new
                                  {
                                      ux.FirstName,
                                      ux.LastName,
                                      ux.Gender,
                                      ux.DateOfBirth,
                                      ux.Email,
                                      ad.AddressLine1,
                                      ad.AddressLine2,
                                      ad.CountryId,
                                      ux.CourseId,
                                      ux.RoleId
                                  });

            return View(resultedRecord.ToList());
         }

        public ActionResult SearchResultGrid()

        {
            FilterViewModel objFilterViewModel = new FilterViewModel();

            // Get user roles from DB
            SearchViewModel objViewModel = new SearchViewModel();

            List<Country> countriesList = new List<Country>();
            List<Course> courseList = new List<Course>();
            List<Role> roleList = new List<Role>();

            var countryList = db.Country.ToList();
            var tempCourseList = db.Courses.ToList();
            var tempRoleList = db.Roles.ToList();

            objViewModel.CountryList = countryList;
            objViewModel.CourseList = tempCourseList;
            objViewModel.RoleList = tempRoleList;

            List<SearchViewModel> searchViewModelList = new List<SearchViewModel>();

            //Get List of users from DB
            var userData = (from p in db.Users select p).ToList();
            //to get address
            var Useraddress = (from a in db.Addresses select a);
            foreach (var item in userData)
            {

                var userAddress = (from a in db.Addresses where a.AddressId == item.AddressId select a).FirstOrDefault();
                SearchViewModel objModel = new SearchViewModel
                {

                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Gender = item.Gender,
                    Email = item.Email,
                    DateOfBirth = item.DateOfBirth,
                    IsActive = item.IsActive,
                    AddressLine1 = userAddress.AddressLine1,
                    AddressLine2 = userAddress.AddressLine1,
                    CountryId = userAddress.CountryId,
                    ZipCode = userAddress.ZipCode


                    //   objModel.Add(objSearchViewModel);

                };
                //objFilterViewModel.List = objModel;
                return PartialView(objFilterViewModel);
            }
        }

    }
}
     


