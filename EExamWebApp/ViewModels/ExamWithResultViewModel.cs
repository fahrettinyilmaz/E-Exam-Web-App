using System.Collections.Generic;
using EExamWebApp.Models;

namespace EExamWebApp.ViewModels
{
    public class ExamWithResultViewModel
    {
        public Exam Exam { get; set; }
        public Result Result { get; set; } // This will be null if no result exists
    }

    public class ExamIndexViewModel
    {
        public List<ExamWithResultViewModel> ExamsWithResults { get; set; }
        public int CourseId { get; set; }
    }

}