using System.Collections.Generic;

namespace EExamWebApp.Models
{
    public class Course
    { 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual User Teacher { get; set; }
    }
}