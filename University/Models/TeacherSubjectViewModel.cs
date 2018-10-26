using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using University.Entities;

namespace University.Models
{
    public class TeacherSubjectViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        

        public int SubjectId { get; set; }

        public string Subject { get; set; }
        public List<TeacherInSubject> TeacherSubjectList { get; set; }
    }
}