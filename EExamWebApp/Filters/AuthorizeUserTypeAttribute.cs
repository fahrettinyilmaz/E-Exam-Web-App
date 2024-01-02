using System.Linq;
using System.Web;
using System.Web.Mvc;
using EExamWebApp.Models;
using EExamWebApp.Data;

namespace EExamWebApp.Filters
{
    public class AuthorizeUserTypeAttribute : AuthorizeAttribute
    {
        private readonly UserType[] _authorizedTypes;
        private AppDbContext db = new AppDbContext();

        public AuthorizeUserTypeAttribute(params UserType[] types)
        {
            _authorizedTypes = types;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            // Retrieve the current user's UserType.
            UserType currentUserType = GetUserType(httpContext.User.Identity.Name);

            return _authorizedTypes.Contains(currentUserType);
        }

        private UserType GetUserType(string username)
        {
            // Get the UserType of the user from the database
            var user = db.Users.SingleOrDefault(u => u.Username == username);
            return user != null ? user.UserType : default(UserType);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                // User is logged in but does not have the required permissions
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "action", "AccessDenied" },
                        { "controller", "Error" }
                    });
            }
            else
            {
                // User is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}