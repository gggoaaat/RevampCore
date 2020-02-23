using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Foundation;
using Revamp.IO.Helpers.Helpers;
using Revamp.IO.Structs.Models;

namespace Revamp.IO.Web.Filters
{
    public class PermissionCheckAttribute : ActionFilterAttribute
    {
        public string[] CheckForThesePrivileges { get; set; }
        public string[] CheckForThesePrivilegesOnCore { get; set; }
        public string[] CheckForTheseRoles { get; set; }
        public string CoreUUID { get; set; }
        public string ApplicationUUID { get; set; }
        public string RoleUUID { get; set; }
        public string GroupUUID { get; set; }
        public bool isActionResult { get; set; } = false;
        private RevampCoreSettings RevampCoreSettings { get; set; }
        public PermissionCheckAttribute(IOptions<RevampCoreSettings> settings)
        {
            RevampCoreSettings = settings.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ConnectToDB _Connect = new ConnectToDB {
                Platform = RevampCoreSettings.Platform,
                DBConnString = RevampCoreSettings.DbConnect,
                SourceDBOwner = RevampCoreSettings.SystemDBName
            };
            IConnectToDB IConnect = _Connect;
            SecurityHelper securityHelper = new SecurityHelper();
            AppHelper appHelper = new AppHelper();

            SessionObjects SO = null;

            try
            {
                byte[] tempSession = null;
                bool sessionPresent = filterContext.HttpContext.Session.TryGetValue("SO", out tempSession);
                SO = sessionPresent ? Tools.Box.FromByteArray<SessionObjects>(tempSession) : new SessionObjects();
            }
            catch
            {
                filterContext.Result = new RedirectResult("~/login");
            }

            if (SO != null && SO.SessionIdentity != null)
            {
                bool hasthesePrivs = false;
                bool PrivsCheck = false;
                if (CheckForThesePrivileges != null && CheckForThesePrivileges.Length > 0)
                {
                    PrivsCheck = true;
                    hasthesePrivs = securityHelper.DoesIdentityHavePrivileges(IConnect, SO.SessionIdentity.Identity.identities_id, CheckForThesePrivileges);
                }

                bool hasthesePrivsOnCore = false;
                bool PrivsOnCoreCheck = false;
                if (CheckForThesePrivilegesOnCore != null && CheckForThesePrivilegesOnCore.Length > 0)
                {
                    
                    var formCollection = filterContext.HttpContext.Request.Form;
                    Guid? coreUUID = null;

                    //Cores
                    if (!string.IsNullOrEmpty(CoreUUID))
                    {
                        if (formCollection.ContainsKey(CoreUUID))
                        {
                            coreUUID = ER_Tools.ConvertToGuid(formCollection[CoreUUID].ToString());
                        }
                        else if (filterContext.HttpContext.Request.Query.ContainsKey(CoreUUID))
                        {
                            coreUUID = ER_Tools.ConvertToGuid(filterContext.HttpContext.Request.Query[CoreUUID].ToString());
                        }
                    }

                    //Applications
                    if (!string.IsNullOrEmpty(ApplicationUUID))
                    {
                        Guid? appUUID = null;

                        if (formCollection.ContainsKey(ApplicationUUID))
                        {
                            appUUID = ER_Tools.ConvertToGuid(formCollection[ApplicationUUID].ToString());
                        }
                        else if (filterContext.HttpContext.Request.Query.ContainsKey(ApplicationUUID))
                        {
                            appUUID = ER_Tools.ConvertToGuid(filterContext.HttpContext.Request.Query[ApplicationUUID].ToString());
                        }

                        coreUUID = appHelper.GetAppCoreUUID(_Connect, appUUID);
                    }

                    //Roles
                    if (!string.IsNullOrEmpty(RoleUUID))
                    {
                        Guid? roleUUID = null;

                        if (formCollection.ContainsKey(RoleUUID))
                        {
                            roleUUID = ER_Tools.ConvertToGuid(formCollection[RoleUUID].ToString());
                        }
                        else if (filterContext.HttpContext.Request.Query.ContainsKey(RoleUUID))
                        {
                            roleUUID = ER_Tools.ConvertToGuid(filterContext.HttpContext.Request.Query[RoleUUID].ToString());
                        }

                        coreUUID = securityHelper.GetRoleCoreUUID(_Connect, roleUUID);
                    }

                    //Groups
                    if (!string.IsNullOrEmpty(GroupUUID))
                    {
                        Guid? groupUUID = null;

                        if (formCollection.ContainsKey(GroupUUID))
                        {
                            groupUUID = ER_Tools.ConvertToGuid(formCollection[GroupUUID].ToString());
                        }
                        else if (filterContext.HttpContext.Request.Query.ContainsKey(GroupUUID))
                        {
                            groupUUID = ER_Tools.ConvertToGuid(filterContext.HttpContext.Request.Query[GroupUUID].ToString());
                        }

                        coreUUID = securityHelper.GetGroupCoreUUID(_Connect, groupUUID);
                    }

                    //Check for privileges on core
                    if (coreUUID != null)
                    {
                        PrivsOnCoreCheck = true;
                        hasthesePrivsOnCore = securityHelper.DoesIdentityHavePrivilegeOnCore(IConnect, SO.SessionIdentity.Identity.identities_id, CheckForThesePrivilegesOnCore, coreUUID);
                    }
                }

                bool hastheseRoles = false;
                bool RolesCheck = false;
                if (CheckForTheseRoles != null && CheckForTheseRoles.Length > 0)
                {
                    RolesCheck = true;
                    hastheseRoles = securityHelper.DoesIdentityHaveRoles(IConnect, CheckForTheseRoles, SO.SessionIdentity.Identity.identities_id);

                }

                if (PrivsCheck || RolesCheck || PrivsOnCoreCheck)
                {
                    //if((PrivsOnCoreCheck && !hasthesePrivsOnCore))
                    //{
                    //    hasthesePrivs = false;
                    //    hastheseRoles = false;
                    //}

                    if (hasthesePrivs == false && hastheseRoles == false && hasthesePrivsOnCore == false)
                    {
                        if (isActionResult)
                        {
                            filterContext.HttpContext.Response.StatusCode = 403;
                            filterContext.Result = new RedirectResult("/error/error404");
                        }
                        else
                        {
                            filterContext.HttpContext.Response.StatusCode = 403;
                            
                            var result = new JsonResult(new
                            {
                                Data = new { Success = false, Data = "Access Denied" },
                                ContentEncoding = System.Text.Encoding.UTF8,
                                ContentType = "application/json"
                                //JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            });
                            filterContext.Result = result;
                        }
                    }
                }
            }

            base.OnActionExecuting(filterContext);

        }
    }
}
