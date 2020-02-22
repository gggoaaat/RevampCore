using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Revamp.IO.SQL.Generators
{
    public class ER_Generate
    {
        public static List<CommandResult> _GENERATE_VIEW(IConnectToDB _Connect, string OriginalTable, string ViewType)
        {
            ER_Generate gen = new ER_Generate();

            return gen.GENERATE_VIEW(_Connect, OriginalTable, ViewType);
        }

        public static List<CommandResult> _GENERATE_VIEW_WITH_CHILDREN(IConnectToDB _Connect, string OriginalTable, string ViewType)
        {
            ER_Generate gen = new ER_Generate();

            return gen.GENERATE_VIEW_WITH_CHILDREN(_Connect, OriginalTable, ViewType);
        }

        public List<CommandResult> GENERATE_VIEW(IConnectToDB _Connect, string OriginalTable, string ViewType)
        {
            List<CommandResult> Logger = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            ER_DDL er_ddl = new ER_DDL();
            DBViews dbViews = new DBViews();
            Tools.Box er_tools = new Tools.Box();

            string _Schema = DBTools.GetSchema(_Connect);

            DataTable SourcedTables = new DataTable();

            StringBuilder sqlquery = new StringBuilder();

            //Find All Parent Tables related to Original Table Submitted
            SourcedTables = GET_ALL_PARENT_TABLES(_Connect, OriginalTable);

            List<DataTable> TableColumns = new List<DataTable>();

            List<ColumnStructure> cs_Select = new List<ColumnStructure>();
            List<sqlSelectStructure> sqlStruct = new List<sqlSelectStructure>();
            List<WhereStructure> sqlWhere = new List<WhereStructure>();

            cs_Select = Build_Column_Struct(_Connect, OriginalTable, SourcedTables);
            sqlStruct = Build_Select_Struct(_Connect, cs_Select, OriginalTable, SourcedTables);

            string temp = er_tools.MaxNameLength(OriginalTable.ToUpper(), (128 - 3));

            StringBuilder _sqlIn = new StringBuilder();
            var viewName = "VW__" + temp;
            _sqlIn.AppendLine("DROP VIEW " + _Schema + "." + viewName);

            CommandResult _Result = new CommandResult();

            #region Drop Search Procedure
            _sqlIn = new StringBuilder();
            _sqlIn.AppendLine("DROP PROCEDURE " + _Schema + ".SP_S_" + viewName + "_SEARCH");
            _Result = new CommandResult();
            string SuccessMessage = "Dropped Procedure";
            _Result._StartTime = DateTime.Now;
            _Result._Response = ER_Query._RUN_NON_QUERY(_Connect, _sqlIn.ToString(), SuccessMessage);
            _Result._Successful = _Result._Response.IndexOf(SuccessMessage, StringComparison.Ordinal) > -1 || _Result._Response.IndexOf("exist", StringComparison.Ordinal) > -1
                    ? true
                    : false;
            _Result._EndTime = DateTime.Now;
            Logger.Add(_Result);
            #endregion

            #region Drop View
            SuccessMessage = "Dropped View";
            _Result._StartTime = DateTime.Now;
            _Result._Response = ER_Query._RUN_NON_QUERY(_Connect, _sqlIn.ToString(), SuccessMessage
            );
            _Result._Successful =
                _Result._Response.IndexOf(SuccessMessage, StringComparison.Ordinal) > -1 ||
                _Result._Response.IndexOf("exist", StringComparison.Ordinal) > -1
                    ? true
                    : false;
            _Result._EndTime = DateTime.Now;
            Logger.Add(_Result);
            #endregion;

            Logger.AddRange(dbViews.ADD_VIEW(_Connect, temp, sqlStruct, ViewType, true));

            #region Generate Search
            Logger.AddRange(ER_Generate.CreateProcedureSearchProcedure(_Connect, "Eminent IT", _Connect.Schema, "", "",
                new List<CommandResult>(),
                selectStruct: new SearchProcedureStruct
                {
                    ProcedurePrefix = "SP_S",
                    GetLatestVersion = true,
                    ProcedureName = viewName + "_SEARCH",
                    SourceName = viewName
                }));
            #endregion

            AfterViewCreated(_Connect, OriginalTable, Logger, er_ddl, temp);

            return Logger;

            //Get 1st Layer Parent Tables
            ////Need a module which fetches the Parent Tables.

            //Get All Columns Related to Parent Tables
            ////Loop Through each table and get columns with 

            //Merge All Columns. Never allow Duplicate Names

            //Create Select structure from all the 

            //Create First sqlStruct for the Original Table being requested

            //Loop Through each Parent Table and create Join on Condition

            //Where Clause will not be used for 

        }

        public List<CommandResult> GENERATE_VIEW_WITH_CHILDREN(IConnectToDB _Connect, string OriginalTable, string ViewType)
        {
            List<CommandResult> Logger = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            ER_DDL er_ddl = new ER_DDL();
            DBViews dbViews = new DBViews();
            Tools.Box er_tools = new Tools.Box();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            DataTable SourcedTables = new DataTable();
            DataTable SourcedChildTables = new DataTable();

            StringBuilder sqlquery = new StringBuilder();

            //Find All Parent Tables related to Original Table Submitted
            SourcedTables = GET_ALL_PARENT_TABLES(_Connect, OriginalTable);

            SourcedChildTables = GET_ALL_W_CHILD_TABLES(_Connect, OriginalTable);


            List<DataTable> TableColumns = new List<DataTable>();

            List<ColumnStructure> cs_Select = new List<ColumnStructure>();
            List<ColumnStructure> cs_Select2 = new List<ColumnStructure>();
            List<sqlSelectStructure> sqlStruct = new List<sqlSelectStructure>();
            List<WhereStructure> sqlWhere = new List<WhereStructure>();

            cs_Select = Build_Column_Struct(_Connect, OriginalTable, SourcedTables);
            if (SourcedChildTables.Rows.Count > 0)
            {
                cs_Select = Build_Child_TO_XML_STUFF(_Connect, cs_Select, SourcedChildTables);
            }
            sqlStruct = Build_Select_Struct(_Connect, cs_Select, OriginalTable, SourcedTables);
            //if (SourcedChildTables.Rows.Count > 0)
            //{
            //    sqlStruct = Build_Child_Select_Struct(_Connect, sqlStruct, cs_Select, SourcedChildTables); 
            //}

            string temp = er_tools.MaxNameLength(OriginalTable.ToUpper(), (128 - 3));

            StringBuilder _sqlIn = new StringBuilder();
            var viewName = "VW__" + temp;


            CommandResult _Result = new CommandResult();

            #region Drop Search Procedure
            _sqlIn = new StringBuilder();
            _sqlIn.AppendLine("DROP PROCEDURE " + _Schema + ".SP_S_" + viewName + "_SEARCH");
            _Result = new CommandResult();
            string SuccessMessage = "Dropped Procedure";
            _Result._StartTime = DateTime.Now;
            _Result._Response = ER_Query._RUN_NON_QUERY(_Connect, _sqlIn.ToString(), SuccessMessage);
            _Result._Successful = _Result._Response.IndexOf(SuccessMessage, StringComparison.Ordinal) > -1 || _Result._Response.IndexOf("exist", StringComparison.Ordinal) > -1
                    ? true
                    : false;
            _Result._EndTime = DateTime.Now;
            Logger.Add(_Result);
            #endregion

            #region Drop View
            _sqlIn = new StringBuilder();
            _sqlIn.AppendLine("DROP VIEW " + _Schema + "." + viewName);
            SuccessMessage = "Dropped View";
            _Result._StartTime = DateTime.Now;
            _Result._Response = ER_Query._RUN_NON_QUERY(_Connect, _sqlIn.ToString(), SuccessMessage
            );
            _Result._Successful =
                _Result._Response.IndexOf(SuccessMessage, StringComparison.Ordinal) > -1 ||
                _Result._Response.IndexOf("exist", StringComparison.Ordinal) > -1
                    ? true
                    : false;
            _Result._EndTime = DateTime.Now;
            Logger.Add(_Result);
            #endregion;

            Logger.AddRange(dbViews.ADD_VIEW(_Connect, temp, sqlStruct, ViewType, true));

            #region Generate Search
            Logger.AddRange(ER_Generate.CreateProcedureSearchProcedure(_Connect, "Eminent IT", _Connect.Schema, "", "",
                new List<CommandResult>(),
                selectStruct: new SearchProcedureStruct
                {
                    ProcedurePrefix = "SP_S",
                    GetLatestVersion = true,
                    ProcedureName = viewName + "_SEARCH",
                    SourceName = viewName
                }));
            #endregion

            AfterViewCreated(_Connect, OriginalTable, Logger, er_ddl, temp);

            return Logger;

            //Get 1st Layer Parent Tables
            ////Need a module which fetches the Parent Tables.

            //Get All Columns Related to Parent Tables
            ////Loop Through each table and get columns with 

            //Merge All Columns. Never allow Duplicate Names

            //Create Select structure from all the 

            //Create First sqlStruct for the Original Table being requested

            //Loop Through each Parent Table and create Join on Condition

            //Where Clause will not be used for 

        }

        private static void AfterViewCreated(IConnectToDB _Connect, string OriginalTable, List<CommandResult> Logger, ER_DDL er_ddl, string temp)
        {
            List<ColumnStructure> cs_ViewColumns = new List<ColumnStructure>();
            cs_ViewColumns.Add(new ColumnStructure { _Name = OriginalTable + "_ID" });

            Logger.AddRange(er_ddl.ADD_INDEX_CLUSTERED(_Connect, temp, "VW__" + temp, "VIEW", cs_ViewColumns, true));

            cs_ViewColumns.Clear();

            ER_DML er_dml = new ER_DML();

            DataTable Stage_Columns_DT = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, "VW__" + temp, false);
            cs_ViewColumns.Add(new ColumnStructure { _Name = OriginalTable + "_ID" });

            int i = 1;
            foreach (DataRow _DR in Stage_Columns_DT.Rows)
            {
                if (_DR.Field<string>("DATA_TYPE").ToLower() != "varbinary")
                {
                    var thisLength = _DR.Field<Int32?>("DATA_LENGTH");

                    //900 is max length of a index column
                    if (thisLength == null || thisLength <= 900 && thisLength != -1)
                    {
                        cs_ViewColumns.Add(new ColumnStructure { _Name = _DR.Field<string>("COLUMN_NAME") });
                    }
                    i++;
                }

                //Why 16 well that's the maximum amount of indexible columns
                if (cs_ViewColumns.Count == 16)
                {
                    break;
                }
            }

            Logger.AddRange(er_ddl.ADD_INDEX_NONCLUSTERED(_Connect, temp, "VW__" + temp, "VIEW", cs_ViewColumns));
        }


        // Comes to life after PATCH_201501181525
        public List<CommandResult> GENERATE_VIEW(IConnectToDB _Connect, string OriginalTable, string ViewType, string ViewData)
        {
            CommandResult _result = new CommandResult();
            ER_DDL er_ddl = new ER_DDL();
            DBViews dbViews = new DBViews();
            Tools.Box er_tools = new Tools.Box();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            DataTable SourcedTables = new DataTable();

            StringBuilder sqlquery = new StringBuilder();

            //Find All Parent Tables related to Original Table Submitted
            SourcedTables = GET_ALL_PARENT_TABLES(_Connect, OriginalTable);

            List<DataTable> TableColumns = new List<DataTable>();

            List<CommandResult> Logger = new List<CommandResult>();

            List<ColumnStructure> cs_Select = new List<ColumnStructure>();
            List<sqlSelectStructure> sqlStruct = new List<sqlSelectStructure>();
            List<WhereStructure> sqlWhere = new List<WhereStructure>();

            cs_Select = Build_Column_Struct(_Connect, OriginalTable, SourcedTables);
            sqlStruct = Build_Select_Struct(_Connect, cs_Select, OriginalTable, SourcedTables, ViewData);

            string temp = er_tools.MaxNameLength(OriginalTable, 26);
            Logger.AddRange(dbViews.ADD_VIEW(_Connect, temp, sqlStruct, ViewType, true, ViewData));

            switch (ViewData.ToLower())
            {

                case "last 24hrs":
                case "last 48hrs":
                case "last hour":
                    //Null Do Nothing                    
                    break;
                default:

                    List<ColumnStructure> cs_ViewColumns = new List<ColumnStructure>();
                    cs_ViewColumns.Add(new ColumnStructure { _Name = OriginalTable + "_ID" });

                    Logger.AddRange(er_ddl.ADD_INDEX_CLUSTERED(_Connect, temp, "VW__" + temp, "VIEW", cs_ViewColumns, true));

                    cs_ViewColumns.Clear();

                    ER_DML er_dml = new ER_DML();

                    DataTable Stage_Columns_DT = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, "VW__" + temp, false);
                    cs_ViewColumns.Add(new ColumnStructure { _Name = OriginalTable + "_ID" });

                    int i = 1;
                    foreach (DataRow _DR in Stage_Columns_DT.Rows)
                    {
                        if (_DR.Field<string>("DATA_TYPE").ToLower() != "varbinary")
                        {
                            cs_ViewColumns.Add(new ColumnStructure { _Name = _DR.Field<string>("COLUMN_NAME") });
                            i++;
                        }

                        if (i == 16)
                        {
                            break;
                        }
                    }

                    Logger.AddRange(er_ddl.ADD_INDEX_NONCLUSTERED(_Connect, temp, "VW__" + temp, "VIEW", cs_ViewColumns));
                    break;

            }
            return Logger;

            //Get 1st Layer Parent Tables
            ////Need a module which fetches the Parent Tables.

            //Get All Columns Related to Parent Tables
            ////Loop Through each table and get columns with 

            //Merge All Columns. Never allow Duplicate Names

            //Create Select structure from all the 

            //Create First sqlStruct for the Original Table being requested

            //Loop Through each Parent Table and create Join on Condition

            //Where Clause will not be used for 

        }

        public DataTable GET_ALL_PARENT_TABLES(IConnectToDB _Connect, string TableName)
        {
            StringBuilder sqlquery = new StringBuilder();
            //Get All Columns Related to Original Table
            sqlquery.Append("Select a.TABLE_SCHEMA, a.TABLE_NAME, a.PARENT_SCHEMA, a.PARENT_TABLE_NAME, a.KEY_NAME, b.TABLE_TYPE from CSA.ER_FOREIGN_KEYS a inner join CSA.ER_TABLES b on a.TABLE_NAME = b.TABLE_NAME WHERE ");
            sqlquery.AppendLine("a.TABLE_NAME = @TABLE_NAME");

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sqlquery.ToString(),
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = TableName } }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            return _DT;
        }

        public DataTable GET_ALL_W_CHILD_TABLES(IConnectToDB _Connect, string TableName)
        {
            StringBuilder sqlquery = new StringBuilder();
            //Get All Columns Related to Original Table
            sqlquery.Append("Select a.TABLE_SCHEMA, a.TABLE_NAME, a.PARENT_SCHEMA, a.PARENT_TABLE_NAME, a.KEY_NAME, b.TABLE_TYPE from CSA.ER_FOREIGN_KEYS a inner join CSA.ER_TABLES b on a.TABLE_NAME = b.TABLE_NAME WHERE ");
            sqlquery.AppendLine("a.PARENT_TABLE_NAME = @TABLE_NAME");

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sqlquery.ToString(),
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = TableName } }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            return _DT;
        }

        public List<ColumnStructure> Build_Column_Struct(IConnectToDB _Connect, string TableName, DataTable ParentTables)
        {

            ER_DML er_dml = new ER_DML();


            DataTable SourcedTables = new DataTable();
            StringBuilder sqlquery = new StringBuilder();
            List<ColumnStructure> cs_Select = new List<ColumnStructure>();
            DataTable TempDataColumns = new DataTable();
            DataTable TempDataColumns2 = new DataTable();
            string TableNames = "";
            TempDataColumns.Clear();

            //cs_Select.Add(new ColumnStructure { _Name = TableName + "_ID", _Table = "a0" });
            //cs_Select.Add(new ColumnStructure { _Name = "BASE_" + TableName + "_ID", _Table = "a0" });
            //cs_Select.Add(new ColumnStructure { _Name = "PREV_" + TableName + "_ID", _Table = "a0" });
            //cs_Select.Add(new ColumnStructure { _Name = TableName + "_UUID", _Table = "a0" });
            //cs_Select.Add(new ColumnStructure { _Name = "BASE_" + TableName + "_UUID", _Table = "a0" });
            //cs_Select.Add(new ColumnStructure { _Name = "PREV_" + TableName + "_UUID", _Table = "a0" });
            //if (TableName.ToUpper() != "IDENTITIES")
            //{
            //    cs_Select.Add(new ColumnStructure { _Name = "IDENTITIES_ID", _Table = "a0" });
            //}
            //cs_Select.Add(new ColumnStructure { _Name = "Enabled", _Table = "a0" });
            //cs_Select.Add(new ColumnStructure { _Name = "DT_CREATED", _Table = "a0" });
            //cs_Select.Add(new ColumnStructure { _Name = "DT_AVAILABLE", _Table = "a0" });
            //cs_Select.Add(new ColumnStructure { _Name = "DT_END", _Table = "a0" });

            TempDataColumns = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName);

            foreach (DataRow i in TempDataColumns.Rows)
            {

                if (!cs_Select.Exists(thisColumn => thisColumn._Name == i["COLUMN_NAME"].ToString().ToUpper()))
                {
                    string ColumnName = i["COLUMN_NAME"].ToString().ToUpper();
                    cs_Select.Add(new ColumnStructure { _Name = ColumnName, _Table = "a0", _Alias = ColumnName });
                }
            }

            int Looper = 0;
            foreach (DataRow ii in ParentTables.Rows)
            {
                Looper++;
                TempDataColumns = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, ii["PARENT_TABLE_NAME"].ToString());

                TableNames += ii["PARENT_TABLE_NAME"].ToString() + " ";
                string tempstring = "";
                foreach (DataRow iv in TempDataColumns.Rows)
                {
                    tempstring = iv["DATA_TYPE"].ToString().ToLower() == "LONG RAW" ? "RAWTOHEX(" + iv["COLUMN_NAME"].ToString() + ")" : iv["COLUMN_NAME"].ToString().ToUpper();

                    if (cs_Select.Exists(thisColumn => thisColumn._Name == iv["COLUMN_NAME"].ToString().ToUpper()))
                    {
                        AliasRedundantColumn(cs_Select, "a", "PARENT_TABLE_NAME", Looper, ii, tempstring, iv);

                    }
                    else
                    {
                        cs_Select.Add(new ColumnStructure
                        {
                            _Name = tempstring,
                            _Table = "a" + Looper.ToString(),
                            _Alias = tempstring
                        });
                    }
                }

                TempDataColumns2.Clear();
            }



            return cs_Select;

        }

        public List<ColumnStructure> Build_Child_Column_Struct(IConnectToDB _Connect, List<ColumnStructure> cs_Select, DataTable ParentTables)
        {

            ER_DML er_dml = new ER_DML();


            DataTable SourcedTables = new DataTable();
            StringBuilder sqlquery = new StringBuilder();
            DataTable TempDataColumns = new DataTable();
            DataTable TempDataColumns2 = new DataTable();
            string TableNames = "";
            TempDataColumns.Clear();

            int Looper = 0;
            foreach (DataRow ii in ParentTables.Rows)
            {
                Looper++;
                TempDataColumns = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, ii["TABLE_NAME"].ToString());

                TableNames += ii["TABLE_NAME"].ToString() + " ";
                string tempstring = "";
                foreach (DataRow iv in TempDataColumns.Rows)
                {
                    tempstring = iv["DATA_TYPE"].ToString().ToLower() == "LONG RAW" ? "RAWTOHEX(" + iv["COLUMN_NAME"].ToString() + ")" : iv["COLUMN_NAME"].ToString().ToUpper();

                    if (cs_Select.Exists(thisColumn => thisColumn._Name == iv["COLUMN_NAME"].ToString().ToUpper()))
                    {
                        AliasRedundantColumn(cs_Select, "c", "TABLE_NAME", Looper, ii, tempstring, iv);

                    }
                    else
                    {
                        cs_Select.Add(new ColumnStructure
                        {
                            _Name = tempstring,
                            _Table = "c" + Looper.ToString(),
                            _Alias = tempstring
                        });
                    }
                }

                TempDataColumns2.Clear();
            }



            return cs_Select;

        }

        public List<ColumnStructure> Build_Child_TO_XML_STUFF(IConnectToDB _Connect, List<ColumnStructure> cs_Select, DataTable ParentTables)
        {

            ER_DML er_dml = new ER_DML();


            DataTable SourcedTables = new DataTable();
            StringBuilder sqlquery = new StringBuilder();
            DataTable TempDataColumns = new DataTable();
            DataTable TempDataColumns2 = new DataTable();
            string TableNames = "";
            TempDataColumns.Clear();

            int Looper = 0;
            foreach (DataRow ii in ParentTables.Rows)
            {
                Looper++;
                TempDataColumns = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, ii["TABLE_NAME"].ToString());

                TableNames += ii["TABLE_NAME"].ToString() + " ";
                string tempstring = "";
                var fkCondition = GENERATE_JOIN_ON_FK(_Connect, ii["KEY_NAME"].ToString(), ii["TABLE_NAME"].ToString(), "c" + Looper, ii["PARENT_TABLE_NAME"].ToString(), "a0");

                foreach (DataRow iv in TempDataColumns.Rows)
                {
                    //    tempstring = iv["DATA_TYPE"].ToString().ToLower() == "LONG RAW" ? "RAWTOHEX(" + iv["COLUMN_NAME"].ToString() + ")" : iv["COLUMN_NAME"].ToString().ToUpper();

                    if (cs_Select.Exists(thisColumn => thisColumn._Name == iv["COLUMN_NAME"].ToString().ToUpper()))
                    {
                        // AliasRedundantColumn(cs_Select, "c", "TABLE_NAME", Looper, ii, tempstring, iv);

                    }
                    else
                    {
                        Dictionary<string, object> thisDic = new Dictionary<string, object>();
                        thisDic["schema"] = ii["TABLE_SCHEMA"].ToString();
                        thisDic["table"] = TableNames;
                        thisDic["tableAlias"] = "c" + Looper;
                        thisDic["columnname"] = iv["COLUMN_NAME"].ToString().ToUpper();
                        thisDic["fkCondition"] = fkCondition; //dynamicallly generate               

                        cs_Select.Add(new ColumnStructure
                        {
                            _Name = "",
                            _SelectSQL = GetStuff(thisDic),
                            _Alias = iv["COLUMN_NAME"].ToString().ToUpper()
                        });
                    }
                }

                TempDataColumns2.Clear();
            }



            return cs_Select;

        }

        private string GetStuff(Dictionary<string, object> meta)
        {
            return string.Format("STUFF((SELECT ',' + convert(varchar(MAX),{2}) FROM {0}.{1} {3} " +
                " WHERE {4} ORDER BY {2} FOR XML PATH('')), 1, 1, '' )",
                meta["schema"], meta["table"], meta["columnname"], meta["tableAlias"], meta["fkCondition"]);
        }

        private static void AliasRedundantColumn(List<ColumnStructure> cs_Select, string sourceAlias, string sourceColumn, int Looper, DataRow ii, string tempstring, DataRow iv)
        {
            var initialAlias = (iv["COLUMN_NAME"] + "_F_" + ii[sourceColumn]).ToUpper();

            List<ColumnStructure> commonAlias = cs_Select.FindAll(thisAlias => thisAlias._Alias.ToUpper().Contains(initialAlias));

            if (commonAlias != null && commonAlias.Count > 0)
            {
                cs_Select.Add(new ColumnStructure
                {
                    _Name = tempstring,
                    _Alias = (iv["COLUMN_NAME"] + "_F_" + ii[sourceColumn]).ToUpper() + "_" + commonAlias.Count,
                    _Table = sourceAlias + Looper.ToString()
                });
            }
            else
            {
                cs_Select.Add(new ColumnStructure
                {
                    _Name = tempstring,
                    _Alias = (iv["COLUMN_NAME"].ToString() + "_F_" + ii[sourceColumn]).ToUpper(),
                    _Table = sourceAlias + Looper.ToString()
                });
            }
        }

        public List<sqlSelectStructure> Build_Select_Struct(IConnectToDB _Connect, List<ColumnStructure> QueryColumns, string TableName, DataTable ParentTables)
        {
            DataTable SourcedTables = new DataTable();
            StringBuilder sqlquery = new StringBuilder();
            List<sqlSelectStructure> sqlStruct = new List<sqlSelectStructure>();
            string TableNames = "";

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            sqlStruct.Add(new sqlSelectStructure
            {
                _TableName = _Schema + "." + TableName,
                _TableAlias = "a0",
                _IncludeColumns = QueryColumns,
                _HasFrom = true,
                _HasJoin = false,
                _HasWhere = false,
                _HasAggregateFunc = false,
                _HasGroupBy = false,
                _HasHaving = false
            });
            //sqlStruct.Add(new sqlSelectStructure
            //{
            //    _TableName = owner + "." + TableName,
            //    _TableAlias = "a0a",
            //    _HasFrom = false,
            //    _HasJoin = true,
            //    _JoinClause = "INNER JOIN",
            //    _JoinOn = "a0." + TableName + "_ID = a0a." + TableName + "_ID and + a0a.ENABLED = 'Y'",
            //    _HasWhere = false,
            //    _HasAggregateFunc = false,
            //    _HasGroupBy = false,
            //    _HasHaving = false
            //});

            int Looper = 0;

            string TempLoop;



            foreach (DataRow i in ParentTables.Rows)
            {
                Looper++;
                TempLoop = "a" + Looper.ToString();
                TableNames += i["PARENT_TABLE_NAME"].ToString() + " | ";

                sqlStruct.Add(new sqlSelectStructure
                {
                    _TableName = i["PARENT_SCHEMA"].ToString() + "." + i["PARENT_TABLE_NAME"].ToString(),
                    _ChildTableName = i["TABLE_SCHEMA"].ToString() + "." + i["TABLE_NAME"].ToString(),
                    _TableAlias = "a" + Looper.ToString(),
                    _HasFrom = false,
                    _HasJoin = true,
                    _JoinClause = "INNER JOIN",
                    _JoinOn = GENERATE_JOIN_ON_FK(_Connect, i["KEY_NAME"].ToString(), i["TABLE_NAME"].ToString(), "a0", i["PARENT_TABLE_NAME"].ToString(), TempLoop), //dynamicallly generate
                    _HasWhere = false,
                    _HasAggregateFunc = false,
                    _HasGroupBy = false,
                    _HasHaving = false
                });


                //Require all 

                //SourcedTables = GET_ALL_PARENT_TABLES(_Connect, i["PARENT_TABLE_NAME"].ToString());

                //foreach (DataRow ii in SourcedTables.Rows)
                //{
                //    Looper++;

                //    if (!TableNames.Contains(ii["PARENT_TABLE_NAME"].ToString()))
                //    {
                //        sqlStruct.Add(new DB_Toolbox.sqlSelectStructure
                //        {
                //            _TableName = ii["PARENT_TABLE_NAME"].ToString(),
                //            _TableAlias = "a" + Looper.ToString(),
                //            _HasFrom = false,
                //            _HasJoin = true,
                //            _JoinClause = "INNER JOIN",
                //            _JoinOn = GENERATE_JOIN_ON(_Connect, i["PARENT_TABLE_NAME"].ToString(), TempLoop, ii["PARENT_TABLE_NAME"].ToString(), "a" + Looper.ToString()), //dynamicallly generate
                //            _HasWhere = false,
                //            _HasAggregateFunc = false,
                //            _HasGroupBy = false,
                //            _HasHaving = false
                //        });
                //    }

                //    TableNames += i["PARENT_TABLE_NAME"].ToString() + " | ";
                //}
            }

            //if (TempTableAlias != "")
            //{
            //    List<WhereStructure> __WhereStruct = new List<WhereStructure>();
            //    WhereStructure __WhereStructRow = new WhereStructure();

            //    //__WhereStructRow.CloseParentheses = ")";
            //    //__WhereStructRow.OpenParentheses = "(";


            //    //__WhereStructRow.WhereClause = TempTableAlias + ".ENABLED = 'Y'";

            //    //__WhereStruct.Add(__WhereStructRow);


            //    sqlStruct.Add(new sqlSelectStructure
            //    {
            //        _HasFrom = false,
            //        _HasJoin = false,
            //        _HasWhere = true,
            //        _WhereClause = __WhereStruct,
            //        _HasAggregateFunc = false,
            //        _HasGroupBy = false,
            //        _HasHaving = false
            //    });
            //}

            return sqlStruct;
        }

        public List<sqlSelectStructure> Build_Child_Select_Struct(IConnectToDB _Connect, List<sqlSelectStructure> sqlStruct, List<ColumnStructure> QueryColumns, DataTable ParentTables)
        {
            DataTable SourcedTables = new DataTable();
            StringBuilder sqlquery = new StringBuilder();
            string TableNames = "";

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);
            //sqlStruct.Add(new sqlSelectStructure
            //{
            //    _TableName = owner + "." + TableName,
            //    _TableAlias = "a0a",
            //    _HasFrom = false,
            //    _HasJoin = true,
            //    _JoinClause = "INNER JOIN",
            //    _JoinOn = "a0." + TableName + "_ID = a0a." + TableName + "_ID and + a0a.ENABLED = 'Y'",
            //    _HasWhere = false,
            //    _HasAggregateFunc = false,
            //    _HasGroupBy = false,
            //    _HasHaving = false
            //});

            int Looper = 0;

            string TempLoop;



            foreach (DataRow i in ParentTables.Rows)
            {
                Looper++;
                TempLoop = "c" + Looper.ToString();
                TableNames += i["TABLE_NAME"].ToString() + " | ";

                sqlStruct.Add(new sqlSelectStructure
                {
                    _TableName = i["TABLE_SCHEMA"].ToString() + "." + i["TABLE_NAME"].ToString(),
                    _ChildTableName = i["PARENT_SCHEMA"].ToString() + "." + i["PARENT_TABLE_NAME"].ToString(),
                    _TableAlias = "c" + Looper.ToString(),
                    _HasFrom = false,
                    _HasJoin = true,
                    _JoinClause = "LEFT JOIN",
                    _JoinOn = GENERATE_JOIN_ON_FK(_Connect, i["KEY_NAME"].ToString(), i["TABLE_NAME"].ToString(), "a0", i["PARENT_TABLE_NAME"].ToString(), TempLoop), //dynamicallly generate
                    _HasWhere = false,
                    _HasAggregateFunc = false,
                    _HasGroupBy = false,
                    _HasHaving = false
                });
            }

            return sqlStruct;
        }


        public List<sqlSelectStructure> Build_Select_Struct(IConnectToDB _Connect, List<ColumnStructure> QueryColumns, string TableName, DataTable ParentTables, string ViewData)
        {
            DataTable SourcedTables = new DataTable();
            StringBuilder sqlquery = new StringBuilder();
            List<sqlSelectStructure> sqlStruct = new List<sqlSelectStructure>();
            string TableNames = "";

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            sqlStruct.Add(new sqlSelectStructure
            {
                _TableName = _Schema + "." + TableName,
                _TableAlias = "a0",
                _IncludeColumns = QueryColumns,
                _HasFrom = true,
                _HasJoin = false,
                _HasWhere = false,
                _HasAggregateFunc = false,
                _HasGroupBy = false,
                _HasHaving = false
            });

            string TempTableAlias = "a0";
            //sqlStruct.Add(new sqlSelectStructure
            //{
            //    _TableName = owner + "." + TableName,
            //    _TableAlias = "a0a",
            //    _HasFrom = false,
            //    _HasJoin = true,
            //    _JoinClause = "INNER JOIN",
            //    _JoinOn = "a0." + TableName + "_ID = a0a." + TableName + "_ID and + a0a.ENABLED = 'Y'",
            //    _HasWhere = false,
            //    _HasAggregateFunc = false,
            //    _HasGroupBy = false,
            //    _HasHaving = false
            //});

            int Looper = 0;

            string TempLoop;



            foreach (DataRow i in ParentTables.Rows)
            {
                Looper++;
                TempLoop = "a" + Looper.ToString();
                TableNames += i["PARENT_TABLE_NAME"].ToString() + " | ";

                sqlStruct.Add(new sqlSelectStructure
                {
                    _TableName = _Schema + "." + i["PARENT_TABLE_NAME"].ToString(),
                    _TableAlias = "a" + Looper.ToString(),
                    _HasFrom = false,
                    _HasJoin = true,
                    _JoinClause = "INNER JOIN",
                    _JoinOn = GENERATE_JOIN_ON_FK(_Connect, i["KEY_NAME"].ToString(), i["TABLE_NAME"].ToString(), "a0", i["PARENT_TABLE_NAME"].ToString(), TempLoop, ViewData), //dynamicallly generate
                    _HasWhere = false,
                    _HasAggregateFunc = false,
                    _HasGroupBy = false,
                    _HasHaving = false
                });


                //Require all 

                //SourcedTables = GET_ALL_PARENT_TABLES(_Connect, i["PARENT_TABLE_NAME"].ToString());

                //foreach (DataRow ii in SourcedTables.Rows)
                //{
                //    Looper++;

                //    if (!TableNames.Contains(ii["PARENT_TABLE_NAME"].ToString()))
                //    {
                //        sqlStruct.Add(new DB_Toolbox.sqlSelectStructure
                //        {
                //            _TableName = ii["PARENT_TABLE_NAME"].ToString(),
                //            _TableAlias = "a" + Looper.ToString(),
                //            _HasFrom = false,
                //            _HasJoin = true,
                //            _JoinClause = "INNER JOIN",
                //            _JoinOn = GENERATE_JOIN_ON(_Connect, i["PARENT_TABLE_NAME"].ToString(), TempLoop, ii["PARENT_TABLE_NAME"].ToString(), "a" + Looper.ToString()), //dynamicallly generate
                //            _HasWhere = false,
                //            _HasAggregateFunc = false,
                //            _HasGroupBy = false,
                //            _HasHaving = false
                //        });
                //    }

                //    TableNames += i["PARENT_TABLE_NAME"].ToString() + " | ";
                //}
            }

            if (TempTableAlias != "")
            {
                List<WhereStructure> __WhereStruct = new List<WhereStructure>();
                WhereStructure __WhereStructRow = new WhereStructure();

                __WhereStructRow.CloseParentheses = ")";
                __WhereStructRow.OpenParentheses = "(";

                switch (ViewData.ToLower())
                {
                    case "enabled only":
                        __WhereStructRow.WhereClause = TempTableAlias + ".ENABLED = 'Y'";
                        break;
                    case "disabled only":
                        __WhereStructRow.WhereClause = TempTableAlias + ".ENABLED = 'N'";
                        break;
                    case "all":
                        __WhereStructRow.WhereClause = TempTableAlias + ".ENABLED in ('Y','N')";
                        break;
                    case "last 24hrs":
                        __WhereStructRow.WhereClause = TempTableAlias + ".DT_CREATED > DATEADD(Hour,-24,GETDATE()) and " + TempTableAlias + ".ENABLED = 'Y'";
                        break;
                    case "last 48hrs":
                        __WhereStructRow.WhereClause = TempTableAlias + ".DT_CREATED > DATEADD(Hour,-48,GETDATE()) and " + TempTableAlias + ".ENABLED = 'Y'";
                        break;
                    case "last hour":
                        __WhereStructRow.WhereClause = TempTableAlias + ".DT_CREATED > DATEADD(Hour,-1,GETDATE()) and " + TempTableAlias + ".ENABLED = 'Y'";
                        break;
                    default:
                        __WhereStructRow.WhereClause = TempTableAlias + ".ENABLED = 'Y'";
                        break;

                }
                __WhereStruct.Add(__WhereStructRow);


                sqlStruct.Add(new sqlSelectStructure
                {
                    _HasFrom = false,
                    _HasJoin = false,
                    _HasWhere = true,
                    _WhereClause = __WhereStruct,
                    _HasAggregateFunc = false,
                    _HasGroupBy = false,
                    _HasHaving = false
                });
            }

            return sqlStruct;
        }

        public string GENERATE_JOIN_ON_FK(IConnectToDB _Connect, string key_name, string Table1, string Table1Alias, string Table2, string Table2Alias)
        {
            //Find ALl Matching Columns and return a and joined string

            StringBuilder sqlquery = new StringBuilder();
            string JoinOn = "";

            sqlquery.Append("SELECT TABLE_NAME, PARENT_TABLE_NAME, COLUMN_NAME, PARENT_COLUMN_NAME from CSA.ER_FOREIGN_KEYS a0 ");
            sqlquery.AppendLine("INNER JOIN CSA.ER_FK_COLUMNS a1 ");
            sqlquery.AppendLine("ON a0.ER_FOREIGN_KEYS_ID = a1.ER_FOREIGN_KEYS_ID");
            sqlquery.AppendLine("WHERE a0.KEY_NAME = @KEY_NAME and a0.TABLE_NAME = @TABLE_NAME and a0.PARENT_TABLE_NAME = @PARENT_TABLE_NAME");

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sqlquery.ToString(),
                _dbParameters = new List<DBParameters> {
                    new DBParameters { ParamName = "KEY_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = key_name },
                    new DBParameters { ParamName = "TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = Table1 },
                    new DBParameters { ParamName = "PARENT_TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = Table2 } }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            int rowCount = _DT.Rows.Count;
            int iNumber = 0;

            if (DCC.Contains("TABLE_NAME"))
            {
                foreach (DataRow i in _DT.Rows)
                {
                    // JoinOn += Table2Alias + "." + "ENABLED = 'Y' and ";
                    JoinOn += Table1Alias + "." + i["COLUMN_NAME"].ToString() + " = " + Table2Alias + "." + i["PARENT_COLUMN_NAME"].ToString();
                    iNumber++;

                    if (iNumber < rowCount) { JoinOn += " AND "; }

                }
            }

            return JoinOn;
        }

        public string GENERATE_JOIN_ON_FK(IConnectToDB _Connect, string key_name, string Table1, string Table1Alias, string Table2, string Table2Alias, string ViewData)
        {
            //Find ALl Matching Columns and return a and joined string

            StringBuilder sqlquery = new StringBuilder();
            string JoinOn = "";

            sqlquery.Append("SELECT TABLE_NAME, PARENT_TABLE_NAME, COLUMN_NAME, PARENT_COLUMN_NAME from CSA.ER_FOREIGN_KEYS a0 ");
            sqlquery.AppendLine("INNER JOIN CSA.ER_FK_COLUMNS a1 ");
            sqlquery.AppendLine("ON a0.ER_FOREIGN_KEYS_ID = a1.ER_FOREIGN_KEYS_ID");
            sqlquery.AppendLine("WHERE a0.KEY_NAME = @KEY_NAME and a0.TABLE_NAME = @TABLE_NAME and a0.PARENT_TABLE_NAME = @PARENT_TABLE_NAME");

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sqlquery.ToString(),
                _dbParameters = new List<DBParameters> {
                    new DBParameters { ParamName = "KEY_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = key_name },
                    new DBParameters { ParamName = "TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = Table1 },
                    new DBParameters { ParamName = "PARENT_TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = Table2 } }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            int rowCount = _DT.Rows.Count;
            int iNumber = 0;

            if (DCC.Contains("TABLE_NAME"))
            {
                foreach (DataRow i in _DT.Rows)
                {
                    switch (ViewData.ToLower())
                    {
                        case "enabled only":
                            JoinOn += Table2Alias + "." + "ENABLED = 'Y' and ";
                            break;
                        case "disabled only":
                            JoinOn += Table2Alias + "." + "ENABLED = 'N' and ";
                            break;
                        case "all":
                            JoinOn += Table2Alias + "." + "ENABLED in ('Y','N') and ";
                            break;
                        default:
                            JoinOn += Table2Alias + "." + "ENABLED = 'Y' and ";
                            break;

                    }

                    JoinOn += Table1Alias + "." + i["COLUMN_NAME"].ToString() + " = " + Table2Alias + "." + i["PARENT_COLUMN_NAME"].ToString();
                    iNumber++;

                    if (iNumber < rowCount) { JoinOn += " AND "; }
                }
            }

            return JoinOn;
        }


        public string GENERATE_JOIN_ON(IConnectToDB _Connect, string Table1, string Table1Alias, string Table2, string Table2Alias)
        {
            //"st.STAGE_NAME = gr.STAGE_NAME"
            //Find ALl Matching Columns and return a and joined string

            StringBuilder sqlquery = new StringBuilder();
            string JoinOn = "";

            sqlquery.Append("SELECT a0.TABLE_NAME, a0.PARENT_TABLE_NAME, a1.COLUMN_NAME, a1.PARENT_COLUMN_NAME ");
            sqlquery.AppendLine("from CSA.ER_FOREIGN_KEYS a0 ");
            sqlquery.AppendLine("INNER JOIN CSA.ER_FK_COLUMNS a1 ");
            sqlquery.AppendLine("   ON a0.ER_FOREIGN_KEYS_ID = a1.ER_FOREIGN_KEYS_ID");
            sqlquery.AppendLine("WHERE a0.TABLE_NAME = @TABLE_NAME and a0.PARENT_TABLE_NAME = @PARENT_TABLE_NAME");

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sqlquery.ToString(),
                _dbParameters = new List<DBParameters> {
                    new DBParameters { ParamName = "TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = Table1 },
                    new DBParameters { ParamName = "PARENT_TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = Table2 } }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            int rowCount = _DT.Rows.Count;
            int iNumber = 0;

            if (DCC.Contains("TABLE_NAME"))
            {
                foreach (DataRow i in _DT.Rows)
                {
                    JoinOn += Table1Alias + "." + i["COLUMN_NAME"].ToString() + " = " + Table2Alias + "." + i["PARENT_COLUMN_NAME"].ToString();
                    iNumber++;

                    if (iNumber < rowCount) { JoinOn += " AND "; }
                }
            }

            return JoinOn;
        }

        public List<CommandResult> GENERATE_ALL_VIEW(IConnectToDB _Connect)
        {
            StringBuilder sqlquery = new StringBuilder();

            List<CommandResult> Logger = new List<CommandResult>();

            sqlquery.Append("Select TABLE_NAME, TABLE_TYPE from CSA.ER_TABLES");
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sqlquery.ToString(),
                _dbParameters = new List<DBParameters>()
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            if (DCC.Contains("TABLE_NAME"))
            {
                foreach (DataRow i in _DT.Rows)
                {
                    Logger.AddRange(GENERATE_VIEW(_Connect, i["TABLE_NAME"].ToString(), i["TABLE_TYPE"].ToString()));
                }
            }

            Logger.AddRange(GENERATE_VIEW(_Connect, "ER_TABLES", "Dictionary")); //Manually Add because it's not in ER_TABLES

            return Logger;
        }

        public string GENERATE_QUERY(IConnectToDB _Connect, List<sqlSelectStructure> QueryStructure)
        {
            StringBuilder _Columnlist = new StringBuilder();
            string temp1;
            Boolean _FromAdded = false;
            Boolean _WhereAdded = false;
            Boolean _GroupByAdded = false;
            Boolean _HavingAdded = false;
            //Boolean _HadAggr = false;
            StringBuilder SQLBuffer = new StringBuilder();

            foreach (sqlSelectStructure i in QueryStructure)
            {
                if (i._HasAggregateFunc == true)
                {
                    if (i._AggregateFunctions == "distinct")
                    {
                        _Columnlist.AppendLine("distinct ");
                        break;
                    }
                }
            }

            foreach (sqlSelectStructure i in QueryStructure)
            {

                if (i._IncludeColumns != null)
                {

                    long iiCounter = 0;
                    foreach (ColumnStructure ii in i._IncludeColumns)
                    {
                        string addTail = (i._IncludeColumns.Count() - 1) != iiCounter ? ", " : " ";

                        if (string.IsNullOrWhiteSpace(ii._Alias))
                        {
                            temp1 = ii._Alias;
                        }
                        else
                        {
                            temp1 = " \"" + ii._Alias + "\"";
                        }

                        if (ii._Name.ToString().ToUpper().Contains("NVL") == true)
                        {
                            _Columnlist.AppendLine(_Columnlist + ii._Name + " " + temp1 + addTail);
                        }
                        //else if (ii._Alias.ToLower() == ("password"))
                        //{
                        //    _Columnlist = _Columnlist + "'**************' " + ii._Alias + " , ";                            
                        //}
                        //else if (ii._Alias.ToLower() == ("confirm password"))
                        //{
                        //    _Columnlist = _Columnlist + "'**************' " + ii._Alias + " , ";
                        //} 
                        else if (ii._SelectSQL != null)
                        {
                            _Columnlist.AppendLine(" (" + ii._SelectSQL + ") " + temp1 + addTail);
                        }
                        else
                        {
                            if (ii._LiteralBefore != null || ii._LiteralAfter != null)
                            {
                                _Columnlist.AppendLine(((ii._LiteralBefore != null ? ("'" + ii._LiteralBefore + "' + ") : "") + "Convert(varchar," + ii._Table + "." + ii._Name + ") " + (ii._LiteralAfter != null ? ("'" + ii._LiteralAfter + "' + ") : "") + temp1 + addTail));
                            }
                            else
                            {
                                _Columnlist.AppendLine(ii._Table + "." + ii._Name + " " + temp1 + addTail);
                            }
                        }

                        iiCounter++;
                    }
                }
            }

            SQLBuffer.AppendLine("SELECT " + _Columnlist + " ");

            int iNumber = 0;
            foreach (sqlSelectStructure i in QueryStructure)
            {
                iNumber++;

                if (i._HasFrom == true &&
                    // i._HasAggregateFunc != true &&
                    i._HasJoin != true &&
                    i._HasWhere != true &&
                    i._HasGroupBy != true &&
                    i._HasHaving != true)
                {
                    if (_FromAdded == false)
                    {
                        SQLBuffer.AppendLine("FROM " + i._TableName + " " + i._TableAlias + " ");
                        _FromAdded = true;
                    }
                }
                else if (i._HasFrom != true &&
                    i._HasAggregateFunc != true &&
                    i._HasJoin == true &&
                    i._HasWhere != true &&
                    i._HasGroupBy != true &&
                    i._HasHaving != true)
                {
                    SQLBuffer.AppendLine(i._JoinClause + " " + i._TableName + " " + i._TableAlias + " ");
                    SQLBuffer.AppendLine("ON (" + i._JoinOn + ") ");
                }
                else if (i._HasFrom != true &&
                    i._HasAggregateFunc != true &&
                    i._HasJoin != true &&
                    i._HasWhere == true &&
                    i._HasGroupBy != true &&
                    i._HasHaving != true)
                {
                    if (_WhereAdded == false)
                    {
                        SQLBuffer.AppendLine("WHERE 1 = 1 ");
                        _WhereAdded = true;
                    }

                    foreach (WhereStructure ii in i._WhereClause)
                    {
                        SQLBuffer.AppendLine(ii.OpenParentheses);
                        SQLBuffer.AppendLine(ii.WhereClause);
                        SQLBuffer.AppendLine(ii.CloseParentheses + " " + ii.ContinuingOperator + " ");

                    }
                }
                else if (i._HasFrom != true &&
                    i._HasAggregateFunc != true &&
                    i._HasJoin != true &&
                    i._HasWhere != true &&
                    i._HasGroupBy == true &&
                    i._HasHaving != true)
                {
                    if (_GroupByAdded == false)
                    {
                        SQLBuffer.AppendLine("GROUP BY " + i._GroupByClause + " ");
                        _GroupByAdded = true;
                    }


                }
                else if (i._HasFrom != true &&
                    i._HasAggregateFunc != true &&
                    i._HasJoin != true &&
                    i._HasWhere != true &&
                    i._HasGroupBy != true &&
                    i._HasHaving == true)
                {
                    if (_HavingAdded == false)
                    {
                        SQLBuffer.AppendLine("Having " + i._HavingClause + " ");
                        _HavingAdded = true;
                    }


                }

            }

            return SQLBuffer.ToString();

        }

        public StringBuilder GENERATE_PROCEDURE(IConnectToDB _Connect, string Name, List<sqlProcedureStructure> ProcedureStructure)
        {
            List<sqlProcedureLineStructure> plc = new List<sqlProcedureLineStructure>();
            List<sqlProcedureParameterStructure> pps = new List<sqlProcedureParameterStructure>();
            sqlProcedureStructure ps = new sqlProcedureStructure();
            StringBuilder Buffer = new StringBuilder();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            foreach (sqlProcedureStructure i in ProcedureStructure)
            {
                Buffer.AppendLine("CREATE OR REPLACE PROCEDURE " + _Schema + "." + Name + " IS ");

                int iNumber = 0;
                foreach (sqlProcedureParameterStructure ii in i._Parameters)
                {
                    iNumber++;

                    Buffer.AppendLine("   P_" + ii._Name + " " + ii._Direction.ToString() + " " + ii._DataType);

                    if (i._Parameters.Count < iNumber)
                        Buffer.Append(", ");
                }

                Buffer.AppendLine("DECLARE ");

                foreach (sqlProcedureLineStructure ii in i._Declare)
                {
                    Buffer.AppendLine("   " + ii.LineEntry);
                }

                Buffer.AppendLine("BEGIN ");

                foreach (sqlProcedureLineStructure ii in i._Body)
                {
                    Buffer.AppendLine("   " + ii.LineEntry);
                }

                Buffer.AppendLine("EXCEPTION ");

                foreach (sqlProcedureLineStructure ii in i._Exception)
                {
                    Buffer.AppendLine("   " + ii.LineEntry);
                }

                Buffer.AppendLine("END; ");
            }


            return Buffer;
        }

        public StringBuilder GENERATE_FUNCTION(IConnectToDB _Connect, string Name, List<sqlProcedureStructure> ProcedureStructure)
        {
            List<sqlProcedureLineStructure> plc = new List<sqlProcedureLineStructure>();
            List<sqlProcedureParameterStructure> pps = new List<sqlProcedureParameterStructure>();
            sqlProcedureStructure ps = new sqlProcedureStructure();
            StringBuilder Buffer = new StringBuilder();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            foreach (sqlProcedureStructure i in ProcedureStructure)
            {
                Buffer.AppendLine("CREATE OR REPLACE FUNCTION " + _Schema + "." + Name + " IS ");

                int iNumber = 0;
                foreach (sqlProcedureParameterStructure ii in i._Parameters)
                {
                    iNumber++;

                    Buffer.AppendLine("   P_" + ii._Name + " " + ii._Direction.ToString() + " " + ii._DataType);

                    if (i._Parameters.Count < iNumber)
                        Buffer.Append(", ");
                }

                Buffer.AppendLine("DECLARE ");

                foreach (sqlProcedureLineStructure ii in i._Declare)
                {
                    Buffer.AppendLine("   " + ii.LineEntry);
                }

                Buffer.AppendLine("BEGIN ");

                foreach (sqlProcedureLineStructure ii in i._Body)
                {
                    Buffer.AppendLine("   " + ii.LineEntry);
                }

                Buffer.AppendLine("EXCEPTION ");

                foreach (sqlProcedureLineStructure ii in i._Exception)
                {
                    Buffer.AppendLine("   " + ii.LineEntry);
                }

                Buffer.AppendLine("END; ");
            }


            return Buffer;
        }

        public List<ColumnStructure> ReOrder_Column_Stage_Struct(IConnectToDB _Connect, List<ColumnStructure> QueryColumns, string stageid)
        {
            DataTable ListofColumns = new DataTable();
            //Column_Order

            StringBuilder sqlquery = new StringBuilder();

            sqlquery.Append("select a.object_sets_id, a.OBJ_PROP_SETS_ID, a.PROPERTY_NAME, ");
            sqlquery.Append("CAST(a.PROPERTY_VALUE as int ) Column_Order, b.PROPERTY_VALUE ");
            sqlquery.Append("	FROM CSA.GRIPS gr  ");
            sqlquery.Append("	inner join CSA.OBJECT_SETS ob on (gr.GRIPS_ID = ob.GRIPS_ID and gr.GRIP_TYPE = 'Form'  ");
            sqlquery.Append("		and ob.OBJECT_TYPE not in ('Button', 'Grid Widget', 'JSON Text')  ");
            sqlquery.Append("		and gr.STAGES_ID = @STAGES_ID) ");
            sqlquery.Append("	inner join CSA.OBJ_PROP_SETS a on (a.OBJECT_SETS_ID = ob.OBJECT_SETS_ID and a.PROPERTY_NAME in ('Column_Order')) ");
            sqlquery.Append("	inner join CSA.OBJ_PROP_SETS b on (b.OBJECT_SETS_ID = ob.OBJECT_SETS_ID and b.PROPERTY_NAME in ('ID')) ");
            sqlquery.Append("	order by Column_Order asc ");

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sqlquery.ToString(),
                _dbParameters = new List<DBParameters> {
                     new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = stageid }
                }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            List<ColumnStructure> New_Order = new List<ColumnStructure>();

            New_Order.Add(QueryColumns[0]);
            New_Order.Add(QueryColumns[1]);
            New_Order.Add(QueryColumns[2]);
            New_Order.Add(QueryColumns[3]);
            New_Order.Add(QueryColumns[4]);

            foreach (DataRow _RowOrder in _DT.Rows)
            {
                foreach (ColumnStructure _Row in QueryColumns)
                {
                    if (_Row._Alias.ToUpper() == _RowOrder.Field<string>("Property_Value").ToUpper())
                    {
                        New_Order.Add(_Row);
                    }
                }
            }

            return New_Order;
        }

        public List<ColumnStructure> Build_Column_Stage_Struct(IConnectToDB _Connect, string stageid, string _stagetype)
        {
            List<ColumnStructure> cs_Select = new List<ColumnStructure>();
            DataTable TempDataColumns = new DataTable();
            DataTable TempDataColumns2 = new DataTable();

            DataTable Properties_DT;
            string sqlquery = "";
            switch (_stagetype.ToLower())
            {
                case "form":
                    sqlquery = "select case(b.OBJECT_TYPE) " +
                        "   WHEN 'Text_Box' THEN '0' " +
                        "   WHEN 'Date' THEN '1' " +
                        "   WHEN 'Number' Then '2' " +
                        "   WHEN 'Total' Then '2' " +
                        "   ELSE '3' " +
                        "   END as ER_ORDER, " +
                        "a.DT_CREATED, a.OBJ_PROP_SETS_ID, a.PROPERTY_NAME, a.PROPERTY_VALUE, a.STAGE_TYPE, b.OBJECT_SETS_ID, b.OBJECT_TYPE " +
                        "FROM CSA.[VW__OBJ_PROP_SETS] a " +
                        "left join CSA.[VW__OBJECT_SETS] b on (a.OBJECT_SETS_ID = b.OBJECT_SETS_ID) " +
                        "inner join ( " +
                        "    select a.DT_CREATED,a.OBJ_PROP_SETS_ID, a.PROPERTY_NAME, a.PROPERTY_VALUE, a.STAGE_TYPE, b.OBJECT_TYPE " +
                        "    FROM CSA.[VW__OBJ_PROP_SETS] a " +
                        "    left join CSA.[VW__OBJECT_SETS] b on (a.OBJECT_SETS_ID = b.OBJECT_SETS_ID) " +
                        "    WHERE a.PROPERTY_NAME = 'ID' and a.GRIP_TYPE = 'Form'  " +
                        "        and a.OBJECT_TYPE NOT IN ('Text', 'Button', 'File', 'JSON Text' ) " +
                        "        and  a.grips_id in " +
                        "            (Select distinct g.Grips_ID from CSA.VW__Grips g " +
                        "            inner join  " +
                        "                (Select a.APPLICATIONS_ID from CSA.APPLICATIONS a " +
                        "                    inner join CSA.STAGES s ON (a.APPLICATIONS_ID = s.APPLICATIONS_ID) " +
                        "                    inner join CSA.GRIPS g ON (g.STAGES_ID = s.STAGES_ID and s.STAGES_ID = @STAGES_ID)) q " +
                        "                        ON (g.APPLICATIONS_ID = q.APPLICATIONS_ID)))  aaa " +
                        "                        ON (aaa.PROPERTY_VALUE = a.PROPERTY_VALUE) " +
                        "                    WHERE a.PROPERTY_NAME = 'ID' and a.GRIP_TYPE = 'Form' " +
                        "                        and a.OBJECT_TYPE NOT IN ('Text', 'Button', 'File', 'JSON Text') " +
                        "                        and  a.grips_id in " +
                        "	                        (Select distinct g.Grips_ID from CSA.VW__Grips g inner join " +
                        "		                        (Select a.APPLICATIONS_ID from CSA.APPLICATIONS a inner join " +
                        "		                        CSA.APPLICATIONS a1 ON (a.ROOT_APPLICATION = a1.ROOT_APPLICATION) " +
                        "		                        inner join CSA.STAGES s ON (a1.APPLICATIONS_ID = s.APPLICATIONS_ID)  " +
                        "		                        inner join CSA.GRIPS g ON (g.STAGES_ID = s.STAGES_ID and s.STAGES_ID = STAGES_ID)) q " +
                        "			                        ON (g.APPLICATIONS_ID = q.APPLICATIONS_ID)) " +
                        "order by ER_ORDER, a.DT_CREATED asc ";

                    ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                    {
                        sqlIn = sqlquery.ToString(),
                        _dbParameters = new List<DBParameters> {
                            new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = stageid }
                         }
                    };

                    Properties_DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
                    DataColumnCollection DCC = Properties_DT.Columns;

                    break;
                case "identity":
                    Properties_DT = new DataTable(); //er_query.RUN_QUERY(_Connect, "SELECT distinct OBJ_PROP_SETS_ID, PROPERTY_NAME, PROPERTY_VALUE FROM " + owner + ".[VW__OBJ_PROP_SETS] WHERE GRIP_TYPE in ('User Settings') and OBJECT_TYPE NOT IN ('Text', 'Button', 'File', 'JSON Text') and  grips_id in (" + ofGrips + ") ");
                    break;
                default:
                    Properties_DT = new DataTable();
                    break;
            }



            //Intercept Here.
            //Do Not place newly added fields in front of old fields. It will hide the data.

            //Compare the all fields for each app version.

            //keep the oldest up front and the new in the rear. 

            string TempColumns = "";
            foreach (DataRow i in Properties_DT.Rows)
            {
                if (i.Field<string>("OBJECT_TYPE").ToLower() != "drop_down" || i.Field<string>("OBJECT_TYPE").ToLower() != "radio_button" || i.Field<string>("OBJECT_TYPE").ToLower() != "check_box")
                {
                }
            }

            switch (_stagetype.ToLower())
            {
                case "form":
                    cs_Select.Add(new ColumnStructure { _Name = "FORMS_ID", _Table = "p0", _Alias = "FORMS_ID" });
                    cs_Select.Add(new ColumnStructure { _Name = "Enabled", _Table = "p0", _Alias = "Enabled" });
                    cs_Select.Add(new ColumnStructure { _Name = "IDENTITIES_ID", _Table = "p0", _Alias = "IDENTITIES_ID" });
                    cs_Select.Add(new ColumnStructure { _Name = "DT_CREATED", _Table = "p0", _Alias = "DT_CREATED" });
                    cs_Select.Add(new ColumnStructure { _Name = "USER_NAME", _Table = "pi0", _Alias = "USER_NAME" });
                    //if (_OnlyMultiSelects)
                    //{
                    //    cs_Select.Add(new ColumnStructure { _Name = "APPLICATIONS_ID", _Table = "fdo_" });
                    //}
                    //else
                    //{
                    cs_Select.Add(new ColumnStructure { _Name = "APPLICATIONS_ID", _Table = "p1", _Alias = "APPLICATIONS_ID" });
                    //}
                    break;
                case "identity":
                    cs_Select.Add(new ColumnStructure { _Name = "IDENTITIES_ID", _Table = "p0" });
                    cs_Select.Add(new ColumnStructure { _Name = "Enabled", _Table = "p0" });
                    cs_Select.Add(new ColumnStructure { _Name = "DT_CREATED", _Table = "p0" });
                    cs_Select.Add(new ColumnStructure { _Name = "DT_AVAILABLE", _Table = "p0" });
                    cs_Select.Add(new ColumnStructure { _Name = "DT_END", _Table = "p0" });
                    cs_Select.Add(new ColumnStructure { _Name = "USER_NAME", _Table = "p0" });
                    cs_Select.Add(new ColumnStructure { _Name = "PASSWORD", _Table = "PWD" });
                    break;

            }

            int Looper = 1;
            switch (_stagetype.ToLower())
            {
                case "form":
                    foreach (DataRow i in Properties_DT.Rows)
                    {

                        if (!TempColumns.Contains(" -" + i["PROPERTY_VALUE"].ToString() + "- "))
                        {
                            if (i.Field<string>("OBJECT_TYPE").ToLower() == "drop_down" || i.Field<string>("OBJECT_TYPE").ToLower() == "radio_button" || i.Field<string>("OBJECT_TYPE").ToLower() == "check_box")
                            {
                                cs_Select.Add(new ColumnStructure
                                {
                                    _Name = "OPTION_VALUE",
                                    _Table = "p" + Looper++.ToString(),
                                    _Alias = i.Field<string>("PROPERTY_VALUE"),
                                    _DataType = i.Field<string>("OBJECT_TYPE"),
                                    _SelectSQL = "select right([tempfield], LEN(tempfield)-1) as test from " +
                                        "(Select ( " +
                                        "Select CAST((SELECT ',' + CAST(fdo.VALUE as varchar ) AS [text()] " +
                                        "FROM CSA.VW__forms_dat_opt fdo " +
                                        "WHERE fdo.FORMS_ID = mp0.FORMS_ID and fdo.PROPERTY_VALUE = '" + i.Field<string>("PROPERTY_VALUE") + "' " +
                                        "and fdo.RENDITION in (Select MAX(RENDITION) from CSA.VW__forms_dat_opt fdo2 where fdo2.FORMS_ID=fdo.FORMS_ID ) " +
                                        "FOR XML PATH('')) as varchar)) as [tempfield] " +
                                        "from CSA.VW__forms mp0 where mp0.FORMS_ID = p0.FORMS_ID) [MAIN]"
                                });


                            }
                            else if (i.Field<string>("OBJECT_TYPE") == "File_Upload")
                            {
                                cs_Select.Add(new ColumnStructure
                                {
                                    _Name = "FORMS_DAT_FILE_ID",
                                    _Table = "p" + Looper++.ToString(),
                                    _Alias = i.Field<string>("PROPERTY_VALUE"),
                                    _DataType = i.Field<string>("OBJECT_TYPE"),
                                    _LiteralBefore = "file/" + i.Field<string>("STAGE_TYPE") + "/"
                                });
                            }
                            else if (i.Field<string>("OBJECT_TYPE") == "signature")
                            {
                                cs_Select.Add(new ColumnStructure
                                {
                                    _Name = "FORMS_DAT_FILE_ID",
                                    _Table = "p" + Looper++.ToString(),
                                    _Alias = i.Field<string>("PROPERTY_VALUE"),
                                    _DataType = i.Field<string>("OBJECT_TYPE"),
                                    _LiteralBefore = "signature/"
                                });
                            }
                            else if (i.Field<string>("OBJECT_TYPE") == "Number")
                            {
                                cs_Select.Add(new ColumnStructure
                                {
                                    _Name = "VALUE",
                                    _Table = "p" + Looper++.ToString(),
                                    _Alias = i.Field<string>("PROPERTY_VALUE"),
                                    _DataType = i.Field<string>("OBJECT_TYPE"),
                                });
                            }
                            else if (i.Field<string>("OBJECT_TYPE") == "Decimal" || i.Field<string>("OBJECT_TYPE") == "Money")
                            {
                                cs_Select.Add(new ColumnStructure
                                {
                                    _Name = "VALUE",
                                    _Table = "p" + Looper++.ToString(),
                                    _Alias = i.Field<string>("PROPERTY_VALUE"),
                                    _DataType = i.Field<string>("OBJECT_TYPE"),
                                });
                            }
                            else
                            {
                                cs_Select.Add(new ColumnStructure
                                {
                                    _Name = "VALUE",
                                    _Table = "p" + Looper++.ToString(),
                                    _Alias = i.Field<string>("PROPERTY_VALUE"),
                                    _DataType = i.Field<string>("OBJECT_TYPE")
                                });
                            }

                            TempColumns = TempColumns + " -" + i["PROPERTY_VALUE"].ToString() + "- ";
                        }
                    }
                    break;
                case "identity":
                    foreach (DataRow i in Properties_DT.Rows)
                    {
                        cs_Select.Add(new ColumnStructure
                        {
                            _Name = "VALUE",
                            _Table = "p" + Looper++.ToString(),
                            _Alias = i.Field<string>("PROPERTY_NAME")
                        });
                    }
                    break;
            }

            return cs_Select;

        }

        public List<sqlSelectStructure> Build_Select_Stage_Struct(IConnectToDB _Connect, List<ColumnStructure> QueryColumns, string stageid, string _stagetype)
        {
            DataTable SourcedTables = new DataTable();
            StringBuilder sqlquery = new StringBuilder();
            List<sqlSelectStructure> sqlStruct = new List<sqlSelectStructure>();
            Boolean HasOptionsTable = false;
            //string TableNames = "";

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            switch (_stagetype.ToLower())
            {
                case "form":
                    sqlStruct.Add(new sqlSelectStructure
                    {
                        _TableName = _Schema + ".VW__Forms",
                        _TableAlias = "p0",
                        _IncludeColumns = QueryColumns,
                        _HasFrom = true,
                        _HasJoin = false,
                        _HasWhere = false,
                        _HasAggregateFunc = true,
                        _AggregateFunctions = "distinct",
                        _HasGroupBy = false,
                        _HasHaving = false
                    });

                    sqlStruct.Add(new sqlSelectStructure
                    {
                        _TableName = _Schema + ".IDENTITIES",
                        _TableAlias = "pi0",
                        _HasFrom = false,
                        _HasJoin = true,
                        _JoinClause = "INNER JOIN",
                        _JoinOn = "pi0.IDENTITIES_ID = p0.IDENTITIES_ID and p0.IDENTITIES_ID is not null",
                        _HasWhere = false,
                        _HasAggregateFunc = false,
                        _HasGroupBy = false,
                        _HasHaving = false
                    });

                    //sqlStruct.Add(new sqlSelectStructure
                    //{
                    //    _TableName = owner + ".APPLICATIONS",
                    //    _TableAlias = "a1",
                    //    _HasFrom = false,
                    //    _HasJoin = true,
                    //    _JoinClause = "INNER JOIN",
                    //    _JoinOn = "a1.ROOT_APPLICATION = a0.ROOT_APPLICATION",
                    //    _HasWhere = false,
                    //    _HasAggregateFunc = false,
                    //    _HasGroupBy = false,
                    //    _HasHaving = false
                    //});



                    //sqlStruct.Add(new sqlSelectStructure
                    //{
                    //    _TableName = owner + ".STAGES",
                    //    _TableAlias = "s0",
                    //    _HasFrom = false,
                    //    _HasJoin = true,
                    //    _JoinClause = "INNER JOIN",
                    //    _JoinOn = "a0.APPLICATIONS_ID = s0.APPLICATIONS_ID and s0.STAGES_ID = " + stageid,
                    //    _HasWhere = false,
                    //    _HasAggregateFunc = false,
                    //    _HasGroupBy = false,
                    //    _HasHaving = false
                    //});

                    //sqlStruct.Add(new sqlSelectStructure
                    //{
                    //    _TableName = owner + ".GRIPS",
                    //    _TableAlias = "g0",
                    //    _HasFrom = false,
                    //    _HasJoin = true,
                    //    _JoinClause = "INNER JOIN",
                    //    _JoinOn = "g0.STAGES_ID = s0.STAGES_ID",
                    //    _HasWhere = false,
                    //    _HasAggregateFunc = false,
                    //    _HasGroupBy = false,
                    //    _HasHaving = false
                    //});

                    //sqlStruct.Add(new sqlSelectStructure
                    //{
                    //    _TableName = owner + ".OBJECT_SETS",
                    //    _TableAlias = "os0",
                    //    _HasFrom = false,
                    //    _HasJoin = true,
                    //    _JoinClause = "INNER JOIN",
                    //    _JoinOn = "os0.GRIPS_ID = g0.GRIPS_ID",
                    //    _HasWhere = false,
                    //    _HasAggregateFunc = false,
                    //    _HasGroupBy = false,
                    //    _HasHaving = false
                    //});

                    //sqlStruct.Add(new sqlSelectStructure
                    //{
                    //    _TableName = owner + ".OBJ_PROP_SETS",
                    //    _TableAlias = "ops0",
                    //    _HasFrom = false,
                    //    _HasJoin = true,
                    //    _JoinClause = "INNER JOIN",
                    //    _JoinOn = "ops0.OBJECT_SETS_ID = os0.OBJECT_SETS_ID and ops0.PROPERTY_NAME = 'ID' and g0.GRIP_TYPE = 'Form' and os0.OBJECT_TYPE NOT IN ('Text', 'Button', 'File', 'JSON Text') ",
                    //    _HasWhere = false,
                    //    _HasAggregateFunc = false,
                    //    _HasGroupBy = false,
                    //    _HasHaving = false
                    //});
                    break;
                case "identity":
                    sqlStruct.Add(new sqlSelectStructure
                    {
                        _TableName = _Schema + ".VW__Identities",
                        _TableAlias = "p0",
                        _IncludeColumns = QueryColumns,
                        _HasFrom = true,
                        _HasJoin = false,
                        _HasWhere = false,
                        _HasAggregateFunc = true,
                        _AggregateFunctions = "distinct",
                        _HasGroupBy = false,
                        _HasHaving = false
                    });

                    //sqlStruct.Add(new sqlSelectStructure
                    //{
                    //    _TableName = owner + ".Identities",
                    //    _TableAlias = "p0a",
                    //    _HasFrom = false,
                    //    _HasJoin = true,
                    //    _JoinClause = "INNER JOIN",
                    //    _JoinOn = "(p0a.ENABLED = 'Y') and p0.IDENTITIES_ID = p0a.IDENTITIES_ID",
                    //    _HasWhere = false,
                    //    _HasAggregateFunc = false,
                    //    _HasGroupBy = false,
                    //    _HasHaving = false
                    //});

                    sqlStruct.Add(new sqlSelectStructure
                    {
                        _TableName = _Schema + ".VW__ID_PASSWORD",
                        _TableAlias = "PWD",
                        _HasFrom = false,
                        _HasJoin = true,
                        _JoinClause = "INNER JOIN",
                        _JoinOn = "p0.IDENTITIES_ID = PWD.IDENTITIES_ID and PWD.RENDITION in (Select max(PWD.RENDITION) from " + _Schema + ".ID_PASSWORD PWD where PWD.IDENTITIES_ID = PWD.IDENTITIES_ID) ",
                        _HasWhere = false,
                        _HasAggregateFunc = false,
                        _HasGroupBy = false,
                        _HasHaving = false
                    });
                    break;
            }

            int Looper = 0;

            string TempLoop;

            foreach (ColumnStructure i in QueryColumns)
            {
                Looper++;
                TempLoop = "p" + Looper.ToString();
                //TableNames += i["PARENT_TABLE_NAME"].ToString() + " | ";
                switch (_stagetype.ToLower())
                {
                    case "form":
                        if (i._Name.ToUpper() != "FORMS_ID"
                            && i._Name.ToUpper() != "ENABLED"
                            && i._Name.ToUpper() != "DT_CREATED"
                            && i._Name.ToUpper() != "DT_AVAILABLE"
                            && i._Name.ToUpper() != "DT_END"
                            && i._Name.ToUpper() != "APPLICATIONS_ID"
                            && i._Name.ToUpper() != "IDENTITIES_ID"
                            && i._Name.ToUpper() != "USER_NAME")
                        {
                            if (i._DataType == "File_Upload" || i._DataType == "signature")
                            {
                                sqlStruct.Add(new sqlSelectStructure
                                {
                                    _TableName = _Schema + ".VW__FORMS_DAT_FILE",
                                    _TableAlias = i._Table,
                                    _HasFrom = false,
                                    _HasJoin = true,
                                    _JoinClause = "LEFT JOIN",
                                    _JoinOn = "p0.FORMS_ID = " + i._Table + ".FORMS_ID and " + i._Table + ".PROPERTY_VALUE = '" + i._Alias + "' and " + i._Table + ".RENDITION in (Select max(" + i._Table + "a.RENDITION) from " + _Schema + ".VW__FORMS_DAT_FILE " + i._Table + "a where " + i._Table + "a.FORMS_ID = " + i._Table + ".FORMS_ID) ", // GENERATE_JOIN_ON(_Connect, "FORMS_DAT_CHAR_ID", "p0", "FORMS_DAT_CHAR_ID", i._Table), //dynamicallly generate
                                    _HasWhere = false,
                                    _HasAggregateFunc = false,
                                    _HasGroupBy = false,
                                    _HasHaving = false
                                });
                            }
                            else if (i._DataType.ToLower() == "drop_down" || i._DataType.ToLower() == "radio_button" || i._DataType.ToLower() == "check_box")
                            {
                                //sqlStruct.Add(new sqlSelectStructure
                                //{
                                //    _TableName = owner + ".VW__FORMS_DAT_CHAR",
                                //    _TableAlias = i._Table.Replace("_opt", ""),
                                //    _HasFrom = false,
                                //    _HasJoin = true,
                                //    _JoinClause = "LEFT JOIN",
                                //    _JoinOn = "p0.FORMS_ID = " + i._Table.Replace("_opt", "") + ".FORMS_ID and " + i._Table.Replace("_opt", "") + ".PROPERTY_VALUE = '" + i._Alias + "' and " + i._Table + ".RENDITION in (Select max(" + i._Table + "a.RENDITION) from " + owner + ".VW__FORMS_DAT_CHAR " + i._Table + "a where " + i._Table + "a.FORMS_ID = " + i._Table + ".FORMS_ID) ", // GENERATE_JOIN_ON(_Connect, "FORMS_DAT_CHAR_ID", "p0", "FORMS_DAT_CHAR_ID", i._Table), //dynamicallly generate
                                //    _HasWhere = false,
                                //    _HasAggregateFunc = false,
                                //    _HasGroupBy = false,
                                //    _HasHaving = false
                                //});

                                if (!HasOptionsTable)
                                {

                                    sqlStruct.Add(new sqlSelectStructure
                                    {
                                        _TableName = _Schema + ".VW__FORMS_DAT_OPT",
                                        _TableAlias = i._Table,
                                        _HasFrom = false,
                                        _HasJoin = true,
                                        _JoinClause = "LEFT JOIN",
                                        _JoinOn = "p0.FORMS_ID = " + i._Table + ".FORMS_ID",
                                        _HasWhere = false,
                                        _HasAggregateFunc = false,
                                        _HasGroupBy = false,
                                        _HasHaving = false
                                    });

                                    HasOptionsTable = true;

                                }

                            }
                            else if (i._DataType.ToLower() == "number")
                            {
                                sqlStruct.Add(new sqlSelectStructure
                                {
                                    _TableName = _Schema + ".VW__FORMS_DAT_NUMB",
                                    _TableAlias = i._Table,
                                    _HasFrom = false,
                                    _HasJoin = true,
                                    _JoinClause = "LEFT JOIN",
                                    _JoinOn = "p0.FORMS_ID = " + i._Table + ".FORMS_ID and " + i._Table + ".PROPERTY_VALUE = '" + i._Alias + "' and " + i._Table + ".RENDITION in (Select max(" + i._Table + "a.RENDITION) from " + _Schema + ".VW__FORMS_DAT_NUMB " + i._Table + "a where " + i._Table + "a.FORMS_ID = " + i._Table + ".FORMS_ID) ", // GENERATE_JOIN_ON(_Connect, "FORMS_DAT_CHAR_ID", "p0", "FORMS_DAT_CHAR_ID", i._Table), //dynamicallly generate
                                    _HasWhere = false,
                                    _HasAggregateFunc = false,
                                    _HasGroupBy = false,
                                    _HasHaving = false
                                });
                            }
                            else if (i._DataType.ToLower() == "date" || i._DataType.ToLower() == "datetime" || i._DataType.ToLower() == "datetime2")
                            {
                                sqlStruct.Add(new sqlSelectStructure
                                {
                                    _TableName = _Schema + ".VW__FORMS_DAT_DATE",
                                    _TableAlias = i._Table,
                                    _HasFrom = false,
                                    _HasJoin = true,
                                    _JoinClause = "LEFT JOIN",
                                    _JoinOn = "p0.FORMS_ID = " + i._Table + ".FORMS_ID and " + i._Table + ".PROPERTY_VALUE = '" + i._Alias + "' and " + i._Table + ".RENDITION in (Select max(" + i._Table + "a.RENDITION) from " + _Schema + ".VW__FORMS_DAT_DATE " + i._Table + "a where " + i._Table + "a.FORMS_ID = " + i._Table + ".FORMS_ID) ",
                                    _HasWhere = false,
                                    _HasAggregateFunc = false,
                                    _HasGroupBy = false,
                                    _HasHaving = false
                                });
                            }
                            else if (i._DataType.ToLower() == "decimal" || i._DataType.ToLower() == "money")
                            {
                                sqlStruct.Add(new sqlSelectStructure
                                {
                                    _TableName = _Schema + ".VW__FORMS_DAT_DECI",
                                    _TableAlias = i._Table,
                                    _HasFrom = false,
                                    _HasJoin = true,
                                    _JoinClause = "LEFT JOIN",
                                    _JoinOn = "p0.FORMS_ID = " + i._Table + ".FORMS_ID and " + i._Table + ".PROPERTY_VALUE = '" + i._Alias + "' and " + i._Table + ".RENDITION in (Select max(" + i._Table + "a.RENDITION) from " + _Schema + ".VW__FORMS_DAT_DECI " + i._Table + "a where " + i._Table + "a.FORMS_ID = " + i._Table + ".FORMS_ID) ",
                                    _HasWhere = false,
                                    _HasAggregateFunc = false,
                                    _HasGroupBy = false,
                                    _HasHaving = false
                                });
                            }
                            else //All Other Objects
                            {
                                sqlStruct.Add(new sqlSelectStructure
                                {
                                    _TableName = _Schema + ".VW__FORMS_DAT_CHAR",
                                    _TableAlias = i._Table,
                                    _HasFrom = false,
                                    _HasJoin = true,
                                    _JoinClause = "LEFT JOIN",
                                    _JoinOn = "p0.FORMS_ID = " + i._Table + ".FORMS_ID and " + i._Table + ".PROPERTY_VALUE = '" + i._Alias + "' and " + i._Table + ".RENDITION in (Select max(" + i._Table + "a.RENDITION) from " + _Schema + ".VW__FORMS_DAT_CHAR " + i._Table + "a where " + i._Table + "a.FORMS_ID = " + i._Table + ".FORMS_ID) ", // GENERATE_JOIN_ON(_Connect, "FORMS_DAT_CHAR_ID", "p0", "FORMS_DAT_CHAR_ID", i._Table), //dynamicallly generate
                                    _HasWhere = false,
                                    _HasAggregateFunc = false,
                                    _HasGroupBy = false,
                                    _HasHaving = false
                                });
                            }
                        }
                        break;
                    case "identity":
                        if (i._Name.ToUpper() != "IDENTITIES_ID"
                            && i._Name.ToUpper() != "ENABLED"
                            && i._Name.ToUpper() != "DT_CREATED"
                            && i._Name.ToUpper() != "DT_AVAILABLE"
                            && i._Name.ToUpper() != "DT_END"
                            && i._Name.ToUpper() != "APPLICATIONS_ID")
                        { //Do Notihing Logi changed
                        }
                        break;
                }

            }

            switch (_stagetype.ToLower())
            {
                case "form":

                    sqlStruct.Add(new sqlSelectStructure
                    {
                        _TableName = _Schema + ".VW__STAGES",
                        _TableAlias = "s0",
                        _HasFrom = false,
                        _HasJoin = true,
                        _JoinClause = "INNER JOIN",
                        _JoinOn = "s0.APPLICATIONS_ID in (" + GetRelatedAppsfromStageID(_Connect, Convert.ToInt32(stageid)) + ") and s0.STAGES_ID = p1.STAGES_ID",
                        _HasWhere = false,
                        _HasAggregateFunc = false,
                        _HasGroupBy = false,
                        _HasHaving = false
                    });

                    break;
            }


            return sqlStruct;
        }

        public string GetRelatedAppsfromStageID(IConnectToDB _Connect, int StageID)
        {
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = "Select a.APPLICATIONS_ID, a.ROOT_APPLICATION from CSA.APPLICATIONS aa inner join CSA.Applications a on (aa.ROOT_APPLICATION = a.ROOT_APPLICATION) inner join  CSA.STAGES s ON (aa.APPLICATIONS_ID = s.APPLICATIONS_ID and s.STAGES_ID = " + StageID.ToString() + ")",
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = StageID.ToString() } }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            string AppID = "";

            if (DCC.Contains("APPLICATIONS_ID"))
            {
                foreach (DataRow _DR in _DT.Rows)
                {
                    AppID = AppID + _DR.Field<int>("APPLICATIONS_ID").ToString() + ",";
                }
            }

            AppID = AppID.TrimEnd(',');

            return AppID;
        }

        public StringBuilder GENERATE_DISABLE_PROCEDURE(IConnectToDB _Connect, string Name)
        {
            List<sqlProcedureStructure> ProcedureStructure = new List<sqlProcedureStructure>();
            List<sqlProcedureLineStructure> plc = new List<sqlProcedureLineStructure>();
            List<sqlProcedureParameterStructure> pps = new List<sqlProcedureParameterStructure>();
            sqlProcedureStructure ps = new sqlProcedureStructure();
            StringBuilder Buffer = new StringBuilder();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            foreach (sqlProcedureStructure i in ProcedureStructure)
            {
                Buffer.AppendLine("CREATE OR REPLACE PROCEDURE " + "DISABLE_OBJECT" + " IS ");

                int iNumber = 0;

                pps = new List<sqlProcedureParameterStructure>();

                pps.Add(new sqlProcedureParameterStructure { _Name = "P_SOURCE", _Direction = ParameterDirection.Input, _DataType = "VARCHAR(30)" });
                pps.Add(new sqlProcedureParameterStructure { _Name = "P_ID", _Direction = ParameterDirection.Input, _DataType = "VARCHAR(30)" });
                pps.Add(new sqlProcedureParameterStructure { _Name = "ENABLE", _Direction = ParameterDirection.Input, _DataType = "CHAR(1)" });
                ProcedureStructure.Add(new sqlProcedureStructure { _Parameters = pps });

                foreach (sqlProcedureParameterStructure ii in i._Parameters)
                {
                    iNumber++;

                    Buffer.AppendLine("   P_" + ii._Name + " " + ii._Direction.ToString() + " " + ii._DataType);

                    if (i._Parameters.Count < iNumber)
                        Buffer.Append(", ");
                }



                Buffer.AppendLine("DECLARE ");

                foreach (sqlProcedureLineStructure ii in i._Declare)
                {
                    Buffer.AppendLine("   " + ii.LineEntry);
                }

                Buffer.AppendLine("BEGIN ");

                foreach (sqlProcedureLineStructure ii in i._Body)
                {
                    Buffer.AppendLine("   " + ii.LineEntry);
                }

                Buffer.AppendLine("EXCEPTION ");

                foreach (sqlProcedureLineStructure ii in i._Exception)
                {
                    Buffer.AppendLine("   " + ii.LineEntry);
                }

                Buffer.AppendLine("END; ");
            }


            return Buffer;
        }

        public List<CommandResult> GENERATE_TOGGLE_PROCEDURE(IConnectToDB _Connect)
        {
            List<CommandResult> _result = new List<CommandResult>();
            _result.Add(new CommandResult
            {
                _Successful = true,
                _Response = "START MODULE GENERATE_TOGGLE_PROCEDURE"
            });
            CommandResult GenerateResult = new CommandResult();

            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_DDL er_ddl = new ER_DDL();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            _result.AddRange(er_ddl.DROP_PROCEDURE(_Connect, "_TOGGLE_OBJECT__RW"));

            try
            {
                //Will Create Procedure for Table
                DataTable dt = new DataTable();
                StringBuilder SQLin = new StringBuilder();
                string outID = "";
                string inID = "";
                string inDataType = "";
                string outDataType = "";
                string _ProcedurePrefix;
                int rowCount = 0;

                string TableName = "_TOGGLE_OBJECT";

                _ProcedurePrefix = "_RW";

                dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, true);

                DataTable table = SpoofColumnTableStruct();
                DataRow _row;

                _row = table.NewRow();
                _row["COLUMN_NAME"] = "ID";
                _row["DATA_TYPE"] = "int";
                table.Rows.Add(_row);

                _row = table.NewRow();
                _row["COLUMN_NAME"] = "ENABLED";
                _row["DATA_TYPE"] = "varchar";
                _row["DATA_LENGTH"] = "1";
                table.Rows.Add(_row);

                _row = table.NewRow();
                _row["COLUMN_NAME"] = "SOURCE";
                _row["DATA_TYPE"] = "VARCHAR";
                _row["DATA_LENGTH"] = "30";
                table.Rows.Add(_row);

                foreach (DataRow row in table.Rows)
                {
                    inID = row["COLUMN_NAME"].ToString();
                    inDataType = row["DATA_TYPE"].ToString();
                    outID = row["COLUMN_NAME"].ToString();
                    outDataType = row["DATA_TYPE"].ToString();
                }

                dt.Clear();

                //STEP 1 - Check if Table Exist
                //Step 2 - Get list of all Columns in Table
                dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, false);
                rowCount = dt.Rows.Count;

                //Step 3 - Create Procedure Begin Definition Syntax
                SQLin.Append("create ");
                SQLin.AppendLine("PROCEDURE " + _Schema + "." + er_tools.MaxNameLength(TableName, 23) + "_" + _ProcedurePrefix);

                switch (_Connect.Platform)
                {

                    case "Microsoft":
                    case "MICROSOFT":
                        //SQLin.AppendLine(" @I_" + inID + " [" + inDataType + "], ");
                        //Step 4 - Create Procedure Params
                        foreach (DataRow row in table.Rows)
                        {
                            if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                            {
                                SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + row["DATA_LENGTH"] + "), ");
                            }
                            else
                            {
                                SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "], ");
                            }
                        }

                        SQLin.AppendLine(" @O_" + "ID" + "[int] OUT ");

                        SQLin.AppendLine("AS");
                        SQLin.AppendLine("BEGIN");
                        SQLin.AppendLine("    SET NOCOUNT ON");

                        ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = "Select Table_Name from CSA.ER_TABLES",
                            _dbParameters = new List<DBParameters>()
                        };

                        DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
                        DataColumnCollection DCC = _DT.Columns;

                        SQLin.AppendLine("IF (@I_ENABLED in ('Y','y', 'N', 'n'))");
                        SQLin.AppendLine("  BEGIN");

                        foreach (DataRow _TableRow in _DT.Rows)
                        {
                            SQLin.AppendLine("      IF (@I_Source = '" + _TableRow.Field<string>("TABLE_NAME") + "') ");
                            SQLin.AppendLine("          BEGIN");
                            SQLin.AppendLine("              Update " + _TableRow.Field<string>("TABLE_NAME") + " SET ENABLED = @I_ENABLED, DT_UPDATED = getdate() where " + _TableRow.Field<string>("TABLE_NAME") + "_id = @I_ID");
                            SQLin.AppendLine("          END ");
                        }

                        SQLin.AppendLine("  END ");

                        SQLin.AppendLine();
                        SQLin.AppendLine("  SET @O_ID = @I_ID");

                        SQLin.AppendLine("END");

                        string SuccessMessage = "Create Procedure " + _Schema + "." + er_tools.MaxNameLength(TableName, 19) + "_" + _ProcedurePrefix + " created";
                        GenerateResult._Response = er_query.RUN_NON_QUERY(_Connect, SQLin.ToString(), SuccessMessage);
                        GenerateResult._Successful = GenerateResult._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                        GenerateResult._EndTime = DateTime.Now;
                        _result.Add(GenerateResult);
                        break;
                    default:
                        GenerateResult._Response = "Invalid DB Platform";
                        GenerateResult._Successful = false;
                        GenerateResult._EndTime = DateTime.Now;
                        _result.Add(GenerateResult);
                        break;
                }
            }
            catch (Exception e)
            {
                GenerateResult._Response = "Error Installing Toggler --- " + e.ToString();
                GenerateResult._Successful = false;
                GenerateResult._EndTime = DateTime.Now;
                _result.Add(GenerateResult);
            }

            return _result;
        }

        //Allows the content for a form to be toggled on and off.
        public List<CommandResult> GENERATE_FORM_TOGGLE_PROCEDURE(IConnectToDB _Connect)
        {
            List<CommandResult> _result = new List<CommandResult>();
            CommandResult GenerateResult = new CommandResult();

            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = _Connect.Schema;

            try
            {
                //Will Create Procedure for Table
                DataTable dt = new DataTable();
                StringBuilder SQLin = new StringBuilder();
                string outID = "";
                string inID = "";
                string inDataType = "";
                string outDataType = "";
                string _ProcedurePrefix;
                int rowCount = 0;

                string TableName = "TOGGLE_FORM_OBJECTS";


                _ProcedurePrefix = "_RW";


                dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, true);

                DataTable table = SpoofColumnTableStruct();
                DataRow _row;

                _row = table.NewRow();
                _row["COLUMN_NAME"] = "FORMS_ID";
                _row["DATA_TYPE"] = "bigint";
                table.Rows.Add(_row);

                _row = table.NewRow();
                _row["COLUMN_NAME"] = "STAGES_ID";
                _row["DATA_TYPE"] = "bigint";
                table.Rows.Add(_row);


                _row = table.NewRow();
                _row["COLUMN_NAME"] = "ENABLED";
                _row["DATA_TYPE"] = "varchar";
                _row["DATA_LENGTH"] = "1";
                table.Rows.Add(_row);

                foreach (DataRow row in table.Rows)
                {
                    inID = row["COLUMN_NAME"].ToString();
                    inDataType = row["DATA_TYPE"].ToString();
                    outID = row["COLUMN_NAME"].ToString();
                    outDataType = row["DATA_TYPE"].ToString();
                }

                dt.Clear();

                //STEP 1 - Check if Table Exist
                //Step 2 - Get list of all Columns in Table
                dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, false);
                rowCount = dt.Rows.Count;

                //Step 3 - Create Procedure Begin Definition Syntax
                SQLin.Append("create ");
                SQLin.AppendLine("PROCEDURE  " + _Schema + "." + er_tools.MaxNameLength(TableName, 23) + "_" + _ProcedurePrefix);

                switch (_Connect.Platform)
                {

                    case "Microsoft":
                    case "MICROSOFT":
                        //SQLin.AppendLine(" @I_" + inID + " [" + inDataType + "], ");
                        //Step 4 - Create Procedure Params
                        foreach (DataRow row in table.Rows)
                        {
                            if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                            {
                                SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + row["DATA_LENGTH"] + "), ");
                            }
                            else
                            {
                                SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "], ");
                            }
                        }

                        SQLin.AppendLine(" @O_" + "ID" + "[bigint] OUT ");

                        SQLin.AppendLine("AS");
                        //SQLin.AppendLine("declare @ID table (ID int)");
                        SQLin.AppendLine("BEGIN");
                        SQLin.AppendLine("    SET NOCOUNT ON");

                        ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = "Select TABLE_NAME from CSA.ER_TABLES WHERE TABLE_TYPE = 'Application Data' and TABLE_NAME like 'FORMS_%'",
                            _dbParameters = new List<DBParameters>()
                        };

                        DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
                        DataColumnCollection DCC = _DT.Columns;

                        SQLin.AppendLine("IF (@I_ENABLED in ('Y','y', 'N', 'n'))");
                        SQLin.AppendLine("  BEGIN");

                        if (DCC.Contains("TABLE_NAME"))
                        {
                            SQLin.AppendLine("          BEGIN");
                            SQLin.AppendLine("              UPDATE CSA.FORMS set enabled = @I_ENABLED WHERE FORMS_ID = @I_FORMS_ID");
                            SQLin.AppendLine("          END ");
                            foreach (DataRow _TableRow in _DT.Rows)
                            {
                                //SQLin.AppendLine("      IF (@I_Source = '" + _TableRow.Field<string>("TABLE_NAME") + "') ");
                                SQLin.AppendLine("          BEGIN");
                                SQLin.AppendLine("              UPDATE CSA." + _TableRow.Field<string>("TABLE_NAME") + " set enabled = @I_ENABLED WHERE FORMS_ID = @I_FORMS_ID and STAGES_ID =  @I_STAGES_ID");
                                SQLin.AppendLine("          END ");
                            }
                        }

                        SQLin.AppendLine("  END ");
                        SQLin.AppendLine();
                        SQLin.AppendLine("  SET @O_ID = @I_FORMS_ID");
                        SQLin.AppendLine("END");

                        string SuccessMessage = "Create Procedure " + _Schema + "." + er_tools.MaxNameLength(TableName, 19) + "_" + _ProcedurePrefix + " created";
                        GenerateResult._Response = er_query.RUN_NON_QUERY(_Connect, SQLin.ToString(), SuccessMessage);
                        GenerateResult._Successful = GenerateResult._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                        GenerateResult._EndTime = DateTime.Now;
                        _result.Add(GenerateResult);
                        break;
                    default:
                        GenerateResult._Response = "Invalid DB Platform";
                        GenerateResult._Successful = false;
                        GenerateResult._EndTime = DateTime.Now;
                        _result.Add(GenerateResult);
                        break;
                }
            }
            catch (Exception e)
            {
                GenerateResult._Response = "Error Installing Toggler --- " + e.ToString();
                GenerateResult._Successful = false;
                GenerateResult._EndTime = DateTime.Now;
                _result.Add(GenerateResult);
            }

            return _result;
        }

        public List<CommandResult> GENERATE_FULL_TEXT_INDEXES(IConnectToDB _Connect)
        {
            ER_DDL er_ddl = new ER_DDL();
            List<CommandResult> Logger = new List<CommandResult>();

            //For Available Data Tables
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = "Select * FROM CSA.ER_TABLES WHERE TABLE_TYPE='Application Data' and (TABLE_NAME like ('%_DAT_CHAR') or TABLE_NAME like ('%_DAT_OPT'))",
                _dbParameters = new List<DBParameters>()
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            if (DCC.Contains("TABLE_NAME"))
            {
                foreach (DataRow _DR in _DT.Rows)
                {
                    Logger.AddRange(er_ddl.ADD_INDEX_FULL_TEXT_SEARCH_FOR_DATA_VIEW(_Connect, _DR.Field<string>("TABLE_NAME"), new ColumnStructure { _Name = "Value" }));
                }
            }

            return Logger;
        }

        public DataTable SpoofColumnTableStruct()
        {
            DataTable table = new DataTable("ReturnData");
            DataColumn column;

            // Create first column and add to the DataTable.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "COLUMN_NAME";
            column.AutoIncrement = false;
            column.Caption = "COLUMN_NAME";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "DATA_TYPE";
            column.AutoIncrement = false;
            column.Caption = "DATA_TYPE";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "DATA_LENGTH";
            column.AutoIncrement = false;
            column.Caption = "DATA_LENGTH";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            return table;
        }

        public List<CommandResult> Generate_Update_Procedure_For_Source(IConnectToDB _Connect, string source)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);


            //Will Create Procedure for Table
            DataTable dt = new DataTable();
            StringBuilder SQLin = new StringBuilder();
            string outID = "";
            string inID = "";
            string inDataType = "";
            string outDataType = "";
            string _ProcedurePrefix;
            int rowCount = 0;

            string TableName = source.ToUpper();


            _ProcedurePrefix = "SP_U_MOD";


            dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, true);

            DataTable table = SpoofColumnTableStruct();
            DataRow _row;

            #region Table Structure
            _row = table.NewRow();
            _row["COLUMN_NAME"] = "Action";
            _row["DATA_TYPE"] = "varchar";
            _row["DATA_LENGTH"] = "MAX";
            table.Rows.Add(_row);

            _row = table.NewRow();
            _row["COLUMN_NAME"] = "SOURCE";
            _row["DATA_TYPE"] = "varchar";
            _row["DATA_LENGTH"] = "MAX";
            table.Rows.Add(_row);

            _row = table.NewRow();
            _row["COLUMN_NAME"] = "SOURCEFIELD";
            _row["DATA_TYPE"] = "varchar";
            _row["DATA_LENGTH"] = "MAX";
            table.Rows.Add(_row);

            _row = table.NewRow();
            _row["COLUMN_NAME"] = "SOURCEID";
            _row["DATA_TYPE"] = "bigint";
            table.Rows.Add(_row);

            _row = table.NewRow();
            _row["COLUMN_NAME"] = "VARVALUE";
            _row["DATA_TYPE"] = "varchar";
            _row["DATA_LENGTH"] = "MAX";
            table.Rows.Add(_row);

            _row = table.NewRow();
            _row["COLUMN_NAME"] = "NUMBVALUE";
            _row["DATA_TYPE"] = "bigint";
            table.Rows.Add(_row);

            _row = table.NewRow();
            _row["COLUMN_NAME"] = "DATEVALUE";
            _row["DATA_TYPE"] = "datetime";
            table.Rows.Add(_row);

            _row = table.NewRow();
            _row["COLUMN_NAME"] = "DECIVALUE";
            _row["DATA_TYPE"] = "decimal";
            table.Rows.Add(_row);

            _row = table.NewRow();
            _row["COLUMN_NAME"] = "RAWVALUE";
            _row["DATA_TYPE"] = "varbinary";
            _row["DATA_LENGTH"] = "MAX";
            table.Rows.Add(_row);
            #endregion

            foreach (DataRow row in table.Rows)
            {
                inID = row["COLUMN_NAME"].ToString();
                inDataType = row["DATA_TYPE"].ToString();
                outID = row["COLUMN_NAME"].ToString();
                outDataType = row["DATA_TYPE"].ToString();
            }

            dt.Clear();

            //STEP 1 - Check if Table Exist
            //Step 2 - Get list of all Columns in Table
            dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName);
            rowCount = dt.Rows.Count;

            string proc_name = _ProcedurePrefix + "_" + er_tools.MaxNameLength(TableName, (128 - (_ProcedurePrefix.Length + 1)));
            string SuccessMessage = "Procedure " + _Schema + "." + proc_name + " created";

            bool runLogic = true;
            //Step 3 - Create Procedure Begin Definition Syntax
            SQLin.Append("create ");
            SQLin.AppendLine("PROCEDURE " + _Schema + "." + proc_name.ToUpper());

            switch (_Connect.Platform.ToUpper())
            {
                case "ORACLE":
                    //TODO: Need to code Oracle Logic
                    break;
                case "MICROSOFT":
                    //SQLin.AppendLine(" @I_" + inID + " [" + inDataType + "], ");
                    //Step 4 - Create Procedure Params
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + row["DATA_LENGTH"] + ") = null, ");
                        }
                        else if (row["DATA_TYPE"].ToString().ToLower() == "varbinary")
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](MAX) = null, ");
                        }
                        else
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "]  = null, ");
                        }
                    }

                    SQLin.AppendLine(" @O_" + "ID" + "[int] OUT ");

                    SQLin.AppendLine("AS");
                    //SQLin.AppendLine("declare @ID table (ID int)");
                    SQLin.AppendLine("BEGIN");
                    SQLin.AppendLine("  SET NOCOUNT ON");


                    //DataTable AllTables = er_query.RUN_QUERY("Microsoft", _ConnStruct.DBConnString, "Select Table_Name from ER_TABLES");

                    SQLin.AppendLine("  IF (LOWER(@I_ACTION) in ('update'))");
                    SQLin.AppendLine("  BEGIN");
                    SQLin.AppendLine("      IF (LOWER(@I_Source) = '" + source.ToLower() + "') ");
                    SQLin.AppendLine("      BEGIN");

                    foreach (DataRow _TableRow in dt.Rows)
                    {
                        if (_TableRow.Field<string>("COLUMN_NAME").ToLower() != (source.ToLower() + "_id") && _TableRow.Field<string>("COLUMN_NAME").ToLower() != "dt_updated")
                        {
                            SQLin.AppendLine("      IF (LOWER(@I_SOURCEFIELD) = '" + _TableRow.Field<string>("COLUMN_NAME").ToLower() + "') ");
                            SQLin.AppendLine("      BEGIN");

                            var columnName = _TableRow.Field<string>("COLUMN_NAME");
                            var dataType = _TableRow.Field<string>("DATA_TYPE");

                            var stringTypes = new string[] { "varchar", "char", "uniqueidentifier" };
                            if (stringTypes.Contains(dataType.ToLower()))
                            {
                                SQLin.AppendLine("              IF(@I_VARVALUE is not null and @I_NUMBVALUE is null and @I_DATEVALUE is null and @I_RAWVALUE is null and @I_DECIVALUE is null )");
                                SQLin.AppendLine("              BEGIN");
                                SQLin.AppendLine("                  Update " + _Schema + "." + source + " SET " + columnName + " = @I_VARVALUE, DT_UPDATED = getdate() where " + source + "_id = @I_SOURCEID");
                                SQLin.AppendLine("              END");
                            }

                            var NumberTypes = new string[] { "int", "bigint", "smallint" };
                            if (NumberTypes.Contains(dataType.ToLower()) && _TableRow.Field<string>("COLUMN_NAME").ToLower() != (source.ToLower() + "_id"))
                            {
                                SQLin.AppendLine("              IF(@I_VARVALUE is null and @I_NUMBVALUE is not null and @I_DATEVALUE is null and @I_RAWVALUE is null and @I_DECIVALUE is null)");
                                SQLin.AppendLine("              BEGIN");
                                SQLin.AppendLine("                  Update " + _Schema + "." + source + " SET " + columnName + " = @I_NUMBVALUE, DT_UPDATED = getdate() where " + source + "_id = @I_SOURCEID");
                                SQLin.AppendLine("              END");
                            }

                            var DateTypes = new string[] { "datetime", "date", "datetime2" };
                            if (DateTypes.Contains(dataType.ToLower()) && _TableRow.Field<string>("COLUMN_NAME").ToLower() != "dt_updated")
                            {
                                SQLin.AppendLine("              IF(@I_VARVALUE is null and @I_NUMBVALUE is null and @I_DATEVALUE is not null and @I_RAWVALUE is null and @I_DECIVALUE is null)");
                                SQLin.AppendLine("              BEGIN");
                                SQLin.AppendLine("                  Update " + _Schema + "." + source + " SET " + columnName + " = @I_DATEVALUE, DT_UPDATED = getdate() where " + source + "_id = @I_SOURCEID");
                                SQLin.AppendLine("              END");
                            }

                            var DecimalTypes = new string[] { "decimal", "money" };
                            if (DecimalTypes.Contains(dataType.ToLower()))
                            {
                                SQLin.AppendLine("              IF(@I_VARVALUE is  null and @I_NUMBVALUE is null and @I_DATEVALUE is null and @I_RAWVALUE is null and @I_DECIVALUE is not null)");
                                SQLin.AppendLine("              BEGIN");
                                SQLin.AppendLine("                  Update " + _Schema + "." + source + " SET " + columnName + " = @I_DECIVALUE, DT_UPDATED = getdate() where " + source + "_id = @I_SOURCEID");
                                SQLin.AppendLine("              END");
                            }

                            if (dataType == "varbinary")
                            {
                                SQLin.AppendLine("              IF(@I_VARVALUE is  null and @I_NUMBVALUE is null and @I_DATEVALUE is null and @I_RAWVALUE is not null and @I_DECIVALUE is null)");
                                SQLin.AppendLine("              BEGIN");
                                SQLin.AppendLine("                  Update " + _Schema + "." + source + " SET " + columnName + " = @I_RAWVALUE, DT_UPDATED = getdate() where " + source + "_id = @I_SOURCEID");
                                SQLin.AppendLine("              END");
                            }

                            SQLin.AppendLine("      END ");
                        }
                    }
                    SQLin.AppendLine("      END ");
                    SQLin.AppendLine("  END ");

                    SQLin.AppendLine();
                    SQLin.AppendLine("  SET @O_ID = @I_SOURCEID");

                    SQLin.AppendLine("END");
                    break;
                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result.attemptedCommand = SQLin.ToString();
                _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            }

            results.Add(_result);

            return results;
        }

        public List<CommandResult> Generate_APP_NAME_VIEW(IConnectToDB _Connect, Boolean _force)
        {
            List<CommandResult> _result = new List<CommandResult>();
            ER_Query er_query = new ER_Query();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = "SELECT count(APPLICATION_NAME) as APPLICATION_NAME FROM CSA.VW__ER_APP_NAMES",
                _dbParameters = new List<DBParameters>()
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            string SuccessMessage = "Successfully Created Existing APP NAME VIEW";

            string _sqlin = "CREATE VIEW CSA.VW__ER_APP_NAMES as SELECT distinct APPLICATION_NAME, CORES_ID " +
                    "FROM CSA.APPLICATIONS " +
                    "UNION " +
                    "SELECT ROOT_APPLICATION, CORES_ID " +
                    "FROM CSA.APPLICATIONS";
            bool runLogic = true;

            if (!DCC.Contains("APPLICATION_NAME"))
            {
                runLogic = false;
                CommandResult thisResult = new CommandResult();
                thisResult._Response = er_query.RUN_NON_QUERY(_Connect, _sqlin, SuccessMessage);
                thisResult._Successful = thisResult._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                _result.Add(thisResult);
            }

            if (runLogic && DCC.Contains("APPLICATION_NAME") && _force)
            {
                string SuccessMessage2 = "Dropped View VW__ER_APP_NAMES";
                string _sqlin2 = "Drop View " + _Schema + ".VW__ER_APP_NAMES";

                CommandResult thisResult = new CommandResult();
                thisResult._Response = er_query.RUN_NON_QUERY(_Connect, _sqlin2, SuccessMessage2);
                thisResult._Successful = thisResult._Response.IndexOf(SuccessMessage2) > -1 ? true : false;
                _result.Add(thisResult);

                SuccessMessage = "Successfully Re-Created Existing APP NAME VIEW";

                thisResult = new CommandResult();
                thisResult._Response = er_query.RUN_NON_QUERY(_Connect, _sqlin, SuccessMessage);
                thisResult._Successful = thisResult._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                _result.Add(thisResult);
            }


            return _result;

        }

        public List<CommandResult> Generate_Search_Proc(IConnectToDB _Connect, string source)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);


            //Will Create Procedure for Table
            DataTable dt = new DataTable();
            StringBuilder SQLin = new StringBuilder();
            string _ProcedurePrefix;
            int rowCount = 0;

            string TableName = source.ToUpper();


            _ProcedurePrefix = "_SEARCH";


            dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, false);

            rowCount = dt.Rows.Count;

            string SuccessMessage = "Procedure " + _Schema + "." + er_tools.MaxNameLength(TableName, 19) + "_" + _ProcedurePrefix + " created";

            bool runLogic = true;

            //Step 3 - Create Procedure Begin Definition Syntax
            SQLin.Append("create ");
            SQLin.AppendLine("PROCEDURE " + _Schema + "." + er_tools.MaxNameLength(TableName, 27) + _ProcedurePrefix);

            #region DECLARATIONS 1
            SQLin.AppendLine(" @I_" + "SEARCH" + " [" + "varchar" + "](" + "MAX" + "), ");
            SQLin.AppendLine(" @I_" + "WHERE" + " [" + "varchar" + "](" + "MAX" + "), ");
            SQLin.AppendLine(" @I_" + "STARTING_ROW" + " [" + "bigint" + "], ");
            SQLin.AppendLine(" @I_" + "LENGTH_OF_SET" + " [" + "bigint" + "], ");
            SQLin.AppendLine(" @I_" + "ORDER_BY" + " [" + "varchar" + "](" + "MAX" + "), ");

            #endregion
            switch (_Connect.Platform.ToUpper())
            {
                case "MICROSOFT":

                    #region DECLARATIONS 2
                    //Step 4 - Create Procedure Params
                    foreach (DataRow row in dt.Rows)
                    {

                        if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                        {
                            if (row["DATA_LENGTH"].ToString() == "-1")
                            {
                                SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + "MAX" + "), ");
                            }
                            else
                            {
                                SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + row["DATA_LENGTH"] + "), ");
                            }
                        }
                        else if (row["DATA_TYPE"].ToString().ToLower() == "varbinary")
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](MAX), ");
                        }
                        else
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "], ");
                        }
                    }

                    SQLin.AppendLine(" @I_" + "VERIFY" + "[CHAR](1) ");
                    #endregion

                    SQLin.AppendLine("AS");
                    //SQLin.AppendLine("declare @ID table (ID int)");
                    SQLin.AppendLine("BEGIN");
                    SQLin.AppendLine("  SET NOCOUNT ON");


                    #region DECLARATIONS 3
                    SQLin.AppendLine("  DECLARE @V_BOOL CHAR(1) = 'T' ");
                    SQLin.AppendLine("  DECLARE @V_SEARCH varchar(MAX) = '%' + @I_SEARCH + '%' ");
                    SQLin.AppendLine("  DECLARE @V_RUN_SEARCH CHAR(1) = 'F' ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("  DECLARE @V_RUN_SEARCH_" + row["COLUMN_NAME"] + " CHAR(1) = 'F' ");
                    }
                    SQLin.AppendLine("  DECLARE @V_RUN_ROWS CHAR(1) = 'F' ");
                    SQLin.AppendLine("  DECLARE @RevampQuery nvarchar(MAX) ");
                    #endregion
                    SQLin.AppendLine("    ");
                    #region Checking Parameter Conditions
                    SQLin.AppendLine("  IF @V_SEARCH != '' and @V_SEARCH is not null and @V_SEARCH != '%%' and RTRIM(@V_SEARCH) != '' and RTRIM(@V_SEARCH) != '%%' ");
                    SQLin.AppendLine("  BEGIN  ");
                    SQLin.AppendLine("    ");
                    SQLin.AppendLine("    SET @V_RUN_SEARCH = 'T' ");
                    SQLin.AppendLine("    ");
                    SQLin.AppendLine("  END  ");
                    SQLin.AppendLine("    ");
                    SQLin.AppendLine("  IF @I_LENGTH_OF_SET != -1  ");
                    SQLin.AppendLine("  BEGIN  ");
                    SQLin.AppendLine("    ");
                    SQLin.AppendLine("    SET @V_RUN_ROWS = 'T' ");
                    SQLin.AppendLine("    ");
                    SQLin.AppendLine("  END  ");


                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("    ");

                        if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                        {
                            SQLin.AppendLine("  IF  @I_" + row["COLUMN_NAME"] + " != '' and  @I_" + row["COLUMN_NAME"] + " is not null and  @I_" + row["COLUMN_NAME"] + " != '%%' and RTRIM( @I_" + row["COLUMN_NAME"] + ") != '' and RTRIM( @I_" + row["COLUMN_NAME"] + ") != '%%' ");
                        }
                        else if (row["DATA_TYPE"].ToString().ToLower() == "varbinary")
                        {
                            SQLin.AppendLine("  IF  @I_" + row["COLUMN_NAME"] + " != '' and  @I_" + row["COLUMN_NAME"] + " is not null  and RTRIM( @I_" + row["COLUMN_NAME"] + ") != '' ");
                        }
                        else if (row["DATA_TYPE"].ToString().ToLower() == "bigint" || row["DATA_TYPE"].ToString().ToLower() == "int")
                        {
                            SQLin.AppendLine("  IF  @I_" + row["COLUMN_NAME"] + " != '' and  @I_" + row["COLUMN_NAME"] + " is not null ");
                        }
                        else
                        {
                            SQLin.AppendLine("  IF  @I_" + row["COLUMN_NAME"] + " != '' and  @I_" + row["COLUMN_NAME"] + " is not null and RTRIM( @I_" + row["COLUMN_NAME"] + ") != ''");
                        }


                        //SQLin.AppendLine("  IF  @I_" + row["COLUMN_NAME"] + " != '' and  @I_" + row["COLUMN_NAME"] + " is not null and  @I_" + row["COLUMN_NAME"] + " != '%%' and RTRIM( @I_" + row["COLUMN_NAME"] + ") != '' and RTRIM( @I_" + row["COLUMN_NAME"] + ") != '%%' ");


                        SQLin.AppendLine("  BEGIN  ");
                        SQLin.AppendLine("    ");
                        SQLin.AppendLine("    SET @V_RUN_SEARCH_" + row["COLUMN_NAME"] + " = 'T' ");
                        SQLin.AppendLine("    ");
                        SQLin.AppendLine("  END  ");

                    }
                    #endregion

                    SQLin.AppendLine("BEGIN TRY");
                    SQLin.AppendLine("SET @RevampQuery = 'select * from ( ' + ");
                    SQLin.AppendLine("  'SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 0)) as rownumb, q1.* ' + ");
                    SQLin.AppendLine("  'FROM ( ' + ");
                    SQLin.AppendLine("  '   SELECT distinct s.* from ' + '" + _Schema + "." + TableName + "' + ' s ' + ");
                    SQLin.AppendLine("  '   WHERE (' + ");
                    SQLin.AppendLine("  '               (' + ");
                    SQLin.AppendLine("  '                   (   @DYN_BOOL !=  @DYN_RUN_SEARCH) ' + ");
                    SQLin.AppendLine("  '                   OR ' + ");
                    SQLin.AppendLine("  '                   ( @DYN_BOOL = @DYN_RUN_SEARCH and ' + ");
                    SQLin.AppendLine("  '                       (' + ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("  '                           (" + row["COLUMN_NAME"] + " like @DYN_SEARCH) or ' + ");

                    }
                    SQLin.AppendLine("  '                           (''Close'' = ''Clause'')' + ");
                    SQLin.AppendLine("  '                       )' + ");
                    SQLin.AppendLine("  '                   )  ' + ");
                    SQLin.AppendLine("  '               ) and' + ");
                    SQLin.AppendLine("  '               (' + ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("  '                ' + ");
                        SQLin.AppendLine("  '                   ( @DYN_BOOL != @DYN_RUN_SEARCH_" + row["COLUMN_NAME"] + " or  ' + ");
                        SQLin.AppendLine("  '                       (@DYN_BOOL = @DYN_RUN_SEARCH_" + row["COLUMN_NAME"] + " and ' + ");
                        SQLin.AppendLine("  '                       ' + ");
                        SQLin.AppendLine("  '                           " + row["COLUMN_NAME"] + " = @DYN_" + row["COLUMN_NAME"] + " ' + ");
                        SQLin.AppendLine("  '                       )' + ");
                        SQLin.AppendLine("  '                   ) AND ' + ");
                    }
                    SQLin.AppendLine("  '                   (''Close'' != ''Clause'')' + ");
                    SQLin.AppendLine("  '               )' + ");
                    SQLin.AppendLine("  '           ) ' + ");
                    SQLin.AppendLine("  '   ) q1 ' + ");
                    SQLin.AppendLine("  ') q2 ' + ");
                    SQLin.AppendLine("  'WHERE ( @DYN_BOOL != @DYN_RUN_ROWS) or (@DYN_BOOL =  @DYN_RUN_ROWS and rownumb between ' + CONVERT(varchar, @I_STARTING_ROW) + ' and ' + CONVERT(varchar,@I_LENGTH_OF_SET) + ') ' + ");
                    SQLin.AppendLine("  'ORDER BY ' + @I_ORDER_BY ");
                    SQLin.AppendLine(" ");
                    SQLin.AppendLine(" EXECUTE sp_executesql @RevampQuery, N'@DYN_BOOL char(1), ");
                    SQLin.AppendLine("                          @DYN_RUN_SEARCH char(1), ");
                    SQLin.AppendLine("                          @DYN_RUN_ROWS char(1), ");
                    SQLin.AppendLine("                          @DYN_SEARCH varchar(MAX), ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("                          @DYN_RUN_SEARCH_" + row["COLUMN_NAME"] + " CHAR(1), ");
                        if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                        {
                            if (row["DATA_LENGTH"].ToString().ToLower() == "-1")
                            {
                                SQLin.AppendLine("                          @DYN_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + "MAX" + "), ");
                            }
                            else
                            {
                                SQLin.AppendLine("                          @DYN_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + row["DATA_LENGTH"] + "), ");
                            }
                        }
                        else if (row["DATA_TYPE"].ToString().ToLower() == "varbinary")
                        {
                            SQLin.AppendLine("                          @DYN_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](MAX), ");
                        }
                        else
                        {
                            SQLin.AppendLine("                          @DYN_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "], ");
                        }
                    }
                    SQLin.AppendLine("                          @DYN_ORDER_BY varchar(MAX)', ");
                    SQLin.AppendLine("                          @DYN_BOOL = @V_BOOL, ");
                    SQLin.AppendLine("                          @DYN_RUN_SEARCH = @V_RUN_SEARCH, ");
                    SQLin.AppendLine("                          @DYN_RUN_ROWS = @V_RUN_ROWS, ");
                    SQLin.AppendLine("                          @DYN_SEARCH = @V_SEARCH, ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("                          @DYN_RUN_SEARCH_" + row["COLUMN_NAME"] + " = @V_RUN_SEARCH_" + row["COLUMN_NAME"] + ", ");
                        SQLin.AppendLine("                          @DYN_" + row["COLUMN_NAME"] + " = @I_" + row["COLUMN_NAME"] + ", ");

                    }
                    SQLin.AppendLine("                          @DYN_ORDER_BY =  @I_ORDER_BY");

                    SQLin.AppendLine("END TRY");
                    SQLin.AppendLine("BEGIN CATCH");
                    //SQLin.AppendLine(" RETURN  @RevampQuery ");
                    SQLin.AppendLine("  DECLARE @O_ERR_NUMB bigint ");
                    SQLin.AppendLine("  DECLARE @O_ERR_MESS varchar(MAX) ");
                    SQLin.AppendLine("  SELECT @O_ERR_NUMB = ERROR_NUMBER() ");
                    SQLin.AppendLine("  SELECT @O_ERR_MESS = ERROR_MESSAGE() ");
                    SQLin.AppendLine("  SELECT @O_ERR_NUMB err_numb, @O_ERR_MESS err_mess ");
                    SQLin.AppendLine("END CATCH ");


                    SQLin.AppendLine("END");

                    break;
                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLin.ToString(), SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            }

            results.Add(_result);

            return results;
        }

        public List<CommandResult> Generate_Search_Count_Proc(IConnectToDB _Connect, string source)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            //Will Create Procedure for Table
            DataTable dt = new DataTable();
            StringBuilder SQLin = new StringBuilder();
            string _ProcedurePrefix = "_SEARCH_COUNT";
            int rowCount = 0;

            string TableName = source.ToUpper();

            dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, false);

            rowCount = dt.Rows.Count;

            string SuccessMessage = "Procedure " + _Schema + "." + er_tools.MaxNameLength(TableName, 19) + "_" + _ProcedurePrefix + " created";

            bool runLogic = true;

            //Step 3 - Create Procedure Begin Definition Syntax
            SQLin.Append("create ");
            SQLin.AppendLine("PROCEDURE " + _Schema + "." + er_tools.MaxNameLength(TableName, 27) + _ProcedurePrefix);

            SQLin.AppendLine(" @I_" + "SEARCH" + " [" + "varchar" + "](" + "MAX" + "), ");
            SQLin.AppendLine(" @I_" + "WHERE" + " [" + "varchar" + "](" + "MAX" + "), ");

            switch (_Connect.Platform.ToUpper())
            {
                case "MICROSOFT":

                    //Step 4 - Create Procedure Params
                    foreach (DataRow row in dt.Rows)
                    {

                        if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + row["DATA_LENGTH"] + "), ");
                        }
                        else if (row["DATA_TYPE"].ToString().ToLower() == "varbinary")
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](MAX), ");
                        }
                        else
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "], ");
                        }
                    }

                    SQLin.AppendLine(" @I_" + "VERIFY" + "[CHAR](1), ");
                    SQLin.AppendLine(" @O_ATTEMPTED_RESULTS VARCHAR(MAX) OUT  ");

                    SQLin.AppendLine("AS");
                    //SQLin.AppendLine("declare @ID table (ID int)");
                    SQLin.AppendLine("BEGIN");
                    SQLin.AppendLine("  SET NOCOUNT ON");


                    SQLin.AppendLine("  DECLARE @V_BOOL CHAR(1) = 'T' ");
                    SQLin.AppendLine("  DECLARE @V_SEARCH varchar(MAX) = '%' + @I_SEARCH + '%' ");
                    SQLin.AppendLine("  DECLARE @V_RUN_SEARCH CHAR(1) = 'F' ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("  DECLARE @V_RUN_SEARCH_" + row["COLUMN_NAME"] + " CHAR(1) = 'F' ");
                    }
                    SQLin.AppendLine("  DECLARE @V_RUN_ROWS CHAR(1) = 'F' ");
                    SQLin.AppendLine("  DECLARE @RevampQuery nvarchar(MAX) ");
                    SQLin.AppendLine("    ");
                    SQLin.AppendLine("  IF @V_SEARCH != '' and @V_SEARCH is not null and @V_SEARCH != '%%' and RTRIM(@V_SEARCH) != '' and RTRIM(@V_SEARCH) != '%%' ");
                    SQLin.AppendLine("  BEGIN  ");
                    SQLin.AppendLine("    ");
                    SQLin.AppendLine("    SET @V_RUN_SEARCH = 'T' ");
                    SQLin.AppendLine("    ");
                    SQLin.AppendLine("  END  ");
                    SQLin.AppendLine("    ");

                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("    ");
                        SQLin.AppendLine("  IF  @I_" + row["COLUMN_NAME"] + " != '' and  @I_" + row["COLUMN_NAME"] + " is not null and  @I_" + row["COLUMN_NAME"] + " != '%%' and RTRIM( @I_" + row["COLUMN_NAME"] + ") != '' and RTRIM( @I_" + row["COLUMN_NAME"] + ") != '%%' ");
                        SQLin.AppendLine("  BEGIN  ");
                        SQLin.AppendLine("    ");
                        SQLin.AppendLine("    SET @V_RUN_SEARCH_" + row["COLUMN_NAME"] + " = 'T' ");
                        SQLin.AppendLine("    ");
                        SQLin.AppendLine("  END  ");
                    }

                    SQLin.AppendLine("BEGIN TRY");
                    SQLin.AppendLine("SET @RevampQuery = 'select @DYN_COUNT = count(*) from ( ' + ");
                    SQLin.AppendLine("  'SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 0)) as rownumb, q1.* ' + ");
                    SQLin.AppendLine("  'FROM ( ' + ");
                    SQLin.AppendLine("  '   SELECT distinct s.* from ' + '" + _Schema + "." + TableName + "' + ' s ' + ");
                    SQLin.AppendLine("  '   WHERE (' + ");
                    SQLin.AppendLine("  '               (' + ");
                    SQLin.AppendLine("  '                   (   @DYN_BOOL !=  @DYN_RUN_SEARCH) ' + ");
                    SQLin.AppendLine("  '                   OR ' + ");
                    SQLin.AppendLine("  '                   ( @DYN_BOOL = @DYN_RUN_SEARCH and ' + ");
                    SQLin.AppendLine("  '                       (' + ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("  '                           (" + row["COLUMN_NAME"] + " like @DYN_SEARCH) or ' + ");

                    }
                    SQLin.AppendLine("  '                           (''Close'' = ''Clause'')' + ");
                    SQLin.AppendLine("  '                       )' + ");
                    SQLin.AppendLine("  '                   )  ' + ");
                    SQLin.AppendLine("  '               ) and' + ");
                    SQLin.AppendLine("  '               (' + ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("  '                ' + ");
                        SQLin.AppendLine("  '                   ( @DYN_BOOL != @DYN_RUN_SEARCH_" + row["COLUMN_NAME"] + " or  ' + ");
                        SQLin.AppendLine("  '                       (@DYN_BOOL = @DYN_RUN_SEARCH_" + row["COLUMN_NAME"] + " and ' + ");
                        SQLin.AppendLine("  '                       ' + ");
                        SQLin.AppendLine("  '                           " + row["COLUMN_NAME"] + " = @DYN_" + row["COLUMN_NAME"] + " ' + ");
                        SQLin.AppendLine("  '                       )' + ");
                        SQLin.AppendLine("  '                   ) AND ' + ");
                    }
                    SQLin.AppendLine("  '                   (''Close'' != ''Clause'')' + ");
                    SQLin.AppendLine("  '               )' + ");
                    SQLin.AppendLine("  '           ) ' + ");
                    SQLin.AppendLine("  '   ) q1 ' + ");
                    SQLin.AppendLine("  ') q2 ' ");
                    SQLin.AppendLine(" ");
                    SQLin.AppendLine(" EXECUTE sp_executesql @RevampQuery, N'@DYN_BOOL char(1), ");
                    SQLin.AppendLine("                          @DYN_RUN_SEARCH char(1), ");
                    SQLin.AppendLine("                          @DYN_SEARCH varchar(MAX), ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("                          @DYN_RUN_SEARCH_" + row["COLUMN_NAME"] + " CHAR(1), ");
                        if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                        {
                            SQLin.AppendLine("                          @DYN_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + row["DATA_LENGTH"] + "), ");
                        }
                        else if (row["DATA_TYPE"].ToString().ToLower() == "varbinary")
                        {
                            SQLin.AppendLine("                          @DYN_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](MAX), ");
                        }
                        else
                        {
                            SQLin.AppendLine("                          @DYN_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "], ");
                        }
                    }
                    SQLin.AppendLine("                          @DYN_COUNT bigint out', ");
                    SQLin.AppendLine("                          @DYN_BOOL = @V_BOOL, ");
                    SQLin.AppendLine("                          @DYN_RUN_SEARCH = @V_RUN_SEARCH, ");
                    SQLin.AppendLine("                          @DYN_SEARCH = @V_SEARCH, ");
                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine("                          @DYN_RUN_SEARCH_" + row["COLUMN_NAME"] + " = @V_RUN_SEARCH_" + row["COLUMN_NAME"] + ", ");
                        SQLin.AppendLine("                          @DYN_" + row["COLUMN_NAME"] + " = @I_" + row["COLUMN_NAME"] + ", ");

                    }

                    SQLin.AppendLine("                          @DYN_COUNT = @O_ATTEMPTED_RESULTS output");
                    SQLin.AppendLine("END TRY");
                    SQLin.AppendLine("BEGIN CATCH");
                    SQLin.AppendLine(" RETURN  @RevampQuery ");
                    SQLin.AppendLine("END CATCH ");


                    SQLin.AppendLine("END");

                    break;
                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLin.ToString(), SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            }

            results.Add(_result);

            return results;
        }

        public List<CommandResult> Generate_Search_Scaffold(IConnectToDB _Connect, string source)
        {
            List<CommandResult> results = new List<CommandResult>();

            results.AddRange(Generate_Search_Proc(_Connect, source));
            results.AddRange(Generate_Search_Count_Proc(_Connect, source));

            return results;
        }

        public List<CommandResult> ADD_PROCEDURE_INSERT(IConnectToDB _Connect, string TableName, string ProcedurePrefix)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);
            #region MyRegion

            /* //Will Create Procedure for Table
             DataTable dt = new DataTable();
             StringBuilder SQLin = new StringBuilder();
             //string tempstring = "";
             string outID = "";
             string outDataType = "";
             string _ProcedurePrefix;

             if (ProcedurePrefix != "")
                 _ProcedurePrefix = ProcedurePrefix;
             else
                 _ProcedurePrefix = "ADD_RW";

             dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, true);

             foreach (DataRow row in dt.Rows)
             {
                 outID = row["COLUMN_NAME"].ToString();
                 outDataType = row["DATA_TYPE"].ToString();
             }

             dt.Clear();

             //STEP 1 - Check if Table Exist
             //Step 2 - Get list of all Columns in Table
             dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, false);

             bool runLogic = true;
             string SuccessMessage = "Procedure " + _Connect.SourceDBOwner + "." + _Schema + "." + er_tools.MaxNameLength(TableName, (128 - (_ProcedurePrefix.Length + 1))) + "_" + _ProcedurePrefix + " created";

             SQLin.Append("create ");
             SQLin.AppendLine("PROCEDURE " + _Schema + "." + er_tools.MaxNameLength(TableName.ToUpper(), (128 - (_ProcedurePrefix.Length + 1))) + "_" + _ProcedurePrefix);

             switch (_Connect.Platform.ToUpper())
             {
                 case "ORACLE":

                     //Step 3 - Create Procedure Begin Definition Syntax
                     SQLin.AppendLine(" ( ");

                     //Step 4 - Create Procedure Params
                     foreach (DataRow row in dt.Rows)
                     {
                         SQLin.AppendLine(" P_" + row["COLUMN_NAME"] + " IN " + row["DATA_TYPE"] + ", ");
                     }

                     SQLin.AppendLine(" R_" + outID + " OUT " + outDataType);
                     //Step 5 - Create Procedure Body Syntax
                     SQLin.AppendLine(") AS ");
                     SQLin.AppendLine("BEGIN ");
                     SQLin.AppendLine(" INSERT into " + _Schema + "." + TableName + " (");

                     int iNumber = 0;
                     int rowCount = dt.Rows.Count;

                     foreach (DataRow row in dt.Rows)
                     {
                         iNumber++;

                         if (iNumber < rowCount)
                             SQLin.AppendLine("  " + row["COLUMN_NAME"] + ", ");
                         else if (iNumber == rowCount)
                             SQLin.AppendLine("  " + row["COLUMN_NAME"] + " ) ");
                     }

                     SQLin.AppendLine(" VALUES ( ");

                     iNumber = 0;
                     foreach (DataRow row in dt.Rows)
                     {
                         iNumber++;

                         if (iNumber < rowCount)

                             if (row["DATA_TYPE"].ToString().ToLower().Contains("raw"))
                             {
                                 SQLin.AppendLine(" HEXTORAW(P_" + row["COLUMN_NAME"] + "), ");
                             }
                             else
                             {
                                 SQLin.AppendLine("  P_" + row["COLUMN_NAME"] + ", ");
                             }
                         else if (iNumber == rowCount)
                             if (row["DATA_TYPE"].ToString().ToLower().Contains("raw"))
                             {
                                 SQLin.AppendLine(" HEXTORAW(P_" + row["COLUMN_NAME"] + ")) ");
                             }
                             else
                             {
                                 SQLin.AppendLine("  P_" + row["COLUMN_NAME"] + " ) ");
                             }
                     }

                     SQLin.AppendLine(" RETURNING " + outID + " INTO R_" + outID + ";");

                     //Step 6 - Create Procedure End Definition Syntax
                     SQLin.AppendLine("END; ");

                     //Step 7 - Submit Procedure Syntax to Database/

                     // return SQLin.ToString();
                     break;

                 case "MICROSOFT":
                     string TableID = TableName + "_ID";
                     string TableBaseID = "BASE_" + TableID;
                     string TablePrevID = "PREV_" + TableID;
                     string TableUUID = TableName + "_UUID";
                     string[] excludeColumns = new string[] { TableID, TableUUID, "DT_CREATED", "DT_UPDATED", };

                     //Step 4 - Create Procedure Params
                     foreach (DataRow row in dt.Rows)
                     {
                         if (!excludeColumns.Contains(row["COLUMN_NAME"]))
                         {
                             if (row["COLUMN_NAME"].ToString().ToUpper() == TableBaseID || row["COLUMN_NAME"].ToString().ToUpper() == TablePrevID)
                             {
                                 SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "] = 0, ");
                             }
                             else if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                             {
                                 string data_length = row["DATA_LENGTH"].ToString();

                                 SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + row["DATA_LENGTH"] + "), ");
                             }
                             else if (row["DATA_TYPE"].ToString().ToLower() == "decimal")
                             {
                                 string precisionScale = row["NUMERIC_PRECISION"] + "," + row["NUMERIC_SCALE"];

                                 SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + precisionScale + "), ");
                             }
                             else if (row["DATA_TYPE"].ToString().ToLower() == "varbinary")
                             {
                                 SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](MAX), ");
                             }
                             else
                             {
                                 SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "], ");
                             }
                         }
                     }

                     SQLin.AppendLine(" @O_" + outID + "[" + outDataType + "] OUT ");

                     SQLin.AppendLine("AS");
                     SQLin.AppendLine("declare @ID table (ID int)");
                     SQLin.AppendLine("BEGIN");
                     SQLin.AppendLine("    SET NOCOUNT ON");

                     SQLin.AppendLine(" INSERT into " + _Schema + "." + TableName + " (");

                     int iNumber1 = 0;
                     int rowCount1 = dt.Rows.Count;

                     foreach (DataRow row in dt.Rows)
                     {
                         iNumber1++;

                         if (!excludeColumns.Contains(row["COLUMN_NAME"]))
                         {
                             if (iNumber1 < rowCount1)
                                 SQLin.AppendLine("  " + row["COLUMN_NAME"] + ", ");
                             else if (iNumber1 == rowCount1)
                                 SQLin.AppendLine("  " + row["COLUMN_NAME"] + " ) ");
                         }
                     }
                     SQLin.AppendLine(" OUTPUT inserted." + outID + " into @ID");
                     SQLin.AppendLine(" VALUES ( ");

                     iNumber1 = 0;
                     foreach (DataRow row in dt.Rows)
                     {
                         iNumber1++;

                         if (!excludeColumns.Contains(row["COLUMN_NAME"]))
                         {
                             if (iNumber1 < rowCount1)

                                 SQLin.AppendLine("  @I_" + row["COLUMN_NAME"] + ", ");
                             else if (iNumber1 == rowCount1)
                                 SQLin.AppendLine("  @I_" + row["COLUMN_NAME"] + " ) ");
                         }
                     }

                     SQLin.AppendLine(" SELECT @O_" + outID + " = ID FROM @ID");
                     SQLin.AppendLine(" END");

                     break;
                 default:
                     _result._Response = "Invalid DB Platform";
                     runLogic = false;
                     break;
             }*/

            #endregion
            GenerateSQL gSQL = new GenerateSQL();
            string SuccessMessage = "Generated PROC";

            string[] exclude = new string[0];
            if (TableName.ToUpper() == "IDENTITIES")
            {
                exclude = new string[] { "IDENTITIES_ID" };
            }

            string SqlCommand = gSQL.GENERATE_INSERT_PROCEDURE(_Connect, _Connect,
                new InsertProcedureStruct
                {
                    SourceName = TableName,
                    updateParent = true,
                    insertUUID = true,
                    ProcedurePrefix = "SP_I",
                    ProcedureName = TableName,
                    excludedColumns = exclude,
                    UsePlaceHoldingValue = true
                });

            //if (runLogic)
            //{
            _result.attemptedCommand = SqlCommand.ToString();
            _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

            if (!_result._Successful)
            {
                Tools.Box._WriteEventLog(TableName, Revamp.IO.Structs.Enums.EventLogType.error);

                Tools.Box.CreateDir(@"C:\TEMP\INSERT");
                System.IO.File.WriteAllText(@"C:\TEMP\INSERT\" + TableName + ".sql", SqlCommand);

            }

            //}

            results.Add(_result);

            return results;
        }

        public List<CommandResult> ADD_PROCEDURE_DELETE(IConnectToDB _Connect, string TableName, string ProcedurePrefix)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            //Will Create Procedure for Table
            DataTable dt = new DataTable();
            StringBuilder SQLin = new StringBuilder();
            //string tempstring = "";
            string outID = "";
            string inID = "";
            string inDataType = "";
            string outDataType = "";
            string _ProcedurePrefix;
            //int iNumber = 0;
            //int rowCount = 0;

            if (ProcedurePrefix != "")
                _ProcedurePrefix = ProcedurePrefix;
            else
                _ProcedurePrefix = "SP_D";

            dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, true);

            foreach (DataRow row in dt.Rows)
            {
                outID = row["COLUMN_NAME"].ToString();
                outDataType = row["DATA_TYPE"].ToString();
                inID = row["COLUMN_NAME"].ToString();
                inDataType = row["DATA_TYPE"].ToString();
            }

            dt.Clear();

            //STEP 1 - Check if Table Exist
            //Step 2 - Get list of all Columns in Table
            dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, false);
            string procName = _ProcedurePrefix + "_" + er_tools.MaxNameLength(TableName, (128 - (_ProcedurePrefix.Length + 1)));
            string SuccessMessage = "Procedure " + _Schema + "." + procName + " created";

            bool runLogic = true;

            //Step 3 - Create Procedure Begin Definition Syntax
            SQLin.Append("CREATE ");
            SQLin.AppendLine("PROCEDURE " + _Schema + "." + procName.ToUpper());
            switch (_Connect.Platform.ToUpper())
            {
                case "ORACLE":
                    SQLin.AppendLine(" ( ");
                    SQLin.AppendLine(" I_" + inID + " IN " + inDataType + ", ");
                    SQLin.AppendLine(" O_" + outID + " OUT " + outDataType + " ");

                    SQLin.AppendLine(") AS ");
                    SQLin.AppendLine("BEGIN ");
                    SQLin.AppendLine("   DELETE FROM " + _Schema + "." + TableName + " ");
                    SQLin.AppendLine("    WHERE " + inID + " = I_" + inID + ";");
                    SQLin.AppendLine();
                    SQLin.AppendLine("   O_" + outID + ":= I_" + inID + ";");

                    //Step 6 - Create Procedure End Definition Syntax
                    SQLin.AppendLine("END; ");

                    //Step 7 - Submit Procedure Syntax to Database/

                    break;

                case "MICROSOFT":
                    SQLin.AppendLine("( @I_" + inID + " [" + inDataType + "] , ");

                    SQLin.AppendLine(" @O_" + outID + " [" + outDataType + "] OUT )");

                    SQLin.AppendLine("AS");

                    SQLin.AppendLine("BEGIN");
                    SQLin.AppendLine("    SET NOCOUNT ON");

                    SQLin.AppendLine(" Delete from " + _Schema + "." + TableName + " ");

                    SQLin.AppendLine("Where " + inID + " = @I_" + inID + " ");

                    SQLin.AppendLine();
                    SQLin.AppendLine("SET @O_" + outID + " = @I_" + inID + " ");

                    SQLin.AppendLine(" END");

                    break;
                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result.attemptedCommand = SQLin.ToString();
                _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            }

            results.Add(_result);

            return results;
        }

        public List<CommandResult> ADD_PROCEDURE_UPDATE(IConnectToDB _Connect, string TableName, string ProcedurePrefix)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = Revamp.IO.DB.Bridge.DBTools.GetSchema(_Connect);

            //Will Create Procedure for Table
            DataTable dt = new DataTable();
            StringBuilder SQLin = new StringBuilder();
            string tempstring = "";
            string outID = "";
            string inID = "";
            string inDataType = "";
            string outDataType = "";
            string _ProcedurePrefix;
            int iNumber = 0;
            int rowCount = 0;

            if (ProcedurePrefix != "")
                _ProcedurePrefix = ProcedurePrefix;
            else
                _ProcedurePrefix = "SP_U";

            dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, true);

            foreach (DataRow row in dt.Rows)
            {
                inID = row["COLUMN_NAME"].ToString();
                inDataType = row["DATA_TYPE"].ToString();
                outID = row["COLUMN_NAME"].ToString();
                outDataType = row["DATA_TYPE"].ToString();
            }

            dt.Clear();

            //STEP 1 - Check if Table Exist
            //Step 2 - Get list of all Columns in Table
            dt = er_dml.GET_COLUMNS_VIA_TABLENAME(_Connect, TableName, false);
            rowCount = dt.Rows.Count;
            string ProcName = _ProcedurePrefix + "_" + er_tools.MaxNameLength(TableName, (128 - (_ProcedurePrefix.Length + 1)));
            string SuccessMessage = "Procedure " + _Schema + "." + ProcName + " created";

            bool runLogic = true;

            //Step 3 - Create Procedure Begin Definition Syntax
            SQLin.Append("create ");
            SQLin.AppendLine("PROCEDURE " + _Schema + "." + ProcName.ToUpper());

            switch (_Connect.Platform.ToUpper())
            {
                case "ORACLE":
                    SQLin.AppendLine(" ( ");
                    SQLin.AppendLine(" I_" + inID + " IN " + inDataType + ", ");
                    //Step 4 - Create Procedure Params

                    foreach (DataRow row in dt.Rows)
                    {
                        SQLin.AppendLine(" I_" + row["COLUMN_NAME"] + " IN " + row["DATA_TYPE"] + ", ");
                    }

                    SQLin.AppendLine(tempstring);

                    SQLin.AppendLine("O_" + outID + " OUT " + outDataType);
                    //Step 5 - Create Procedure Body Syntax
                    SQLin.AppendLine(") AS ");
                    SQLin.AppendLine("BEGIN ");

                    SQLin.AppendLine("UPDATE " + _Schema + "." + TableName + " ");
                    SQLin.AppendLine(" SET ");

                    //tempstring = "";

                    iNumber = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        iNumber++;

                        SQLin.AppendLine(row["COLUMN_NAME"] + " = I_" + row["COLUMN_NAME"]);

                        if (iNumber != rowCount)
                            SQLin.Append(", ");
                    }

                    SQLin.AppendLine("Where " + inID + " = I_" + inID + ";");

                    SQLin.AppendLine();
                    SQLin.AppendLine("R_" + inID + " := I_" + inID + ";");

                    //Step 6 - Create Procedure End Definition Syntax
                    SQLin.AppendLine("END; ");

                    //Step 7 - Submit Procedure Syntax to Database/                   
                    break;
                case "MICROSOFT":
                    SQLin.AppendLine(" @I_" + inID + " [" + inDataType + "], ");
                    //Step 4 - Create Procedure Params

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["DATA_TYPE"].ToString().ToLower() == "varchar")
                        {
                            string data_length = row.Field<Int32>("DATA_LENGTH") == -1 ? "MAX" : row.Field<Int32>("DATA_LENGTH").ToString();

                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + data_length + "), ");
                        }
                        else if (row["DATA_TYPE"].ToString().ToLower() == "decimal")
                        {
                            string data_length = row["DATA_LENGTH"].ToString();

                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](" + row["NUMERIC_PRECISION"] + "," + row["NUMERIC_SCALE"] + "), ");
                        }
                        else if (row["DATA_TYPE"].ToString().ToLower() == "varbinary")
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "](MAX), ");
                        }
                        else
                        {
                            SQLin.AppendLine(" @I_" + row["COLUMN_NAME"] + " [" + row["DATA_TYPE"] + "], ");
                        }
                    }

                    SQLin.AppendLine(" @O_" + outID + "[" + outDataType + "] OUT ");

                    SQLin.AppendLine("AS");
                    //SQLin.AppendLine("declare @ID table (ID int)");
                    SQLin.AppendLine("BEGIN");
                    SQLin.AppendLine("    SET NOCOUNT ON");

                    SQLin.AppendLine(" Update " + _Schema + "." + TableName + " ");
                    SQLin.AppendLine(" SET ");

                    int iNumber1 = 0;
                    int rowCount1 = dt.Rows.Count;

                    foreach (DataRow row in dt.Rows)
                    {
                        iNumber1++;

                        SQLin.AppendLine(row["COLUMN_NAME"] + " = @I_" + row["COLUMN_NAME"]);

                        if (iNumber1 != rowCount)
                            SQLin.Append(", ");
                    }
                    SQLin.AppendLine("Where " + inID + " = @I_" + inID + " ");

                    SQLin.AppendLine();
                    SQLin.AppendLine("SET @O_" + outID + " = @I_" + inID + " ");

                    SQLin.AppendLine(" END");

                    break;

                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }


            if (runLogic)
            {
                _result.attemptedCommand = SQLin.ToString();
                _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

            }

            results.Add(_result);

            return results;
        }

        public static List<CommandResult> CreateProcedureSearchProcedure(IConnectToDB _NewConnect, string _Author, string _Schema, string DBOBJTypeID, string _selectedTemplateID, List<CommandResult> _Results, SearchProcedureStruct selectStruct)
        {
            ER_Query er_query = new ER_Query();
            if (er_query.CanIdentityConnect(_NewConnect))
            {
                ConnectToDB _CSAConnect = _NewConnect.Copy();
                _CSAConnect.Schema = "CSA";
                ConnectToDB _concreteNewConnect = (ConnectToDB)_NewConnect;

                StringBuilder _sqlIn = new StringBuilder();

                string _ProcedureName = selectStruct.ProcedurePrefix + "_" + selectStruct.ProcedureName;

                string _CreateDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();

                string _Description = "";

                _sqlIn.AppendLine(" -- =============================================");
                _sqlIn.AppendLine(" -- Author:		" + _Author + "");
                _sqlIn.AppendLine(" -- Create date: " + _CreateDate + " ");
                _sqlIn.AppendLine(" -- Description: " + _Description + " ");
                _sqlIn.AppendLine(" -- =============================================");

                GenerateSQL genproc = new GenerateSQL();
                _sqlIn.AppendLine(genproc.GENERATE_DYNAMIC_SEARCH_PROCEDURE(_CSAConnect, _concreteNewConnect, selectStruct));

                CommandResult _Result = new CommandResult();

                _Result._StartTime = DateTime.Now;
                _Result._Response = er_query.RUN_NON_QUERY(_NewConnect, _sqlIn.ToString(),
                    "Success - Procedure " + _ProcedureName + " created in the " + _Schema +
                    " schema.");
                _Result._Successful = _Result._Response.IndexOf("Success") != -1 ? true : false;
                _Result._EndTime = DateTime.Now;

                Tools.Box.CreateDir(@"C:\TEMP\SEARCH");

                if (_Result._Successful == false)
                {
                    System.IO.File.WriteAllText(@"C:\TEMP\SEARCH\" + _ProcedureName + ".sql", _sqlIn.ToString());
                }

                _Results.Add(_Result);

            }
            else
            {
                _Results.Add(new CommandResult { _StartTime = DateTime.Now, _Successful = false, _Response = "Error - Can't connect to DB.", _EndTime = DateTime.Now });
            }

            return _Results;
        }


    }
}
