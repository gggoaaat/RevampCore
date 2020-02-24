using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Revamp.Core.Services;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;

namespace Revamp.Core.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    [Revamp.IO.Web.Filters.CheckReturnURLFilter]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private RevampCoreSettings RevampCoreSettings { get; set; }
        private readonly IMvcApplication mvcApplication;
        private ConnectToDB _Connect { get; set; }

        public LoginController(IOptions<RevampCoreSettings> settings, IMvcApplication iMvcApplication, IHostingEnvironment hostingEnvironment)
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
        }

        public async Task<IActionResult> Index(SessionObjects SO)
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            SO = new SessionObjects();
            SO.appSettings = RevampCoreSettings;
            return View(SO);
        }


    }
}