using System.Linq;
using System.Web.Mvc;
using EExamWebApp.Data;

namespace EExamWebApp.Controllers
{
    public class BaseController : Controller
    {
        protected int GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (var context = new AppDbContext())
                {
                    var username = User.Identity.Name;
                    var user = context.Users.SingleOrDefault(u => u.Username == username);

                    if (user != null)
                    {
                        return user.Id;
                    }
                }
            }

            return -1; // or throw an exception, or handle this case as appropriate
        }
    }
}