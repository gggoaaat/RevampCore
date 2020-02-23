using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Revamp.IO.Web.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public HttpContext Current => new HttpContextAccessor().HttpContext;
        protected void HandleUnauthorizedRequest(AuthorizationFilterContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var thisRequest = Current.Request.Path.Value;
                var httpRequestFeature = Current.Request.HttpContext.Features.Get<IHttpRequestFeature>();
                
                if (thisRequest == "/")
                {
                    filterContext.Result = new UnauthorizedResult();
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/loggedout?ReturnUrl=" + (httpRequestFeature.Path.ToLower() + httpRequestFeature.QueryString));
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccessDenied" }));
            }
        }

    }
}
