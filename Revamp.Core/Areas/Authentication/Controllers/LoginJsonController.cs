using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Revamp.Core.Services;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Foundation;
using Revamp.IO.Structs.Models;
using System.Security.Claims;

namespace Revamp.Core.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    [Route("loginjson/[action]")]
    [AllowAnonymous]
    public class LoginJsonController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private RevampCoreSettings RevampCoreSettings { get; set; }
        private readonly IMvcApplication mvcApplication;
        private ConnectToDB _Connect { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginJsonController(IOptions<RevampCoreSettings> settings, IMvcApplication iMvcApplication, IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            RevampCoreSettings = settings.Value;
            mvcApplication = iMvcApplication;
            _Connect = new ConnectToDB
            {
                Platform = RevampCoreSettings.Platform,
                DBConnString = RevampCoreSettings.DbConnect,
                SourceDBOwner = RevampCoreSettings.SystemDBName
            };
            _httpContextAccessor = httpContextAccessor; 
        }

        public JsonResult prompt(Guid? token)
        {
            var jsonContent = Json(new
            {
                access = false
            });
            var prompt = System.IO.File.ReadAllText(_hostingEnvironment.ContentRootPath + "/wwwroot/Areas/Dynamic/Templates/Dynamic/hb_prompt.html");
            var disclaimer = System.IO.File.ReadAllText(_hostingEnvironment.ContentRootPath + "/wwwroot/Areas/Authentication/Templates/Login/Disclaimer.html");
            var loaderEllipses = System.IO.File.ReadAllText(_hostingEnvironment.ContentRootPath + "/wwwroot/Areas/Dynamic/Templates/Dynamic/Ellipses.html");

            jsonContent = Json(new
            {
                access = true,
                d = IO.Tools.Box.Base64Encode(prompt),
                d2 = IO.Tools.Box.Base64Encode(disclaimer),
                d3 = IO.Tools.Box.Base64Encode(loaderEllipses)
            });

            return jsonContent;
        }
        // GET: Authentication/LoginJson        
        public async Task<JsonResult> get(Guid? token)
        {
            var jsonContent = Json(new
            {
                access = false
            });
            if (token != null)
            {
                List<DynamicModels.RootReportFilter> filters = new List<DynamicModels.RootReportFilter> {
                new DynamicModels.RootReportFilter { FilterName = "APPLICATION_NAME_", ParamValue = "Revamp System" },
                new DynamicModels.RootReportFilter { FilterName = "STAGE_NAME_", ParamValue = "login" },
                new DynamicModels.RootReportFilter { FilterName = "GRIP_NAME_", ParamValue = "login" }
            };

                DataTable Result = await _DynamicOutputProcedures._DynoProcSearchAsync(_Connect, "Custom Query", "SP_S_VW_APPLICATION_OBJECTS_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "APPLICATIONS_ID asc, STAGES_ID asc, GRIPS_ID asc, OBJECT_SETS_ID asc, OBJ_PROP_SETS_ID asc ", start = 0, verify = "T" },
                    filters);

                string captchaText = await mvcApplication.ReturnViewToStringAsync(new CommonModels.MVCGetPartial
                {
                    _thisController = ControllerContext,
                    ViewName = "Shared/_Captcha",
                    model = null
                });
                string AntiForgeryObject = await GetAntiForgeryObject();

                string AntiForgeryToken = await GetAntiForgeryToken();
                string AntiForgeryObject2 = await GetAntiForgeryObject();

                var content = System.IO.File.ReadAllText(_hostingEnvironment.ContentRootPath + "/Areas/Authentication/Templates/Login/Objects.html");

                ArrayList JsonData = ER_Tools._GetObjectListFromDataTable(new ArrayList(), Result);
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(JsonData);
                //var thisUser = HttpContext.User.Identity.Name.ToString();
                var thisUser = "";// _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                bool isCACEnabled = RevampCoreSettings.IsCacEnabled;
                jsonContent = Json(new
                {
                    access = true,
                    d = Revamp.IO.Tools.Box.Base64Encode(jsonString),
                    c = Revamp.IO.Tools.Box.Base64Encode(captchaText),
                    a = AntiForgeryObject,
                    u = thisUser,
                    s = Revamp.IO.Tools.Box.Base64Encode(content),
                    z = AntiForgeryToken,
                    a2 = AntiForgeryObject2,
                    ca = isCACAvailable(),
                    ic = isCACEnabled,

                });
            }

            var jsonResult = Json(jsonContent);
            
            return jsonResult;
        }

        private async Task<string> GetAntiForgeryObject()
        {
            var thisHtml = await mvcApplication.ReturnViewToStringAsync(new CommonModels.MVCGetPartial
            {
                _thisController = ControllerContext,
                ViewName = "Shared/_AntForgeryObject",
                model = null
            });

            return Revamp.IO.Tools.Box.Base64Encode(thisHtml);
        }

        private async Task<string> GetAntiForgeryToken()
        {
            var thisHtml = await mvcApplication.ReturnViewToStringAsync(new CommonModels.MVCGetPartial
            {
                _thisController = ControllerContext,
                ViewName = "Shared/_AntiForgeryTokens",
                model = null
            });

            return Revamp.IO.Tools.Box.Base64Encode(thisHtml);
        }

        public async Task<JsonResult> start()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();

            var jsonResult = Json(Guid.NewGuid().ToString());

            return jsonResult;
        }

         public async Task<JsonResult> isCac()
         {
             bool CacIsPresent = await isCACAvailable();

             var jsonResult = Json(CacIsPresent);
 
             return jsonResult;
         }
         
        private async Task<bool> isCACAvailable()
        {
            bool CacIsPresent = false;
            X509Certificate2 cs = await Request.HttpContext.Connection.GetClientCertificateAsync();
            if (cs != null)
            {
                string EDIPI = System.Text.RegularExpressions.Regex.Match(cs.Subject, @"\d{10}").Value;

                CacIsPresent = string.IsNullOrWhiteSpace(EDIPI) ? false : true;
            }

            return CacIsPresent;
        }
        
    }
}