using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Revamp.IO.Structs.Models;

namespace Revamp.IO.Web.Filters
{
    public class ClearSessionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionObjects _SessionModel;
            try
            {
                byte[] tempSession = null;
                bool sessionPresent = filterContext.HttpContext.Session.TryGetValue("_SessionObject", out tempSession);                
                _SessionModel = sessionPresent ? Revamp.IO.Tools.Box.FromByteArray<SessionObjects>(tempSession) : new SessionObjects();
            }
            catch
            {
                _SessionModel = new SessionObjects();
            }
            if (_SessionModel != null && _SessionModel.SessionIdentity != null && _SessionModel.SessionIdentity.Identity != null && !string.IsNullOrWhiteSpace(_SessionModel.SessionIdentity.Identity.username))
            {
                filterContext.Result = new RedirectResult("~/login");
            }

            var httpContext = filterContext.HttpContext;
            //var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];

            base.OnActionExecuting(filterContext);

        }


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {


            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            base.OnResultExecuting(filterContext);

        }
    }
}
