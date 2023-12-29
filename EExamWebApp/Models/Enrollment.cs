using System;

namespace EExamWebApp.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; } 
        public DateTime EnrollmentDate { get; set; } 

        public virtual Course Course { get; set; }
        public virtual User Student { get; set; }
        
        public Enrollment()
        {
            EnrollmentDate = DateTime.Now;
        }
    }


}