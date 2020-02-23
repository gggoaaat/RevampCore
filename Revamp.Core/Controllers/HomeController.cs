using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Revamp.Core.Models;
using Revamp.Core.Services;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;

namespace Revamp.Core.Controllers
{

    public class HomeController : Controller
    {
        private RevampCoreSettings RevampCoreSettings { get; set; }
        private readonly IMvcApplication mvcApplication;
        private ConnectToDB dbConn2 = MvcApplication.Connect;
        public HomeController(IOptions<RevampCoreSettings> settings, IMvcApplication viewRenderService)
        {
            RevampCoreSettings = settings.Value;
            mvcApplication = viewRenderService;
        }

        public async Task<IActionResult> Index()
        {
            
            CommonModels.MVCGetPartial thisModel = new CommonModels.MVCGetPartial
            {
                ViewName = "_AntiForgery",
                _TempData = null,
                _thisController = ControllerContext
            };
            
            var result = await mvcApplication.ReturnViewToStringAsync(thisModel);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
