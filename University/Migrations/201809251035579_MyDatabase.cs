namespace University.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.City",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        StateId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("dbo.State", t => t.StateId, cascadeDelete: false)//changes done
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        ZipCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.City", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CountryId)
                .Index(t => t.StateId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        CountryId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: false)//changes done
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Email = c.String(),
                        IsVerified = c.Boolean(nullable: false),
                        Password = c.String(),
                        Hobbies = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Course", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.SubjectInCourse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Course", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Subject", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Subject",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeacherInSubject",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subject", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserInRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserId = c.String(),
                        Role_RoleId = c.Int(),
                        User_UserId = c.Int(),
                        Role_RoleId1 = c.Int(),
                        Role_RoleId2 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.Role_RoleId)
                .ForeignKey("dbo.User", t => t.User_UserId)
                .ForeignKey("dbo.Role", t => t.Role_RoleId1)
                .ForeignKey("dbo.Role", t => t.Role_RoleId2)
                .Index(t => t.Role_RoleId)
                .Index(t => t.User_UserId)
                .Index(t => t.Role_RoleId1)
                .Index(t => t.Role_RoleId2);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.UserInRole", "Role_RoleId2", "dbo.Role");
            DropForeignKey("dbo.UserInRole", "Role_RoleId1", "dbo.Role");
            DropForeignKey("dbo.UserInRole", "User_UserId", "dbo.User");
            DropForeignKey("dbo.UserInRole", "Role_RoleId", "dbo.Role");
            DropForeignKey("dbo.User", "CourseId", "dbo.Course");
            DropForeignKey("dbo.TeacherInSubject", "UserId", "dbo.User");
            DropForeignKey("dbo.TeacherInSubject", "SubjectId", "dbo.Subject");
            DropForeignKey("dbo.SubjectInCourse", "SubjectId", "dbo.Subject");
            DropForeignKey("dbo.SubjectInCourse", "CourseId", "dbo.Course");
            DropForeignKey("dbo.Addresses", "UserId", "dbo.User");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropForeignKey("dbo.City", "StateId", "dbo.State");
            DropForeignKey("dbo.Addresses", "StateId", "dbo.State");
            DropForeignKey("dbo.Addresses", "CountryId", "dbo.Country");
            DropForeignKey("dbo.Addresses", "CityId", "dbo.City");
            DropIndex("dbo.UserInRole", new[] { "Role_RoleId2" });
            DropIndex("dbo.UserInRole", new[] { "Role_RoleId1" });
            DropIndex("dbo.UserInRole", new[] { "User_UserId" });
            DropIndex("dbo.UserInRole", new[] { "Role_RoleId" });
            DropIndex("dbo.TeacherInSubject", new[] { "SubjectId" });
            DropIndex("dbo.TeacherInSubject", new[] { "UserId" });
            DropIndex("dbo.SubjectInCourse", new[] { "CourseId" });
            DropIndex("dbo.SubjectInCourse", new[] { "SubjectId" });
            DropIndex("dbo.User", new[] { "CourseId" });
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.State", new[] { "CountryId" });
            DropIndex("dbo.Addresses", new[] { "CityId" });
            DropIndex("dbo.Addresses", new[] { "StateId" });
            DropIndex("dbo.Addresses", new[] { "CountryId" });
            DropIndex("dbo.Addresses", new[] { "UserId" });
            DropIndex("dbo.City", new[] { "StateId" });
            DropTable("dbo.UserInRole");
            DropTable("dbo.Role");
            DropTable("dbo.TeacherInSubject");
            DropTable("dbo.Subject");
            DropTable("dbo.SubjectInCourse");
            DropTable("dbo.Course");
            DropTable("dbo.User");
            DropTable("dbo.State");
            DropTable("dbo.Country");
            DropTable("dbo.Addresses");
            DropTable("dbo.City");
        }
    }
}
