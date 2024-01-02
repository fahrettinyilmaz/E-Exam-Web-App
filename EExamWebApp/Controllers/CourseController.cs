using System;
using System.Linq;
using System.Web.Mvc;
using EExamWebApp.Models;
using EExamWebApp.Data;
using EExamWebApp.Filters;
using EExamWebApp.ViewModels;

namespace EExamWebApp.Controllers
{
    public class CourseController : BaseController
    {
        private AppDbContext db = new AppDbContext();

        // Action to show list of courses (for both Admin and Teacher)
        public ActionResult Index()
        {
            var courses = db.Courses.ToList();
            return View(courses);
        }

        // Action for Admin to create a new course
        [AuthorizeUserType(UserType.Admin)]
        public ActionResult CreateCourse()
        {
            // You might need to pass a list of teachers to the view for selection
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserType(UserType.Admin)]
        public ActionResult CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // Action for Admin to assign a teacher to a course
        [AuthorizeUserType(UserType.Admin)]
        public ActionResult AssignTeacher(int courseId)
        {
            // Get the course and list of teachers for assignment
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserType(UserType.Admin)]
        public ActionResult AssignTeacher(int courseId, int teacherId)
        {
            // Logic to assign the teacher to the course
            return RedirectToAction("Index");
        }
        
        public ActionResult GetAvailableStudents(int courseId)
        {
            // Fetch all students
            var allStudents = db.Users.Where(u => u.UserType == UserType.Student).ToList();

            // Fetch students already enrolled in the selected course
            var enrolledStudents = db.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student)
                .ToList();

            // Get students not enrolled in the course
            var availableStudents = allStudents.Except(enrolledStudents).ToList();

            // Prepare the view model
            var viewModel = new CourseViewModel
            {
                Students = availableStudents.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name + " " + s.LastName
                }).ToList()
            };
            
           

            // Return the partial view with the filtered list of students
            return PartialView("_AvailableStudentsPartial", viewModel);
        }

        // Action for Teacher to add students to a course
        [AuthorizeUserType(UserType.Teacher)]
        public ActionResult AssignStudents(int? selectedCourseId)
        {
            var students = db.Users.Where(u => u.UserType == UserType.Student).ToList();
         
            int currentTeacherId = GetCurrentUserId(); // Implement this method to get current teacher's ID
            var model = new CourseViewModel()
            {
                Courses = db.Courses
                    .Where(c => c.Teacher.Id == currentTeacherId)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Title
                    })
                ,
                
                StudentEmails = db.Users
                    .Where(u => u.UserType == UserType.Student)
                    .Select(s => new { s.Id, s.Email })
                    .ToDictionary(s => s.Id.ToString(), s => s.Email)
            };

            if (selectedCourseId.HasValue)
            {
                model.SelectedCourseId = selectedCourseId.Value;
                model.CurrentStudents = db.Enrollments
                    .Where(e => e.CourseId == selectedCourseId.Value)
                    .Select(e => e.Student)
                    .ToList();
            }

            return View(model);
        }


        public ActionResult GetCurrentStudents(int courseId)
        {
            // Fetch current students for the selected course
            var currentStudents = db.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student);


            return PartialView("_CurrentStudentsPartial", new CourseViewModel()
            {
                CurrentStudents = currentStudents
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserType(UserType.Teacher)]
        public ActionResult AssignStudents(FormCollection form)
        {
            int courseId = int.Parse(form["SelectedCourseId"]);
            string[] studentIdStrings = form.GetValues("SelectedStudentIds");
            int[] studentIds = studentIdStrings?.Select(int.Parse).ToArray();


            // Assuming you have a DbContext 'db'
            var course = db.Courses.FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                // Handle the case where the course is not found
                return HttpNotFound();
            }

            foreach (var studentId in studentIds)
            {
                Console.WriteLine(studentId);
                // Check if the student is already enrolled in the course
                var existingEnrollment =
                    db.Enrollments.FirstOrDefault(e => e.CourseId == courseId && e.Student.Id == studentId);
                if (existingEnrollment == null)
                {
                    var student = db.Users.FirstOrDefault(u => u.Id == studentId);
                    if (student != null)
                    {
                        // Create a new enrollment if the student is not already enrolled
                        var newEnrollment = new Enrollment
                        {
                            Course = course,
                            Student = student,
                            EnrollmentDate = DateTime.Now
                        };
                        db.Enrollments.Add(newEnrollment);
                    }
                }
            }

            // Save the changes to the database
            db.SaveChanges();

            // Redirect to an appropriate action after adding students
            return RedirectToAction("AssignStudents"); // Adjust as needed
        }


        // Other necessary actions for course management...
    }
}