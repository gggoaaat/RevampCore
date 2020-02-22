using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.Structs;

namespace Revamp.IO.Helpers.Helpers
{
    public class DictionaryHelper
    {
        //TODO: Delete All References to This
        public DataTable FindAll(IConnectToDB _Connect, string _type)
        {
            string viewname = "";

            switch (_type.ToLower())
            {
                case "tables":
                case "table":
                    viewname = "VW__ER_TABLES";
                    break;
                case "views":
                case "view":
                    viewname = "VW__ER_VIEWS";
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    viewname = "VW__ER_PRIMARY_KEYS";
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    viewname = "VW__ER_FOREIGN_KEYS";
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    viewname = "VW__ER_UNIQUE_KEYS";
                    break;
                case "index":
                case "indexes":
                    viewname = "VW__ER_INDEXES";
                    break;
            }

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();

            if (!string.IsNullOrWhiteSpace(viewname))
            {                
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewname + "_SEARCH", new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
            }

            return TempDataTable;
        }

        public DataTable Find(IConnectToDB _Connect, string _type, string id)
        {
            ER_Query er_query = new ER_Query();

            string viewName = "";
            string columnName = "";

            switch (_type.ToLower())
            {
                case "tables":
                case "table":
                    viewName = "VW__ER_TABLES";
                    columnName = "er_tables_id";
                    break;
                case "views":
                case "view":
                    viewName = "VW__ER_VIEWS";
                    columnName = "er_views_id";
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    viewName = "VW__ER_PRIMARY_KEYS";
                    columnName = "er_primary_keys_id";
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    viewName = "VW__ER_FOREIGN_KEYS";
                    columnName = "er_foreign_keys_id";
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    viewName = "VW__ER_UNIQUE_KEYS";
                    columnName = "er_unique_keys_id";
                    break;
                case "index":
                case "indexes":
                    viewName = "VW__ER_INDEXES";
                    columnName = "er_indexes_id";
                    break;
            }

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();

            if (!string.IsNullOrWhiteSpace(viewName))
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = columnName + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = id });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH", new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            }

            return TempDataTable;
        }

        public DataTable FindbyColumnID(IConnectToDB _Connect, string _type, string _column, string _value)
        {
            string Procedure = "";
            switch (_type.ToLower())
            {
                case "tables":
                case "table":
                    Procedure = "VW__ER_TABLES";                    
                    break;
                case "views":
                case "view":
                    Procedure = "VW__ER_VIEWS";
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    Procedure = "VW__ER_PRIMARY_KEYS";
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    Procedure = "VW__ER_FOREIGN_KEYS";
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    Procedure = "VW__ER_UNIQUE_KEYS";
                    break;
                case "index":
                case "indexes":
                    Procedure = "VW__ER_INDEXES";
                    break;

            }

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();

            if (!string.IsNullOrWhiteSpace(Procedure))
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + Procedure + "_SEARCH", new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            }
            return TempDataTable;
        }

        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, string _type, List<string> _value)
        {
            string viewname = "";           

            switch (_type.ToLower())
            {
                case "tables":
                case "table":
                    viewname = "VW__ER_TABLES";
                    break;
                case "views":
                case "view":
                    viewname = "VW__ER_VIEWS";
                    break;
                case "primary":
                case "primary keys":
                case "primary key":
                case "pk":
                case "pkey":
                case "pkeys":
                    viewname = "VW__ER_PRIMARY_KEYS";
                    break;
                case "foreign":
                case "foreign keys":
                case "foreign key":
                case "fk":
                case "fkey":
                case "fkeys":
                    viewname = "VW__ER_PRIMARY_KEYS";
                    break;
                case "unqiue":
                case "unqiue keys":
                case "unqiue key":
                case "uk":
                case "ukey":
                case "ukeys":
                    viewname = "VW__ER_UNIQUE_KEYS";
                    break;
                case "index":
                case "indexes":
                    viewname = "VW__ER_INDEXES";
                    break;
            }

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();

            if (!string.IsNullOrWhiteSpace(viewname))
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column, DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = String.Join(",", _value) });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewname + "_SEARCH", new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            }
            return TempDataTable;
        }

        public ViewTablesDictionary GetTable(ViewTablesDictionary Table, DataRow _DR)
        {
            return Table = new ViewTablesDictionary
            {
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                enabled = _DR.Field<string>("enabled"),
                er_tables_id = _DR.Field<long?>("er_tables_id"),
                table_name = _DR.Field<string>("table_name"),
                table_type = _DR.Field<string>("table_type")
            };

             
        }

        public List<ViewTablesDictionary> GetTables (List<ViewTablesDictionary> ViewTables, DataTable _DT)
        {
            foreach (DataRow TableRow in _DT.Rows)
            {
                ViewTables.Add(GetTable(new ViewTablesDictionary(), TableRow));
            }

            return ViewTables;
        }

        public ViewPrimaryKeysDictionary GetPKey(ViewPrimaryKeysDictionary Table, DataRow _DR)
        {
            return Table = new ViewPrimaryKeysDictionary
            {
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                enabled = _DR.Field<string>("enabled"),
                er_primary_keys_id = _DR.Field<long?>("er_primary_keys_id"),
                table_name = _DR.Field<string>("table_name"),
                key_name = _DR.Field<string>("key_name")
            };


        }

        public List<ViewPrimaryKeysDictionary> GetPKeys(List<ViewPrimaryKeysDictionary> ViewPKeys, DataTable _DT)
        {

            foreach (DataRow PKeysRow in _DT.Rows)
            {
                ViewPKeys.Add(GetPKey(new ViewPrimaryKeysDictionary(), PKeysRow));
            }

            return ViewPKeys;
        }

        public ViewUniqueKeysDictionary GetUKey(ViewUniqueKeysDictionary UKey, DataRow _DR)
        {
            return UKey = new ViewUniqueKeysDictionary
            {
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_Created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                enabled = _DR.Field<string>("enabled"),
                er_unique_keys_id = _DR.Field<long?>("er_unique_keys_id"),
                key_name = _DR.Field<string>("key_name"),
                table_name = _DR.Field<string>("table_name")
            };
        }

        public List<ViewUniqueKeysDictionary> GetUKeys(List<ViewUniqueKeysDictionary> ViewUKeys, DataTable _DT)
        {
            foreach (DataRow PKeysRow in _DT.Rows)
            {
                ViewUKeys.Add(GetUKey(new ViewUniqueKeysDictionary(), PKeysRow));
            }

            return ViewUKeys;
        }

        public ViewForeignKeysDictionary GetFKey(ViewForeignKeysDictionary FKey, DataRow _DR)
        {
            return FKey = new ViewForeignKeysDictionary
            {
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                enabled = _DR.Field<string>("enabled"),
                er_foreign_keys_id = _DR.Field<long?>("er_foreign_keys_id"),
                key_name = _DR.Field<string>("key_name"),
                parent_table_name = _DR.Field<string>("parent_table_name"),
                table_name = _DR.Field<string>("table_name")
            };
        }

        public List<ViewForeignKeysDictionary> GetFKeys(List<ViewForeignKeysDictionary> ViewFKeys, DataTable _DT)
        {
            foreach (DataRow PKeysRow in _DT.Rows)
            {
                ViewFKeys.Add(GetFKey(new ViewForeignKeysDictionary(), PKeysRow));
            }

            return ViewFKeys;
        }

        public ViewViewsDictionary GetView(ViewViewsDictionary ViewView, DataRow _DR)
        {
            return ViewView = new ViewViewsDictionary
            {
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                enabled = _DR.Field<string>("enabled"),
                er_views_id = _DR.Field<long?>("er_views_id"),
                view_name = _DR.Field<string>("view_name"),
                view_type = _DR.Field<string>("view_type")
            };
        }

        public List<ViewViewsDictionary> GetViews(List<ViewViewsDictionary> ViewViews, DataTable _DT)
        {
            foreach (DataRow ViewRow in _DT.Rows)
            {
                ViewViews.Add(GetView(new ViewViewsDictionary(), ViewRow));
            }

            return ViewViews;
        }

        public ViewIndexesDictionary GetIndex(ViewIndexesDictionary ViewIndexes, DataRow _DR)
        {
            return ViewIndexes = new ViewIndexesDictionary
            {
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                enabled = _DR.Field<string>("enabled"),
                er_indexes_id = _DR.Field<long?>("er_indexes_id"),
                index_name = _DR.Field<string>("index_name"),
                index_type = _DR.Field<string>("index_type"),
                source_name = _DR.Field<string>("source_name"),
                source_type = _DR.Field<string>("source_type")
            };
        }

        public List<ViewIndexesDictionary> GetIndexes(List<ViewIndexesDictionary> ViewIndexes, DataTable _DT)
        {
            foreach (DataRow IndexRow in _DT.Rows)
            {
                ViewIndexes.Add(GetIndex(new ViewIndexesDictionary(), IndexRow));
            }

            return ViewIndexes;
        }
    }
}