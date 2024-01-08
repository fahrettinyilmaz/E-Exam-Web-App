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
        
        [HttpPost]
        public ActionResult SubmitExam(ExamViewModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve the relevant exam from the database
            var exam = _db.Exams.Include(e => e.Questions.Select(q => q.Options))
                .FirstOrDefault(e => e.Id == model.ExamId);

            if (exam == null)
            {
                // Handle the case where the exam does not exist
                return HttpNotFound();
            }

            int correctAnswers = 0;

            // Iterate through each question in the submitted model
            foreach (var questionViewModel in model.Questions)
            {
              
                
                    // Check if the selected option is the correct one
                    var selectedOption = _db.Options.FirstOrDefault(o => o.Id == questionViewModel.SelectedOptionId);
                    if (selectedOption != null && selectedOption.IsCorrect)
                    {
                        correctAnswers++;
                    }
                
            }

            // Calculate the score, e.g., as a percentage
            double score = (double)correctAnswers / exam.Questions.Count * 100;

            // Create a new Result object
            var result = new Result
            {
                UserId = GetCurrentUserId(),
                ExamId = model.ExamId,
                Score = score,
                ExamDate = DateTime.Now
            };

            // Save the result to the database
            _db.Results.Add(result);
            _db.SaveChanges();

            // Redirect to a confirmation page, result page, or elsewhere as appropriate
            return RedirectToAction("Index", "Exams", new { courseId = exam.CourseId });
        }

        
        
        
        // GET: Exams
        [Authorize]
        public ActionResult Index(int? courseId)
        {   
            ViewBag.CourseId = courseId;
            ViewBag.UserRole = GetUserRole();
            if (courseId == null)
            {
                return RedirectToAction("Index"); // Redirect as appropriate
            }

            var userId = GetCurrentUserId();
                
            using (var context = new AppDbContext())
            {
                var exams = context.Exams
                    .Where(e => e.CourseId == courseId.Value)
                    .ToList();
        
                var examsWithResults = exams.Select(exam => new ExamWithResultViewModel
                {
                    Exam = exam,
                    Result = context.Results.FirstOrDefault(r => r.ExamId == exam.Id && r.UserId == userId)
                }).ToList();

                var viewModel = new ExamIndexViewModel
                {
                    ExamsWithResults = examsWithResults,
                    CourseId = courseId.Value
                };

                return View(viewModel);
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
                return RedirectToAction("Index" , "Exams", new { courseId = exam.CourseId });
            }

            ViewBag.CourseId = new SelectList(_db.Courses, "Id", "Title", exam.CourseId);
            return RedirectToAction("Index", "Exams", new { courseId = exam.CourseId });

        }

        // GET: Exams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var exam = _db.Exams.Find(id);
            if (exam == null) return HttpNotFound();
            return View(exam);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var exam = _db.Exams.Include(e => e.Questions.Select(q => q.Options))
                .FirstOrDefault(e => e.Id == id);

            if (exam != null)
            {
                // First, remove all related options
                foreach (var question in exam.Questions.ToList())
                {
                    _db.Options.RemoveRange(question.Options);
                }

                // Next, remove the questions themselves
                _db.Questions.RemoveRange(exam.Questions);

                // Finally, remove the exam
                _db.Exams.Remove(exam);

                // Save changes to the database
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "Exams", new { courseId = exam.CourseId });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}