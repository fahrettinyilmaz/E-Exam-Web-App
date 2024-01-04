using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EExamWebApp.Data;
using EExamWebApp.Filters;
using EExamWebApp.Models;
using EExamWebApp.ViewModels;

namespace EExamWebApp.Controllers
{
    public class CourseController : BaseController
    {
        private readonly AppDbContext _db = new AppDbContext();
        
        public ActionResult Exams(int? courseId)
        {
            ViewBag.UserRole = GetUserRole();
            if (courseId == null)
            {
                // Handle the missing parameter case, e.g., redirect to another page or show an error message
                return RedirectToAction("Index"); // Example redirection
            }

            using (var context = new AppDbContext())
            {
                var exams = context.Exams.Where(e => e.CourseId == courseId.Value).ToList();
                ViewBag.CourseId = courseId.Value;
                return View(exams);
            }
        }
        
        

       

        public ActionResult Index()
        {
            return View();
        }
        // Action for Admin to create a new course
        [AuthorizeUserType(UserType.Admin)] // Custom authorization attribute
        public ActionResult CreateCourse()
        {
            var teachers = _db.Users
                .Where(u => u.UserType == UserType.Teacher)
                .Select(u => new 
                { 
                    Id = u.Id, 
                    FullName = u.Name + " " + u.LastName 
                })
                .ToList();

            ViewBag.Teachers = new SelectList(teachers, "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserType(UserType.Admin)]
        public ActionResult CreateCourse(Course course)
        {
            if (!ModelState.IsValid) return View(course);
            _db.Courses.Add(course);
            _db.SaveChanges();
            return RedirectToAction("Index");
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
            var allStudents = _db.Users.Where(u => u.UserType == UserType.Student).ToList();

            // Fetch students already enrolled in the selected course
            var enrolledStudents = _db.Enrollments
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
                    Text = s.Name + @" " + s.LastName
                }).ToList()
            };


            // Return the partial view with the filtered list of students
            return PartialView("_AvailableStudentsPartial", viewModel);
        }

        [Authorize]
        public ActionResult ListCourses()
        {
            
            var userRole = GetUserRole();
            var courses = new List<Course>();
            ViewBag.UserRole = userRole;
           
            // Pass this dictionary to the view
            

            switch (userRole)
            {
                case "Admin":
                    courses = _db.Courses.ToList();
                    break;

                case "Teacher":
                    var teacherId = GetCurrentUserId();
                    courses = _db.Courses.Where(c => c.Teacher.Id == teacherId).ToList();
                    break;

                case "Student":
                    var studentId = GetCurrentUserId();
                    courses = _db.Enrollments.Where(e => e.Student.Id == studentId)
                        .Select(e => e.Course)
                        .Distinct()
                        .ToList();
                    break;
            }


            return View(courses);
        }


        // Action for Teacher to add students to a course
        [AuthorizeUserType(UserType.Teacher)]
        public ActionResult AssignStudents(int? selectedCourseId)
        {
            var students = _db.Users.Where(u => u.UserType == UserType.Student).ToList();

            var currentTeacherId = GetCurrentUserId(); // Implement this method to get current teacher's ID
            var model = new CourseViewModel
            {
                Courses = _db.Courses
                    .Where(c => c.Teacher.Id == currentTeacherId)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Title
                    }),

                StudentEmails = _db.Users
                    .Where(u => u.UserType == UserType.Student)
                    .Select(s => new { s.Id, s.Email })
                    .ToDictionary(s => s.Id.ToString(), s => s.Email)
            };

            if (selectedCourseId.HasValue)
            {
                model.SelectedCourseId = selectedCourseId.Value;
                model.CurrentStudents = _db.Enrollments
                    .Where(e => e.CourseId == selectedCourseId.Value)
                    .Select(e => e.Student)
                    .ToList();
            }

            return View(model);
        }


        public ActionResult GetCurrentStudents(int courseId)
        {
            // Fetch current students for the selected course
            var currentStudents = _db.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student);


            return PartialView("_CurrentStudentsPartial", new CourseViewModel
            {
                CurrentStudents = currentStudents
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserType(UserType.Teacher)]
        public ActionResult AssignStudents(FormCollection form)
        {
            var courseId = int.Parse(form["SelectedCourseId"]);
            var studentIdStrings = form.GetValues("SelectedStudentIds");
            var studentIds = studentIdStrings?.Select(int.Parse).ToArray();


            // Assuming you have a DbContext 'db'
            var course = _db.Courses.FirstOrDefault(c => c.Id == courseId);
            if (course == null)
                // Handle the case where the course is not found
                return HttpNotFound();

            foreach (var studentId in studentIds)
            {
                Console.WriteLine(studentId);
                // Check if the student is already enrolled in the course
                var existingEnrollment =
                    _db.Enrollments.FirstOrDefault(e => e.CourseId == courseId && e.Student.Id == studentId);
                if (existingEnrollment == null)
                {
                    var student = _db.Users.FirstOrDefault(u => u.Id == studentId);
                    if (student != null)
                    {
                        // Create a new enrollment if the student is not already enrolled
                        var newEnrollment = new Enrollment
                        {
                            Course = course,
                            Student = student,
                            EnrollmentDate = DateTime.Now
                        };
                        _db.Enrollments.Add(newEnrollment);
                    }
                }
            }

            // Save the changes to the database
            _db.SaveChanges();

            // Redirect to an appropriate action after adding students
            return RedirectToAction("AssignStudents"); // Adjust as needed
        }


        // Other necessary actions for course management...
    }
}