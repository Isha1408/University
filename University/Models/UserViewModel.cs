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
      
        [Required(ErrorMessage = "FirstName is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Select Gender")]
        public string Gender { get; set; }
        [Required]
        [DisplayName("DOB")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,}",
        ErrorMessage = "Password should be of minimum 6 characters with at least 1 Uppercase Alphabet, 1 Lowercase Alphabet, 1 Number and 1 Special Character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirm your Password")]
        [Required]
        public string ConfirmPassword { get; set; }
        public string Hobbies { get; set; }
        [Required]
        //public int AddressId { get; set; }
        [DisplayName("Temporary Address")]
        public string AddressLine1 { get; set; }
        [Required]
        [DisplayName("Permanant Address")]
        public string AddressLine2 { get; set; }
        [Required]
        [DisplayName("Country")]
      
        public int CountryId { get; set; }
        [DisplayName("State")]
        [Required]
        public int StateId { get; set; }
        [DisplayName("City")]
        [Required]
        public int CityId { get; set; }
        [DisplayName("Role")]
        public int RoleId { get; set; }
        [DisplayName("Course")]
        [Required]
        public int CourseId { get; set; }
        [Required]
        [DisplayName("Zip Code")]
        [RegularExpression(@"^(\d{5,9})$", ErrorMessage = "ZipCode is not valid.")]
        public int ZipCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }
        public List<Course> Course { get; set; }

        public List<Role> RoleList { get; set; }

      


    }
}