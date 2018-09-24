using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table("Course")]
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }
        [Required]
        [MaxLength(255)]
        public string CourseName { get; set; }
        public virtual  ICollection<User> Users { get; set; }
        public virtual ICollection<SubjectInCourse> SubjectInCourses { get; set; }
    }
}