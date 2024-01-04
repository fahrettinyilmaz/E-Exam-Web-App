namespace EExamWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseIdAdded2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Exams", "Course_Id", "dbo.Courses");
            DropIndex("dbo.Exams", new[] { "Course_Id" });
            RenameColumn(table: "dbo.Exams", name: "Course_Id", newName: "CourseId");
            AlterColumn("dbo.Exams", "CourseId", c => c.Int(nullable: false));
            CreateIndex("dbo.Exams", "CourseId");
            AddForeignKey("dbo.Exams", "CourseId", "dbo.Courses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exams", "CourseId", "dbo.Courses");
            DropIndex("dbo.Exams", new[] { "CourseId" });
            AlterColumn("dbo.Exams", "CourseId", c => c.Int());
            RenameColumn(table: "dbo.Exams", name: "CourseId", newName: "Course_Id");
            CreateIndex("dbo.Exams", "Course_Id");
            AddForeignKey("dbo.Exams", "Course_Id", "dbo.Courses", "Id");
        }
    }
}
