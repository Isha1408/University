using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Entities;

namespace University.Models
{
    public class SearchViewModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Gender { get; set; }
        [DisplayName("DOB")]
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }

        [DisplayName("Temporary Address")]
        public string AddressLine1 { get; set; }
        [DisplayName("Permanant Address")]
        public string AddressLine2 { get; set; }
        [DisplayName("Country")]
        public int CountryId { get; set; }
        public List<Country> CountryList { get; set; }
       

        [DisplayName("State")]
        public int StateId { get; set; }

        public List<State> StateList { get; set; }

        [DisplayName("City")]
        public int CityId { get; set; }
        public List<City> CityList { get; set; }
        [DisplayName("Role")]
        public int RoleId { get; set; }
       public List<Role> RoleList { get; set; }
        [DisplayName("Course")]
        public int CourseId { get; set; }
        public List<Course> CourseList { get; set; }
        [DisplayName("Zip Code")]
        public int ZipCode { get; set; }

        public bool IsActive { get; set; }

    }
    public class FilterViewModel
    {
        public List<SearchViewModel> SearchList { get; set; }
        [Display(Name = "First Name")]

        public string FirstName { get; set; }
        [Display(Name = "Last Name")]


        public string LastName { get; set; }
        public int RoleId { get; set; }
        public List<Role> RoleList { get; set; }
        public int CourseId { get; set; }
        public List<Course> CourseList { get; set; }
        public string Gender { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString =
        //        "{0:yyyy-MM-dd}",
        //  ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
     

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Temporary Address")]
        public string AddressLine1 { get; set; }
        [Display(Name = " Permanant Address")]
        public string AddressLine2 { get; set; }
        [Display(Name = "Country")]

        public int CountryId { get; set; }
        public string Country { get; set; }
        public List<Country> CountryList { get; set; }
      
        public int ZipCode { get; set; }
        public bool IsActive { get; set; }

    }




}



