using System.Web.Mvc;

namespace EExamWebApp.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}