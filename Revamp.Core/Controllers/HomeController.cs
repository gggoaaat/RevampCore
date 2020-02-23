using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Revamp.Core.Models;
using Revamp.Core.Services;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;

namespace Revamp.Core.Controllers
{
    public class HomeController : Controller
    {
        private _DynamicOutputProcedures dop = new _DynamicOutputProcedures();
        private RevampCoreSettings RevampCoreSettings { get; set; }
        private readonly IMvcApplication mvcApplication;
        private ConnectToDB _Connect { get; set; }

        public HomeController(IOptions<RevampCoreSettings> settings, IMvcApplication iMvcApplication)
        {
            RevampCoreSettings = settings.Value;
            mvcApplication = iMvcApplication;
            _Connect = new ConnectToDB
            {
                Platform = RevampCoreSettings.Platform,
                DBConnString = RevampCoreSettings.DbConnect,
                SourceDBOwner = RevampCoreSettings.SystemDBName
            };
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

            List<DynamicModels.RootReportFilter> getBaseAppfilters = new List<DynamicModels.RootReportFilter>();

            getBaseAppfilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, ParamValue = "Y" });
            
            DataTable getApps = await _DynamicOutputProcedures._DynoProcSearchAsync(_Connect, "Custom Query", "SP_S_VW__APPLICATIONS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "", start = 0, verify = "T" },
                getBaseAppfilters);

            Revamp.IO.Tools.Box._WriteEventLog("Test", IO.Structs.Enums.EventLogType.success);

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
