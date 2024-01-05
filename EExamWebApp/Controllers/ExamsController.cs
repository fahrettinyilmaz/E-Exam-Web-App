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
    //New
    public class ExamsController : BaseController
    {
        private readonly AppDbContext _db = new AppDbContext();
    
        [AuthorizeUserType(UserType.Student)]
        public ActionResult Take(int id)
        {
            // Retrieve the exam by id from your database or service
            var exam = _db.Exams.Where( e => e.Id == id).Include(e => e.Questions.Select(q => q.Options)).FirstOrDefault();
            if (exam == null)
            {
                return HttpNotFound();
            }

            // Transform the exam data into the ExamViewModel
            var model = new ExamViewModel
            {
                ExamId = exam.Id,
                Title = exam.Title,
                Questions = exam.Questions.Select(q => new ExamQuestionViewModel
                {
                    QuestionId = q.Id,
                    QuestionText = q.Text,
                    Options = q.Options.Select(o => new ExamOptionViewModel
                    {
                        OptionId = o.Id,
                        OptionText = o.Text
                    }).ToList()
                }).ToList()
            };

            // Pass the model to the view
            return View(model);
        }
        public ActionResult SubmitExam  (int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var exam = _db.Exams.Find(id);
            if (exam == null) return HttpNotFound();
            return RedirectToAction("Index");
        }
        
        
        
        // GET: Exams
        [Authorize]
        public ActionResult Index(int? courseId)
        {
            ViewBag.UserRole = GetUserRole();
            if (courseId == null)
                // Handle the missing parameter case, e.g., redirect to another page or show an error message
                return RedirectToAction("Index"); // Example redirection

            using (var context = new AppDbContext())
            {
                var exams = context.Exams.Where(e => e.CourseId == courseId.Value).ToList();
                ViewBag.CourseId = courseId.Value;
                return View(exams);
            }
        }

        [AuthorizeUserType(UserType.Teacher)]
        // GET: Exams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var exam = _db.Exams.Find(id);
            if (exam == null) return HttpNotFound();
            return View(exam);
        }

        [AuthorizeUserType(UserType.Teacher)]
        public ActionResult Create(int courseId)
        {
            ViewBag.CourseId = courseId; // Pass courseId to the view
            return View();
        }

        // POST: Exam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserType(UserType.Teacher)]
        public ActionResult Create(int courseId, Exam exam)
        {
            if (ModelState.IsValid)
                using (var context = new AppDbContext())
                {
                    exam.CourseId = courseId; // Assign courseId to the exam
                    context.Exams.Add(exam);
                    context.SaveChanges();
                    return RedirectToAction("Index", new { courseId });
                }

            ViewBag.CourseId = courseId; // Pass courseId back to the view in case of error
            return View(exam);
        }

        
        // GET: Exams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var exam = _db.Exams.Find(id);
            if (exam == null) return HttpNotFound();
            ViewBag.CourseId = new SelectList(_db.Courses, "Id", "Title", exam.CourseId);
            return View(exam);
        }

        // POST: Exams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,CourseId,AvailableFrom,Duration")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(exam).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(_db.Courses, "Id", "Title", exam.CourseId);
            return View(exam);
        }

        // GET: Exams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var exam = _db.Exams.Find(id);
            if (exam == null) return HttpNotFound();
            return View(exam);
        }

        // POST: Exams/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var exam = _db.Exams.Find(id);
            _db.Exams.Remove(exam);
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