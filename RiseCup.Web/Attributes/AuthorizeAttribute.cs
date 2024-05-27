using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace RiseCup.Web.Attributes
{
    public class SimpleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _requiredRoles;

        public SimpleAuthorizeAttribute(params string[] requiredRoles)
        {
            _requiredRoles = requiredRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }

            if (!_requiredRoles.Any(httpContext.User.IsInRole))
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private void HandleUnauthorizedRequest(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Error" },
                        { "action", "Unauthorized" }
                    });
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}