using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using EExamWebApp.Data;
using EExamWebApp.Filters;
using EExamWebApp.Models;

namespace EExamWebApp.Controllers
{
    [AuthorizeUserType(UserType.Admin)]
    public class AdminController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ApproveUsers()
        {
            var usersPendingApproval = db.Users.Where(u => !u.IsApproved).ToList();
            return View(usersPendingApproval);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult ListUsers()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("ApproveUsers");
            }

            return View(user);
        }

        public ActionResult EditUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ApproveUsers");
            }

            return View(user);
        }

        public ActionResult DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(int id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("ApproveUsers");
        }
    }
}