using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EExamWebApp.Data;
using EExamWebApp.Models;


namespace EExamWebApp.Controllers
{
    public class HomeController : BaseController
    {
        private string GetUserRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (var context = new AppDbContext())
                {
                    var username = User.Identity.Name;
                    var user = context.Users.SingleOrDefault(u => u.Username == username);

                    if (user != null)
                    {
                        return user.UserType.ToString();
                    }
                }
            }

            return "Guest";
        }

        // [Authorize]
        public ActionResult Index()
        {
            string userRole = GetUserRole();
            ViewBag.UserRole = userRole;
            using (var context = new AppDbContext())
            {
                if (userRole == "Admin")
                {
                    return RedirectToAction("ListUsers", "Admin");
                }
                else if (userRole == "Teacher" || userRole == "Student")
                {
                    List<Course> courses = new List<Course>();

                    if (userRole == "Teacher")
                    {
                        int teacherId = GetCurrentUserId();
                        courses = context.Courses
                            .Where(c => c.Teacher.Id == teacherId)
                            .ToList();
                    }
                    else if (userRole == "Student")
                    {
                        int studentId = GetCurrentUserId();
                        courses = context.Enrollments
                            .Where(e => e.Student.Id == studentId)
                            .Select(e => e.Course)
                            .Distinct()
                            .ToList();
                    }

                    return View("ListCourses", courses);
                }

                // Redirect to a default view if none of the roles match
                return View();
            }
        }

        [Authorize]
        public ActionResult ListCourses()
        {
            using (var context = new AppDbContext())
            {
                List<Course> courses = context.Courses.ToList();
                return View(courses);
            }
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