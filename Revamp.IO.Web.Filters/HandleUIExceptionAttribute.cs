using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Revamp.IO.Web.Filters
{
    public class HandleUIExceptionAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.Exception != null)
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                //filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                filterContext.Result = ((AccessDenied)filterContext.Exception).exceptionDetails;
            }
        }
    }

    public class AccessDenied : ApplicationException
    {
        public JsonResult exceptionDetails;
        public AccessDenied(JsonResult exceptionDetails)
        {
            this.exceptionDetails = exceptionDetails;
        }
        public AccessDenied(string message) : base(message) { }
        public AccessDenied(string message, Exception inner) : base(message, inner) { }
        protected AccessDenied(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
