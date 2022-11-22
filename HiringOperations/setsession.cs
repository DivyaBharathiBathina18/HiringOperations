using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace HiringOperations.Controllers
{
    public class SetSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filtercontext)
        {
            var value = filtercontext.HttpContext.Session.GetString("name");
            if (value == null)
            {
                filtercontext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary {
                            {
                           "controller", "Login" },
                            { "action","Login" }
                        });
            }
        }
    }
}
