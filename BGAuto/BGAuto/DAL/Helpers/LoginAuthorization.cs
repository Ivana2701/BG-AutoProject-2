
using BGAuto.DAL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using BGAuto.Models.DbEntities;

namespace BGAuto.DAL.Helpers
{
    public class LoginAuthorization : ActionFilterAttribute
    {

        private readonly ISessionManager _SessionManag;
        public LoginAuthorization(ISessionManager sessionManag)
        {
            _SessionManag = sessionManag;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Users usr = new Users();

            usr = _SessionManag.GetLoginUserFromSession();


            // --if user session null, then redirect to  login page
            if (usr == null || usr.UserID < 1)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "AdminAuthentication",
                    action = "Login",
                    area = ""
                }));

            }


            base.OnActionExecuting(filterContext);
        }
    }
}
