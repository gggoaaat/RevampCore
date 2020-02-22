using Revamp.IO.Structs;
using Revamp.IO.Structs.Models.RevampSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Revamp.IO.DB.Bridge
{
    //public class ConnectToDB
    //{
    //    public string Platform { get; set; }
    //    public string DBConnString { get; set; }
    //    public string SourceDBOwner { get; set; }
    //    public string ConnServer { get; set; }
    //    public string Password { get; set; }
    //    public string Schema { get; set; }
    //    public string Schema2 { get; set; }
    //    public string authType { get; set; }
    //    public int? TimeOutTime { get; set; }

    //    public ConnectToDB Copy()
    //    {
    //        return (IConnectToDB)Clone();
    //    }

    //    public object Clone()
    //    {
    //        return this.MemberwiseClone();
    //    }
    //}

    public class ER_DDL
    {
        public static CommandResult _ADD_COLUMN(IConnectToDB _Connect, string TableName, string ColumnName, string ObjectType, string DefaultValue, bool isNull)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_COLUMN(_Connect, TableName, ColumnName, ObjectType, DefaultValue, isNull);
        }
        public CommandResult ADD_COLUMN(IConnectToDB _Connect, string TableName, string ColumnName, string ObjectType, string DefaultValue, bool isNull)
        {
            Tools.Box er_tools = new Tools.Box();
            DBTools dbTools = new DBTools();
            ER_Query er_query = new ER_Query();

            CommandResult _result = new CommandResult();

            string _Schema = DBTools.GetSchema(_Connect);

            //if isNull is true/false influences syntax
            string _isNull = "";

            //if DefaultValue is empty or not influences syntax structure
            string _DefaultValue = "";
            //string _ColumnName = "&quot;" + ColumnName + "&quot;";
            string _ColumnName = " " + ColumnName + " ";
            //string _TableName = "&quot;" + TableName + "&quot;";
            string _TableName = " " + TableName + " ";

            string SuccessMessage = "Column " + er_tools.MaxNameLength(_ColumnName, 128) + " has been added to table " + _TableName;

            if (isNull == false)
            {
                _isNull = "Not Null";
            }

            if (DefaultValue != "")
            {
                _DefaultValue = "default '" + DefaultValue + "'";
            }
            //Initiate buffer for SQL syntax.
            StringBuilder SqlCommand = new StringBuilder();
            SqlCommand.Append("alter table " + _Schema + "." + TableName + " ");
            string tempObjectType = dbTools.GET_DATATYPE(_Connect, ObjectType);
            SqlCommand.Append("add " + er_tools.MaxNameLength(ColumnName.ToUpper(), 128) + " " + tempObjectType + " " + _DefaultValue + " " + _isNull);

            _result.attemptedCommand = SqlCommand.ToString();
            switch (_Connect.Platform.ToUpper())
            {
                case "ORACLE":
                    _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
                    break;
                case "MICROSOFT":

                    _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
                    break;
                default:
                    _result._Response = "Invalid DB Platform";
                    break;
            }
            _result._Response = _result._Response.Contains("unique") ? "WARNING " + _result._Response : _result._Response;
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 || _result._Response.Contains("unique") ? true : false;
            _result._EndTime = DateTime.Now;
            return _result;

        }

        public static List<CommandResult> _ADD_COLUMNS(IConnectToDB _Connect, string TableName, List<ColumnStructure> ColumnList)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_COLUMNS(_Connect, TableName, ColumnList);
        }

        public List<CommandResult> ADD_COLUMNS(IConnectToDB _Connect, string TableName, List<ColumnStructure> ColumnList)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();

            switch (_Connect.Platform)
            {
                case "Oracle":
                case "ORACLE":

                    foreach (ColumnStructure i in ColumnList)
                    {
                        HoldResult.Add(ADD_COLUMN(_Connect, TableName, i._Name, i._DataType, i._DefaultValue, i._IsNull));
                    }
                    break;

                case "Microsoft":
                case "MICROSOFT":

                    foreach (ColumnStructure i in ColumnList)
                    {
                        HoldResult.Add(ADD_COLUMN(_Connect, TableName, i._Name, i._DataType, i._DefaultValue, i._IsNull));
                    }
                    break;

                default:
                    HoldResult.Add(new CommandResult { _Response = "Invalid DB Platform", _Successful = false });
                    break;
            }

            return HoldResult;
        }

        public static List<CommandResult> _ADD_INDEX_NONE_CLUSTERED(IConnectToDB _Connect, string _Name, string ForTable, string TableType, List<ColumnStructure> Columns1)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_INDEX_NONCLUSTERED(_Connect, _Name, ForTable, TableType, Columns1);
        }

        public List<CommandResult> ADD_INDEX_NONCLUSTERED(IConnectToDB _Connect, string _Name, string ForTable, string TableType, List<ColumnStructure> Columns1)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";

            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 6)) + "__NCIX";

            string SuccessMessage = "None Clustered Index " + tempstringNAME + " created";

            bool runLogic = true;

            switch (_Connect.Platform.ToUpper())
            {
                case "MICROSOFT":
                    //sqlBuffer.Append("ALTER TABLE " + _Connect.Schema + "." + ForTable + " ");
                    //sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    //sqlBuffer.Append("UNIQUE (" + tempstringColumns + ") ");

                    sqlBuffer.Append("CREATE NONCLUSTERED INDEX [" + tempstringNAME + "]");
                    sqlBuffer.Append("ON " + _Schema + "." + ForTable + " (" + tempstringColumns + ")");

                    break;

                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result.attemptedCommand = sqlBuffer.ToString();
                _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                if (_result._Successful)
                {
                    IConnectToDB csaConnect = _Connect.Copy();
                    csaConnect.Schema = "CSA";
                    er_dml.ADD_Dictionary_Index(csaConnect, new Dictionary.AddIndex
                    {
                        I_INDEX_NAME = tempstringNAME,
                        I_INDEX_TYPE = "None Clustered",
                        I_SOURCE_NAME = ForTable,
                        I_SOURCE_TYPE = TableType
                    });
                }
            }

            results.Add(_result);

            return results;
        }




        public List<CommandResult> ADD_INDEX_NONCLUSTERED(IConnectToDB _Connect, string _Name, string ForTable, string TableType, List<ColumnStructure> Columns1, List<ColumnStructure> IncludeColumns)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";
            string tempstringIncludeColumns = "";

            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            iNumber = 0;
            foreach (ColumnStructure i in IncludeColumns)
            {
                iNumber++;

                tempstringIncludeColumns = tempstringIncludeColumns + i._Name;

                if (iNumber < IncludeColumns.Count())
                {
                    tempstringIncludeColumns = tempstringIncludeColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 6)) + "__NCIX";
            switch (_Connect.Platform.ToUpper())
            {
                case "MICROSOFT":

                    sqlBuffer.Append("CREATE NONCLUSTERED INDEX [" + tempstringNAME + "]");
                    sqlBuffer.Append("ON " + _Schema + "." + ForTable + " (" + tempstringColumns + ")");
                    sqlBuffer.Append("INCLUDE " + " (" + tempstringIncludeColumns + ")");

                    string SuccessMessage = "None Clustered Index " + tempstringNAME + " created";
                    _result._Response = er_query.RUN_NON_QUERY(_Connect, sqlBuffer.ToString(), SuccessMessage);
                    _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

                    if (_result._Response.Contains("already"))
                    {
                        _result._Response += tempstringNAME + " has already been created in the database.";
                    }
                    else
                    {
                        if (_result._Response == "None Clustered Index " + tempstringNAME + " created")
                        {
                            IConnectToDB csaConnect = _Connect.Copy();
                            csaConnect.Schema = "CSA";

                            er_dml.ADD_Dictionary_Index(csaConnect, new Dictionary.AddIndex
                            {
                                I_INDEX_NAME = tempstringNAME,
                                I_INDEX_TYPE = "None Clustered",
                                I_SOURCE_NAME = ForTable,
                                I_SOURCE_TYPE = TableType
                            });
                        }
                    }

                    break;
                default:
                    _result._Response = "Invalid DB Platform";
                    break;
            }

            results.Add(_result);
            return results;
        }

        public string ADD_INDEX_NONCLUSTERED_SQL(IConnectToDB _Connect, string _Name, string ForTable, string TableType, List<ColumnStructure> Columns1)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";

            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 6)) + "__NCIX";
            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":
                    //sqlBuffer.Append("ALTER TABLE " + _Connect.Schema + "." + ForTable + " ");
                    //sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    //sqlBuffer.Append("UNIQUE (" + tempstringColumns + ") ");

                    sqlBuffer.Append("CREATE NONCLUSTERED INDEX [" + tempstringNAME + "]");
                    sqlBuffer.Append("ON " + _Schema + "." + ForTable + " (" + tempstringColumns + ")");

                    return sqlBuffer.ToString();

                default:
                    return "Invalid DB Platform";
            }
        }

        public List<CommandResult> ADD_INDEX_CLUSTERED(IConnectToDB _Connect, string _IndexName, string _SourceName, string _SourceType, List<ColumnStructure> Columns1, bool isitUnique)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();

            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";

            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_IndexName, (128 - 5)) + (isitUnique == true ? "_UIDX" : "_CIDX");
            string SuccessMessage = "Clustered Index " + tempstringNAME + " created";

            bool runLogic = true;
            switch (_Connect.Platform.ToUpper())
            {
                case "MICROSOFT":

                    sqlBuffer.Append("CREATE " + (isitUnique == true ? "UNIQUE" : "") + " CLUSTERED INDEX [" + tempstringNAME + "]");
                    sqlBuffer.Append("ON " + _Schema + "." + _SourceName + " (" + tempstringColumns + ")");

                    break;

                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result.attemptedCommand = sqlBuffer.ToString();
                _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                if (_result._Successful)
                {
                    IConnectToDB csaConnect = _Connect.Copy();
                    csaConnect.Schema = "CSA";

                    er_dml.ADD_Dictionary_Index(csaConnect, new Dictionary.AddIndex
                    {
                        I_INDEX_NAME = tempstringNAME,
                        I_INDEX_TYPE = (isitUnique == true ? "UNIQUE CLUSTERED INDEX" : "CLUSTERED INDEX"),
                        I_SOURCE_NAME = _SourceName,
                        I_SOURCE_TYPE = _SourceType
                    });
                }
            }

            results.Add(_result);

            return results;
        }

        public string ADD_INDEX_CLUSTERED_SQL(IConnectToDB _Connect, string _IndexName, string _SourceName, string _SourceType, List<ColumnStructure> Columns1, bool isitUnique)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";

            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_IndexName, (128 - 5)) + (isitUnique == true ? "_UIDX" : "_CIDX");
            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":

                    sqlBuffer.Append("CREATE " + (isitUnique == true ? "UNIQUE" : "") + " CLUSTERED INDEX [" + tempstringNAME + "]");
                    sqlBuffer.Append("ON " + _Schema + "." + _SourceName + " (" + tempstringColumns + ")");

                    return sqlBuffer.ToString();

                default:
                    return "Invalid DB Platform";
            }
        }

        public List<CommandResult> ADD_INDEX_FULL_TEXT_SEARCH_FOR_DATA_VIEW(IConnectToDB _Connect, string source_table, ColumnStructure column)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);
            string SuccessMessage = "Created Index for " + column._Name + " of table " + source_table;
            StringBuilder sqlBuffer = new StringBuilder();

            sqlBuffer.Append("CREATE FULLTEXT INDEX ON " + _Schema + ".VW__" + source_table + " ");
            sqlBuffer.Append("(" + column._Name + ") ");
            sqlBuffer.Append("KEY INDEX " + source_table + "_uidx ");
            sqlBuffer.Append("ON " + _Connect.RevampSystemName + "_Catalog ");

            _result._Response = er_query.RUN_NON_QUERY(_Connect, sqlBuffer.ToString(), SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

            results.Add(_result);

            return results;
        }

        public string ADD_INDEX_FULL_TEXT_SEARCH_FOR_DATA_VIEW_SQL(IConnectToDB _Connect, string source_table, ColumnStructure column)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            StringBuilder sqlBuffer = new StringBuilder();

            sqlBuffer.Append("CREATE FULLTEXT INDEX ON " + _Schema + ".VW__" + source_table + " ");
            sqlBuffer.Append("(" + column._Name + ") ");
            sqlBuffer.Append("KEY INDEX " + source_table + "_uidx ");
            sqlBuffer.Append("ON " + _Connect.RevampSystemName + "_Catalog ");

            return sqlBuffer.ToString();
        }

        public string ADD_KEY_FOREIGN(IConnectToDB _Connect, string _Name, string ForTable, string ParentTable, List<string> Columns1, List<string> ParentColumns2)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();

            //CREATES FOREIGN KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_FK";

            string _Schema = DBTools.GetSchema(_Connect);
            string _Schema2 = _Connect.Schema2 == null || _Connect.Schema2.Trim() == "" ? _Schema : _Connect.Schema2;

            switch (_Connect.Platform)
            {
                case "Oracle":
                case "ORACLE":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("FOREIGN KEY (" + string.Join(",", Columns1.ToArray()) + ") ");
                    sqlBuffer.Append("REFERENCES " + _Schema2 + "." + ParentTable + " (" + string.Join(",", Columns1.ToArray()) + ") ENABLE");

                    //return sqlBuffer.ToString();
                    return er_query.RUN_NON_QUERY(_Connect, sqlBuffer.ToString(), "Foreign Key Constraint " + tempstringNAME + " created");

                case "Microsoft":
                case "MICROSOFT":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("FOREIGN KEY (" + string.Join(",", Columns1.ToArray()) + ") ");
                    sqlBuffer.Append("REFERENCES " + _Schema2 + "." + ParentTable + " (" + string.Join(",", Columns1.ToArray()) + ") ");

                    //return sqlBuffer.ToString();
                    return er_query.RUN_NON_QUERY(_Connect, sqlBuffer.ToString(), "Foreign Key Constraint " + tempstringNAME + " created");

                default:
                    return "Invalid DB Platform";
            }
        }

        public static List<CommandResult> _ADD_KEY_FOREIGN(IConnectToDB _Connect, string _Name, string ForTable, string ParentTable, List<ColumnStructure> Columns1, List<ColumnStructure> ParentColumns2)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_KEY_FOREIGN(_Connect, _Name, ForTable, ParentTable, Columns1, ParentColumns2);
        }

        public List<CommandResult> ADD_KEY_FOREIGN(IConnectToDB _Connect, string _Name, string ForTable, string ParentTable, List<ColumnStructure> Columns1, List<ColumnStructure> ParentColumns2, bool cascadeEnabled = false)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            CommandResult _result = new CommandResult();
            List<CommandResult> results = new List<CommandResult>();

            string _Schema = DBTools.GetSchema(_Connect);
            string _Schema2 = string.IsNullOrWhiteSpace(_Connect.Schema2) ? _Schema : _Connect.Schema2;
            //CREATES FOREIGN KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";
            string tempstringColumns2 = "";

            tempstringColumns = COLUMNS2STRING(Columns1);

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_FK";

            tempstringColumns2 = COLUMNS2STRING(ParentColumns2);

            string SuccessMessage = "Foreign Key Constraint " + tempstringNAME + " created";

            bool runLogic = true;
            switch (_Connect.Platform.ToUpper())
            {
                case "ORACLE":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("FOREIGN KEY (" + tempstringColumns + ") ");
                    sqlBuffer.Append("REFERENCES " + _Schema2 + "." + ParentTable + " (" + tempstringColumns2 + ") ENABLE");
                    sqlBuffer.Append(" update cascade");
                    break;

                case "MICROSOFT":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("FOREIGN KEY (" + tempstringColumns + ") ");
                    sqlBuffer.Append("REFERENCES " + _Schema2 + "." + ParentTable + " (" + tempstringColumns2 + ") ");
                    if (cascadeEnabled)
                    {
                        sqlBuffer.Append("ON UPDATE CASCADE");
                    }
                    break;

                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result.attemptedCommand = sqlBuffer.ToString();
                _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                if (_result._Successful)
                {
                    List<Dictionary.AddForeignKeyColumn> FKCs = new List<Dictionary.AddForeignKeyColumn>();
                    List<Dictionary.AddForeignKeyColumn> FKCsParents = new List<Dictionary.AddForeignKeyColumn>();

                    for (int i = 0; i < Columns1.Count(); i++)
                    {
                        FKCs.Add(new Dictionary.AddForeignKeyColumn
                        {
                            I_COLUMN_NAME = Columns1[i]._Name,
                            I_COLUMN_DATATYPE = Columns1[i]._DataType,
                            I_PARENT_COLUMN_NAME = ParentColumns2[i]._Name
                        });
                    }

                    IConnectToDB CSAConnect = _Connect.Copy();
                    CSAConnect.Schema = "CSA";

                    er_dml.ADD_Dictionary_FK(CSAConnect, new Dictionary.AddForeignKey
                    {
                        I_KEY_NAME = tempstringNAME,
                        I_TABLE_SCHEMA = _Schema,
                        I_TABLE_NAME = ForTable,
                        I_PARENT_SCHEMA = _Schema2,
                        I_PARENT_TABLE_NAME = ParentTable,
                        V_FK_ColumnsList1 = FKCs
                    });
                }
            }

            results.Add(_result);

            return results;
        }

        public string ADD_KEY_FOREIGN_SQL(IConnectToDB _Connect, string _Name, string ForTable, string ParentTable, List<ColumnStructure> Columns1, List<ColumnStructure> ParentColumns2)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);
            string _Schema2 = _Connect.Schema2 == null || _Connect.Schema2.Trim() == "" ? _Schema : _Connect.Schema2;

            //CREATES FOREIGN KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";
            string tempstringColumns2 = "";

            tempstringColumns = COLUMNS2STRING(Columns1);

            //foreach (ColumnStructure i in Columns1)
            //{
            //    iNumber++;

            //    tempstringColumns = tempstringColumns + i._Name;

            //    if (iNumber < Columns1.Count())
            //    {
            //        tempstringColumns = tempstringColumns + ", ";
            //    }
            //}

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_FK";
            tempstringColumns2 = COLUMNS2STRING(ParentColumns2);

            //foreach (ColumnStructure i in ParentColumns2)
            //{
            //    iNumber++;
            //    tempstringColumns2 = tempstringColumns2 + i._Name;

            //    if (iNumber < Columns1.Count())
            //    {
            //        tempstringColumns2 = tempstringColumns2 + ", ";
            //    }

            //}

            switch (_Connect.Platform)
            {
                case "Oracle":
                case "ORACLE":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("FOREIGN KEY (" + tempstringColumns + ") ");
                    sqlBuffer.Append("REFERENCES " + _Schema2 + "." + ParentTable + " (" + tempstringColumns2 + ") ENABLE");

                    return sqlBuffer.ToString();

                case "Microsoft":
                case "MICROSOFT":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("FOREIGN KEY (" + tempstringColumns + ") ");
                    sqlBuffer.Append("REFERENCES " + _Schema2 + "." + ParentTable + " (" + tempstringColumns2 + ") ");

                    //er_dml.ADD_Dictionary_FK(_Connect, tempstringNAME, ForTable, ParentTable, Columns1, ParentColumns2);
                    //return sqlBuffer.ToString();
                    return sqlBuffer.ToString();

                default:
                    return "Invalid DB Platform";
            }
        }

        public List<CommandResult> ADD_KEY_PRIMARY(IConnectToDB _Connect, string _Name, string ForTable, List<string> Columns1)
        {
            List<CommandResult> _result = new List<CommandResult>();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATES Primary KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_PK";

            //ALTER TABLE table_name
            //add CONSTRAINT constraint_name PRIMARY KEY (column1, column2, ... column_n);


            CommandResult result1 = new CommandResult();
            string SuccessMessage = "Primary Key Constraint " + tempstringNAME + " created";
            switch (_Connect.Platform.ToUpper())
            {
                case "ORACLE":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("Primary KEY (" + string.Join(",", Columns1.ToArray()) + ") ENABLE");

                    result1._Response = er_query.RUN_NON_QUERY(_Connect, sqlBuffer.ToString(), SuccessMessage);
                    result1._Successful = result1._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                    break;
                case "MICROSOFT":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("Primary KEY (" + string.Join(",", Columns1.ToArray()) + ") ");

                    result1._Response = er_query.RUN_NON_QUERY(_Connect, sqlBuffer.ToString(), SuccessMessage);
                    result1._Successful = result1._Response.IndexOf(SuccessMessage) > -1 ? true : false;

                    if (result1._Successful)
                    {

                        IConnectToDB csaConnect = _Connect.Copy();
                        csaConnect.Schema = "CSA";

                        ER_DML er_dml = new ER_DML();
                        er_dml.ADD_Dictionary_PK(csaConnect, new Dictionary.AddPrimaryKey
                        {
                            I_KEY_NAME = tempstringNAME,
                            I_TABLE_NAME = ForTable,
                            Columns = new List<Dictionary.AddPrimaryKeyColumns>()
                        });
                    }

                    break;

                default:

                    result1._Response = "Invalid DB Platform";
                    result1._Successful = false;
                    break;
            }

            _result.Add(result1);

            return _result;
        }

        public static List<CommandResult> _ADD_KEY_PRIMARY(IConnectToDB _Connect, string _Name, string ForTable, List<ColumnStructure> Columns1)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_KEY_PRIMARY(_Connect, _Name, ForTable, Columns1);
        }

        public List<CommandResult> ADD_KEY_PRIMARY(IConnectToDB _Connect, string _Name, string ForTable, List<ColumnStructure> Columns1)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            CommandResult _result = new CommandResult();
            List<CommandResult> results = new List<CommandResult>();
            List<Dictionary.AddPrimaryKeyColumns> PKColumns = new List<Dictionary.AddPrimaryKeyColumns>();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATES Primary KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";
            int iNumber = 0;


            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }

                PKColumns.Add(new Dictionary.AddPrimaryKeyColumns { I_COLUMN_NAME = i._Name, I_COLUMN_DATATYPE = i._DataType });
            }

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_PK";
            string SuccessMessage = "Primary Key Constraint " + tempstringNAME + " created";
            //ALTER TABLE table_name
            //add CONSTRAINT constraint_name PRIMARY KEY (column1, column2, ... column_n);                    

            bool runLogic = true;
            switch (_Connect.Platform.ToUpper())
            {
                case "ORACLE":

                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("Primary KEY (" + tempstringColumns + ") ENABLE");
                    break;

                case "MICROSOFT":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("Primary KEY (" + tempstringColumns + ")");
                    break;

                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result._Response = er_query.RUN_NON_QUERY(_Connect, sqlBuffer.ToString(), SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                if (_result._Successful)
                {
                    string[] listofExcludedTables = new string[] { "OBJECT_LAYERS", "ER_TABLES" };

                    if (!listofExcludedTables.Contains(tempstringNAME))
                    {
                        IConnectToDB csaConnect = _Connect.Copy();
                        csaConnect.Schema = "CSA";

                        Dictionary.AddPrimaryKey ok = er_dml.ADD_Dictionary_PK(csaConnect, new Dictionary.AddPrimaryKey
                        {
                            I_KEY_NAME = tempstringNAME,
                            I_TABLE_NAME = ForTable,
                            Columns = PKColumns
                        });
                    };
                }
            }

            results.Add(_result);

            return results;
        }

        public string ADD_KEY_PRIMARY_SQL(IConnectToDB _Connect, string _Name, string ForTable, List<ColumnStructure> Columns1)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATES Primary KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";
            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_PK";

            //ALTER TABLE table_name
            //add CONSTRAINT constraint_name PRIMARY KEY (column1, column2, ... column_n);

            switch (_Connect.Platform)
            {
                case "Oracle":
                case "ORACLE":

                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("Primary KEY (" + tempstringColumns + ") ENABLE");

                    return sqlBuffer.ToString();

                case "Microsoft":
                case "MICROSOFT":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("Primary KEY (" + tempstringColumns + ")");

                    return sqlBuffer.ToString();

                default:
                    return "Invalid DB Platform";
            }
        }

        public static List<CommandResult> _ADD_KEY_UNIQUE(IConnectToDB _Connect, string _Name, string ForTable, List<string> Columns1)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_KEY_UNIQUE(_Connect, _Name, ForTable, Columns1);
        }

        public List<CommandResult> ADD_KEY_UNIQUE(IConnectToDB _Connect, string _Name, string ForTable, List<string> Columns1)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder SqlCommand = new StringBuilder();
            string tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_UK";
            string SuccessMessage = "Unique Constraint " + tempstringNAME + " created";

            bool runLogic = true;

            switch (_Connect.Platform.ToUpper())
            {
                case "Oracle":
                    SqlCommand.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    SqlCommand.Append("add CONSTRAINT " + tempstringNAME + " ");
                    SqlCommand.Append("UNIQUE (" + string.Join(",", Columns1.ToArray()) + ") ENABLE");
                    break;

                case "MICROSOFT":
                    SqlCommand.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    SqlCommand.Append("add CONSTRAINT " + tempstringNAME + " ");
                    SqlCommand.Append("UNIQUE (" + string.Join(",", Columns1.ToArray()) + ") ");

                    break;
                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result.attemptedCommand = SqlCommand.ToString();
                _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                if (_result._Successful)
                {
                    ER_DML er_dml = new ER_DML();
                    IConnectToDB csaConnect = _Connect.Copy();
                    csaConnect.Schema = "CSA";

                    Dictionary.AddUniqueKey UKResult = er_dml.ADD_Dictionary_UK(csaConnect, new Dictionary.AddUniqueKey { I_KEY_NAME = tempstringNAME, I_TABLE_NAME = ForTable, V_UK_ColumnsList1 = new List<Dictionary.AddUniqueKeyColumn>() });

                    CommandResult _ukResult = new CommandResult();
                    _ukResult.attemptedCommand = UKResult.V_ATTEMPTED_SQL;
                    _ukResult._Successful = UKResult.O_ER_UNIQUE_KEYS_ID > 0 ? true : true;
                    _ukResult._Response = _ukResult._Successful ? "Added Unique Key " + tempstringNAME + " to dictionary" : UKResult.O_ERR_MESS;

                    results.Add(_ukResult);
                }
            }

            results.Add(_result);

            return results;
        }

        public string ADD_KEY_UNIQUE_SQL(IConnectToDB _Connect, string _Name, string ForTable, List<string> Columns1)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_UK";
            switch (_Connect.Platform)
            {
                case "Oracle":
                case "ORACLE":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("UNIQUE (" + string.Join(",", Columns1.ToArray()) + ") ENABLE");

                    return sqlBuffer.ToString();

                case "Microsoft":
                case "MICROSOFT":
                    sqlBuffer.Append("ALTER TABLE " + _Connect.RevampSystemName + "." + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("UNIQUE (" + string.Join(",", Columns1.ToArray()) + ") ");

                    return sqlBuffer.ToString();

                default:
                    return "Invalid DB Platform";
            }
        }

        public List<CommandResult> ADD_KEY_UNIQUE(IConnectToDB _Connect, string _Name, string ForTable, List<ColumnStructure> Columns1)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";

            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_UK";
            string SuccessMessage = "Unique Constraint " + tempstringNAME + " created";

            bool runLogic = true;

            switch (_Connect.Platform.ToUpper())
            {
                case "ORACLE":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("UNIQUE (" + tempstringColumns + ") ENABLE");
                    break;
                case "MICROSOFT":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("UNIQUE (" + tempstringColumns + ") ");
                    break;
                default:
                    _result._Response = "Invalid DB Platform";
                    runLogic = false;
                    break;
            }

            if (runLogic)
            {
                _result._Response = er_query.RUN_NON_QUERY(_Connect, sqlBuffer.ToString(), SuccessMessage);
                _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                if (_result._Successful)
                {
                    List<Dictionary.AddUniqueKeyColumn> UKCs = new List<Dictionary.AddUniqueKeyColumn>();
                    for (int i = 0; i < Columns1.Count(); i++)
                    {
                        UKCs.Add(new Dictionary.AddUniqueKeyColumn
                        {
                            I_COLUMN_NAME = Columns1[i]._Name,
                            I_COLUMN_DATATYPE = Columns1[i]._DataType,
                        });
                    }

                    IConnectToDB csaConnect = _Connect.Copy();
                    csaConnect.Schema = "CSA";

                    Dictionary.AddUniqueKey UKResult = er_dml.ADD_Dictionary_UK(csaConnect, new Dictionary.AddUniqueKey { I_KEY_NAME = tempstringNAME, I_TABLE_NAME = ForTable, V_UK_ColumnsList1 = UKCs });

                    CommandResult _ukResult = new CommandResult();
                    _ukResult.attemptedCommand = UKResult.V_ATTEMPTED_SQL;
                    _ukResult._Successful = UKResult.O_ER_UNIQUE_KEYS_ID > 0 ? true : true;
                    _ukResult._Response = _ukResult._Successful ? "Added Unique Key " + tempstringNAME + " to dictionary" : UKResult.O_ERR_MESS;

                    results.Add(_ukResult);
                }
            }

            results.Add(_result);

            return results;
        }

        public string ADD_KEY_UNIQUE_SQL(IConnectToDB _Connect, string _Name, string ForTable, List<ColumnStructure> Columns1)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";

            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 3)) + "_UK";
            switch (_Connect.Platform)
            {
                case "Oracle":
                case "ORACLE":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("UNIQUE (" + tempstringColumns + ") ENABLE");

                    return sqlBuffer.ToString();

                case "Microsoft":
                case "MICROSOFT":
                    sqlBuffer.Append("ALTER TABLE " + _Schema + "." + ForTable + " ");
                    sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    sqlBuffer.Append("UNIQUE (" + tempstringColumns + ") ");

                    return sqlBuffer.ToString();

                default:
                    return "Invalid DB Platform";
            }
        }

        public List<CommandResult> ADD_KEY_UNIQUE_WITH_CONDITION(IConnectToDB _Connect, string _Name, string ForTable, List<ColumnStructure> Columns1, string whereClause)
        {
            List<CommandResult> _result = new List<CommandResult>();

            CommandResult keyResult = new CommandResult();

            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";

            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 5)) + "_UICO";
            switch (_Connect.Platform.ToUpper())
            {
                case "MICROSOFT":

                    sqlBuffer.Append("CREATE UNIQUE NONCLUSTERED INDEX [" + tempstringNAME + "]");
                    sqlBuffer.Append("ON " + _Schema + "." + ForTable + " (" + tempstringColumns + ")");
                    sqlBuffer.Append("WHERE " + whereClause);

                    string SuccessMessage = "Unique Constraint " + tempstringNAME + " created";

                    keyResult._Response = er_query.RUN_NON_QUERY(_Connect, sqlBuffer.ToString(), SuccessMessage);
                    keyResult._Successful = keyResult._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                    keyResult._EndTime = DateTime.Now;

                    if (keyResult._Successful)
                    {
                        List<Dictionary.AddUniqueKeyColumn> UKCs = new List<Dictionary.AddUniqueKeyColumn>();
                        for (int i = 0; i < Columns1.Count(); i++)
                        {
                            UKCs.Add(new Dictionary.AddUniqueKeyColumn
                            {
                                I_COLUMN_NAME = Columns1[i]._Name,
                                I_COLUMN_DATATYPE = Columns1[i]._DataType,
                            });
                        }

                        IConnectToDB csaConnect = _Connect.Copy();
                        csaConnect.Schema = "CSA";

                        Dictionary.AddUniqueKey UKResult = er_dml.ADD_Dictionary_UK(csaConnect, new Dictionary.AddUniqueKey { I_KEY_NAME = tempstringNAME, I_TABLE_NAME = ForTable, V_UK_ColumnsList1 = UKCs });

                        CommandResult _ukResult = new CommandResult();
                        _ukResult.attemptedCommand = UKResult.V_ATTEMPTED_SQL;
                        _ukResult._Successful = UKResult.O_ER_UNIQUE_KEYS_ID > 0 ? true : true;
                        _ukResult._Response = _ukResult._Successful ? "Added Unique Key " + tempstringNAME + " to dictionary" : UKResult.O_ERR_MESS;

                        _result.Add(_ukResult);
                    }

                    break;

                default:
                    keyResult._Response = "Invalid DB Platform";
                    keyResult._Successful = false;
                    keyResult._EndTime = DateTime.Now;

                    break;
            }

            _result.Add(keyResult);

            return _result;
        }

        public string ADD_KEY_UNIQUE_WITH_CONDITION_SQL(IConnectToDB _Connect, string _Name, string ForTable, List<ColumnStructure> Columns1, string whereClause)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            string _Schema = DBTools.GetSchema(_Connect);

            //CREATE UNIQUE KEY
            StringBuilder sqlBuffer = new StringBuilder();
            string tempstringNAME;
            string tempstringColumns = "";

            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            tempstringNAME = er_tools.MaxNameLength(_Name, (128 - 5)) + "_UICO";
            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":
                    //sqlBuffer.Append("ALTER TABLE " + _Connect.Schema + "." + ForTable + " ");
                    //sqlBuffer.Append("add CONSTRAINT " + tempstringNAME + " ");
                    //sqlBuffer.Append("UNIQUE (" + tempstringColumns + ") ");

                    sqlBuffer.Append("CREATE UNIQUE NONCLUSTERED INDEX [" + tempstringNAME + "]");
                    sqlBuffer.Append("ON " + _Schema + "." + ForTable + " (" + tempstringColumns + ")");
                    sqlBuffer.Append("WHERE " + whereClause);

                    return sqlBuffer.ToString();

                default:
                    return "Invalid DB Platform";
            }
        }

        //public string SQL_Add_Materialized_View_log(string DB_PLATFORM, string connAuth, string ForTable)
        //{
        //    ER_Query er_query = new ER_Query();

        //    return er_query.RUN_NON_QUERY(_Connect, "CREATE MATERIALIZED VIEW LOG ON " + ForTable + " WITH ROWID", "MATERIALIZED VIEW LOG " + ForTable + " created.)");
        //}



        public static List<CommandResult> _ADD_TABLE(IConnectToDB _Connect, string Name, string TableType, bool useIdentityUUID = false, string RootColumn = "")
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_TABLE(_Connect, Name, TableType, useIdentityUUID, RootColumn);
        }

        public List<CommandResult> ADD_TABLE(IConnectToDB _Connect, string Name, string TableType, bool useIdentityUUID = false, string RootColumn = "")
        {
            if (string.IsNullOrWhiteSpace(RootColumn))
            {
                Tools.Box er_tools = new Tools.Box();

                List<CommandResult> _results = new List<CommandResult>();
                string tempstringNAME = er_tools.MaxNameLength(Name.ToUpper(), (128 - 3));

                string _Schema = DBTools.GetSchema(_Connect);

                CommandResult _result = new CommandResult();
                switch (_Connect.Platform)
                {
                    case "Microsoft":
                    case "MICROSOFT":

                        string SuccessMessage = "Table " + Name + " successfully created.";
                        _results.AddRange(TableCreateSyntax(_Connect,
                            Name: Name,
                            RootColumn: Name,
                            TableType: TableType,
                            useIdentityUUID: useIdentityUUID,
                            tempstringNAME: tempstringNAME,
                            _Schema: _Schema,
                            SuccessMessage: SuccessMessage));

                        _result._Successful = true;
                        _result._Response = "Table SQL Finished";
                        break;
                    default:
                        _result._Response = "Wrong Platform";
                        break;
                }

                _result._EndTime = DateTime.Now;

                _results.Add(_result);
                return _results;
            }
            else
            {
                return ADD_GUID_TABLE(_Connect, Name, RootColumn, TableType, useIdentityUUID);
            }
        }

        public List<CommandResult> ADD_GUID_TABLE(IConnectToDB _Connect, string Name, string RootColumn, string TableType, bool useIdentityUUID = false)
        {
            Tools.Box er_tools = new Tools.Box();

            List<CommandResult> _results = new List<CommandResult>();
            string tempstringNAME = er_tools.MaxNameLength(Name.ToUpper(), (128 - 3));

            string _Schema = DBTools.GetSchema(_Connect);

            CommandResult _result = new CommandResult();
            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":

                    string SuccessMessage = "Table " + Name + " successfully created.";
                    _results.AddRange(TableCreateSyntax(_Connect,
                        Name: Name,
                        RootColumn: RootColumn,
                        TableType: TableType,
                        useIdentityUUID: useIdentityUUID,
                        tempstringNAME: tempstringNAME,
                        _Schema: _Schema,
                        SuccessMessage: SuccessMessage));
                    _result._Successful = true;
                    _result._Response = "Table Created";
                    break;
                default:
                    _result._Response = "Attempt to create table Wrong Platform";
                    break;
            }

            _result._EndTime = DateTime.Now;

            _results.Add(_result);
            return _results;
        }

        private static List<CommandResult> TableCreateSyntax(IConnectToDB _Connect, string Name, string RootColumn, string TableType, bool useIdentityUUID, string tempstringNAME, string _Schema, string SuccessMessage)
        {

            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_DDL er_ddl = new ER_DDL();

            List<CommandResult> _results = new List<CommandResult>();
            StringBuilder SqlCommand = new StringBuilder();

            SqlCommand.AppendLine("CREATE TABLE " + _Schema + "." + tempstringNAME + "(");
            SqlCommand.AppendLine(RootColumn.ToUpper() + "_ID bigint not null IDENTITY(1000,1),");
            SqlCommand.AppendLine("BASE_" + RootColumn.ToUpper() + "_ID bigint not null DEFAULT 0,");
            SqlCommand.AppendLine("PREV_" + RootColumn.ToUpper() + "_ID bigint not null DEFAULT 0,");
            SqlCommand.AppendLine(RootColumn.ToUpper() + "_UUID uniqueidentifier NOT NULL DEFAULT NEWSEQUENTIALID(),");
            SqlCommand.AppendLine("BASE_" + RootColumn.ToUpper() + "_UUID uniqueidentifier,");
            SqlCommand.AppendLine("PREV_" + RootColumn.ToUpper() + "_UUID uniqueidentifier,");
            if (useIdentityUUID)
            {
                SqlCommand.AppendLine(Name.ToUpper() != "IDENTITIES" ? "IDENTITIES_UUID uniqueidentifier NOT NULL," : "IDENTITIES__UUID uniqueidentifier NOT NULL,");
            }
            else
            {
                SqlCommand.AppendLine(Name.ToUpper() != "IDENTITIES" ? "IDENTITIES_ID bigint NOT NULL DEFAULT 0," : "IDENTITIES__ID bigint NOT NULL DEFAULT 0,");
            }
            SqlCommand.AppendLine("ENABLED char(1) default 'Y',");
            SqlCommand.AppendLine("DT_CREATED datetime default getdate() not null,");
            SqlCommand.AppendLine("DT_UPDATED datetime default getdate() not null,");
            SqlCommand.AppendLine("DT_AVAILABLE datetime, DT_END datetime )");

            CommandResult _result = new CommandResult();
            _result._CommandName = "Create Table " + Name;
            _result.attemptedCommand = SqlCommand.ToString();
            _result._Response = er_query.RUN_NON_QUERY(_Connect, _result.attemptedCommand, SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

            string[] listOfExcludedTables = new string[] { "OBJECT_LAYERS", "ER_TABLES" };
            List<ColumnStructure> ExistingColumnsList = new List<ColumnStructure>();
            ExistingColumnsList.Add(new ColumnStructure { _Name = tempstringNAME.ToUpper() + "_ID", _DataType = "bigint", _DefaultValue = "", _IsNull = false });
            // _results.AddRange(er_ddl.ADD_KEY_UNIQUE(_Connect, tempstringNAME.ToUpper() + "_ID", tempstringNAME, ExistingColumnsList));

            if (_result._Successful && !listOfExcludedTables.Contains(tempstringNAME))
            {
                ConnectToDB CSASchame = _Connect.Copy();
                CSASchame.Schema = "CSA";
                Dictionary.AddTable resultholder = er_dml.ADD_Dictionary_Table(CSASchame, new Dictionary.AddTable
                {
                    I_TABLE_SCHEMA = _Schema,
                    I_TABLE_NAME = Name,
                    I_TABLE_TYPE = TableType
                });

                CommandResult _result2 = new CommandResult();

                _result2._Successful = true;
                _result2._Response = resultholder.O_ER_TABLES_ID > 0 ? Name + " Added Table to Dictionary" : "Error Failed to add " + Name + " to Dictionary";
                _result2._Successful = resultholder.O_ER_TABLES_ID > 0 ? true : false;
                _result2._EndTime = DateTime.Now;

                _results.Add(_result2);
            }

            _results.Add(_result);

            return _results;
        }

        public static List<CommandResult> _ADD_TABLE_CHAR(IConnectToDB _Connect, string TableName, string Char_Table, List<ColumnStructure> ColumnsList)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_TABLE_CHAR(_Connect, TableName, Char_Table, ColumnsList);
        }
        private List<CommandResult> ADD_TABLE_CHAR(IConnectToDB _Connect, string TableName, string Char_Table, List<ColumnStructure> ColumnsList)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();

            CommonDataTabeSyntax(TableName, ColumnsList);

            ColumnsList.Add(new ColumnStructure { _Name = "Value", _DataType = "varchar2(4000)", _IsNull = false, _DefaultValue = "" });

            HoldResult.AddRange(ADD_COLUMNS(_Connect, Char_Table, ColumnsList));

            ColumnsList.Clear();

            ////Primary Key for every table but the following.
            //if (TableName.ToUpper() != "OBJ_PROP_SETS")
            //{

            //    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            //    ColumnsList.Add(new ColumnStructure { _Name = "OBJ_PROP_SETS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            //    ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            //    HoldResult.AddRange(ADD_KEY_PRIMARY(_Connect, Char_Table + "_1", Char_Table, ColumnsList));

            //    ColumnsList.Clear();
            //}

            CommonDataFKSyntax(_Connect, TableName, Char_Table, ColumnsList, HoldResult);

            return HoldResult;
        }

        public static List<CommandResult> _ADD_TABLE_DATE(IConnectToDB _Connect, string TableName, string Date_Table, List<ColumnStructure> ColumnsList)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_TABLE_DATE(_Connect, TableName, Date_Table, ColumnsList);
        }

        private List<CommandResult> ADD_TABLE_DATE(IConnectToDB _Connect, string TableName, string Date_Table, List<ColumnStructure> ColumnsList)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();

            CommonDataTabeSyntax(TableName, ColumnsList);

            ColumnsList.Add(new ColumnStructure { _Name = "Value", _DataType = "date", _IsNull = false, _DefaultValue = "" });

            HoldResult.AddRange(ADD_COLUMNS(_Connect, Date_Table, ColumnsList));

            ColumnsList.Clear();

            //Primary Key for every table but the following.
            //if (TableName.ToUpper() != "OBJ_PROP_SETS")
            //{

            //    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            //    ColumnsList.Add(new ColumnStructure { _Name = "OBJ_PROP_SETS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            //    ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            //    HoldResult.AddRange(ADD_KEY_PRIMARY(_Connect, Date_Table + "_1", Date_Table, ColumnsList));

            //    ColumnsList.Clear();
            //}

            CommonDataFKSyntax(_Connect, TableName, Date_Table, ColumnsList, HoldResult);

            return HoldResult;
        }

        public static List<CommandResult> _ADD_TABLE_DECIMAL(IConnectToDB _Connect, string TableName, string Decimal_Table, List<ColumnStructure> ColumnsList)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_TABLE_DECIMAL(_Connect, TableName, Decimal_Table, ColumnsList);
        }

        private List<CommandResult> ADD_TABLE_DECIMAL(IConnectToDB _Connect, string TableName, string Decimal_Table, List<ColumnStructure> ColumnsList)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();

            CommonDataTabeSyntax(TableName, ColumnsList);

            ColumnsList.Add(new ColumnStructure { _Name = "Value", _DataType = "Money", _IsNull = false, _DefaultValue = "" });

            HoldResult.AddRange(ADD_COLUMNS(_Connect, Decimal_Table, ColumnsList));

            ColumnsList.Clear();

            ////Primary Key for every table but the following.
            //if (TableName.ToUpper() != "OBJ_PROP_SETS")
            //{
            //    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            //    ColumnsList.Add(new ColumnStructure { _Name = "OBJ_PROP_SETS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            //    ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            //    HoldResult.AddRange(ADD_KEY_PRIMARY(_Connect, Decimal_Table + "_1", Decimal_Table, ColumnsList));

            //    ColumnsList.Clear();
            //}

            CommonDataFKSyntax(_Connect, TableName, Decimal_Table, ColumnsList, HoldResult);

            return HoldResult;
        }

        public static List<CommandResult> _ADD_TABLE_FILE(IConnectToDB _Connect, string TableName, string File_Table, List<ColumnStructure> ColumnsList)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_TABLE_FILE(_Connect, TableName, File_Table, ColumnsList);
        }

        private List<CommandResult> ADD_TABLE_FILE(IConnectToDB _Connect, string TableName, string File_Table, List<ColumnStructure> ColumnsList)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();

            CommonDataTabeSyntax(TableName, ColumnsList);

            ColumnsList.Add(new ColumnStructure { _Name = "FILE_NAME", _DataType = "Characters(255)", _DefaultValue = "", _IsNull = false });
            ColumnsList.Add(new ColumnStructure { _Name = "FILE_SIZE", _DataType = "bigint", _DefaultValue = "", _IsNull = false });
            ColumnsList.Add(new ColumnStructure { _Name = "CONTENT_TYPE", _DataType = "Characters(120)", _DefaultValue = "", _IsNull = false });

            ColumnsList.Add(new ColumnStructure { _Name = "Value", _DataType = "Raw", _IsNull = false, _DefaultValue = "" });

            HoldResult.AddRange(ADD_COLUMNS(_Connect, File_Table, ColumnsList));

            ColumnsList.Clear();

            ////Primary Key for every table but the following.
            //if (TableName.ToUpper() != "OBJ_PROP_SETS")
            //{
            //    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            //    ColumnsList.Add(new ColumnStructure { _Name = "OBJ_PROP_SETS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            //    ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            //    HoldResult.AddRange(ADD_KEY_PRIMARY(_Connect, File_Table + "_1", File_Table, ColumnsList));

            //    ColumnsList.Clear();
            //}

            CommonDataFKSyntax(_Connect, TableName, File_Table, ColumnsList, HoldResult);

            return HoldResult;
        }

        public static List<CommandResult> _ADD_TABLE_NUMB(IConnectToDB _Connect, string TableName, string Number_Table, List<ColumnStructure> ColumnsList)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_TABLE_NUMB(_Connect, TableName, Number_Table, ColumnsList);
        }

        private List<CommandResult> ADD_TABLE_NUMB(IConnectToDB _Connect, string TableName, string Number_Table, List<ColumnStructure> ColumnsList)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();

            CommonDataTabeSyntax(TableName, ColumnsList);

            ColumnsList.Add(new ColumnStructure { _Name = "Value", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            HoldResult.AddRange(ADD_COLUMNS(_Connect, Number_Table, ColumnsList));

            ColumnsList.Clear();

            //////Primary Key for every table but the following.
            ////if (TableName.ToUpper() != "OBJ_PROP_SETS")
            ////{

            ////    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            ////    ColumnsList.Add(new ColumnStructure { _Name = "OBJ_PROP_SETS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            ////    ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            ////    HoldResult.AddRange(ADD_KEY_PRIMARY(_Connect, Number_Table + "_1", Number_Table, ColumnsList));

            ////    ColumnsList.Clear();
            ////}
            CommonDataFKSyntax(_Connect, TableName, Number_Table, ColumnsList, HoldResult);

            return HoldResult;
        }

        public static List<CommandResult> _ADD_TABLE_OPT(IConnectToDB _Connect, string TableName, string Option_Table, List<ColumnStructure> ColumnsList)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_TABLE_OPT(_Connect, TableName, Option_Table, ColumnsList);
        }

        private List<CommandResult> ADD_TABLE_OPT(IConnectToDB _Connect, string TableName, string Option_Table, List<ColumnStructure> ColumnsList)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();
            CommonDataTabeSyntax(TableName, ColumnsList);

            ColumnsList.Add(new ColumnStructure { _Name = "Value", _DataType = "varchar2(500)", _IsNull = false, _DefaultValue = "" });

            HoldResult.AddRange(ADD_COLUMNS(_Connect, Option_Table, ColumnsList));

            ColumnsList.Clear();

            //Primary Key for every table but the following.
            if (TableName.ToUpper() != "OBJ_PROP_OPT_SETS")
            {
                //ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                //ColumnsList.Add(new ColumnStructure { _Name = "OBJ_PROP_SETS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                //ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

                //HoldResult.Add(ADD_KEY_PRIMARY(_Connect, OwnerName, Option_Table + "_1", Option_Table, ColumnsList));

                //ColumnsList.Clear();
            }

            CommonDataFKSyntax(_Connect, TableName, Option_Table, ColumnsList, HoldResult);

            if (TableName.ToUpper() != "OBJ_PROP_OPT_SETS" && TableName.ToUpper() != "OBJ_PROP_SETS")
            {
                ColumnsList.Add(new ColumnStructure { _Name = "OBJ_PROP_OPT_SETS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

                HoldResult.AddRange(ADD_KEY_FOREIGN(_Connect, Option_Table + "_0", Option_Table, "OBJ_PROP_OPT_SETS", ColumnsList, ColumnsList));

                ColumnsList.Clear();
            }

            return HoldResult;
        }

        private void CommonDataFKSyntax(IConnectToDB _Connect, string TableName, string Char_Table, List<ColumnStructure> ColumnsList, List<CommandResult> HoldResult)
        {
            string[] CASTGOOP = new string[] { "CORES", "APPLICATIONS", "STAGES", "GRIPS", "OBJECT_SETS", "IDENTITIES" };

            //if (TableName.ToUpper() != "IDENTITIES")
            //{
            ColumnsList.Add(new ColumnStructure { _Name = TableName + "_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
            HoldResult.AddRange(ADD_KEY_FOREIGN(_Connect, Char_Table + "_1", Char_Table, TableName, ColumnsList, ColumnsList));
            ColumnsList.Clear();
            //    }
        }
        private static void CommonDataTabeSyntax(string TableName, List<ColumnStructure> ColumnsList)
        {
            //string[] CASTGOOP = new string[] { "CORES", "APPLICATIONS", "STAGES", "GRIPS", "OBJECT_SETS", "OBJ_PROP_SETS" };

            //foreach (string item in CASTGOOP)
            //{
            //    if (TableName.ToUpper() != item)
            //    {
            ColumnsList.Add(new ColumnStructure { _Name = TableName + "_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
            //   }
            // }
            //if (TableName.ToUpper() != "IDENTITIES")
            //{

            //    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            //}
            //if (TableName.ToUpper() != "OBJ_PROP_SETS")
            //{
            //    ColumnsList.Add(new ColumnStructure { _Name = "Property_Value", _DataType = "Characters(230)", _IsNull = false, _DefaultValue = "" });
            //}

            //if (TableName.ToUpper() != "OBJ_PROP_OPT_SETS")
            //{
            //    ColumnsList.Add(new ColumnStructure { _Name = "OBJ_PROP_OPT_SETS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
            //    //ColumnsList.Add(new ColumnStructure { _Name = "Property_Value", _DataType = "Characters(230)", _IsNull = false, _DefaultValue = "" });
            //}
        }

        public static string _ADD_TRIGGER(IConnectToDB _Connect, string Action, string When, string ForTable, StringBuilder Body)
        {
            ER_DDL ddl = new ER_DDL();

            return ddl.ADD_TRIGGER(_Connect, Action, When, ForTable, Body);
        }

        public string ADD_TRIGGER(IConnectToDB _Connect, string Action, string When, string ForTable, StringBuilder Body)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();

            string _Schema = DBTools.GetSchema(_Connect);

            string tempstringNAME = er_tools.MaxNameLength(ForTable, (128 - 6)) + "_" + "INSRT";

            StringBuilder TriggerBody = new StringBuilder();

            switch (When.ToLower())
            {
                case "before":
                case "for":
                    switch (Action.ToLower())
                    {
                        case "insert":
                            TriggerBody.Append("CREATE Trigger " + _Schema + "." + tempstringNAME + " ");
                            TriggerBody.AppendLine("ON " + ForTable + " ");
                            TriggerBody.AppendLine("FOR INSERT ");
                            TriggerBody.AppendLine("AS ");
                            TriggerBody.AppendLine("DECLARE ");
                            TriggerBody.AppendLine("@" + ForTable + "_ID int, ");
                            TriggerBody.AppendLine("@outval int");
                            TriggerBody.AppendLine("BEGIN ");
                            TriggerBody.AppendLine("SET NOCOUNT ON;");
                            TriggerBody.AppendLine("SET @" + ForTable + "_ID = (SELECT " + ForTable + "_ID FROM INSERTED) ");
                            TriggerBody.Append(Body + " ");
                            TriggerBody.AppendLine("END; ");
                            break;

                        case "update":
                            break;

                        case "delete":
                            break;
                    }
                    break;

                case "after":
                    switch (Action.ToLower())
                    {
                        case "insert":
                            break;

                        case "update":
                            break;

                        case "delete":
                            break;
                    }
                    break;
            }

            //return sqlStatement.ToString();
            return er_query.RUN_NON_QUERY(_Connect, TriggerBody.ToString(), "Trigger &quot;" + tempstringNAME + "&quot; created.");
        }


        public string COLUMNS2STRING(List<ColumnStructure> Columns1)
        {
            string tempstringColumns = "";
            int iNumber = 0;
            foreach (ColumnStructure i in Columns1)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i._Name;

                if (iNumber < Columns1.Count())
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            return tempstringColumns;
        }

        public string COLUMNS2STRING(DataTable Columns1, string ColumnName)
        {
            string tempstringColumns = "";
            int iNumber = 0;
            foreach (DataRow i in Columns1.Rows)
            {
                iNumber++;

                tempstringColumns = tempstringColumns + i.Field<string>(ColumnName.ToUpper());

                if (iNumber < Columns1.Rows.Count)
                {
                    tempstringColumns = tempstringColumns + ", ";
                }
            }

            return tempstringColumns;
        }

        public string CREATE_FULL_TEXT_INDEX(IConnectToDB _Connect, string ForTable, string ForField)
        {
            ER_Query er_query = new ER_Query();

            return er_query.RUN_NON_QUERY(_Connect, "CREATE FULLTEXT INDEX ON Production.Document " +
            "( " +
            "    Document                         --Full-text index column name  " +
            "        TYPE COLUMN FileExtension    --Name of column that contains file type information " +
            "        Language 2057                 --2057 is the LCID for British English " +
            ") " +
            "KEY INDEX ui_ukDoc ON AdvWksDocFTCat --Unique index " +
            "WITH CHANGE_TRACKING AUTO            --Population type", "Created Full Text Index");
        }

        public List<CommandResult> DROP_INDEX(IConnectToDB _Connect, string index_table, string index_name)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();

            ER_Query er_query = new ER_Query();

            StringBuilder SQLBuffer = new StringBuilder();

            string _Schema = DBTools.GetSchema(_Connect);

            SQLBuffer.Append("DROP INDEX " + _Schema + "." + index_table + "." + index_name.ToUpper());

            string SuccessMessage = "Success Index " + index_name + " for table " + index_table + " has been dropped.";

            _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLBuffer.ToString(), SuccessMessage).ToString();
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

            if (!_result._Successful && _result._Response.Contains("Cannot drop the index "))
            {
                _result._Response += "Index" + index_name + " doesn't need to be dropped because it doesn't exist";
            }

            results.Add(_result);

            return results;
        }

        public List<CommandResult> DROP_KEY_CONSTRAINT(IConnectToDB _Connect, string key_table, string key_name)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            ER_Query er_query = new ER_Query();

            StringBuilder SQLBuffer = new StringBuilder();

            string _Schema = DBTools.GetSchema(_Connect);

            SQLBuffer.Append("ALTER TABLE " + _Schema + "." + key_table + " DROP CONSTRAINT " + key_name);

            string SuccessMessage = "Success Constraint  " + key_name + " for table " + key_table + " has been dropped.";

            _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLBuffer.ToString(), SuccessMessage).ToString();
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

            if (!_result._Successful && _result._Response.Contains("Could not drop constraint"))
            {
                _result._Response += "Key " + key_name + " does not exist so it doesn't need to be dropped";
            }
            else
            {
                er_query.RUN_NON_QUERY(_Connect, "Delete FROM " + _Connect.Schema + ".ER_FK_COLUMNS where ER_FOREIGN_KEYS_ID in (select ER_FOREIGN_KEYS_ID from " + _Connect.Schema + ".ER_FOREIGN_KEYS where KEY_NAME =  '" + key_name + "')", "Success row for  " + key_name + " in ER_FK_COLUMNS " + key_table + " has been deleted.").ToString();

                er_query.RUN_NON_QUERY(_Connect, "Delete FROM " + _Connect.Schema + ".ER_FOREIGN_KEYS where KEY_NAME = '" + key_name + "'", "Success row for  " + key_name + " in ER_FOREIGN_KEYS " + key_table + " has been deleted.").ToString();
            }

            results.Add(_result);

            return results;
        }

        public List<CommandResult> DROP_PROCEDURE(IConnectToDB _Connect, string PROCEDURE_Name)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();

            ER_Query er_query = new ER_Query();

            StringBuilder SQLBuffer = new StringBuilder();

            string _Schema = DBTools.GetSchema(_Connect);

            SQLBuffer.Append("DROP PROCEDURE " + _Schema + "." + PROCEDURE_Name.ToUpper());

            string SuccessMessage = "Success procedure " + PROCEDURE_Name + " has been dropped.";

            _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLBuffer.ToString(), SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 || _result._Response.IndexOf("exist") > -1 ? true : false;

            if (_result._Successful)
            {
                //TODO: Remove Objects from Dictionary
                //er_dml.ADD_Dictionary_View(_Connect, _ViewName, ViewType);
            }

            results.Add(_result);

            return results;
        }
        public static List<CommandResult> _DROP_VIEW(IConnectToDB _Connect, string ViewName)
        {
            ER_DDL _ddl = new ER_DDL();

            return _ddl.DROP_VIEW(_Connect, ViewName);
        }
        public List<CommandResult> DROP_VIEW(IConnectToDB _Connect, string ViewName)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            StringBuilder SQLBuffer = new StringBuilder();

            string _Schema = DBTools.GetSchema(_Connect);

            SQLBuffer.Append("DROP VIEW " + _Schema + "." + ViewName);

            string SuccessMessage = "Success " + ViewName + " has been dropped.";

            _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLBuffer.ToString(), SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 || _result._Response.IndexOf("exist") > -1 ? true : false;
            if (_result._Successful)
            {
                er_dml.DROP_Dictionary_View(_Connect, ViewName);
            }

            results.Add(_result);

            return results;
        }

        public List<CommandResult> DROP_TABLE(IConnectToDB _Connect, string TableName)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            StringBuilder SQLBuffer = new StringBuilder();

            string _Schema = DBTools.GetSchema(_Connect);

            SQLBuffer.Append("DROP TABLE " + _Schema + "." + TableName);

            string SuccessMessage = "Success " + TableName + " has been dropped.";

            _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLBuffer.ToString(), SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            if (_result._Successful)
            {
                er_dml.DROP_Dictionary_View(_Connect, TableName);
            }

            results.Add(_result);

            return results;
        }

        public List<CommandResult> MOD_CASCADE_FK(IConnectToDB _Connect, string KEY_NAME)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult result = new CommandResult();
            CommandResult result1 = null;
            Tools.Box er_tools = new Tools.Box();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = "Select * from CSA.ER_FOREIGN_KEYS where KEY_NAME = @KEY_NAME",
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "KEY_NAME", MSSqlParamDataType = SqlDbType.VarBinary, ParamValue = KEY_NAME } }
            };

            DataTable KEYINFO = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            string TABLE_NAME = KEYINFO.Rows[0]["TABLE_NAME"].ToString();
            string PARENT_TABLE_NAME = KEYINFO.Rows[0]["PARENT_TABLE_NAME"].ToString();

            SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = "Select * from CSA.ER_FK_COLUMNS where ER_FOREIGN_KEYS_ID = @ER_FOREIGN_KEYS_ID order by ER_FK_COLUMNS_ID",
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "ER_FOREIGN_KEYS_ID", MSSqlParamDataType = SqlDbType.VarBinary, ParamValue = KEYINFO.Rows[0]["ER_FOREIGN_KEYS_ID"].ToString() } }
            };

            DataTable KEYCOLUMNS = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);
            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":

                    string _sql = "alter table CSA." + TABLE_NAME + " " + "drop constraint " + KEY_NAME + " ";
                    string SuccessMessage = "SUCCESS Foreign Key " + KEY_NAME + " has been successfully dropped.";

                    result.attemptedCommand = _sql;
                    result._Response = er_query.RUN_NON_QUERY(_Connect, result.attemptedCommand, SuccessMessage);
                    result._Successful = result._Response.IndexOf(SuccessMessage) > -1 ? true : false;

                    if (result._Successful)
                    {
                        result1 = new CommandResult();
                        result1._CommandName = "Mod Key";
                        _sql = "alter table CSA." + TABLE_NAME + " " +
                           "add constraint " + KEY_NAME + " " +
                           "  foreign key (" + COLUMNS2STRING(KEYCOLUMNS, "COLUMN_NAME") + ") " +
                           "  references CSA." + PARENT_TABLE_NAME + "(" + COLUMNS2STRING(KEYCOLUMNS, "PARENT_COLUMN_NAME") + ") " +
                           "  on update cascade";

                        SuccessMessage = "SUCCESS Foreign Key " + KEY_NAME + " has been successfully recreated with cascade update option.";
                        result1.attemptedCommand = _sql;
                        result1._Response = er_query.RUN_NON_QUERY(_Connect, result1.attemptedCommand, SuccessMessage);
                        result1._Successful = result1._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                    }
                    else
                    {
                        result._Response = "Error occurred during drop";
                    }

                    break;
                default:

                    result._Response = "Wrong Platform";
                    break;
            }
            results.Add(result);
            if (result1 != null)
            {
                results.Add(result1);
            }

            return results;
        }

    }
}
