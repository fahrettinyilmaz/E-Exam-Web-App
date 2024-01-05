using System.Collections.Generic;

namespace EExamWebApp.ViewModels
{
    public class ExamViewModel
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public List<ExamQuestionViewModel> Questions { get; set; }
    }

    public class ExamQuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<ExamOptionViewModel> Options { get; set; }

        // Added to store the student's selected answer
        public int SelectedOptionId { get; set; }
    }

    public class ExamOptionViewModel
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
    }

}