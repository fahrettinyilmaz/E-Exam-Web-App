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
        private readonly AppDbContext _db = new AppDbContext();

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ApproveUsers()
        {
            var usersPendingApproval = _db.Users.Where(u => !u.IsApproved).ToList();
            return View(usersPendingApproval);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveUsers(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null) return HttpNotFound();

            user.IsApproved = true;
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("ApproveUsers");
        }


        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult ListUsers()
        {
            var users = _db.Users.ToList();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("ApproveUsers");
            }

            return View(user);
        }

        public ActionResult EditUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null) return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("ApproveUsers");
            }

            return View(user);
        }

        public ActionResult DeleteUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null) return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(int id)
        {
            var user = _db.Users.Find(id);
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("ApproveUsers");
        }
    }
}