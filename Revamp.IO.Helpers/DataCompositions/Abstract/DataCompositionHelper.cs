using System.Collections.Generic;
using System.Web;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;

namespace Revamp.IO.Helpers.DataCompositions
{
    public abstract class DataCompositionHelper : IDCHelper
    {
        public abstract int TemplateOrdinal { get; }
        public abstract DynamicModels.ReportDefinitions ReportMapper(IConnectToDB _Connection, DynamicModels.ReportDefinitions thisDefinition);
        public abstract List<DynamicModels.RootReport> Reports { get; }

        public virtual void CustomReportLogic(DynamicModels.ReportLogicModel logicModel, HttpRequestBase Request)
        {
            foreach (DynamicModels.RootReportFilter _thisReport in logicModel.CurrentReportSelected.ReportFilters)
            {
                #region Basic List Search
                DynamicModels.RootReportFilter thisCurrentFilter = Tools.Box.Clone<DynamicModels.RootReportFilter>(_thisReport);
                var P_THIS_FILTERNAME = logicModel.useQsCol ? logicModel.qscol.Get("P_" + thisCurrentFilter.FilterName) : Request.Params["P_" + thisCurrentFilter.FilterName];

                if (P_THIS_FILTERNAME != null && logicModel.UsedFilters.Exists(S => S.FilterName.ToUpper() == thisCurrentFilter.FilterName.ToUpper()) == false)
                {
                    thisCurrentFilter.DBType = thisCurrentFilter.SearchDBType;
                    thisCurrentFilter.ParamSize = thisCurrentFilter.SearchParamSize;
                    thisCurrentFilter.ParamValue = P_THIS_FILTERNAME.ToString();
                    logicModel.UsedFilters.Add(thisCurrentFilter);
                }
                #endregion

                #region Equals Search
                DynamicModels.RootReportFilter thisCurrentFilter_ = Tools.Box.Clone<DynamicModels.RootReportFilter>(_thisReport);
                var P_THIS_FILTERNAME_ = logicModel.useQsCol ? logicModel.qscol.Get("P_" + thisCurrentFilter_.FilterName + "_") : Request.Params["P_" + thisCurrentFilter_.FilterName + "_"];

                if (P_THIS_FILTERNAME_ != null && logicModel.UsedFilters.Exists(S => S.FilterName.ToUpper() == thisCurrentFilter_.FilterName.ToUpper() + "_") == false)
                {
                    thisCurrentFilter_.DBType = thisCurrentFilter_.SearchDBType;
                    thisCurrentFilter_.FilterName = thisCurrentFilter_.FilterName.ToUpper() + "_";
                    thisCurrentFilter_.ParamSize = thisCurrentFilter_.SearchParamSize;
                    thisCurrentFilter_.ParamValue = P_THIS_FILTERNAME_.ToString();
                    logicModel.UsedFilters.Add(thisCurrentFilter_);
                }
                #endregion

                #region Basic Not in List Search
                DynamicModels.RootReportFilter thisCurrentFilterExclude = Tools.Box.Clone<DynamicModels.RootReportFilter>(_thisReport);
                var P_THIS_EXCLUDE_FILTERNAME = logicModel.useQsCol ? logicModel.qscol.Get("P_EXCLUDE_" + thisCurrentFilterExclude.FilterName) : Request.Params["P_EXCLUDE_" + thisCurrentFilterExclude.FilterName];

                if (P_THIS_EXCLUDE_FILTERNAME != null && logicModel.UsedFilters.Exists(S => S.FilterName.ToUpper() == "EXCLUDE_" + thisCurrentFilterExclude.FilterName.ToUpper()) == false)
                {
                    thisCurrentFilterExclude.DBType = thisCurrentFilterExclude.SearchDBType;
                    thisCurrentFilterExclude.ParamSize = thisCurrentFilterExclude.SearchParamSize;
                    thisCurrentFilterExclude.ParamValue = P_THIS_EXCLUDE_FILTERNAME.ToString();
                    thisCurrentFilterExclude.FilterName = "EXCLUDE_" + thisCurrentFilterExclude.FilterName;
                    logicModel.UsedFilters.Add(thisCurrentFilterExclude);
                } 
                #endregion
            }
        }
    }
}
