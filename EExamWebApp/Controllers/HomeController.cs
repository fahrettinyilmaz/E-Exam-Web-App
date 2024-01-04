using System.Linq;
using System.Web.Mvc;
using EExamWebApp.Data;

namespace EExamWebApp.Controllers
{
    public class HomeController : BaseController
    {
        private string GetUserRole()
        {
            if (User.Identity.IsAuthenticated)
                using (var context = new AppDbContext())
                {
                    var username = User.Identity.Name;
                    var user = context.Users.SingleOrDefault(u => u.Username == username);

                    if (user != null) return user.UserType.ToString();
                }

            return "Guest";
        }

        // [Authorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.UserFullName = GetCurrentUser().Name + " " + GetCurrentUser().LastName;
            else
                ViewBag.UserRole = "Guest";
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}