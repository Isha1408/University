using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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
   



}



