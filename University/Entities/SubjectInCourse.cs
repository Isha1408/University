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
        [Required]
        [Key, Column(Order = 1)]
        public int SubjectId { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        public int CourseId{ get; set; }
    }
}