using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Revamp.IO.Helpers.DataCompositions
{
    public interface IDCHelper
    {
        int TemplateOrdinal { get; }

        /// <summary>
        /// list of individual reports (most probably each defined as private static on the implementing/inheriting class)
        /// </summary>
        List<DynamicModels.RootReport> Reports { get; }

        /// <summary>
        /// this essentially returns the data structure for the report specified in thisDefinition.searchReport
        /// </summary>
        /// <param name="_Connection">db connection to use</param>
        /// <param name="thisDefinition">the report we want mapping information for</param>
        /// <returns></returns>
        DynamicModels.ReportDefinitions ReportMapper(IConnectToDB _Connection, DynamicModels.ReportDefinitions thisDefinition);

        /// <summary>
        /// todo: get rid of this in the entire chain
        /// </summary>
        /// <param name="logicModel"></param>
        /// <param name="Request"></param>
        void CustomReportLogic(DynamicModels.ReportLogicModel logicModel, HttpRequestBase Request);
    }
}
