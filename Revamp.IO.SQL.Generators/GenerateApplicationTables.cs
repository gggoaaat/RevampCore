using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Foundation;
using Revamp.IO.Structs;
using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Revamp.IO.SQL.Generators
{
    public class GenerateApplicationTables
    {
        [Serializable]
        public class GenerateApplication
        {
            public string I_APPLICATIONS_UUID { get; set; }
            public string I_SCHEMA { get; set; }
            public string I_TABLENAME { get; set; }
            public string O_RESULT { get; set; }
            public string O_SYNTAX { get; set; }
        }

        [Serializable]
        public class CreateTableDefinition
        {
            public DataTable theseObjects { get; set; } = new DataTable();
            public Database sourceDatabase { get; set; } = new Database();
            public string desiredSchema { get; set; }
            public IEnumerable<string> listOfObjects { get; set; }
            public string objectLayer { get; set; }
            public string rootTablePrefix { get; set; }
            public string BindRootFamilyOnObject { get; set; }
            public string BindRootColumnUUID { get; set; }
            public ParentTableDefinition ThisParent { get; set; }
        }

        [Serializable]
        public class ParentTableDefinition
        {
            public bool BuildParentScaffold { get; set; }
            public string ParentTablePrefix { get; set; }
            public string ParentObjectLayer { get; set; }
            public string BindParentRootColumnFamilyName { get; set; }
            public string BindParentRootColumnUUID { get; set; }
            public string FkColumnAvailableInBothTable { get; set; }
            public string FkRelatedColumnType { get; set; }
        }

        // "Text_Box", "Email", "Paragraph_Text", "Rich_Text", "Name", "Phone", "Required", "Url", "Credit_Card", "Link", "Zip", "Check_Box", "Attribute"
        public static string[] StringTypes = { "text_box", "text", "SharedObject", "email", "paragraph_text", "rich_text", "name", "phone", "required", "url", "credit_card", "link", "zip", "radio_button", "attribute", "drop_down" };
        //"Decimal", "Number", "Paragraph_Text", "Rich_Text" 
        public static string[] NumberTypes = { "decimal", "number", "currency", "paragraph_text", "rich_text", "total" };
        //"Date", "Time"
        public static string[] DateAndTimeTypes = { "date", "time", "times", "timestamp" };
        //"Signature", "File_Upload", "HTML" 
        public static string[] ByteTypes = { "password", "signature", "file_upload", "html" };
        //"Radio_Button", "Drop_Down"
        public static string[] MultiTypes = { "check_box" };

        public static List<CommandResult> Create(IConnectToDB _Connect, GenerateApplication thisModel)
        {
            List<CommandResult> results = new List<CommandResult>();
            string SchemaAndName = thisModel.I_SCHEMA + "." + thisModel.I_TABLENAME;
            // string Path = @"SQL\", ObjectName = "GET_OBJECTS";

            #region MyRegion Old Code but good way of doing sql file call
            //Load SQL File That has First Query
            //string ServerPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath + @"\" + Path + @"\" + ObjectName + ".sql";
            //StringBuilder _sqlIn = Tools.Box.convertStringArray(System.IO.File.ReadAllLines(ServerPath), new StringBuilder());
            //DataTable theseObjects = ER_Query._RUN_PARAMETER_QUERY(_Connect, new ER_Query.Parameter_Run
            //{
            //    sqlIn = _sqlIn.ToString(),
            //     _dbParameters = new List<DBParameters>
            //     {
            //          new DBParameters{ ParamName = "I_APPLICATIONS_UUID" ,
            //              MSSqlParamDataType = SqlDbType.UniqueIdentifier,
            //              ParamValue = thisModel.I_APPLICATIONS_UUID
            //          }
            //     }
            //}); 
            #endregion
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = thisModel.I_APPLICATIONS_UUID });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_LAYER_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "IO Tag" });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PROPERTY_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "ID, Required, Max, Value" });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_OBJECT_TYPE", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Grid Widget, JSON Text, Button, Image" });

            DataTable theseObjects = DB.Binds.IO.Dynamic._DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_APPLICATION_OBJECTS_SEARCH",
            new DataTableDotNetModelMetaData { length = -1, order = "CORE_NAME, APPLICATION_NAME, STAGES_ID, STAGE_NAME, GRIPS_ID, GRIP_NAME, OBJECT_SETS_ID, OBJ_PROP_SETS_ID, PROPERTY_NAME, PROPERTY_VALUE", start = 0, verify = "T" },
            Filters);

            String connectionString = _Connect.DBConnString;

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            ServerConnection conn = new ServerConnection(sqlConnection);
            Server srv = new Server(conn);
            Database sourceDatabase = srv.Databases[_Connect.SourceDBOwner];

            DataColumnCollection theseObjectsDCC = theseObjects.Columns;

            string desiredSchema = "";
            List<string> lines = new List<string>();
            if (theseObjectsDCC.Contains("CORE_NAME") && theseObjects.Rows.Count > 0 && !string.IsNullOrWhiteSpace(thisModel.I_TABLENAME) && !string.IsNullOrWhiteSpace(thisModel.I_APPLICATIONS_UUID))
            {
                IEnumerable<string> Stages = theseObjects.AsEnumerable().Select(r => r.Field<string>("STAGE_NAME").Replace(" ", "_").Replace("-", "_")).Distinct();

                desiredSchema = theseObjects.Rows[0].Field<string>("CORE_NAME").Replace(" ", "_").Replace("-", "_");
                results.AddRange(CreateTablesCode(_Connect, new CreateTableDefinition
                {
                    objectLayer = "Generated", //Case Matters
                    BindRootFamilyOnObject = "STAGE_NAME",
                    BindRootColumnUUID = "BASE_STAGES_UUID",
                    rootTablePrefix = "S",
                    theseObjects = theseObjects,
                    sourceDatabase = sourceDatabase,
                    desiredSchema = desiredSchema,
                    listOfObjects = Stages,
                    ThisParent = new ParentTableDefinition
                    {
                        BuildParentScaffold = true,
                        ParentObjectLayer = "Generated", //Case Matters
                        ParentTablePrefix = "A",
                        BindParentRootColumnFamilyName = "APPLICATIONS",
                        BindParentRootColumnUUID = "BASE_APPLICATIONS_UUID",
                        FkColumnAvailableInBothTable = "APPLICATIONS_UUID",
                        FkRelatedColumnType = "guid"
                    }
                }, false));


            }

            return results;
        }

        private static List<CommandResult> CreateTablesCode(IConnectToDB _Connect, CreateTableDefinition thisDef, bool isChildCall)
        {
            List<CommandResult> results = new List<CommandResult>();
            ScaffoldStructure thisAppStructure = new ScaffoldStructure();
            foreach (var thisCurrentObject in thisDef.listOfObjects)
            {
                ER_DDL er_ddl = new ER_DDL();

                List<ColumnStructure> ExistingColumnsList = new List<ColumnStructure>();
                List<ColumnStructure> MetaColumnsList = new List<ColumnStructure>();
                List<ColumnStructure> ScaffoldColumns = new List<ColumnStructure>();
                List<ColumnStructure> ScaffoldColumnsForReOrder = new List<ColumnStructure>();

                DataTable thisStageObjects = thisDef.theseObjects.AsEnumerable().Where(r => r.Field<string>(thisDef.BindRootFamilyOnObject).Replace(" ", "_").Replace("-", "_") == thisCurrentObject).CopyToDataTable();
                DataColumnCollection ccthisStageObjects = thisStageObjects.Columns;
                Scaffolds scaffolds = new Scaffolds();

                ConnectToDB forCore = _Connect.Copy();
                forCore.Schema = thisDef.desiredSchema;

                string ObjectName = "";
                string BaseObjectUUID = "";
                string parentBaseColumnUUID = ""; //Unrelated to Outside 
                List<MultiValueChildScaffold> ObjectChildren = new List<MultiValueChildScaffold>();
                if (ccthisStageObjects.Contains(thisDef.BindRootFamilyOnObject))
                {
                    ObjectName = string.IsNullOrWhiteSpace(ObjectName) ? thisStageObjects.Rows[0].Field<string>(thisDef.BindRootFamilyOnObject).Replace(" ", "_") : ObjectName;
                    BaseObjectUUID = string.IsNullOrWhiteSpace(BaseObjectUUID) ? thisStageObjects.Rows[0].Field<Guid>(thisDef.BindRootColumnUUID).ToString().Replace("-", "_") : BaseObjectUUID;

                    string StageTableName = thisDef.rootTablePrefix + "_" + BaseObjectUUID;
                    string RootColumnName = StageTableName;

                    parentBaseColumnUUID = string.IsNullOrWhiteSpace(parentBaseColumnUUID) ? thisStageObjects.Rows[0].Field<Guid>(thisDef.ThisParent.BindParentRootColumnUUID).ToString().Replace("-", "_") : parentBaseColumnUUID;
                    string ParentTable = thisDef.ThisParent.ParentTablePrefix + "_" + parentBaseColumnUUID;

                    #region Create App Table
                    //if (!thisDef.sourceDatabase.Tables.Contains(ParentTable, forCore.Schema))
                    //{
                        thisDef.ThisParent.BindParentRootColumnFamilyName = ParentTable;
                        thisDef.ThisParent.FkColumnAvailableInBothTable = ParentTable + "_UUID";
                        //thisDef.ThisParent.BindParentRootColumnUUID = "BASE_" + ParentTable + "_UUID";

                        thisAppStructure = new ScaffoldStructure
                        {
                            _Connect = forCore.Copy(),
                            Name = ParentTable,
                            ScaffoldType = Tools.Box.Clone<string>(thisDef.ThisParent.ParentObjectLayer),
                            ColumnsList = Tools.Box.Clone<List<ColumnStructure>>(ScaffoldColumns),
                            useIdentityUUID = true,
                            RootColumn = Tools.Box.Clone<string>(thisDef.ThisParent.BindParentRootColumnFamilyName)
                        };

                        ScaffoldColumns.Add(new ColumnStructure { _Name = "CORES_UUID", _DataType = "guid", _IsNull = false });
                        ScaffoldColumns.Add(new ColumnStructure { _Name = "APPLICATIONS_UUID", _DataType = "guid", _IsNull = false });
                        results.AddRange(scaffolds.SYNC_SCAFFOLD(forCore, ParentTable, thisDef.ThisParent.ParentObjectLayer, ScaffoldColumns, true, thisDef.ThisParent.BindParentRootColumnFamilyName, true));

                        ExistingColumnsList.Clear(); MetaColumnsList.Clear(); ScaffoldColumns.Clear();
                        ExistingColumnsList.Add(new ColumnStructure { _Name = "IDENTITIES_UUID", _DataType = "Guid", _DefaultValue = "", _IsNull = false });

                        IConnectToDB Link2CSA = forCore.Copy();
                        Link2CSA.Schema2 = "CSA";

                        results.AddRange(er_ddl.ADD_KEY_FOREIGN(Link2CSA, "FK_" + ParentTable + "_" + "IDENTITIES", ParentTable, "IDENTITIES", ExistingColumnsList, ExistingColumnsList, true));
                       // results.AddRange(ER_DDL._DROP_VIEW(Link2CSA, ParentTable));
                        results.AddRange(ER_Generate._GENERATE_VIEW(Link2CSA, ParentTable, "Generated"));
                    //}
                    //else
                    //{
                    //    //thisDef.ThisParent.FkColumnAvailableInBothTable = ParentTable + "_UUID";
                    //    thisDef.ThisParent.BuildParentScaffold = false;
                    //}
                    #endregion

                    #region Create Stage Table & Any Children
                    //if (!thisDef.sourceDatabase.Tables.Contains(TableName, forCore.Schema))
                    if (!thisDef.sourceDatabase.Tables.Contains(StageTableName, forCore.Schema) || true)
                    {
                        bool createdAppScaffold = false;
                        long? T_OBJECT_SETS_ID = 0;
                        long? @T_PREV_OBJECT_SETS_ID = 0;
                        if (!createdAppScaffold && thisDef.ThisParent.BuildParentScaffold)
                        {
                            createdAppScaffold = true;
                            forCore.Schema = thisDef.desiredSchema;
                        }

                        ScaffoldColumns.Add(new ColumnStructure { _Name = "CORES_UUID", _DataType = "guid", _IsNull = false });
                        ScaffoldColumns.Add(new ColumnStructure { _Name = "APPLICATIONS_UUID", _DataType = "guid", _IsNull = false });
                        ScaffoldColumns.Add(new ColumnStructure { _Name = "STAGES_UUID", _DataType = "guid", _IsNull = false });

                        #region Loop Through Every Column
                        for (int i = 0; i < thisStageObjects.AsEnumerable().Count(); i++)
                        {
                            thisDef.desiredSchema = string.IsNullOrWhiteSpace(thisDef.desiredSchema) ? thisStageObjects.Rows[i].Field<string>("CORE_NAME").Replace(" ", "_") : thisDef.desiredSchema;

                            DataRow item = thisStageObjects.Rows[i];
                            string PROPERTY_NAME, PROPERTY_VALUE, OBJECT_TYPE;
                            AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);

                            if (T_PREV_OBJECT_SETS_ID != T_OBJECT_SETS_ID)
                            {
                                if (PROPERTY_NAME == "ID")
                                {
                                    ColumnStructure thisColumn = new ColumnStructure();
                                    bool addScaffold = true;
                                    if (Array.IndexOf(StringTypes, OBJECT_TYPE.ToLower()) > -1)
                                    {
                                        HandleVarCharTypes(thisStageObjects, ref T_OBJECT_SETS_ID, out T_PREV_OBJECT_SETS_ID, ref i, ref item, ref PROPERTY_NAME, ref PROPERTY_VALUE, ref OBJECT_TYPE, out thisColumn);
                                    }
                                    else if (Array.IndexOf(NumberTypes, OBJECT_TYPE.ToLower()) > -1)
                                    {
                                        HandleNumberTypes(thisStageObjects, ref T_OBJECT_SETS_ID, out T_PREV_OBJECT_SETS_ID, ref i, ref item, ref PROPERTY_NAME, ref PROPERTY_VALUE, ref OBJECT_TYPE, out thisColumn);
                                    }
                                    else if ((Array.IndexOf(DateAndTimeTypes, OBJECT_TYPE.ToLower()) > -1))
                                    {
                                        HandleDateAndTimeTypes(thisStageObjects, ref T_OBJECT_SETS_ID, out T_PREV_OBJECT_SETS_ID, ref i, ref item, ref PROPERTY_NAME, ref PROPERTY_VALUE, ref OBJECT_TYPE, out thisColumn);
                                    }
                                    else if ((Array.IndexOf(ByteTypes, OBJECT_TYPE.ToLower()) > -1))
                                    {
                                        HandleByteTypes(thisStageObjects, ref T_OBJECT_SETS_ID, out T_PREV_OBJECT_SETS_ID, ref i, ref item, ref PROPERTY_NAME, ref PROPERTY_VALUE, ref OBJECT_TYPE, out thisColumn);
                                    }
                                    else if ((Array.IndexOf(MultiTypes, OBJECT_TYPE.ToLower()) > -1))
                                    {

                                        #region Define Multi Object for post creation.
                                        List<ColumnStructure> TheseChildColumns = new List<ColumnStructure>();
                                        var objectName = thisStageObjects.Rows[i].Field<string>("PROPERTY_VALUE");
                                        TheseChildColumns.Add(new ColumnStructure { _Name = "CORES_UUID", _DataType = "guid", _IsNull = false });
                                        TheseChildColumns.Add(new ColumnStructure { _Name = "APPLICATIONS_UUID", _DataType = "guid", _IsNull = false });
                                        TheseChildColumns.Add(new ColumnStructure { _Name = "STAGES_UUID", _DataType = "guid", _IsNull = false });
                                        TheseChildColumns.Add(new ColumnStructure { _Name = objectName, _DataType = "Characters(MAX)", _IsNull = false });
                                        ObjectChildren.Add(new MultiValueChildScaffold
                                        {
                                            SchemaName = thisDef.desiredSchema,
                                            TableName = "O_" + thisStageObjects.Rows[i].Field<Guid>("BASE_OBJECT_SETS_UUID").ToString().Replace(" ", "_").Replace("-", "_"),
                                            ObjectLayer = "Design",
                                            RootColumnName = PROPERTY_VALUE,
                                            ParentRootColumnName = RootColumnName,
                                            ScaffoldColumns = TheseChildColumns
                                        });

                                        addScaffold = false;
                                        #endregion
                                    }

                                    if (addScaffold)
                                    {
                                        ScaffoldColumns.Add(thisColumn);
                                    }
                                }

                            }

                            T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
                        }
                        #endregion
                        forCore.Schema = thisDef.desiredSchema;
                    }


                    if (thisStageObjects.AsEnumerable().Count() > 0 && !string.IsNullOrWhiteSpace(BaseObjectUUID) && !string.IsNullOrWhiteSpace(parentBaseColumnUUID))
                    {
                        ScaffoldColumnsForReOrder = new List<ColumnStructure>();
                        ScaffoldColumnsForReOrder.Add(new ColumnStructure { _Name = thisDef.ThisParent.FkColumnAvailableInBothTable, _DataType = thisDef.ThisParent.FkRelatedColumnType, _IsNull = false });
                        ScaffoldColumnsForReOrder.AddRange(ScaffoldColumns);

                        ScaffoldColumns = ScaffoldColumnsForReOrder;

                        forCore.Schema = thisDef.desiredSchema;

                        ScaffoldStructure thisStage = new ScaffoldStructure
                        {
                            _Connect = forCore.Copy(),
                            Name = StageTableName,
                            ScaffoldType = Tools.Box.Clone<string>(thisDef.objectLayer),
                            ColumnsList = Tools.Box.Clone<List<ColumnStructure>>(ScaffoldColumns),
                            useIdentityUUID = true,
                            RootColumn = Tools.Box.Clone<string>(RootColumnName)
                        };

                        results.AddRange(scaffolds.SYNC_SCAFFOLD(forCore, StageTableName, thisDef.objectLayer, ScaffoldColumns, true, RootColumnName, true));

                        #region Add Foreign Key to Parent Application Table
                        forCore.Schema2 = thisDef.desiredSchema;
                        ExistingColumnsList.Clear(); MetaColumnsList.Clear(); ScaffoldColumns.Clear();
                        ExistingColumnsList.Add(new ColumnStructure { _Name = thisDef.ThisParent.FkColumnAvailableInBothTable, _DataType = thisDef.ThisParent.FkRelatedColumnType, _DefaultValue = "", _IsNull = false });

                        results.AddRange(er_ddl.ADD_KEY_FOREIGN(forCore, "FK_" + StageTableName, StageTableName, ParentTable, ExistingColumnsList, ExistingColumnsList, true));
                        #endregion

                        #region Add Foreign Key to Identities
                        ExistingColumnsList.Clear(); MetaColumnsList.Clear(); ScaffoldColumns.Clear();
                        ExistingColumnsList.Add(new ColumnStructure { _Name = "IDENTITIES_UUID", _DataType = "Guid", _DefaultValue = "", _IsNull = false });

                        // IConnectToDB Link2CSA = forCore.Copy();
                        Link2CSA = forCore.Copy();
                        Link2CSA.Schema2 = "CSA";
                        results.AddRange(er_ddl.ADD_KEY_FOREIGN(Link2CSA, "FK_" + StageTableName + "_IDENTITIES", StageTableName, "IDENTITIES", ExistingColumnsList, ExistingColumnsList, false));
                        #endregion

                        #region Create Each Multi Object Table
                        foreach (MultiValueChildScaffold item in ObjectChildren)
                        {
                            ScaffoldStructure thisMultiObject = new ScaffoldStructure
                            {
                                _Connect = forCore.Copy(),
                                Name = Tools.Box.Clone<string>(item.TableName),
                                ScaffoldType = Tools.Box.Clone<string>(item.ObjectLayer),
                                ColumnsList = Tools.Box.Clone<List<ColumnStructure>>(item.ScaffoldColumns),
                                useIdentityUUID = true,
                                RootColumn = Tools.Box.Clone<string>(item.TableName)
                            };

                            thisStage.children.Add(thisMultiObject);

                            List<ColumnStructure> itemScaffoldColumnsForReOrder = new List<ColumnStructure>();

                            itemScaffoldColumnsForReOrder.Add(new ColumnStructure { _Name = StageTableName + "_UUID", _DataType = thisDef.ThisParent.FkRelatedColumnType, _IsNull = false });
                            itemScaffoldColumnsForReOrder.AddRange(item.ScaffoldColumns);
                            item.ScaffoldColumns = itemScaffoldColumnsForReOrder;

                            results.AddRange(scaffolds.SYNC_SCAFFOLD(forCore, item.TableName, item.ObjectLayer, item.ScaffoldColumns, true, item.TableName, true));

                            #region Add Foreign to Parent Stage Table
                            ExistingColumnsList.Clear(); MetaColumnsList.Clear(); ScaffoldColumns.Clear();
                            // ExistingColumnsList.Add(new ColumnStructure { _Name = item.ParentRootColumnName, _DataType = "Guid", _DefaultValue = "", _IsNull = false });
                            ExistingColumnsList.Add(new ColumnStructure { _Name = StageTableName + "_UUID", _DataType = thisDef.ThisParent.FkRelatedColumnType, _DefaultValue = "", _IsNull = false });

                            results.AddRange(er_ddl.ADD_KEY_FOREIGN(forCore, "FK_" + item.TableName + "_" + item.RootColumnName, item.TableName, StageTableName, ExistingColumnsList, ExistingColumnsList, true)); 
                            #endregion

                            #region Add Foreign Key to Identities
                            ExistingColumnsList.Clear(); MetaColumnsList.Clear(); ScaffoldColumns.Clear();
                            ExistingColumnsList.Add(new ColumnStructure { _Name = "IDENTITIES_UUID", _DataType = "Guid", _DefaultValue = "", _IsNull = false });

                            IConnectToDB Link2CSA2 = forCore.Copy();
                            Link2CSA2.Schema2 = "CSA";
                            results.AddRange(er_ddl.ADD_KEY_FOREIGN(Link2CSA2, "FK_" + item.TableName + "_" + "IDENTITIES", item.TableName, "IDENTITIES", ExistingColumnsList, ExistingColumnsList, false)); 
                            #endregion

                            //results.AddRange(ER_DDL._DROP_VIEW(forCore, item.TableName));
                           // results.AddRange(ER_Generate._GENERATE_VIEW(forCore, item.TableName, "Generated"));
                        }

                        thisAppStructure.children.Add(thisStage);

                        //results.AddRange(ER_DDL._DROP_VIEW(forCore, StageTableName));
                        results.AddRange(ER_Generate._GENERATE_VIEW_WITH_CHILDREN(forCore, StageTableName, "Generated"));
                        #endregion
                    }

                    #endregion
                }
                else
                {
                    //IF CASTGOOP Not Present
                    //Update Logic
                }
            }

            return results;
        }

        [Serializable]
        public class ScaffoldStructure
        {
            public IConnectToDB _Connect { get; set; }
            public string Name { get; set; }
            public string ScaffoldType { get; set; }
            public List<ColumnStructure> ColumnsList { get; set; }
            public bool useIdentityUUID { get; set; } = false;
            public string RootColumn { get; set; } = "";
            public List<ScaffoldStructure> children { get; set; } = new List<ScaffoldStructure>();
        }

        [Serializable]
        public class MultiValueChildScaffold
        {
            public string SchemaName { get; set; }
            public string TableName { get; set; }
            public string ObjectLayer { get; set; }
            public List<ColumnStructure> ScaffoldColumns { get; set; } = new List<ColumnStructure>();
            public string RootColumnName { get; set; }
            public string ParentRootColumnName { get; set; }
        }

        private static void HandleVarCharTypes(DataTable theseObjects, ref long? T_OBJECT_SETS_ID, out long? T_PREV_OBJECT_SETS_ID, ref int i, ref DataRow item, ref string PROPERTY_NAME, ref string PROPERTY_VALUE, ref string OBJECT_TYPE, out ColumnStructure thisColumn)
        {
            thisColumn = new ColumnStructure { _Name = PROPERTY_VALUE, _DataType = "Characters(MAX)", _DefaultValue = "", _IsNull = true };
            T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
            //Increment i
            i++;
            for (int y = i; y < theseObjects.AsEnumerable().Count(); y++)
            {
                item = theseObjects.Rows[i];
                AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                if (T_PREV_OBJECT_SETS_ID == T_OBJECT_SETS_ID)
                {
                    item = theseObjects.Rows[y];

                    if (PROPERTY_NAME == "Max")
                    {
                        thisColumn._DataType = "Characters(" + PROPERTY_VALUE + ")";
                    }

                    if (PROPERTY_NAME == "Required" && PROPERTY_VALUE == "true")
                    {
                        thisColumn._IsNull = false;
                        thisColumn._DefaultValue = " ";
                    }
                    i++;

                    T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
                }
                else
                {
                    i--;
                    item = theseObjects.Rows[i];
                    AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                    break;
                }
            }
        }

        private static void HandleNumberTypes(DataTable theseObjects, ref long? T_OBJECT_SETS_ID, out long? T_PREV_OBJECT_SETS_ID, ref int i, ref DataRow item, ref string PROPERTY_NAME, ref string PROPERTY_VALUE, ref string OBJECT_TYPE, out ColumnStructure thisColumn)
        {
            var _DataType = "";
            switch (OBJECT_TYPE.ToLower())
            {
                case "number":
                    _DataType = "bigint";
                    break;
                case "currency":
                case "decimal":
                    _DataType = "money";
                    break;
            }

            thisColumn = new ColumnStructure { _Name = PROPERTY_VALUE, _DataType = _DataType, _DefaultValue = "", _IsNull = true };
            T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
            //Increment i
            i++;
            for (int y = i; y < theseObjects.AsEnumerable().Count(); y++)
            {
                item = theseObjects.Rows[i];
                AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                if (T_PREV_OBJECT_SETS_ID == T_OBJECT_SETS_ID)
                {
                    item = theseObjects.Rows[y];

                    if (PROPERTY_NAME.ToLower() == "required" && PROPERTY_VALUE.ToLower() == "true")
                    {
                        thisColumn._IsNull = false;
                        thisColumn._DefaultValue = "0";
                    }
                    i++;

                    T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
                }
                else
                {
                    i--;
                    item = theseObjects.Rows[i];
                    AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                    break;
                }
            }
        }

        private static void HandleDateAndTimeTypes(DataTable theseObjects, ref long? T_OBJECT_SETS_ID, out long? T_PREV_OBJECT_SETS_ID, ref int i, ref DataRow item, ref string PROPERTY_NAME, ref string PROPERTY_VALUE, ref string OBJECT_TYPE, out ColumnStructure thisColumn)
        {
            var _DataType = "";
            switch (OBJECT_TYPE.ToLower())
            {
                case "time":
                case "times":
                    _DataType = "time(7)";
                    break;
                case "date":
                case "datetime":
                    //_DataType = "datetime";
                    _DataType = "datetime2";
                    break;
                case "timestamp":
                    _DataType = "timestamp";
                    break;
            }

            thisColumn = new ColumnStructure { _Name = PROPERTY_VALUE, _DataType = _DataType, _DefaultValue = "", _IsNull = true };
            T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
            //Increment i
            i++;
            for (int y = i; y < theseObjects.AsEnumerable().Count(); y++)
            {
                item = theseObjects.Rows[i];
                AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                if (T_PREV_OBJECT_SETS_ID == T_OBJECT_SETS_ID)
                {
                    item = theseObjects.Rows[y];

                    if (PROPERTY_NAME == "Required" && PROPERTY_VALUE == "true")
                    {
                        thisColumn._IsNull = false;
                        thisColumn._DefaultValue = DateTime.MinValue.ToString();
                    }
                    i++;

                    T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
                }
                else
                {
                    i--;
                    item = theseObjects.Rows[i];
                    AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                    break;
                }
            }
        }

        private static void HandleByteTypes(DataTable theseObjects, ref long? T_OBJECT_SETS_ID, out long? T_PREV_OBJECT_SETS_ID, ref int i, ref DataRow item, ref string PROPERTY_NAME, ref string PROPERTY_VALUE, ref string OBJECT_TYPE, out ColumnStructure thisColumn)
        {
            var _DataType = "varbinary";

            thisColumn = new ColumnStructure { _Name = PROPERTY_VALUE, _DataType = _DataType, _DefaultValue = "", _IsNull = true };
            T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
            //Increment i
            i++;
            for (int y = i; y < theseObjects.AsEnumerable().Count(); y++)
            {
                item = theseObjects.Rows[i];
                AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                if (T_PREV_OBJECT_SETS_ID == T_OBJECT_SETS_ID)
                {
                    item = theseObjects.Rows[y];

                    if (PROPERTY_NAME == "Required" && PROPERTY_VALUE == "true")
                    {
                        thisColumn._IsNull = false;
                        thisColumn._DefaultValue = ER_Tools.ConvertObjectToString(new byte[0]);
                    }
                    i++;

                    T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
                }
                else
                {
                    i--;
                    item = theseObjects.Rows[i];
                    AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                    break;
                }
            }
        }

        private static void HandleMultiTypes(MultiValueChildScaffold ObjectChildren, DataTable theseObjects, ref long? T_OBJECT_SETS_ID, out long? T_PREV_OBJECT_SETS_ID, ref int i, ref DataRow item, ref string PROPERTY_NAME, ref string PROPERTY_VALUE, ref string OBJECT_TYPE, out ColumnStructure thisColumn)
        {
            var _DataType = "Characters(MAX)";

            thisColumn = new ColumnStructure { _Name = PROPERTY_VALUE, _DataType = _DataType, _DefaultValue = "", _IsNull = true };
            T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
            //Increment i
            i++;
            for (int y = i; y < theseObjects.AsEnumerable().Count(); y++)
            {
                item = theseObjects.Rows[i];
                AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                if (T_PREV_OBJECT_SETS_ID == T_OBJECT_SETS_ID)
                {
                    item = theseObjects.Rows[y];

                    i++;

                    T_PREV_OBJECT_SETS_ID = T_OBJECT_SETS_ID;
                }
                else
                {
                    i--;
                    item = theseObjects.Rows[i];
                    AssignValue(out T_OBJECT_SETS_ID, item, out PROPERTY_NAME, out PROPERTY_VALUE, out OBJECT_TYPE);
                    break;
                }
            }
        }

        private static void AssignValue(out long? T_OBJECT_SETS_ID, DataRow item, out string PROPERTY_NAME, out string PROPERTY_VALUE, out string OBJECT_TYPE)
        {
            T_OBJECT_SETS_ID = item.Field<long?>("OBJECT_SETS_ID");
            PROPERTY_NAME = item.Field<string>("PROPERTY_NAME");
            PROPERTY_VALUE = item.Field<string>("PROPERTY_VALUE");
            OBJECT_TYPE = item.Field<string>("OBJECT_TYPE");
        }
    }
}
