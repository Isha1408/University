using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using University.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace University.Models
{
    public class UserContext : DbContext
    {
        public UserContext():base("DBContext")
        { }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //   // modelBuilder.Entity<State>().** HasOptional **(x => x.Country).WithMany(e => e.States);
        //    throw new UnintentionalCodeFirstException();
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<State>()
                .HasRequired<Country>(s => s.Id)
                .WithMany(g => g.States)
                .HasForeignKey<int>(s => s.Id);
        }
    
    public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserInRole> UserInRoles { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual  DbSet<State> States { get; set; }
        public virtual  DbSet<City> City { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectInCourse> SubjectInCourses { get; set; }
        public virtual DbSet<TeacherInSubject> TeacherInSubjects { get; set; }

    }
}