using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Revamp.IO.Helpers.Helpers
{
    public class GripsHelper
    {
        public DataTable FindAll(IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GRIPS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable Find(IConnectToDB _Connect, string _id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GRIPS_ID_", DBType = SqlDbType.BigInt, ParamValue = _id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GRIPS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "grips_id asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GRIPS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, List<string> _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", ParamValue = String.Join(",", _value) });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GRIPS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public List<ViewObjectSetModel> GetObjectSetsViaGrip(IConnectToDB _Connect, string grips_id)
        {
            ObjectSetsHelper objectsets = new ObjectSetsHelper();

            Fetcher fetch = new Fetcher();

            DataTable objectsetsdt;

            if (grips_id.ToLower() == "all")
            {
                objectsetsdt = objectsets.FindAll(_Connect);
            }
            else
            {
                objectsetsdt = objectsets.FindbyColumnID(_Connect, "grips_id", grips_id);
            }

            List<ViewObjectSetModel> ObjectSetsList = new List<ViewObjectSetModel>();

            ViewObjectSetModel[] ObjectSets = new ViewObjectSetModel[objectsetsdt.Rows.Count];

            for (int i = 0; i < objectsetsdt.Rows.Count; i++)
            {
                string thisObjectSet = new JObject(objectsetsdt.Columns.Cast<DataColumn>()
                                         .Select(c => new JProperty(c.ColumnName, JToken.FromObject(objectsetsdt.Rows[i][c])))
                                   ).ToString(Formatting.None);

                ObjectSets[i] = JsonConvert.DeserializeObject<ViewObjectSetModel>(thisObjectSet);

                ObjectSets[i].ObjectPropSets = objectsets.GetObjectPropSetsViaObjectSet(_Connect, ObjectSets[i].object_sets_id.ToString());

                ObjectSetsList.Add(ObjectSets[i]);
            }

            return ObjectSetsList;
        }

        public GripModels SingleGripView(ViewGripModel Stage, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            ViewGripModel GripModel = JsonConvert.DeserializeObject<ViewGripModel>(thisAppRow);

            GripModels GM = new GripModels { GripView = GripModel };

            return GM;

        }

        public List<GripModels> GetGrips(IConnectToDB _Connect, StageModels Stage, Boolean _GetObjectSets, Boolean _GetPropertySets, Boolean _GetOptionProperties)
        {
            List<GripModels> GripModels = new List<GripModels>();

            DataTable _DT = FindbyColumnID(_Connect, "stages_id", Stage.StageView.stages_id.ToString());

            foreach (DataRow AppStageRow in _DT.Rows)
            {
                GripModels GripModel1 = new GripModels();
                GripModel1 = SingleGripView(new ViewGripModel(), AppStageRow);

                if (_GetObjectSets)
                {
                    ObjectSetsHelper OSH = new ObjectSetsHelper();
                    GripModel1.ObjectSets = OSH.GetSets(_Connect, GripModel1, _GetPropertySets, _GetOptionProperties);
                }


                GripModels.Add(GripModel1);
            }

            return GripModels;
        }





    }
}