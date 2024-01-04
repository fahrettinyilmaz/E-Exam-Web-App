using System;
using System.Collections.Generic;

namespace EExamWebApp.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public DateTime AvailableFrom { get; set; }

        // Add a TimeSpan for Duration
        public TimeSpan Duration { get; set; }
    }
}