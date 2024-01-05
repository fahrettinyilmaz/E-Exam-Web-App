using System.Collections.Generic;

namespace EExamWebApp.ViewModels
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public List<OptionViewModel> Options { get; set; }
    }
}