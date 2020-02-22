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
    public class ObjectSetsHelper
    {
        public DataTable FindAll(IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJECT_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable Find(IConnectToDB _Connect, string _id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_SETS_ID_", ParamValue = _id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJECT_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", ParamValue = _value });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJECT_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, List<string> _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column, ParamValue = String.Join(",", _value) });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJECT_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public List<ViewObjectPropSetsModel> GetObjectPropSetsViaObjectSet(IConnectToDB _Connect, string object_sets_id)
        {
            //ObjectSets objectsets = new ObjectSets();
            ObjectPropSetsHelper objectpropsets = new ObjectPropSetsHelper();

            DataTable objectpropsetsdt;

            if (object_sets_id.ToLower() == "all")
            {
                objectpropsetsdt = objectpropsets.FindAll(_Connect);
            }
            else
            {
                objectpropsetsdt = objectpropsets.FindbyColumnID(_Connect, "object_sets_id", object_sets_id);
            }

            List<ViewObjectPropSetsModel> ObjectPropSetsList = new List<ViewObjectPropSetsModel>();

            ViewObjectPropSetsModel[] ObjectPropSets = new ViewObjectPropSetsModel[objectpropsetsdt.Rows.Count];

            for (int i = 0; i < objectpropsetsdt.Rows.Count; i++)
            {
                string thisPropSet = new JObject(objectpropsetsdt.Columns.Cast<DataColumn>()
                                        .Select(c => new JProperty(c.ColumnName, JToken.FromObject(objectpropsetsdt.Rows[i][c])))
                                  ).ToString(Formatting.None);

                ObjectPropSets[i] = JsonConvert.DeserializeObject<ViewObjectPropSetsModel>(thisPropSet);

                ObjectPropSets[i].ObjectPropOptSets = objectpropsets.GetPropOptSetsViaPropSets(_Connect, ObjectPropSets[i].obj_prop_sets_id.ToString());

                ObjectPropSetsList.Add(ObjectPropSets[i]);
            }

            return ObjectPropSetsList;
        }

        public ObjectSetModels SingleSetView(ViewObjectSetModel SetView, DataRow _DR)
        {
            DataTable thisObjectSetDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisObjectSetDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisObjectSetDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            ViewObjectSetModel SetModel = JsonConvert.DeserializeObject<ViewObjectSetModel>(thisAppRow);

            ObjectSetModels OSM = new ObjectSetModels { SetView = SetModel };

            return OSM;
        }

        public List<ObjectSetModels> GetSets(IConnectToDB _Connect, GripModels Grip, Boolean _GetPropertySets, Boolean _GetOptionProperties)
        {
            List<ObjectSetModels> Sets = new List<ObjectSetModels>();

            DataTable _DT = FindbyColumnID(_Connect, "grips_id", Grip.GripView.grips_id.ToString());

            foreach (DataRow GripSetRow in _DT.Rows)
            {
                ObjectSetModels Set = new ObjectSetModels();
                Set.SetView = new ViewObjectSetModel();
                Set = SingleSetView(Set.SetView, GripSetRow);

                if (_GetPropertySets)
                {
                    ObjectPropSetsHelper PSH = new ObjectPropSetsHelper();
                    Set.ObjectPropSets = PSH.GetProps(_Connect, Set, _GetOptionProperties);

                }

                Sets.Add(Set);
            }

            return Sets;
        }

    }
}