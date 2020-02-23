using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Revamp.IO.Foundation;
using Revamp.IO.Structs.Enums;

namespace Revamp.IO.Web.Filters
{
    public class CheckReturnURLFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var value = filterContext.HttpContext.Request.Query["returnUrl"];
            if (!string.IsNullOrWhiteSpace(value))
            {
                // NOTE: this assumes all your controllers derive from Controller.
                // If they don't, you'll need to set the value in OnActionExecuted instead
                // or use an IAsyncActionFilter
                if (filterContext.Controller is Controller controller)
                {
                    //controller.ViewData["ReturnUrl"] = value.ToString();
                    if (!controller.Url.IsLocalUrl(value.ToString()))
                    {
                        filterContext.Result = new RedirectResult("~/loggedout");
                    }
                }
            }

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
