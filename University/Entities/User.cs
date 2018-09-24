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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string Password { get; set; }
        //[DataType(DataType.Password)]
        // [Compare("Password", ErrorMessage = "Confirm your Password")]
        //public string ConfirmPassword { get; set; }
        public string Hobbies { get; set; }
        public bool IsActive { get; set; }
      
       
        public int RoleId { get; set; }
        //[ForeignKey("Role Id")]
        public virtual Role Role { get; set; }
      
        public int CourseId { get; set; }
        //[ForeignKey("Course Id")]
        public virtual Course course { get; set; }
        public virtual ICollection< Address> Address { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public virtual ICollection<TeacherInSubject> TeacherInSubjects { get; set; }
        public virtual ICollection<UserInRole> UserInRoles{ get; set; }


    }
}