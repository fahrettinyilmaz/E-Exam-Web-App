// using System.Data.Entity;
// using System.Web.Mvc;
// using EExamWebApp.Data;
// using EExamWebApp.Filters;
// using EExamWebApp.Models;
//
// namespace EExamWebApp.Controllers
// {
//     public class ExamController : BaseController
//     {
//         public ActionResult Index()
//         {
//             return View();
//         }
//         // GET: Exam by Course ID
//
//
//         // GET: Exam/Details/5
//         public ActionResult Details(int id)
//         {
//             using (var context = new AppDbContext())
//             {
//                 var exam = context.Exams.Find(id);
//                 if (exam == null) return HttpNotFound();
//                 return View(exam);
//             }
//         }
//
//         // GET: Exam/Create
//         [AuthorizeUserType(UserType.Teacher)]
//         public ActionResult Create(int courseId)
//         {
//             ViewBag.CourseId = courseId; // Pass courseId to the view
//             return View();
//         }
//
//         // // POST: Exam/Create
//         // [HttpPost]
//         // [ValidateAntiForgeryToken]
//         // [AuthorizeUserType(UserType.Teacher)]
//         // public ActionResult Create(int courseId, Exam exam)
//         // {
//         //     if (ModelState.IsValid)
//         //         using (var context = new AppDbContext())
//         //         {
//         //             exam.CourseId = courseId; // Assign courseId to the exam
//         //             context.Exams.Add(exam);
//         //             context.SaveChanges();
//         //             return RedirectToAction("Index", new { courseId });
//         //         }
//         //
//         //     ViewBag.CourseId = courseId; // Pass courseId back to the view in case of error
//         //     return View(exam);
//         // }
//
//         // GET: Exam/Edit/5
//         [AuthorizeUserType(UserType.Teacher)]
//         public ActionResult Edit(int id)
//         {
//             using (var context = new AppDbContext())
//             {
//                 var exam = context.Exams.Find(id);
//                 if (exam == null) return HttpNotFound();
//                 ViewBag.CourseId = exam.CourseId; // Pass courseId to the view
//                 return View(exam);
//             }
//         }
//
//         // POST: Exam/Edit/5
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         [AuthorizeUserType(UserType.Teacher)]
//         public ActionResult Edit(Exam exam)
//         {
//             if (ModelState.IsValid)
//                 using (var context = new AppDbContext())
//                 {
//                     context.Entry(exam).State = EntityState.Modified;
//                     context.SaveChanges();
//                     return RedirectToAction("Index", new { courseId = exam.CourseId });
//                 }
//
//             ViewBag.CourseId = exam.CourseId; // Pass courseId back to the view in case of error
//             return View(exam);
//         }
//
//         // GET: Exam/Delete/5
//         [AuthorizeUserType(UserType.Teacher)]
//         public ActionResult Delete(int id)
//         {
//             using (var context = new AppDbContext())
//             {
//                 var exam = context.Exams.Find(id);
//                 if (exam == null) return HttpNotFound();
//                 ViewBag.CourseId = exam.CourseId; // Pass courseId to the view
//                 return View(exam);
//             }
//         }
//
//         // POST: Exam/Delete/5
//         [HttpPost]
//         [ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         [AuthorizeUserType(UserType.Teacher)]
//         public ActionResult DeleteConfirmed(int id)
//         {
//             using (var context = new AppDbContext())
//             {
//                 var exam = context.Exams.Find(id);
//                 if (exam == null) return HttpNotFound();
//
//                 context.Exams.Remove(exam);
//                 context.SaveChanges();
//                 return RedirectToAction("Index", new { courseId = exam.CourseId });
//             }
//         }
//     }
// }

