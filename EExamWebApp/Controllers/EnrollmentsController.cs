using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EExamWebApp.Data;
using EExamWebApp.Filters;
using EExamWebApp.Models;
using EExamWebApp.ViewModels;

namespace EExamWebApp.Controllers
{
    public class EnrollmentsController : BaseController
    {
        private readonly AppDbContext _db = new AppDbContext();


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

        // GET: Enrollments
        public ActionResult Index()
        {
            var enrollments = _db.Enrollments.Include(e => e.Course);
            return View(enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var enrollment = _db.Enrollments.Find(id);
            if (enrollment == null) return HttpNotFound();
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_db.Courses, "Id", "Title");
            return View();
        }

        // POST: Enrollments/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseId,EnrollmentDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _db.Enrollments.Add(enrollment);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(_db.Courses, "Id", "Title", enrollment.CourseId);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var enrollment = _db.Enrollments.Find(id);
            if (enrollment == null) return HttpNotFound();
            ViewBag.CourseId = new SelectList(_db.Courses, "Id", "Title", enrollment.CourseId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseId,EnrollmentDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(enrollment).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(_db.Courses, "Id", "Title", enrollment.CourseId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var enrollment = _db.Enrollments.Find(id);
            if (enrollment == null) return HttpNotFound();
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var enrollment = _db.Enrollments.Find(id);
            _db.Enrollments.Remove(enrollment);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}