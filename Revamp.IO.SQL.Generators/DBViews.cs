using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.SQL.Generators
{
    public class DBViews
    {
        public List<CommandResult> ADD_VIEW(IConnectToDB _Connect, string ViewName, List<sqlSelectStructure> QueryStructure, string ViewType, bool WithBinding)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_Generate er_generate = new ER_Generate();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            string _ViewName = "VW__" + er_tools.MaxNameLength(ViewName, (128 - 4));
            StringBuilder SQLBuffer = new StringBuilder();

            if (WithBinding)
            {
                SQLBuffer.AppendLine("CREATE VIEW " + _Schema + "." + _ViewName + " WITH SCHEMABINDING AS ");
            }
            else
            {
                SQLBuffer.AppendLine("CREATE VIEW " + _Schema + "." + _ViewName + "  AS ");
            }
            SQLBuffer.AppendLine(er_generate.GENERATE_QUERY(_Connect, QueryStructure));

            string SuccessMessage = "View " + _ViewName + " created.";

            _result.attemptedCommand = SQLBuffer.ToString();
            _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

            if (_result._Successful)
            {
                IConnectToDB csaConnect = _Connect.Copy();
                csaConnect.Schema = "CSA";

                er_dml.ADD_Dictionary_View(csaConnect, new Structs.Models.RevampSystem.Dictionary.AddView { I_VIEW_NAME = _ViewName, I_VIEW_TYPE = ViewType, I_VIEWDATA = "" });
            }

            results.Add(_result);

            return results;
        }


        public List<CommandResult> ADD_VIEW(IConnectToDB _Connect, string ViewName, List<sqlSelectStructure> QueryStructure, string ViewType, bool WithBinding, string ViewData)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_Generate er_generate = new ER_Generate();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            string _ViewName = "VW__" + er_tools.MaxNameLength(ViewName, (128 - 4));
            StringBuilder SQLBuffer = new StringBuilder();

            if (WithBinding)
            {
                SQLBuffer.AppendLine("CREATE VIEW " + _Schema + "." + _ViewName + " WITH SCHEMABINDING AS ");
            }
            else
            {
                SQLBuffer.AppendLine("CREATE VIEW " + _Schema + "." + _ViewName + "  AS ");
            }
            SQLBuffer.Append(er_generate.GENERATE_QUERY(_Connect, QueryStructure));

            string SuccessMessage = "View " + _ViewName + " created.";

            _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLBuffer.ToString(), SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

            if (_result._Successful)
            {
                IConnectToDB csaConnect = _Connect.Copy();
                csaConnect.Schema = "CSA";

                er_dml.ADD_Dictionary_View(csaConnect, new Structs.Models.RevampSystem.Dictionary.AddView { I_VIEW_NAME = _ViewName, I_VIEW_TYPE = ViewType, I_VIEWDATA = ViewData });
            }

            results.Add(_result);

            return results;
        }

    }
}
