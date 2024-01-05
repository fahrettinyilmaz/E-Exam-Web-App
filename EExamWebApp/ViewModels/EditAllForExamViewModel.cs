using System.Collections.Generic;

namespace EExamWebApp.ViewModels
{
    public class EditAllForExamViewModel
    {
        public int ExamId { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}