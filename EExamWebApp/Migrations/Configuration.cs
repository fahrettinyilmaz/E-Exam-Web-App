using System.Data.Entity.Migrations;
using EExamWebApp.Data;

namespace EExamWebApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}