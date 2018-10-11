using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Entities;

namespace University.Models
{
    // This model contains all the properties required for User Registration form.
    public class UserViewModel
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
       [Required(ErrorMessage = "Please Select Gender")]
        public string Gender { get; set; }
        [DisplayName("DOB")]
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
       //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,}",
      //ErrorMessage = "Password should be of minimum 6 characters with at least 1 Uppercase Alphabet, 1 Lowercase Alphabet, 1 Number and 1 Special Character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirm your Password")]
        public string ConfirmPassword { get; set; }
        public string Hobbies { get; set; }
        public int AddressId { get; set; }
        [DisplayName("Temporary Address")]
        public string AddressLine1 { get; set; }
        [DisplayName("Permanant Address")]
        public string AddressLine2 { get; set; }
        [DisplayName("Country")]
        public int CountryId { get; set; }
        [DisplayName("State")]
        public int StateId { get; set; }
        [DisplayName("City")]
        public int CityId { get; set; }
        [DisplayName("Role")]
        public int RoleId { get; set; }
        [DisplayName("Course")]
        public int CourseId { get; set; }
        [DisplayName("Zip Code")]
        public int ZipCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }







    }
}