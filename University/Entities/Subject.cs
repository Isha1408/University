using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table(" Subject")]
   
        
        public class Subject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public virtual ICollection< SubjectInCourse> SubjectInCourses { get; set; }
        public virtual ICollection<TeacherInSubject> TeacherInSubjects { get; set; }
        public bool IsActive { get; set; }
    }
}