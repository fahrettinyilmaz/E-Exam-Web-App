using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace EExamWebApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EExamWebApp.Data.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}