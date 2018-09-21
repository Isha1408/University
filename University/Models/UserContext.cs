using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using University.Entities;
using System.Data.Entity;


namespace University.Models
{
    public class UserContext : DbContext
    {
        public UserContext():base("DBContext")
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectInCourse> SubjectInCourses { get; set; }
        public DbSet<TeacherInSubject> TeacherInSubjects { get; set; }

    }
}