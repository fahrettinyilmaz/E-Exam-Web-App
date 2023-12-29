namespace EExamWebApp.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; } 
        
        public virtual Question Question { get; set; }
    }

}