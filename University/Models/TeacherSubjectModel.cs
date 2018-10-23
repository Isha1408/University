using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University.Models
{
    public class TeacherSubjectModel
    {
        public int SubjectId { get; set; }
        public int CourseId { get; set; }

        public int UserId { get; set; }
    }
}