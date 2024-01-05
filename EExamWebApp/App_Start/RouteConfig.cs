using System.Web.Mvc;
using System.Web.Routing;

namespace EExamWebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "AccessDenied",
                "AccessDenied",
                new { controller = "Error", action = "AccessDenied" }
            );
            routes.MapRoute(
                "GetCurrentStudents",
                "Enrollments/GetCurrentStudents/{id}",
                new { controller = "Courses", action = "GetCurrentStudents" }
            );
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}