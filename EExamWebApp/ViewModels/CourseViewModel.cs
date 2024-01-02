using System.Collections.Generic;
using System.Web.Mvc;
using EExamWebApp.Models;

namespace EExamWebApp.ViewModels
{
    public class CourseViewModel
    {
        public int SelectedCourseId { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }
        public int[] SelectedStudentIds { get; set; }
        public Dictionary<string, string> StudentEmails { get; set; }
        public IEnumerable<SelectListItem> Students { get; set; }
        public IEnumerable<User> CurrentStudents { get; set; }
    }
}