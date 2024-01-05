using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EExamWebApp.Data;
using EExamWebApp.Filters;
using EExamWebApp.Models;

namespace EExamWebApp.Controllers
{
    public class CoursesController : BaseController
    {
        private readonly AppDbContext _db = new AppDbContext();

        [Authorize]
        public ActionResult Exams(int? courseId)
        {
            ViewBag.UserRole = GetUserRole();
            if (courseId == null)
                // Handle the missing parameter case, e.g., redirect to another page or show an error message
                return RedirectToAction("Index"); // Example redirection


            var exams = _db.Exams.Where(e => e.CourseId == courseId.Value).ToList();
            ViewBag.CourseId = courseId.Value;
            return View(exams);
        }

        // GET: Courses
        [Authorize]
        public ActionResult Index()
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


        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var course = _db.Courses.Find(id);
            if (course == null) return HttpNotFound();
            return View(course);
        }

        // GET: Courses/Create
        [AuthorizeUserType(UserType.Admin)]
        public ActionResult Create()
        {
            var teachers = _db.Users
                .Where(u => u.UserType == UserType.Teacher)
                .Select(u => new
                {
                    u.Id,
                    FullName = u.Name + " " + u.LastName
                })
                .ToList();

            ViewBag.Teachers = new SelectList(teachers, "Id", "FullName");
            return View();
        }

        // POST: Courses/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,TeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Add(course);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeacherId = new SelectList(_db.Users, "Id", "Username", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var course = _db.Courses.Find(id);
            if (course == null) return HttpNotFound();
            ViewBag.TeacherId = new SelectList(_db.Users, "Id", "Username", course.TeacherId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,TeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(course).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeacherId = new SelectList(_db.Users, "Id", "Username", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var course = _db.Courses.Find(id);
            if (course == null) return HttpNotFound();
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var course = _db.Courses.Find(id);
            _db.Courses.Remove(course);
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