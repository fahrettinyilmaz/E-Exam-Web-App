using System.Collections.Generic;

namespace EExamWebApp.ViewModels
{
    public class CreateQuestionViewModel
    {
        public int ExamId { get; set; }
        public List<QuestionInputModel> Questions { get; set; } = new List<QuestionInputModel>();
    }

    public class QuestionInputModel
    {
        public string Text { get; set; }
        public List<OptionInputModel> Options { get; set; } = new List<OptionInputModel>();
    }

    public class OptionInputModel
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}