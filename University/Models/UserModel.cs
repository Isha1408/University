using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Entities;

namespace University.Models
{
    public class UserModel
    {
        
        public int UserId { get; set; }
        //[Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
      //  [Required(ErrorMessage = "Please Select Gender")]
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
       // public bool IsVerified { get; set; }
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,}",
      //  ErrorMessage = "Password should be of minimum 6 characters with at least 1 Uppercase Alphabet, 1 Lowercase Alphabet, 1 Number and 1 Special Character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirm your Password")]
        public string ConfirmPassword { get; set; }
        public bool IsVerified { get; set; }
        public string Hobbies { get; set; }
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int CountryId { get; set; }
        //public IEnumerable<SelectListItem>Country { get; set; }
        public int StateId { get; set; }
        //public IEnumerable<SelectListItem> State { get; set; }
        public int CityId { get; set; }
        //public IEnumerable<SelectListItem> City { get; set; }

        public int RoleId { get; set; }
        //public IEnumerable<SelectListItem> Role { get; set; }


        public int CourseId { get; set; }
        public int ZipCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }







    }
}