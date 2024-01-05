using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EExamWebApp.Data;
using EExamWebApp.Filters;
using EExamWebApp.Models;

namespace EExamWebApp.Controllers
{
    //New
    public class ExamsController : BaseController
    {
        private readonly AppDbContext _db = new AppDbContext();

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

        // // POST: Exams/Create
        // // // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // // // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Create([Bind(Include = "Id,Title,Description,CourseId,AvailableFrom,Duration")] Exam exam)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         db.Exams.Add(exam);
        //         db.SaveChanges();
        //         return RedirectToAction("Index");
        //     }
        //
        //     ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", exam.CourseId);
        //     return View(exam);
        // }

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
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
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