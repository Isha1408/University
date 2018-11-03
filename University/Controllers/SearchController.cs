//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using University.Models;

//namespace University.Controllers
//{
//    public class SearchController : Controller
//    {
//        UserContext objEntities = new UserContext();
//        /// <summary>
//        /// Partial search view grid
//        /// </summary>
//        /// <returns></returns>
//        public ActionResult _SearchGridPartial()
//        {

//            return View();
//        }
//        /// <summary>
//        /// GET data from db
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public ActionResult SearchView()
//        {
//            try
//            {
//                var roles = (from b in objEntities.Roles select b).ToList();
//                var model = new FilterViewModel();
//                model.Role = roles.Select(x => new SelectListItem
//                {
//                   // Value = x.RoleId,
//                    Text = x.RoleName
//                }).ToList();

//                //GET : COURSE FOR USERS
//                var course = (from b in objEntities.Courses select b).ToList();
//                model.Course = course.Select(x => new SelectListItem
//                {
//                    Value = x.CourseId.ToString(),
//                    Text = x.CourseName
//                });

//                //TO GET COUNTRY ,STATES AND CITY
//                var country = objEntities.Country.ToList();
//                List<SelectListItem> listCountry = new List<SelectListItem>();
//                List<SelectListItem> listState = new List<SelectListItem>();
//                List<SelectListItem> listCity = new List<SelectListItem>();

//                listCountry.Add(new SelectListItem { Text = "", Value = "0" });
//                listState.Add(new SelectListItem { Text = "", Value = "0" });
//                listCity.Add(new SelectListItem { Text = "", Value = "0" });

//                foreach (var m in country)
//                {
//                    listCountry.Add(new SelectListItem { Text = m.Name, Value = m.CountryId.ToString() });
//                }
//                ViewBag.country = listCountry;
//                ViewBag.State = listState;
//                ViewBag.City = listCity;

//                List<SearchViewModel> objSearchViewModel = new List<SearchViewModel>();

//                //Get List of users from DB
//                var data = (from p in objEntities.Users select p).ToList();
//                foreach (var item in data)
//                {
//                    //To get Address From DB
//                    //var userAddressInfo = (from p in objEntities.Addresses
//                    //                       where p.UserId == item.UserId
//                    //                       select p).FirstOrDefault();

//                    var getCourse = (from c in objEntities.Courses
//                                     where c.CourseId == item.CourseId
//                                     select c).FirstOrDefault();

//                    //var getRole = (from role in objEntities.Roles
//                    //               join user in objEntities.UserInRoles
//                    //               on role.RoleId equals user.RoleId
//                    //               where user.UserId == item.UserId
//                    //               select role.RoleName).FirstOrDefault();


//        //            if (userAddressInfo != null)

//        //            {
//        //                SearchViewModel searchView = new SearchViewModel
//        //                {

//        //                    FirstName = item.FirstName,
//        //                    LastName = item.LastName,
//        //                    Gender = item.Gender,
//        //                    Email = item.Email,
//        //                    CourseId = item.CourseId,
//        //                    RoleName = getRole,
//        //                    DOB = item.DOB,
//        //                    IsActive = item.IsActive,
//        //                    CurrentAddress = userAddressInfo.CurrentAddress,
//        //                    PermanantAddress = userAddressInfo.PermanantAddress,
//        //                    Country = userAddressInfo.Countries.CountryName,
//        //                    States = userAddressInfo.States.StateName,
//        //                    Cities = userAddressInfo.Cities.CityName,
//        //                    Pincode = userAddressInfo.Pincode,
//        //                    IsVerified = item.IsVerified,

//        //                    DateCreated = item.DateCreated
//        //                };
//        //                objSearchViewModel.Add(searchView);
//        //            }
//        //            model.List = objSearchViewModel;
//        //        };
//        //        return View(model);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return View(ex);
//        //    }
//        //}
//        /// <summary>
//        /// Search Page 
//        /// </summary>
//        /// <param name="objFilterViewModel"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult SearchView(FilterViewModel objFilterViewModel)
//        {
//            //TO GET ROLES FROM DATABASE
//            var roles = (from b in objEntities.Roles select b).ToList();
//            var model = new FilterViewModel
//            {
//                Role = roles.Select(x => new SelectListItem
//                {
//                   // Value = x.RoleId,
//                    Text = x.RoleName
//                }).ToList()
//            };

//            //GET : COURSE FOR USERS
//            var course = (from b in objEntities.Courses select b).ToList();
//            model.Course = course.Select(x => new SelectListItem
//            {
//                Value = x.CourseId.ToString(),
//                Text = x.CourseName
//            });

//            //TO GET COUNTRY ,STATES AND CITY
//            var country = objEntities.Country.ToList();
//            List<SelectListItem> listCountry = new List<SelectListItem>();
//            List<SelectListItem> listState = new List<SelectListItem>();
//            List<SelectListItem> listCity = new List<SelectListItem>();
//            foreach (var m in country)
//            {
//                listCountry.Add(new SelectListItem { Text = m.Name, Value = m.CountryId.ToString() });
//            }
//            ViewBag.country = listCountry;
//            ViewBag.State = listState;
//            ViewBag.City = listCity;

//            //to compare filters' data in database.
//            var searchBar = (from
//                                  user in objEntities.Users
//                             join userRole in objEntities.UserInRoles on user.UserId equals userRole.UserId
//                             join Address in objEntities.Addresses on user.AddressId equals Address.AddressId
//                             where user.FirstName == objFilterViewModel.FirstName || string.IsNullOrEmpty(objFilterViewModel.FirstName)
//                             where user.LastName == objFilterViewModel.LastName || string.IsNullOrEmpty(objFilterViewModel.LastName)
//                             where user.Gender == objFilterViewModel.Gender || string.IsNullOrEmpty(objFilterViewModel.Gender)
//                             where user.Email == objFilterViewModel.Email || string.IsNullOrEmpty(objFilterViewModel.Email)
//                             where user.DateOfBirth == objFilterViewModel.DOB || objFilterViewModel.DOB == null
//                             where user.IsActive == objFilterViewModel.IsActive
//                             where user.IsVerified == objFilterViewModel.IsVerified
//                             where user.CourseId == objFilterViewModel.CourseId || objFilterViewModel.CourseId == null
//                             where Address.AddressLine1 == objFilterViewModel.CurrentAddress || string.IsNullOrEmpty(objFilterViewModel.CurrentAddress)
//                             where Address.AddressLine2 == objFilterViewModel.PermanantAddress || string.IsNullOrEmpty(objFilterViewModel.PermanantAddress)
//                             where Address.CountryId == objFilterViewModel.CountryId || string.IsNullOrEmpty(objFilterViewModel.Country)
//                             where Address.StateId == objFilterViewModel.StateId || string.IsNullOrEmpty(objFilterViewModel.States)
//                             where Address.CityId == objFilterViewModel.CityId || string.IsNullOrEmpty(objFilterViewModel.Cities)
//                             //where Address.ZipCode == model.Pincode || model.Pincode == null
//                            // where userRole.RoleId == objFilterViewModel.RoleId || string.IsNullOrEmpty(objFilterViewModel.RoleId)

//                             select new SearchViewModel
//                             {
//                                 FirstName = user.FirstName,
//                                 LastName = user.LastName,
//                                 Gender = user.Gender,
//                                 DOB = user.DOB,
//                                 Email = user.Email,
//                                 IsVerified = user.IsVerified,
//                                 IsActive = user.IsActive,
//                                 CourseName = user.Courses.CourseName,
//                                 AddressLine1 = Address.CurrentAddress,
//                                 AddressLine2 = Address.PermanantAddress,
//                                 Country = Address.Countries.CountryName,
//                                 States = Address.States.StateName,
//                                 Cities = Address.Cities.CityName,
//                                 Pincode = Address.Pincode,
//                                 DateCreated = user.DateCreated,
//                                 RoleName = userRole.NetRoles.RoleName
//                             }).ToList();

//            model.List = searchBar;
//            return View(model);
//        }
        
//    }
//}