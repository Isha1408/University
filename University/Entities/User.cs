using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table(" User")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        [DisplayName("Last Name")]
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
        [Compare("Password", ErrorMessage = "Confirm your Password")]
        public string ConfirmPassword { get; set; }
        public string Hobbies { get; set; }
        public bool IsActive { get; set; }
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
       public virtual Address Address { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
      
       public int CourseId { get; set; }
       [ForeignKey("CourseId")]
        public virtual Course course { get; set; }
       
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public virtual ICollection<TeacherInSubject> TeacherInSubjects { get; set; }
        public virtual ICollection<UserInRole> UserInRoles{ get; set; }
        

    }
}