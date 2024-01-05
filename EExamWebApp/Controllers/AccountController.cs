using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using EExamWebApp.Data;
using EExamWebApp.Models;

namespace EExamWebApp.Controllers
{
    public class AccountController : Controller
    {
        public static SelectList GetUserTypesForRegistration()
        {
            var values = from UserType e in Enum.GetValues(typeof(UserType))
                where e != UserType.Admin // Exclude Admin
                select new { Id = e, Name = e.ToString() };

            return new SelectList(values, "Id", "Name");
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var (isAuthenticated, isApproved) = AuthenticateUser(user.Username, user.Password);

                if (isAuthenticated)
                {
                    if (isApproved)
                    {
                        Session["UserEmail"] = GetEmailByUsername(user.Username);
                        FormsAuthentication.SetAuthCookie(user.Username, false); // If using Forms Authentication
                        return RedirectToAction("Index", "Home");
                    }

                    // User is authenticated but not approved
                    return RedirectToAction("RegistrationPending");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }

            // If we got this far, something failed; redisplay the form
            return View(user);
        }

        private IEnumerable<SelectListItem> GetUserTypes()
        {
            // This is an example. Replace it with your actual logic to get user types.
            // For instance, this could be a database call to retrieve user types.

            var userTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Admin", Value = "0" },
                new SelectListItem { Text = "User", Value = "1" },
                new SelectListItem { Text = "Guest", Value = "2" }
                // Add other user types as necessary
            };

            return userTypes;
        }


        // GET: Account/Register
        public ActionResult Register()
        {
            ViewBag.UserTypeSelectList = new SelectList(GetUserTypes(), "Value", "Text");
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.IsApproved = false; // Set the user as not approved by default
                var registrationSuccessful = RegisterUser(user);
                if (registrationSuccessful)
                    // Optionally, you can redirect to a "Registration Successful" page
                    return RedirectToAction("RegistrationPending");
                ModelState.AddModelError("", "User already exists or an error occurred.");
            }

            // If we got this far, something failed; redisplay the form
            return View(user);
        }

        public ActionResult Logout()
        {
            // Invalidate the user's authentication cookie
            FormsAuthentication.SignOut();

            // Optionally, you can clear the session if you store any user information there
            Session.Clear();

            // Redirect the user to the login page, home page, or any other page
            return RedirectToAction("Login", "Account");
        }

        public ActionResult RegistrationPending()
        {
            return View();
        }


        private (bool isAuthenticated, bool isApproved) AuthenticateUser(string username, string password)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password)) return (true, user.IsApproved);

                return (false, false);
            }
        }


        private bool RegisterUser(User user)
        {
            using (var context = new AppDbContext())
            {
                // Check if the user already exists
                var existingUser =
                    context.Users.FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);
                if (existingUser != null)
                    // User already exists
                    return false;

                // Hash the password - very important for security
                user.Password = HashPassword(user.Password);

                // Add the user to the database
                context.Users.Add(user);
                context.SaveChanges();

                return true;
            }
        }

        private string HashPassword(string password)
        {
            // Implement password hashing here
            // For example, using BCrypt.Net: https://www.nuget.org/packages/BCrypt.Net-Next/
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private string GetEmailByUsername(string username)
        {
            using (var context = new AppDbContext())
            {
                return context.Users.Where(u => u.Username == username).Select(u => u.Email).FirstOrDefault();
            }
        }
    }
}