namespace EExamWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamDateDuration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "AvailableFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.Exams", "Duration", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exams", "Duration");
            DropColumn("dbo.Exams", "AvailableFrom");
        }
    }
}
