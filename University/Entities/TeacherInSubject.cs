using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University.Entities
{
    [Table(" TeacherInSubject")]
    public class TeacherInSubject
    {
      
        [Key]
        public int Id { get; set; }
        //[ForeignKey("User Id")]

        public int UserId { get; set; }

       // [ForeignKey("Subject Id")]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual User User { get; set; }
    }
}