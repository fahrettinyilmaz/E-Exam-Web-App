using System.Linq;
using System.Web.Mvc;
using EExamWebApp.Data;
using EExamWebApp.Models;

namespace EExamWebApp.Controllers
{
    public class BaseController : Controller
    {
        protected User GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
                using (var context = new AppDbContext())
                {
                    var username = User.Identity.Name;
                    var user = context.Users.SingleOrDefault(u => u.Username == username);

                    if (user != null) return user;
                }

            return null;
        }

        protected int GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
                using (var context = new AppDbContext())
                {
                    var username = User.Identity.Name;
                    var user = context.Users.SingleOrDefault(u => u.Username == username);

                    if (user != null) return user.Id;
                }

            return -1; // or throw an exception, or handle this case as appropriate
        }

        protected string GetUserRole()
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
    }
}