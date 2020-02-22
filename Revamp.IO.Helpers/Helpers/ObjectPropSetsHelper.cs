using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Revamp.IO.Helpers.Helpers
{
    public class ObjectPropSetsHelper
    {

        public DataTable FindbyGripId(IConnectToDB _Connect, string gripsid, string objectsetsid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GRIPS_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = gripsid });

            if (!string.IsNullOrWhiteSpace(objectsetsid) && objectsetsid != "0")
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_SETS_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = objectsetsid });
            }

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }
        public DataTable FindAll(IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable Find(IConnectToDB _Connect, string _id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_PROP_SETS_ID_", ParamValue = _id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindbyNameandStage(IConnectToDB _Connect, string propname, string stagetype, string stagename)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PROPERTY_NAME_", ParamValue = propname });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_TYPE_", ParamValue = stagetype });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_NAME_", ParamValue = stagename });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public ViewObjectPropSetFile PropFile(ViewObjectPropSetFile PropSet, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            PropSet = JsonConvert.DeserializeObject<ViewObjectPropSetFile>(thisAppRow);

            return PropSet;
        }

        public ViewObjectPropFile PropFile(ViewObjectPropFile PropSet, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            PropSet = JsonConvert.DeserializeObject<ViewObjectPropFile>(thisAppRow);

            return PropSet;
        }

        public string GetLabelFromBytes(IConnectToDB _Connect, string objPropSetsId)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJ_PROP_SETS_ID_", DBType = SqlDbType.BigInt, ParamValue = objPropSetsId });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_FILE_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            char[] chars = new char[0];
            if (TempDataTable.Rows.Count > 0)
            {
                IOHelper io = new IOHelper();
                //labelstring= io.getStringFromBytes(TempDataTable.Rows[0]["FILE_DATA"].ToString());
                byte[] bytes = (byte[])TempDataTable.Rows[0]["FILE_DATA"];
                chars = new char[bytes.Length / sizeof(char)];
                System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);

            }
            return new string(chars);
        }
        public string GetRichtextFromBytes(IConnectToDB _Connect, long? forms_id, long? object_prop_sets_id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJ_PROP_SETS_ID_", DBType = SqlDbType.BigInt, ParamValue = object_prop_sets_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "FORMS_ID_", DBType = SqlDbType.BigInt, ParamValue = forms_id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__FORMS_DAT_FILE_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            char[] chars = new char[0];
            if (TempDataTable.Rows.Count > 0)
            {
                IOHelper io = new IOHelper();
                //labelstring= io.getStringFromBytes(TempDataTable.Rows[0]["FILE_DATA"].ToString());
                byte[] bytes = (byte[])TempDataTable.Rows[0]["Value"];
                chars = new char[bytes.Length / sizeof(char)];
                System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);

            }
            return new string(chars);
        }
        public ObjectPropSetModels FindExtras(IConnectToDB _Connect, ObjectPropSetModels PropSet, Guid? _id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJ_PROP_SETS_UUID_", DBType = SqlDbType.UniqueIdentifier, ParamValue = _id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_DAT_FILE_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            PropSet = PopulatePropertyFiles(PropSet, TempDataTable);

            ViewObjectPropSetFile OPS = new ViewObjectPropSetFile();

            return PropSet;
        }

        public List<ViewObjectPropFile> FindFiles(IConnectToDB _Connect, List<ViewObjectPropFile> Files, string _id, string type)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            if (type == "object_sets")                
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_SETS_UUID_", DBType = SqlDbType.BigInt, ParamValue = _id });
            else
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJ_PROP_SETS_UUID_", DBType = SqlDbType.BigInt, ParamValue = _id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_Dat_File_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            Files = PopulatePropertyFiles(Files, TempDataTable);

            return Files;
        }

        public string GetObjPropFile(IConnectToDB _Connect, Guid? object_sets_uuid)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();
            ER_Tools tools = new ER_Tools();
            string imageURL = "";

            if (object_sets_uuid != null)
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_SETS_UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = object_sets_uuid });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_OBJ_PROP_FILES_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "VALUE", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
            }

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("VALUE"))
            {
                byte[] value = TempDataTable.Rows[0].Field<byte[]>("VALUE");

                if (value.Length > 0)
                {
                    imageURL = tools.getStringFromBytes(value);
                }
            }

            return imageURL;
        }

        private List<ViewObjectPropFile> PopulatePropertyFiles(List<ViewObjectPropFile> Files, DataTable TempDataTable)
        {

            foreach (DataRow PropFileRow in TempDataTable.Rows)
            {
                Files.Add(PropFile(new ViewObjectPropFile(), PropFileRow));
            }

            return Files;
        }

        private ObjectPropSetModels PopulatePropertyFiles(ObjectPropSetModels PropSet, DataTable TempDataTable)
        {
            PropSet.PropSetView = new PropSetView();
            PropSet.PropSetView.PropFiles = new List<ViewObjectPropSetFile>();

            foreach (DataRow PropFileRow in TempDataTable.Rows)
            {
                PropSet.PropSetView.PropFiles.Add(PropFile(new ViewObjectPropSetFile(), PropFileRow));
            }

            return PropSet;
        }

        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", ParamValue = _value });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "obj_prop_sets_id asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, List<string> _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", ParamValue = String.Join(",", _value) });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public List<ViewObjectPropOptSetsModel> GetPropOptSetsViaPropSets(IConnectToDB _Connect, string obj_prop_sets_id)
        {
            ObjectPropOptSets objectpropoptsets = new ObjectPropOptSets();

            DataTable objectpropoptsetsdt;


            if (obj_prop_sets_id.ToLower() == "all")
            {
                objectpropoptsetsdt = objectpropoptsets.FindAll(_Connect);
            }
            else
            {
                objectpropoptsetsdt = objectpropoptsets.FindbyColumnID(_Connect, "obj_prop_sets_id", obj_prop_sets_id);
            }

            List<ViewObjectPropOptSetsModel> ObjectPropOptSetsList = new List<ViewObjectPropOptSetsModel>();

            ViewObjectPropOptSetsModel[] ObjectPropOptSets = new ViewObjectPropOptSetsModel[objectpropoptsetsdt.Rows.Count];

            int i = 0;

            foreach (DataRow datarowdc in objectpropoptsetsdt.Rows)
            {

                ObjectPropOptSets[i] = new ViewObjectPropOptSetsModel
                {
                    dt_available = datarowdc.Field<DateTime?>("dt_available"),
                    dt_created = datarowdc.Field<DateTime>("dt_created"),
                    dt_end = datarowdc.Field<DateTime?>("dt_end"),
                    enabled = datarowdc.Field<string>("enabled"),
                    has_child = datarowdc.Field<string>("has_child"),
                    has_parent = datarowdc.Field<string>("has_parent"),
                    obj_prop_opt_sets_id = datarowdc.Field<long?>("obj_prop_opt_sets_id"),
                    obj_prop_sets_id = datarowdc.Field<long?>("obj_prop_sets_id"),
                    object_prop_type = datarowdc.Field<string>("object_prop_type"),
                    object_sets_id = datarowdc.Field<long?>("object_sets_id"),
                    option_value = datarowdc.Field<string>("option_value"),
                    option_name = datarowdc.Field<string>("option_name"),
                    parent_obj_prop_sets_id = datarowdc.Field<long?>("parent_obj_prop_sets_id"),
                    property_name = datarowdc.Field<string>("property_name"),
                    property_value = datarowdc.Field<string>("property_value"),
                    value_datatype = datarowdc.Field<string>("value_datatype")

                };
                //ObjectPropSets[i].stage_name = datarowdc["stage_name"].ToString();
                ObjectPropOptSetsList.Add(ObjectPropOptSets[i]);
                i++;
            }

            return ObjectPropOptSetsList;
        }

        public ObjectPropSetModels SinglePropSetView(PropSetView SinglePropSetView, DataRow _DR)
        {
            SinglePropSetView = new PropSetView
            {
                containers_id = _DR.Field<long?>("containers_id"),
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                stage_name = _DR.Field<string>("stage_name"),
                stage_type = _DR.Field<string>("stage_type"),
                grip_name = _DR.Field<string>("grip_name"),
                grip_type = _DR.Field<string>("grip_type"),
                identities_id = _DR.Field<long?>("identities_id"),
                object_type = _DR.Field<string>("object_type"),
                enabled = _DR.Field<string>("enabled"),
                object_layer = _DR.Field<string>("object_layer"),
                object_sets_id = _DR.Field<long?>("object_sets_id"),
                has_child = _DR.Field<string>("has_child"),
                has_parent = _DR.Field<string>("has_parent"),
                obj_prop_sets_id = _DR.Field<long?>("obj_prop_sets_id"),
                object_prop_type = _DR.Field<string>("object_prop_type"),
                property_name = _DR.Field<string>("property_name"),
                parent_obj_prop_sets_id = _DR.Field<long?>("parent_obj_prop_sets_id"),
                property_value = _DR.Field<string>("property_value"),
                value_datatype = _DR.Field<string>("value_datatype"),
                grips_id = _DR.Field<long?>("grips_id"),
            };

            ObjectPropSetModels Set = new ObjectPropSetModels { PropSetView = SinglePropSetView };

            return Set;

        }


        public List<ObjectPropSetModels> GetProps(IConnectToDB _Connect, ObjectSetModels Set, Boolean _GetOptionProperties)
        {
            List<ObjectPropSetModels> PropSetModels = new List<ObjectPropSetModels>();

            ObjectPropSetModels PropSet = new ObjectPropSetModels();
            //Set = new ObjectSetModels();

            DataTable _DT = FindbyColumnID(_Connect, "object_sets_id", Set.SetView.object_sets_id.ToString());

            foreach (DataRow PropSetRow in _DT.Rows)
            {
                PropSet = SinglePropSetView(PropSet.PropSetView, PropSetRow);

                if (_GetOptionProperties)
                {
                    //ObjectSetsHelper OSH = new ObjectSetsHelper();

                    //GripModel1.ObjectSets = OSH.GetSets(GripModel1, _GetPropertySets, _GetOptionProperties);
                }


                PropSetModels.Add(PropSet);
            }

            return PropSetModels;
        }

    }








}