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
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        
        public virtual Subject Subject { get; set; }

       
       public int CourseId { get; set; }
       [ForeignKey("CourseId")]

       public virtual Course Course { get; set; }

    }
}