namespace EExamWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeacherIdAddedCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.Users");
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            RenameColumn(table: "dbo.Courses", name: "Teacher_Id", newName: "TeacherId");
            AlterColumn("dbo.Courses", "TeacherId", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "TeacherId");
            AddForeignKey("dbo.Courses", "TeacherId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "TeacherId", "dbo.Users");
            DropIndex("dbo.Courses", new[] { "TeacherId" });
            AlterColumn("dbo.Courses", "TeacherId", c => c.Int());
            RenameColumn(table: "dbo.Courses", name: "TeacherId", newName: "Teacher_Id");
            CreateIndex("dbo.Courses", "Teacher_Id");
            AddForeignKey("dbo.Courses", "Teacher_Id", "dbo.Users", "Id");
        }
    }
}
