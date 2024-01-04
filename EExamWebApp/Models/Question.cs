using System.Collections.Generic;

namespace EExamWebApp.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public virtual ICollection<Option> Options { get; set; } // Collection of Options

        public virtual Exam Exam { get; set; }
    }
}