using System.Collections.Generic;

namespace EExamWebApp.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}