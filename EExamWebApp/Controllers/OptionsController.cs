// using System.Data.Entity;
// using System.Linq;
// using System.Net;
// using System.Web.Mvc;
// using EExamWebApp.Data;
// using EExamWebApp.Filters;
// using EExamWebApp.Models;
//
// namespace EExamWebApp.Controllers
// {
//     
//     [AuthorizeUserType(UserType.Teacher)]
//     public class OptionsController : BaseController
//     {
//         private readonly AppDbContext _db = new AppDbContext();
//
//         // GET: Options
//         public ActionResult Index()
//         {
//             return View(_db.Options.ToList());
//         }
//
//         // GET: Options/Details/5
//         public ActionResult Details(int? id)
//         {
//             if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//             var option = _db.Options.Find(id);
//             if (option == null) return HttpNotFound();
//             return View(option);
//         }
//
//         // GET: Options/Create
//         public ActionResult Create()
//         {
//             return View();
//         }
//
//         // POST: Options/Create
//         // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
//         // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public ActionResult Create([Bind(Include = "Id,Text,IsCorrect")] Option option)
//         {
//             if (ModelState.IsValid)
//             {
//                 _db.Options.Add(option);
//                 _db.SaveChanges();
//                 return RedirectToAction("Index");
//             }
//
//             return View(option);
//         }
//
//         // GET: Options/Edit/5
//         public ActionResult Edit(int? id)
//         {
//             if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//             var option = _db.Options.Find(id);
//             if (option == null) return HttpNotFound();
//             return View(option);
//         }
//
//         // POST: Options/Edit/5
//         // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
//         // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public ActionResult Edit([Bind(Include = "Id,Text,IsCorrect")] Option option)
//         {
//             if (ModelState.IsValid)
//             {
//                 _db.Entry(option).State = EntityState.Modified;
//                 _db.SaveChanges();
//                 return RedirectToAction("Index");
//             }
//
//             return View(option);
//         }
//
//         // GET: Options/Delete/5
//         public ActionResult Delete(int? id)
//         {
//             if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//             var option = _db.Options.Find(id);
//             if (option == null) return HttpNotFound();
//             return View(option);
//         }
//
//         // POST: Options/Delete/5
//         [HttpPost]
//         [ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public ActionResult DeleteConfirmed(int id)
//         {
//             var option = _db.Options.Find(id);
//             _db.Options.Remove(option);
//             _db.SaveChanges();
//             return RedirectToAction("Index");
//         }
//
//         protected override void Dispose(bool disposing)
//         {
//             if (disposing) _db.Dispose();
//             base.Dispose(disposing);
//         }
//     }
// }