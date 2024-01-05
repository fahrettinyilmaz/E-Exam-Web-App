using System.Data.Entity;
using EExamWebApp.Models;

namespace EExamWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=mssql")
        {
        }

        // DbSets for each of your entities
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        
        public DbSet<Result> Results { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}