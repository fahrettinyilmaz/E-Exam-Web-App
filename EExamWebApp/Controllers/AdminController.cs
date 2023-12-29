using System.Linq;
using System.Web.Mvc;
using EExamWebApp.Data;

namespace EExamWebApp.Controllers
{
    public class AdminController :Controller
    { 
        private AppDbContext db = new AppDbContext();
        public ActionResult ApproveUsers()
        {
            
            var usersPendingApproval = db.Users.Where(u => !u.IsApproved).ToList();
            return View(usersPendingApproval);
        }

    }
}