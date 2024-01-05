namespace EExamWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedResultAndEnrollmentFinalScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Enrollments", "FinalScore", c => c.Double(nullable: false));
            AddColumn("dbo.Enrollments", "LetterGrade", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Enrollments", "LetterGrade");
            DropColumn("dbo.Enrollments", "FinalScore");
        }
    }
}
