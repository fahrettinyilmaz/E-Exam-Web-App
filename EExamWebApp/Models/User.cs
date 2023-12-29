using System.Collections.Generic;

namespace EExamWebApp.Models
{
    public enum UserType
    {
        Admin,
        Teacher,
        Student
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public string Email { get; set; }
        public UserType UserType { get; set; } 
        public bool IsApproved { get; set; } = false;
        
        public virtual ICollection<Course> Courses { get; set; }
        
        public virtual ICollection<Enrollment> Enrollments { get; set; }

    }
}