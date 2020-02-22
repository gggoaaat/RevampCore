using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Revamp.IO.SQL.Generators
{
    public class Scaffolds
    {
        public List<CommandResult> ADD_SCAFFOLD(IConnectToDB _Connect, string Name, string ScaffoldType, List<ColumnStructure> ColumnsList, bool useIdentityUUID = false, string RootColumn = "")
        {
            return SYNC_SCAFFOLD(_Connect, Name, ScaffoldType, ColumnsList, useIdentityUUID, RootColumn, false);
        }

        public List<CommandResult> SYNC_SCAFFOLD(IConnectToDB _Connect, string Name, string ScaffoldType, List<ColumnStructure> ColumnsList, bool useIdentityUUID = false, string RootColumn = "", bool sync = false)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();
            ER_DDL er_ddl = new ER_DDL();
            ER_Generate gen = new ER_Generate();
            HoldResult.Add(new CommandResult
            {
                _Response = "--Start Scaffold " + Name + "",
                _Successful = true
            });
            List<string> ColumnsList2 = new List<string>();

            ColumnsList2.Add(Name + "_ID");

            HoldResult.AddRange(ER_DDL._ADD_TABLE(_Connect, Name, ScaffoldType, useIdentityUUID, RootColumn));

            bool tableAlreadyExists = (HoldResult.Exists(x => x._CommandName == "Create Table " + Name && !x._Successful));

            if (tableAlreadyExists && sync == false)
            {
                HoldResult.Add(new CommandResult
                {
                    _Response = Name + " was already created successfully during installation",
                    _Successful = true
                });
            }
            else
            {
                HoldResult.AddRange(er_ddl.ADD_COLUMNS(_Connect, Name, ColumnsList));
                if (tableAlreadyExists == false)
                {
                    HoldResult.AddRange(er_ddl.ADD_KEY_UNIQUE(_Connect, Name + "_ID", Name, ColumnsList2));
                }
                if (useIdentityUUID || true)
                {
                    List<string> ColumnsListUUID = new List<string>();

                    if (tableAlreadyExists == false)
                    {
                        ColumnsListUUID.Add(Name + "_UUID");
                        HoldResult.AddRange(er_ddl.ADD_KEY_UNIQUE(_Connect, Name + "_UUID", Name, ColumnsListUUID));
                    }

                }

                if (tableAlreadyExists && sync)
                {
                    HoldResult.AddRange(er_ddl.DROP_PROCEDURE(_Connect, "SP_I_" + Name + ""));
                    HoldResult.AddRange(er_ddl.DROP_PROCEDURE(_Connect, "SP_U_" + Name + ""));
                    HoldResult.AddRange(er_ddl.DROP_PROCEDURE(_Connect, "SP_D_" + Name + ""));
                    HoldResult.AddRange(er_ddl.DROP_PROCEDURE(_Connect, "SP_U_MOD_" + Name + ""));
                    HoldResult.AddRange(er_ddl.DROP_PROCEDURE(_Connect, "SP_S_" + Name + "_SEARCH"));

                }
                HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, Name, ""));
                HoldResult.AddRange(ER_Generate.CreateProcedureSearchProcedure(_Connect, "Eminent IT", _Connect.Schema,
                    "",
                    "",
                    new List<CommandResult>(),
                    selectStruct: new SearchProcedureStruct
                    {
                        ProcedurePrefix = "SP_S",
                        GetLatestVersion = true,
                        ProcedureName = Name + "_SEARCH",
                        SourceName = Name
                    }));

                HoldResult.AddRange(gen.Generate_Update_Procedure_For_Source(_Connect, Name));
                HoldResult.AddRange(gen.ADD_PROCEDURE_UPDATE(_Connect, Name, ""));
                HoldResult.AddRange(gen.ADD_PROCEDURE_DELETE(_Connect, Name, ""));
            }

            HoldResult.Add(new CommandResult
            {
                _Response = "--EndScaffold " + Name + "",
                _Successful = true
            });

            return HoldResult;
        }

        public List<CommandResult> REMOVE_SCAFFOLD(IConnectToDB _Connect, string Name, bool PreserveTable, bool PreserveProcedures)
        {
            List<CommandResult> results = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            ER_DDL ddl = new ER_DDL();

            results.Add(new CommandResult { _Response = "--Start Remove Scaffold " + Name + "", _Successful = true });

            if (!PreserveProcedures)
            {
                results.AddRange(ddl.DROP_PROCEDURE(_Connect, "SP_I_" + Name + ""));
                results.AddRange(ddl.DROP_PROCEDURE(_Connect, "SP_U_" + Name));
                results.AddRange(ddl.DROP_PROCEDURE(_Connect, "SP_D_" + Name));
                results.AddRange(ddl.DROP_PROCEDURE(_Connect, Name + "O_"));
                results.AddRange(ddl.DROP_PROCEDURE(_Connect, "SP_S_" + Name + "_SEARCH"));
                //results.AddRange(DROP_PROCEDURE(_Connect, Name + "_SEARCH_COUNT"));
            }

            if (!PreserveTable)
            {
                results.AddRange(ddl.DROP_TABLE(_Connect, Name));
            }


            results.Add(new CommandResult { _Response = "--End Remove Scaffold " + Name + "", _Successful = true });

            return results;
        }

        public List<CommandResult> ADD_SCAFFOLD_ACCESS(IConnectToDB _Connect, string TableName)
        {
            string[] excludedTableNames = { "IDENTITIES", "GROUPS", "ROLES", "PRIVILEGES" };

            Tools.Box er_tools = new Tools.Box();
            ER_Generate gen = new ER_Generate();

            List<CommandResult> HoldResult = new List<CommandResult>();

            string newTableName = er_tools.MaxNameLength(TableName, (128 - 16));
            string Perm_Table = newTableName + "_SEC_PERM";
            string Role_Table = newTableName + "_SEC_ROLE";
            string Privs_Table = newTableName + "_SEC_PRIV";
            string GroupRole_Table = newTableName + "_SEC_GRRO";

            HoldResult.Add(new CommandResult
            {
                _Successful = true,
                _Response = "---Start Security Scaffold " + TableName + "",
                _EndTime = DateTime.Now
            });
            List<ColumnStructure> ColumnsList = new List<ColumnStructure>();

            //Run as long as Tables isn't listed
            if (Array.IndexOf(excludedTableNames, TableName.ToUpper()) < 0)
            {
                #region Create Tables For Permission Structures
                HoldResult.AddRange(ER_DDL._ADD_TABLE(_Connect, GroupRole_Table, "Group Role"));
                //HoldResult.AddRange(ER_DDL._ADD_TABLE(_Connect, Perm_Table, "Permission"));
                // HoldResult.AddRange(ER_DDL._ADD_TABLE(_Connect, Privs_Table, "Privilege"));
                HoldResult.AddRange(ER_DDL._ADD_TABLE(_Connect, Role_Table, "Role"));
                #endregion

                if (false)
                {
                    #region Permissions Table
                    ColumnsList.Add(new ColumnStructure { _Name = "OBJECT_TYPE", _DataType = "Characters(120)", _IsNull = false, _DefaultValue = "" });
                    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                    ColumnsList.Add(new ColumnStructure { _Name = "ROLES_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

                    HoldResult.AddRange(ER_DDL._ADD_COLUMNS(_Connect, Perm_Table, ColumnsList));

                    HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, Perm_Table, ""));

                    #region PERMS TABLE Foreign Keys
                    ColumnsList.Clear();
                    ColumnsList.Add(new ColumnStructure { _Name = "ROLES_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Perm_Table + "_1", Perm_Table, "Roles", ColumnsList, ColumnsList));

                    ColumnsList.Clear();
                    ColumnsList.Add(new ColumnStructure { _Name = "IDENTITIES_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Perm_Table + "_2", Perm_Table, "Identities", ColumnsList, ColumnsList));

                    ColumnsList.Clear();
                    ColumnsList.Add(new ColumnStructure { _Name = "OBJECT_TYPE", _DataType = "Characters(120)", _IsNull = false, _DefaultValue = "" });
                    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Perm_Table + "_3", Perm_Table, "Objects", ColumnsList, ColumnsList));

                    ColumnsList.Clear();
                    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Perm_Table + "_4", Perm_Table, TableName, ColumnsList, ColumnsList));
                    #endregion

                    #endregion 
                }

                #region Roles Table
                ColumnsList.Clear();
                ColumnsList.Add(new ColumnStructure { _Name = "OBJECT_TYPE", _DataType = "Characters(120)", _IsNull = false, _DefaultValue = "" });
                ColumnsList.Add(new ColumnStructure { _Name = TableName + "_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                if (TableName.ToLower() != "applications")
                {
                    ColumnsList.Add(new ColumnStructure { _Name = "APPLICATIONS_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                }
                ColumnsList.Add(new ColumnStructure { _Name = "ROLES_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });

                HoldResult.AddRange(ER_DDL._ADD_COLUMNS(_Connect, Role_Table, ColumnsList));
                HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, Role_Table, ""));
                HoldResult.AddRange(gen.Generate_Update_Procedure_For_Source(_Connect, Role_Table));
                HoldResult.AddRange(gen.ADD_PROCEDURE_UPDATE(_Connect, Role_Table, ""));
                HoldResult.AddRange(gen.ADD_PROCEDURE_DELETE(_Connect, Role_Table, ""));
                HoldResult.AddRange(ER_Generate.CreateProcedureSearchProcedure(_Connect, "Eminent IT", _Connect.Schema,
                    "",
                    "",
                    new List<CommandResult>(),
                    selectStruct: new SearchProcedureStruct
                    {
                        ProcedurePrefix = "SP_S",
                        GetLatestVersion = true,
                        ProcedureName = Role_Table + "_SEARCH",
                        SourceName = Role_Table
                    }));

                #region Foreign Keys

                ColumnsList.Clear();
                ColumnsList.Add(new ColumnStructure { _Name = "ROLES_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Role_Table + "_SEC_1", Role_Table, "Roles", ColumnsList, ColumnsList));

                ColumnsList.Clear();
                ColumnsList.Add(new ColumnStructure { _Name = "OBJECT_TYPE", _DataType = "Characters(120)", _IsNull = false, _DefaultValue = "" });
                HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Role_Table + "_SEC_2", Role_Table, "Objects", ColumnsList, ColumnsList));

                ColumnsList.Clear();
                ColumnsList.Add(new ColumnStructure { _Name = TableName + "_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Role_Table + "_SEC_3", Role_Table, TableName, ColumnsList, ColumnsList));

                //if (TableName.ToLower() != "applications")
                //{
                //    ColumnsList.Clear();
                //    ColumnsList.Add(new ColumnStructure { _Name = "APPLICATIONS_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                //    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Role_Table + "_SEC_4", Role_Table, "APPLICATIONS", ColumnsList, ColumnsList));
                //}
                #endregion

                #endregion

                if (false)
                {
                    #region Privileges Table
                    ColumnsList.Clear();

                    ColumnsList.Add(new ColumnStructure { _Name = "OBJECT_TYPE", _DataType = "Characters(120)", _IsNull = false, _DefaultValue = "" });
                    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                    ColumnsList.Add(new ColumnStructure { _Name = "PRIVILEGES_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                    HoldResult.AddRange(ER_DDL._ADD_COLUMNS(_Connect, Privs_Table, ColumnsList));

                    HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, Privs_Table, ""));

                    #region Priv Table Foreign Keys
                    ColumnsList.Clear();
                    ColumnsList.Add(new ColumnStructure { _Name = "PRIVILEGES_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Privs_Table + "_1", Privs_Table, "Privileges", ColumnsList, ColumnsList));

                    ColumnsList.Clear();
                    ColumnsList.Add(new ColumnStructure { _Name = "IDENTITIES_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Privs_Table + "_2", Privs_Table, "Identities", ColumnsList, ColumnsList));

                    ColumnsList.Clear();
                    ColumnsList.Add(new ColumnStructure { _Name = "OBJECT_TYPE", _DataType = "Characters(120)", _IsNull = false, _DefaultValue = "" });
                    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Privs_Table + "_3", Privs_Table, "Objects", ColumnsList, ColumnsList));

                    ColumnsList.Clear();
                    ColumnsList.Add(new ColumnStructure { _Name = TableName + "_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });
                    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, Privs_Table + "_4", Privs_Table, TableName, ColumnsList, ColumnsList));
                    #endregion

                    #endregion 
                }

                #region Group Roles Table
                ColumnsList.Clear();

                ColumnsList.Add(new ColumnStructure { _Name = "OBJECT_TYPE", _DataType = "Characters(120)", _IsNull = false, _DefaultValue = "" });
                ColumnsList.Add(new ColumnStructure { _Name = TableName + "_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                if (TableName.ToLower() != "applications")
                {
                    ColumnsList.Add(new ColumnStructure { _Name = "APPLICATIONS_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                }
                ColumnsList.Add(new ColumnStructure { _Name = "GROUPS_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                ColumnsList.Add(new ColumnStructure { _Name = "ROLES_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });

                HoldResult.AddRange(ER_DDL._ADD_COLUMNS(_Connect, GroupRole_Table, ColumnsList));
                HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, GroupRole_Table, ""));
                HoldResult.AddRange(gen.Generate_Update_Procedure_For_Source(_Connect, GroupRole_Table));
                HoldResult.AddRange(gen.ADD_PROCEDURE_UPDATE(_Connect, GroupRole_Table, ""));
                HoldResult.AddRange(gen.ADD_PROCEDURE_DELETE(_Connect, GroupRole_Table, ""));
                HoldResult.AddRange(ER_Generate.CreateProcedureSearchProcedure(_Connect, "Eminent IT", _Connect.Schema,
                    "",
                    "",
                    new List<CommandResult>(),
                    selectStruct: new SearchProcedureStruct
                    {
                        ProcedurePrefix = "SP_S",
                        GetLatestVersion = true,
                        ProcedureName = GroupRole_Table + "_SEARCH",
                        SourceName = GroupRole_Table
                    }));

                ColumnsList.Clear();
                ColumnsList.Add(new ColumnStructure { _Name = "ROLES_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, GroupRole_Table + "_1", GroupRole_Table, "Roles", ColumnsList, ColumnsList));

                ColumnsList.Clear();
                ColumnsList.Add(new ColumnStructure { _Name = "GROUPS_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, GroupRole_Table + "_2", GroupRole_Table, "Groups", ColumnsList, ColumnsList));

                //if (TableName.ToLower() != "applications")
                //{
                //    ColumnsList.Clear();
                //    ColumnsList.Add(new ColumnStructure { _Name = "APPLICATIONS_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
                //    HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, GroupRole_Table + "_SEC_3", Role_Table, "APPLICATIONS", ColumnsList, ColumnsList));
                //}

                #endregion 
            }

            HoldResult.Add(new CommandResult
            {
                _Successful = true,
                _Response = "---End Security Scaffold " + TableName + "",
                _EndTime = DateTime.Now
            });

            return HoldResult;
        }

        public List<CommandResult> ADD_SCAFFOLD_DATA(IConnectToDB _Connect, string TableName)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();
            CommandResult _result = new CommandResult();
            Tools.Box er_tools = new Tools.Box();
            ER_Generate gen = new ER_Generate();

            string newTableName = er_tools.MaxNameLength(TableName, (128 - 16));

            string Char_Table = newTableName + "_Dat_Char";
            string Date_Table = newTableName + "_Dat_Date";
            string Number_Table = newTableName + "_Dat_Numb";
            string File_Table = newTableName + "_Dat_File";
            string Options_Table = newTableName + "_Dat_Opt";
            string Decimal_Table = newTableName + "_Dat_Deci";

            string Stages_Name = TableName + "_Stages";

            HoldResult.Add(new CommandResult
            {
                _Successful = true,
                _Response = "---Start Data Scaffold " + TableName + "",
                _EndTime = DateTime.Now
            });

            List<ColumnStructure> ColumnsList = new List<ColumnStructure>();

            string[] tablesList = { Char_Table, Number_Table, Date_Table, File_Table, Options_Table, Decimal_Table };

            foreach (var t in tablesList)
            {
                HoldResult.AddRange(ER_DDL._ADD_TABLE(_Connect, t, "Application Data"));
            }

            //ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _DefaultValue = "0", _IsNull = false });
            HoldResult.AddRange(ER_DDL._ADD_TABLE_CHAR(_Connect, TableName, Char_Table, ColumnsList));

            //ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _DefaultValue = "0", _IsNull = false });
            HoldResult.AddRange(ER_DDL._ADD_TABLE_NUMB(_Connect, TableName, Number_Table, ColumnsList));

            //ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _DefaultValue = "0", _IsNull = false });
            HoldResult.AddRange(ER_DDL._ADD_TABLE_DATE(_Connect, TableName, Date_Table, ColumnsList));

            //ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _DefaultValue = "0", _IsNull = false });
            HoldResult.AddRange(ER_DDL._ADD_TABLE_FILE(_Connect, TableName, File_Table, ColumnsList));

            //ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _DefaultValue = "0", _IsNull = false });
            //HoldResult.AddRange(ER_DDL._ADD_TABLE_OPT(_Connect, TableName, Options_Table, ColumnsList));

            //ColumnsList.Add(new ColumnStructure { _Name = "RENDITION", _DataType = "bigint", _DefaultValue = "0", _IsNull = false });
            HoldResult.AddRange(ER_DDL._ADD_TABLE_DECIMAL(_Connect, TableName, Decimal_Table, ColumnsList));

            foreach (var t in tablesList)
            {
                ColumnsList.Clear();
                var item = new ColumnStructure
                {
                    _Name = t + "_UUID",
                    _DefaultValue = "",
                    _IsNull = false,
                    _DataType = "Guid" // t != Decimal_Table ? "bigint" : "Money";
                };

                ColumnsList.Add(item);
                HoldResult.AddRange(ER_DDL._ADD_INDEX_NONE_CLUSTERED(_Connect, t + "_CI", t, "TABLE", ColumnsList));
                HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, t, ""));
            }

            HoldResult.Add(new CommandResult
            {
                _Successful = true,
                _Response = "---End Data Scaffold " + TableName + "",
                _EndTime = DateTime.Now
            });
            return HoldResult;
        }

        public List<CommandResult> ADD_SCAFFOLD_FLOW(IConnectToDB _Connect, string Name, string ScaffoldType, List<ColumnStructure> ColumnsList)
        {
            Tools.Box er_tools = new Tools.Box();

            List<CommandResult> HoldResult = new List<CommandResult>();
            ER_Generate gen = new ER_Generate();
            string newTableName = er_tools.MaxNameLength(Name, (128 - 16));

            string Actions_Table = newTableName + "_Flo_Acts";
            string Identities_Table = newTableName + "_Flo_Iden";
            string Conditions_Table = newTableName + "_Flo_Cond";
            string if_table = newTableName + "_Flo_If";
            string then_table = newTableName + "_Flo_Then";

            HoldResult.Add(new CommandResult { _Response = "--Start FLOW Scaffold " + Name + "", _Successful = true });
            List<string> ColumnsList2 = new List<string>();
            ColumnsList2.Add(Name + "_ID");
            HoldResult.AddRange(ER_DDL._ADD_TABLE(_Connect, Name, ScaffoldType));
            HoldResult.AddRange(ER_DDL._ADD_COLUMNS(_Connect, Name, ColumnsList));
            HoldResult.AddRange(ER_DDL._ADD_KEY_UNIQUE(_Connect, Name + "_ID", Name, ColumnsList2));
            HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, Name, ""));
            HoldResult.AddRange(gen.ADD_PROCEDURE_UPDATE(_Connect, Name, ""));
            HoldResult.AddRange(gen.ADD_PROCEDURE_DELETE(_Connect, Name, ""));
            HoldResult.Add(new CommandResult { _Response = "--EndScaffold " + Name + "", _Successful = true });

            return HoldResult;
        }

        /// <summary>
        /// This adds the ability to generate notifications for a specific table. It accepts a tablename. It then create a new
        /// Notification table from the received tablename. There is also a foreign link which is created between parent table
        /// and notification table.
        /// </summary>
        /// <param name="DB_PLATFORM"></param>
        /// <param name="connAuth"></param>
        /// <param name="OwnerName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public List<CommandResult> ADD_SCAFFOLD_NOTIFICATION(IConnectToDB _Connect, string TableName)
        {
            Tools.Box er_tools = new Tools.Box();
            ER_DDL ddl = new ER_DDL();
            ER_Generate gen = new ER_Generate();
            List<CommandResult> HoldResult = new List<CommandResult>();

            string newTableName = er_tools.MaxNameLength(TableName, (128 - 16));
            string Notification_Table = newTableName + "_NTFY";

            HoldResult.Add(new CommandResult { _Response = "---Start Notification Scaffold " + TableName + "", _Successful = true });
            List<ColumnStructure> ColumnsList = new List<ColumnStructure>();

            //HoldResult.Add(ER_DDL._ADD_TABLE(_Connect, Perm_Table, "Permission"));
            HoldResult.AddRange(ER_DDL._ADD_TABLE(_Connect, Notification_Table, "Notification"));

            ColumnsList.Add(new ColumnStructure { _Name = "OBJECT_TYPE", _DataType = "Characters(120)", _IsNull = false, _DefaultValue = "" });
            ColumnsList.Add(new ColumnStructure { _Name = TableName + "_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });
            //ColumnsList.Add(new ColumnStructure { _Name = "APPLICATIONS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            HoldResult.AddRange(ddl.ADD_COLUMNS(_Connect, Notification_Table, ColumnsList));

            HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, Notification_Table, ""));

            ColumnsList.Clear();

            ColumnsList.Add(new ColumnStructure { _Name = TableName + "_UUID", _DataType = "Guid", _IsNull = false, _DefaultValue = "" });

            HoldResult.AddRange(ddl.ADD_KEY_FOREIGN(_Connect, TableName + "_NTFY", Notification_Table, TableName, ColumnsList, ColumnsList));

            //ColumnsList.Clear();

            //ColumnsList.Add(new ColumnStructure { _Name = "APPLICATIONS_ID", _DataType = "bigint", _IsNull = false, _DefaultValue = "" });

            //HoldResult.Add(ADD_KEY_FOREIGN(_Connect, TableName + "APP_NTFY", Notification_Table, "Applications", ColumnsList, ColumnsList));

            StringBuilder TriggerBody = new StringBuilder();

            TriggerBody.AppendLine("EXEC SP_I_" + TableName + "_NTFY 'Notification', @" + TableName + "_ID, @outval;");

            ER_DDL._ADD_TRIGGER(_Connect, "Insert", "Before", TableName, TriggerBody);

            HoldResult.Add(new CommandResult { _Response = "---End Security Scaffold " + TableName + "", _Successful = true });
            return HoldResult;
        }

        //Creates a relationship lookup table for one or more tables
        public List<CommandResult> ADD_SCAFFOLD_RELATIONSHIPS(IConnectToDB _Connect, string Name, List<TableStructure> SourceTables)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();
            ER_Generate gen = new ER_Generate();
            string tableName = Name + "_REL";
            HoldResult.Add(new CommandResult
            {
                _Successful = true,
                _Response = "---Start Scaffold " + tableName + "",
                _EndTime = DateTime.Now
            });

            HoldResult.AddRange(ER_DDL._ADD_TABLE(_Connect, tableName, "Relationship"));
            HoldResult.Add(ER_DDL._ADD_COLUMN(_Connect, tableName, "OBJECT_TYPE", "Characters(120)", "", false));

            for (int i = 0; i < SourceTables.Count; i++)
            {
                for (int ii = 0; ii < SourceTables[i].ColumnStructure.Count; ii++)
                {
                    HoldResult.AddRange(ER_DDL._ADD_COLUMNS(_Connect, tableName, SourceTables[i].ColumnStructure));
                }

                for (int ii = 0; ii < SourceTables[i].RelationshipColumnStructure.Count; ii++)
                {
                    HoldResult.AddRange(ER_DDL._ADD_COLUMNS(_Connect, tableName, SourceTables[ii].RelationshipColumnStructure));
                }
            }

            List<string> ColumnsList2 = new List<string>();
            ColumnsList2.Add(tableName + "_ID");
            List<ColumnStructure> JoinedColumns = new List<ColumnStructure>();
            HoldResult.AddRange(ER_DDL._ADD_KEY_UNIQUE(_Connect, tableName + "_ID", tableName, ColumnsList2));

            for (int i = 0; i < SourceTables.Count; i++)
            {
                JoinedColumns.AddRange(SourceTables[i].ColumnStructure);
            }
            for (int i = 0; i < SourceTables.Count; i++)
            {
                JoinedColumns.AddRange(SourceTables[i].RelationshipColumnStructure);
            }

            HoldResult.AddRange(ER_DDL._ADD_KEY_PRIMARY(_Connect, tableName, tableName, JoinedColumns));

            for (int i = 0; i < SourceTables.Count; i++)
            {
                HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, tableName + "_" + i.ToString(), tableName, SourceTables[i]._TableName, SourceTables[i].ColumnStructure, SourceTables[i].RelationshipColumnStructure));
            }
            for (int i = 0; i < SourceTables.Count; i++)
            {
                HoldResult.AddRange(ER_DDL._ADD_KEY_FOREIGN(_Connect, tableName + "_A" + i.ToString(), tableName, SourceTables[i]._TableName, SourceTables[i].RelationshipColumnStructure, SourceTables[i].RelationshipColumnStructure));
            }


            HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, tableName, ""));
            HoldResult.AddRange(gen.ADD_PROCEDURE_UPDATE(_Connect, tableName, ""));
            HoldResult.AddRange(gen.ADD_PROCEDURE_DELETE(_Connect, tableName, ""));

            ER_Generate er_gen = new ER_Generate();
            HoldResult.AddRange(er_gen.Generate_Update_Procedure_For_Source(_Connect, tableName));
            //HoldResult.AddRange(er_gen.GENERATE_VIEW(_Connect, tableName, "DICTIONARY", "ALL"));

            HoldResult.Add(new CommandResult
            {
                _Successful = true,
                _Response = "---End Scaffold " + tableName + "",
                _EndTime = DateTime.Now
            });
            return HoldResult;
        }

        public List<CommandResult> EXTEND_SCAFFOLD(IConnectToDB _Connect, string Name)
        {
            List<CommandResult> HoldResult = new List<CommandResult>();
            ER_Generate gen = new ER_Generate();
            HoldResult.Add(new CommandResult
            {
                _Successful = true,
                _Response = "---Start Scaffold " + Name + "",
                _EndTime = DateTime.Now
            });
            List<string> ColumnsList2 = new List<string>();
            ColumnsList2.Add(Name + "_ID");
            //Add(ADD_TABLE(Name));
            //HoldResult.AddRange(ADD_COLUMNS(_Connect, Name, ColumnsList));
            //HoldResult.Add(ADD_KEY_UNIQUE(_Connect, Name, ColumnsList2));
            //HoldResult.Add(er_ddl.ADD_ORACLE_SEQUENCE(_Connect, Name));
            //HoldResult.Add(er_ddl.ADD_SEQUENCE_TRIGGER(_Connect, Name, "Before", "INSERT", (Name + "_SQ"), (Name + "_ID")));
            HoldResult.AddRange(gen.ADD_PROCEDURE_INSERT(_Connect, Name, ""));
            HoldResult.AddRange(gen.ADD_PROCEDURE_UPDATE(_Connect, Name, ""));
            HoldResult.AddRange(gen.ADD_PROCEDURE_DELETE(_Connect, Name, ""));

            HoldResult.Add(new CommandResult
            {
                _Successful = true,
                _Response = "---End Scaffold " + Name + "",
                _EndTime = DateTime.Now
            });
            return HoldResult;
        }
    }
}
