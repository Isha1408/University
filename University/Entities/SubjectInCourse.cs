using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table(" SubjectInCourse")]
    public class SubjectInCourse
    {
      
        [Key]
    
        public int Id { get; set; }

        [ForeignKey("Subject Id")]
        public int SubjectId { get; set; }

        [ForeignKey("Course Id")]
        public int CourseId { get; set; }

    public ICollection<Course> Courses { get; set; }
    public ICollection<Subject> Subjects { get; set; }
    }
}