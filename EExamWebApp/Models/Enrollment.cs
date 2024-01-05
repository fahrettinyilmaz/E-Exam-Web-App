using System;

namespace EExamWebApp.Models
{
    public class Enrollment
    {
        public Enrollment()
        {
            EnrollmentDate = DateTime.Now;
        }

        public int Id { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public virtual Course Course { get; set; }
        public virtual User Student { get; set; }

        // New properties
        public double FinalScore { get; set; } // Numeric score
        public string LetterGrade { get; set; } // Letter grade

        // Method to determine letter grade based on final score
        public void CalculateLetterGrade()
        {
            // Revised grading logic for AA to FF scale
            if (FinalScore >= 90) LetterGrade = "AA";
            else if (FinalScore >= 85) LetterGrade = "BA";
            else if (FinalScore >= 80) LetterGrade = "BB";
            else if (FinalScore >= 75) LetterGrade = "CB";
            else if (FinalScore >= 70) LetterGrade = "CC";
            else if (FinalScore >= 60) LetterGrade = "DC";
            else if (FinalScore >= 40) LetterGrade = "DD";
            else LetterGrade = "FF";
        }

    }
}