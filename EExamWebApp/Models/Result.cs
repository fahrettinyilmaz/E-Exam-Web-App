using System;

namespace EExamWebApp.Models
{
    public class Result
    {
        public int Id { get; set; }

        public int UserId { get; set; } // Reference to the student
        public virtual User Student { get; set; }

        public int ExamId { get; set; } // Reference to the exam
        public virtual Exam Exam { get; set; }

        public double Score { get; set; } // Store the score or percentage
        public DateTime ExamDate { get; set; } // The date when the exam was taken

        // You can add more properties here as needed
        // For example, a property for storing whether the exam was passed or not
    }
}