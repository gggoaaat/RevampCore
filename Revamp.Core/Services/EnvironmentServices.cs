using Microsoft.Extensions.Options;
using Revamp.Core.Models;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using System.Threading.Tasks;

namespace Revamp.Core.Services
{
    public class MvcApplication : IMvcApplication
    {        
        private RevampCoreSettings IRevampCoreSettings { get; set; }
        private readonly IViewRenderService _viewRenderService;
        public MvcApplication(IOptions<RevampCoreSettings> settings, IViewRenderService viewRenderService)
        {
            IRevampCoreSettings = settings.Value;
            _viewRenderService = viewRenderService;
        }
        
        #region ConnectObject
        private ConnectToDB _Connect()
        {
            return new ConnectToDB
            {
                Platform = "Microsoft",
                DBConnString = IRevampCoreSettings.DbConnect,
                SourceDBOwner = IRevampCoreSettings.SystemDBName,
                Schema = "CSA"
            };
        }

        private static ConnectToDB _ConnectPrivate = null;

        public static ConnectToDB Connect
        {
            get
            {
                if (_ConnectPrivate != null)
                {
                    ConnectToDB ResetConnect = _ConnectPrivate.Copy();

                    ResetConnect.Schema = "CSA";
                    ResetConnect.Schema2 = "";

                    return ResetConnect;
                }
                else
                {
                    ConnectToDB _Connect = new ConnectToDB();
                    _ConnectPrivate = _Connect.Copy();

                    return _ConnectPrivate;
                }
            }
        }

        #endregion

        public async Task<string> ReturnViewToStringAsync(CommonModels.MVCGetPartial thisModel)
        {
            var result = await _viewRenderService.ReturnViewToStringAsync(thisModel);

            return result;
        }
    }
}
