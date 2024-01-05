namespace EExamWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamIdAddedQuestion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Exam_Id", "dbo.Exams");
            DropIndex("dbo.Questions", new[] { "Exam_Id" });
            RenameColumn(table: "dbo.Questions", name: "Exam_Id", newName: "ExamId");
            AlterColumn("dbo.Questions", "ExamId", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "ExamId");
            AddForeignKey("dbo.Questions", "ExamId", "dbo.Exams", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "ExamId", "dbo.Exams");
            DropIndex("dbo.Questions", new[] { "ExamId" });
            AlterColumn("dbo.Questions", "ExamId", c => c.Int());
            RenameColumn(table: "dbo.Questions", name: "ExamId", newName: "Exam_Id");
            CreateIndex("dbo.Questions", "Exam_Id");
            AddForeignKey("dbo.Questions", "Exam_Id", "dbo.Exams", "Id");
        }
    }
}
