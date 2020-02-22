using Microsoft.Data.SqlClient;
//using Microsoft.Security.Application;
using Revamp.IO.Structs;
using Revamp.IO.Structs.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using static Revamp.IO.Structs.Models.SQLProcedureModels;

namespace Revamp.IO.DB.Bridge
{
    public class ER_Procedure
    {
        public static BIG_CALL SQL_PROCEDURE(IConnectToDB _Connect, BIG_CALL RUN)
        {
            ER_Procedure er_proc = new ER_Procedure();

            return er_proc._SQL_PROCEDURE(_Connect, RUN);
        }


        public BIG_CALL _SQL_PROCEDURE(IConnectToDB _Connect, BIG_CALL RUN)
        {
            ER_Env er_env = new ER_Env();

            //string _return = "";
            //DataSet ProcReturn = new DataSet();


            switch (_Connect.Platform)
            {

                case "Microsoft":
                case "MICROSOFT":
                    using (SqlConnection connection = new SqlConnection(_Connect.DBConnString))
                    {
                        SqlCommand DBProcedure = new SqlCommand();

                        //Pass DB Connection Settings
                        DBProcedure.Connection = connection;
                        //Pass Procedure Name

                        connection.Open();

                        if (RUN.COMMANDS != null)
                        {
                            for (int c = 0; c < RUN.COMMANDS.Count; c++)
                            {
                                DataTable _table = er_env.ProcDataTable();
                                DataRow _row;

                                DBProcedure.CommandText = DBTools.GetSchema(_Connect) + "." + RUN.COMMANDS[c].ProcedureName; ;

                                //Set Commandtype to Storeprocedure
                                DBProcedure.CommandType = CommandType.StoredProcedure;

                                SqlParameter[] param = new SqlParameter[RUN.COMMANDS[c]._dbParameters.Count];
                                int paramcount = 0;

                                //Iterate through all passed parameters.
                                int count = RUN.COMMANDS[c]._dbParameters.Count;

                                for (int i = 0; i < count; i++)
                                {
                                    //Add Parameters to DBProcedure
                                    //DBProcedure.Parameters.Add(i.ParamName, i.MSSqlParamDataType, i.ParamSize, i.ParamValue, i.ParamDirection);
                                    //DBProcedure.Parameters.Add(i.ParamName, i.MSSqlParamDataType).Value = i.ParamValue;

                                    param[paramcount] = new SqlParameter();

                                    param[paramcount].Direction = RUN.COMMANDS[c]._dbParameters[i].ParamDirection;
                                    param[paramcount].ParameterName = RUN.COMMANDS[c]._dbParameters[i].ParamName;
                                    param[paramcount].SqlDbType = RUN.COMMANDS[c]._dbParameters[i].MSSqlParamDataType;

                                    param[paramcount].Size = RUN.COMMANDS[c]._dbParameters[i].ParamSize;




                                    if (param[paramcount].SqlDbType == SqlDbType.Int || param[paramcount].SqlDbType == SqlDbType.BigInt)
                                    {
                                        if (RUN.COMMANDS[c]._dbParameters[i].ParamValue == null || String.IsNullOrEmpty(RUN.COMMANDS[c]._dbParameters[i].ParamValue.ToString()))
                                        {
                                            param[paramcount].Value = DBNull.Value;
                                        }
                                        else
                                        {
                                            string thisValue = RUN.COMMANDS[c]._dbParameters[i].ParamValue == null
                                            && String.IsNullOrEmpty(RUN.COMMANDS[c]._dbParameters[i].ParamValue.ToString()) ? "0" : RUN.COMMANDS[c]._dbParameters[i].ParamValue.ToString();
                                            if (param[paramcount].SqlDbType == SqlDbType.Int)
                                            {
                                                param[paramcount].Value = Tools.Box.ConvertToInt32(thisValue);
                                            }
                                            else
                                            {
                                                param[paramcount].Value = Tools.Box.ConvertToInt64(thisValue);
                                            }
                                        }
                                    }
                                    else if (param[paramcount].SqlDbType == SqlDbType.Decimal)
                                    {
                                        if (RUN.COMMANDS[c]._dbParameters[i].ParamValue == null)
                                        {
                                            param[paramcount].Value = DBNull.Value;
                                        }
                                        else
                                        {
                                            decimal number;
                                            Decimal.TryParse(RUN.COMMANDS[c]._dbParameters[i].ParamValue.ToString(), out number);
                                            param[paramcount].Value = number;
                                        }
                                    }
                                    else
                                    {
                                        param[paramcount].Value = RUN.COMMANDS[c]._dbParameters[i].ParamValue != null ? RUN.COMMANDS[c]._dbParameters[i].ParamValue : DBNull.Value;
                                    }

                                    if (RUN.COMMANDS[c]._dbParameters[i].DynamicallyAssign)
                                    {

                                        List<PARAMATER_VALUE> thisValue = RUN.PARAMETER_CONTAINER.Where(e => e.PARAMETER == RUN.COMMANDS[c]._dbParameters[i].DynamicPartner).ToList();

                                        param[paramcount].Value = thisValue.First().VALUE;
                                    }


                                    DBProcedure.Parameters.Add(param[paramcount]);

                                    paramcount++;
                                }

                                string preppedSQL = "";
                                switch (RUN.COMMANDS[c].ProcedureType.ToLower())
                                {
                                    case "value":
                                    case "object":
                                        try
                                        {
                                            //Open Connection

                                            DBProcedure.CommandTimeout = 60;

                                            preppedSQL = DBProcedure.CommandAsSql();
                                            DBProcedure.ExecuteNonQuery();

                                            int iNumber = 0;


                                            for (int ii = 0; ii < RUN.COMMANDS[c]._dbParameters.Count; ii++)
                                            {
                                                //Resulting Data will be added to returning table.
                                                _row = _table.NewRow();
                                                _row["ChildType"] = RUN.COMMANDS[c]._dbParameters[ii].ParamDirection.ToString();
                                                _row["ChildItem"] = DBProcedure.Parameters[iNumber].ParameterName.ToString();
                                                _row["ChildSQL"] = ii == 0 ? preppedSQL : "";

                                                if (RUN.COMMANDS[c]._dbParameters[ii].MSSqlParamDataType == SqlDbType.VarBinary && RUN.COMMANDS[c]._dbParameters[ii].ParamValue != null)
                                                {
                                                    //byte[] _bytes = (byte[])DBProcedure.Parameters[iNumber].Value;
                                                    //_row["ChildValue"] = _bytes.ToString();
                                                    //byte[] ok = er_tools.GetBytes(_bytes);
                                                    //_row["ChildValue"] = er_tools.GetString((byte[])DBProcedure.Parameters[iNumber].Value);
                                                    //_row["ChildValue"] = Convert.ToBase64String((byte[])DBProcedure.Parameters[iNumber].Value);
                                                    //string ok = Convert.ToBase64String(_bytes);
                                                    _row["ChildValue"] = Convert.ToBase64String((byte[])DBProcedure.Parameters[iNumber].Value);
                                                }
                                                else
                                                {
                                                    _row["ChildValue"] = DBProcedure.Parameters[iNumber].Value.ToString();
                                                }
                                                _table.Rows.Add(_row);

                                                iNumber++;
                                            }

                                        }
                                        catch (SqlException ex)
                                        {
                                            //Return Oracle Error.
                                            _row = _table.NewRow();
                                            _row["ChildType"] = "Error";
                                            _row["ChildItem"] = "Exception";
                                            ER_Query er_query = new ER_Query();
                                            _row["ChildValue"] = er_query.DB_ERROR_FORMATTER(_Connect, ex);
                                            _row["ChildSQL"] = preppedSQL;
                                            _table.Rows.Add(_row);
                                        }
                                        finally
                                        {
                                            ////Close connection.
                                            //connection.Close();
                                            //connection.Dispose();
                                        }

                                        break;
                                    case "query":
                                    case "table":
                                        try
                                        {
                                            //Open Connection
                                            //connection.Open();
                                            DBProcedure.CommandTimeout = 60;
                                            SqlDataReader _ResultQuery = DBProcedure.ExecuteReader();

                                            DataTable _ResultDataTable = new DataTable();
                                            _ResultDataTable.Load(_ResultQuery);

                                            //overwrite previous _table structure.
                                            _table = _ResultDataTable;

                                        }
                                        catch (SqlException ex)
                                        {
                                            //Return Oracle Error.
                                            _row = _table.NewRow();
                                            _row["ChildType"] = "Error";
                                            _row["ChildItem"] = "Exception";
                                            ER_Query er_query = new ER_Query();
                                            _row["ChildValue"] = er_query.DB_ERROR_FORMATTER(_Connect, ex);
                                            _table.Rows.Add(_row);

                                        }
                                        finally
                                        {
                                            ////Close connection.
                                            //connection.Close();
                                            //connection.Dispose();
                                        }

                                        break;
                                }

                                RUN.COMMANDS[c].result = new Results();
                                RUN.COMMANDS[c].result.resulttype = ResultsType.DataTable;
                                RUN.COMMANDS[c].result._DataTable = _table;
                            }
                        }

                        //Close connection.
                        connection.Close();
                        connection.Dispose();
                    }
                    break;
            }

            return RUN;
        }

        public static BIG_CALL SQL_PROCEDURE_PARAMS(IConnectToDB _Connect, BIG_CALL RUN)
        {
            ER_Procedure er_proc = new ER_Procedure();

            return er_proc._SQL_PROCEDURE_PARAMS(_Connect, RUN);

        }



        public BIG_CALL _SQL_PROCEDURE_PARAMS(IConnectToDB _Connect, BIG_CALL RUN /*, string ProcedureType, string ProcedureName, List<DBParameters> _dbParameters*/)
        {
            //List<SQL_PROCEDURE_CALL> infoCOMMANDS = new List<SQL_PROCEDURE_CALL>();

            //for (int c = 0; c < RUN.COMMANDS.Count; c++)
            //{

            //    DBParameters[] dbParameters = new DBParameters[RUN.COMMANDS[c]._dbParameters.Count];

            //    List<DBParameters> infoList = new List<DBParameters>();

            //    int iNumber = 0;
            //    switch (_Connect.Platform)
            //    {

            //        case "Microsoft":
            //        case "MICROSOFT":
            //            foreach (DBParameters i in RUN.COMMANDS[c]._dbParameters)
            //            {
            //                dbParameters[iNumber] = new DBParameters();
            //                dbParameters[iNumber].ParamName = i.ParamName;
            //                dbParameters[iNumber].MSSqlParamDataType = i.MSSqlParamDataType;
            //                dbParameters[iNumber].ParamDirection = i.ParamDirection; //Not used by MSSQL
            //                dbParameters[iNumber].ParamValue = i.ParamValue; //Not used by MSSQL
            //                dbParameters[iNumber].ParamSize = i.ParamSize;
            //                dbParameters[iNumber].ParamReturn = i.ParamReturn;

            //                infoList.Add(dbParameters[iNumber]);

            //                iNumber++; //++ Increments by 1.
            //            }
            //            break;
            //    }

            //    SQL_PROCEDURE_CALL thisCommand = new SQL_PROCEDURE_CALL
            //    {
            //        ProcedureName = RUN.COMMANDS[c].ProcedureName,
            //        ProcedureType = RUN.COMMANDS[c].ProcedureType,
            //        _dbParameters = infoList                      
            //    };

            //    infoCOMMANDS.Add(thisCommand);

            //    RUN.COMMANDS[c] = thisCommand;
            //}

            return SQL_PROCEDURE(_Connect, RUN);
        }

        public static sqlTransBlocks SQL_BUILD_BLOCK(IConnectToDB _Connect, string TransactionName, sqlTransBlocks BLOCKS)
        {
            ER_Procedure proc = new ER_Procedure();

            return proc._SQL_BUILD_BLOCK(_Connect, TransactionName, BLOCKS);
        }

        public static string TransactionNameGenerator(SessionObjects SO, string TransactionType)
        {
            if (SO == null)
            {
                SO = new SessionObjects();
            }

            if (SO._IdentityModel == null)
            {
                SO._IdentityModel = new IdentityModel();
            }

            string userNameTransform = SO._IdentityModel.username.ToUpper();
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string TransactionName = currentTime + "_" + TransactionType + "_" + userNameTransform;
            return TransactionName;
        }

        public static string TransactionNameGenerator(string username, string TransactionType)
        {
            string userNameTransform = username.ToUpper();
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string TransactionName = currentTime + "_" + TransactionType + "_" + userNameTransform;
            return TransactionName;
        }

        public sqlTransBlocks _SQL_BUILD_BLOCK(IConnectToDB _Connect, string TransactionName, sqlTransBlocks BLOCKS)
        {
            List<SQLContent> _theseSQLContents = new List<SQLContent>();

            for (int i = 0; i < BLOCKS.Series.Count; i++)
            {
                //TODO: Delete this later once verified.
                #region Delete this code once verify it's not used.
                //List<DBParameters> TheseParams = new List<DBParameters>();
                //if (BLOCKS.CASTGO[i].Application != null)
                //{
                //    TheseParams = BLOCKS.Series[i].EntryProcedureParameters;
                //}

                //else if (BLOCKS.CASTGO[i].Stage != null)
                //{
                //    TheseParams = BLOCKS.Series[i].EntryProcedureParameters;
                //}

                //else if (BLOCKS.CASTGO[i].Grip != null)
                //{
                //    TheseParams = BLOCKS.Series[i].EntryProcedureParameters;
                //}

                //else if (BLOCKS.CASTGO[i].ObjectSet != null)
                //{
                //    TheseParams = BLOCKS.Series[i].EntryProcedureParameters;
                //}

                //else if (BLOCKS.CASTGO[i].PropertySet != null)
                //{
                //    TheseParams = BLOCKS.Series[i].EntryProcedureParameters;
                //}

                //else if (BLOCKS.CASTGO[i].OptionSet != null)
                //{
                //    TheseParams = BLOCKS.Series[i].EntryProcedureParameters;
                //}

                //else if (BLOCKS.CASTGO[i].EnterFile != null)
                //{
                //    TheseParams = BLOCKS.Series[i].EntryProcedureParameters;
                //}

                //else if (BLOCKS.CASTGO[i].EnterFilePoint != null)
                //{
                //    TheseParams = BLOCKS.Series[i].EntryProcedureParameters;
                //}

                //else
                //{
                //    TheseParams = BLOCKS.Series[i].EntryProcedureParameters;
                // } 
                #endregion

                #region antiXSS This Block the DB from saving any script.
                SqlDbType[] StringTypes = { SqlDbType.VarChar, SqlDbType.NVarChar, SqlDbType.NText, SqlDbType.Char, SqlDbType.NChar };

                foreach (DBParameters item in BLOCKS.Series[i].EntryProcedureParameters)
                {
                    if (item.AvoidAntiXss != true && Array.IndexOf(StringTypes, item.MSSqlParamDataType) > -1 && item.ParamValue != null && !string.IsNullOrWhiteSpace(item.ParamValue.ToString()))
                    {
                        item.ParamValue = Sanitizer.GetSafeHtmlFragment(item.ParamValue.ToString());
                    }
                }
                #endregion

                SQLContent _sqlContent = GetSQLFromCommand(new SQLContent
                {
                    ProcedureName = BLOCKS.Series[i].ProcedureName,
                    Parameters = BLOCKS.Series[i].EntryProcedureParameters,
                });

                _theseSQLContents.Add(_sqlContent);
            }


            ArrayList _SqlOut = new ArrayList();

            List<ParameterSet> AllOutParams = new List<ParameterSet>();

            string _Schema = DBTools.GetSchema(_Connect);

            _SqlOut.Add("DECLARE @TRANSNAMEROOT varchar(MAX) = 'TRANSACTION " + TransactionName + "'");
            _SqlOut.Add("BEGIN ");
            _SqlOut.Add("    BEGIN Transaction @TRANSNAMEROOT ");
            _SqlOut.Add("    BEGIN TRY ");
            foreach (var item in BLOCKS.Commands)
            {
                _SqlOut.Add(item.Syntax);
                _SqlOut.Add("");
            }
            DeclarationBlock(_theseSQLContents, _SqlOut, AllOutParams);
            ExecutionBlock(BLOCKS, _theseSQLContents, _SqlOut, AllOutParams, _Schema);
            _SqlOut.Add("        COMMIT TRANSACTION @TRANSNAMEROOT ");
            _SqlOut.Add("    END TRY ");
            _SqlOut.Add("    BEGIN CATCH ");
            _SqlOut.Add("        ROLLBACK TRANSACTION @TRANSNAMEROOT ");
            _SqlOut.Add("        SELECT ERROR_NUMBER() as [ERROR_NUMBER], ERROR_MESSAGE() as [ERROR_MESSAGE] ");
            _SqlOut.Add("   END CATCH ");
            _SqlOut.Add("END");

            string SQLTotal = "";
            try
            {
                string createPath = @"C:\TEMP\TRANSACTIONS\";

                // Determine whether the directory exists.
                if (!Directory.Exists(createPath))
                {
                    // Try to create the directory.
                    DirectoryInfo dir = Directory.CreateDirectory(createPath);
                }

                StreamWriter file = File.CreateText(createPath + TransactionName + ".sql");
                foreach (var line in _SqlOut)
                {
                    file.WriteLine(line);
                    SQLTotal += line + Environment.NewLine;
                }

                // Close the StreamWriter to flush all the data to the file!
                file.Close();
            }
            catch (Exception)
            {

                throw;
            }

            BLOCKS.SQLBlock = SQLTotal;

            return BLOCKS;
        }

        private static void ExecutionBlock(sqlTransBlocks BLOCKS, List<SQLContent> _theseSQLContents, ArrayList _SqlOut, List<ParameterSet> AllOutParams, string _Schema)
        {
            for (int i = 0; i < _theseSQLContents.Count(); i++)
            {
                _SqlOut.Add("");
                _SqlOut.Add("        EXEC [" + _Schema + "].[" + BLOCKS.Series[i].ProcedureName.ToUpper() + "]");
                for (int j = 0; j < _theseSQLContents[i].AllParams.Count(); j++)
                {
                    string valueAssignment = "";

                    string thisParam = _theseSQLContents[i].AllParams[j].ParamName;
                    thisParam = thisParam.StartsWith("@") ? thisParam : "@" + thisParam;
                    _theseSQLContents[i].AllParams[j].ParamName = thisParam;

                    List<ParameterSet> tempParamSet = AllOutParams.Where(x => x.ParamName.Substring(3, x.ParamName.Length - 3) == thisParam.Substring(3, thisParam.Length - 3)).ToList();

                    if (BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null
                        && BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString().Contains("@T_"))
                    {
                        valueAssignment = BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString();
                    }
                    else if (tempParamSet.Count > 0
                        && thisParam.Substring(3, thisParam.Length - 3) == tempParamSet.First().ParamName.Substring(3, tempParamSet.First().ParamName.Length - 3)
                        && Tools.Box.ConvertToInt64(BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue) != 0)
                    {
                        //string thisParam = _theseSQLContents[i].AllParams[j].ParamValue != null && _theseSQLContents[i].AllParams[j].ParamValue.Contains("@T_") ? _theseSQLContents[i].AllParams[j].ParamValue : tempParam;
                        valueAssignment = "@T_" + thisParam.Substring(3, thisParam.Length - 3) + " ";
                    }
                    else
                    {
                        switch (BLOCKS.Series[i].EntryProcedureParameters[j].MSSqlParamDataType)
                        {
                            case SqlDbType.BigInt:
                            case SqlDbType.Int:
                            case SqlDbType.SmallInt:
                                valueAssignment = BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null && BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() != String.Empty ? BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() : "NULL";
                                //valueAssignment = BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null && BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() != String.Empty ? BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() : null;
                                break;
                            case SqlDbType.VarBinary:

                                byte[] tempBytes = null;

                                var tempBinary = BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue;


                                switch (tempBinary.GetType().ToString())
                                {
                                    case "System.Byte[]":
                                        tempBytes = tempBinary != null ? (byte[])tempBinary : null;
                                        break;
                                    default:
                                        tempBytes = tempBinary != null && !string.IsNullOrWhiteSpace((string)tempBinary) ? DBTools._GetBytes(tempBinary.ToString()) : null;
                                        break;
                                }

                                if (tempBytes != null)
                                {
                                    //valueAssignment = Convert.ToBase64String(tempBytes);     
                                    valueAssignment = "0x" + BitConverter.ToString(tempBytes).Replace("-", "");
                                }
                                else
                                {
                                    //valueAssignment = "0x";
                                    valueAssignment = "NULL";
                                }
                                break;
                            case SqlDbType.Decimal:
                            case SqlDbType.Float:
                            case SqlDbType.Real:
                                valueAssignment = BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null && BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() != String.Empty ? BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() : "NULL";
                                //valueAssignment = BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null && BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() != String.Empty ? BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() : "";
                                break;
                            case SqlDbType.Date:
                            case SqlDbType.DateTime:
                            case SqlDbType.DateTime2:
                            case SqlDbType.SmallDateTime:
                                //valueAssignment = "N'" + (BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null ? BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() : "") + "'";
                                //valueAssignment = (BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null ? BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() : "NULL");
                                var thisDateParamValue = BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue;
                                if (thisDateParamValue != null && !string.IsNullOrWhiteSpace(thisDateParamValue.ToString()))
                                {

                                    string pattern = "yyyy-MM-dd";
                                    DateTime parsedDate;
                                    DateTime.TryParseExact(thisDateParamValue.ToString(), pattern, null, DateTimeStyles.None, out parsedDate);

                                    valueAssignment = (thisDateParamValue != null && !string.IsNullOrEmpty((string)thisDateParamValue) ? "N'" + (parsedDate).ToString(@"M-dd-yyyy hh:mm:ss tt") + "'" : "NULL");
                                }
                                else
                                {
                                    valueAssignment = "NULL";
                                }

                                break;
                            case SqlDbType.UniqueIdentifier:
                                var thisUUIDParamValue = BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue;
                                valueAssignment = (thisUUIDParamValue != null && !string.IsNullOrEmpty(thisUUIDParamValue.ToString()) ? "'" + BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString().Replace("'", "''") + "'" : "NULL");
                                break;
                            default:
                                //--valueAssignment = "N'" + (BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null ? BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString() : "") + "'";
                                //valueAssignment = "N'" + (BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null ? BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString().Replace("'","''") : "") + "'";

                                valueAssignment = (BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null ? "N'" + BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString().Replace("'", "''") + "'" : "NULL");
                                break;
                        }
                    }

                    string End = _theseSQLContents[i].AllParams.Count() - 1 == j ? " " : ", ";



                    if (BLOCKS.Series[i].EntryProcedureParameters[j].ParamDirection == ParameterDirection.Output)
                    {
                        _SqlOut.Add("            " + thisParam + " = " + valueAssignment + " OUTPUT" + End);
                    }
                    else
                    {
                        _SqlOut.Add("            " + thisParam + " = " + valueAssignment + End);
                    }
                }

                bool addPrefix = false;

                string totalSelect = "";
                for (int j = 0; j < BLOCKS.Series[i].EntryProcedureParameters.Count(); j++)
                {

                    string End = _theseSQLContents[i].AllParams.Count() - 1 == j ? " " : ", ";
                    if (addPrefix == false)
                    {
                        addPrefix = true;
                        totalSelect = "        select '" + i + "' this_interval, ";
                    }
                    //if (BLOCKS.Series[i].EntryProcedureParameters[j].ParamDirection == ParameterDirection.Input)
                    //{
                    string thisParamHere = _theseSQLContents[i].AllParams[j].ParamName;
                    string thisValueHere = BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue != null ? BLOCKS.Series[i].EntryProcedureParameters[j].ParamValue.ToString().Replace("'", "''") : "";
                    string prefix = thisParamHere.Contains("@O_") ? "T_" : "I_";
                    if (prefix.Contains("I_"))
                    {
                        totalSelect = totalSelect + "'" + thisValueHere + "' as  [I_" + thisParamHere.Substring(3, thisParamHere.Length - 3) + "] " + End;
                    }
                    else
                    {
                        totalSelect = totalSelect + "@" + prefix + thisParamHere.Substring(3, thisParamHere.Length - 3) + " as  [T_" + thisParamHere.Substring(3, thisParamHere.Length - 3) + "] " + End;
                    }
                    // }
                    //if (BLOCKS.Series[i].EntryProcedureParameters[j].ParamDirection == ParameterDirection.Output)
                    //{
                    //    string thisParamHere = _theseSQLContents[i].AllParams[j].ParamName;
                    //    totalSelect = totalSelect + "@T_" + thisParamHere.Substring(3, thisParamHere.Length - 3) + " as  [T_" + thisParamHere.Substring(3, thisParamHere.Length - 3) + "] " + End;
                    //}                    
                }

                _SqlOut.Add("");
                _SqlOut.Add(totalSelect);

                if (true)
                {
                    _SqlOut.Add("");
                }
            }
        }

        private static void DeclarationBlock(List<SQLContent> _theseSQLContents, ArrayList _SqlOut, List<ParameterSet> AllOutParams)
        {
            _SqlOut.Add("");
            for (int i = 0; i < _theseSQLContents.Count(); i++)
            {
                for (int j = 0; j < _theseSQLContents[i].OutParams.Count(); j++)
                {
                    string thisParam = _theseSQLContents[i].OutParams[j].ParamName.ToUpper();
                    thisParam = thisParam.StartsWith("@") ? thisParam : "@" + thisParam;
                    _theseSQLContents[i].OutParams[j].ParamName = thisParam;

                    List<ParameterSet> tempParamSet = AllOutParams.Where(x => x.ParamName.Substring(3, x.ParamName.Length - 3).ToUpper() == thisParam.Substring(3, thisParam.Length - 3).ToUpper()).ToList();

                    if (tempParamSet.Count == 0)
                    {
                        switch (_theseSQLContents[i].OutParams[j].ParamType.ToString().ToLower())
                        {
                            case "varchar":
                                _SqlOut.Add("        DECLARE @T_" + thisParam.Substring(3, thisParam.Length - 3).ToUpper() + " " + _theseSQLContents[i].OutParams[j].ParamType.ToString() + "(MAX) ");
                                break;
                            case "uniqueidentifier":
                                _SqlOut.Add("        DECLARE @T_" + thisParam.Substring(3, thisParam.Length - 3).ToUpper() + " " + _theseSQLContents[i].OutParams[j].ParamType.ToString() + " ");
                                break;
                            default:
                                _SqlOut.Add("        DECLARE @T_" + thisParam.Substring(3, thisParam.Length - 3).ToUpper() + " " + _theseSQLContents[i].OutParams[j].ParamType.ToString() + " ");
                                break;
                        }
                    }

                    AllOutParams.Add(_theseSQLContents[i].OutParams[j]);
                }
            }
        }

        public SQLContent GetSQLFromCommand(SQLContent thisContent)
        {


            //string ProcedureName = thisContent.ProcedureName;
            //List<string> Params = new List<string>();

            //DynamicSQLContent returnContent = new DynamicSQLContent();

            SQLContent _this = new SQLContent();
            List<ParameterSet> AllParams = new List<ParameterSet>();
            List<ParameterSet> OutParams = new List<ParameterSet>();

            //_SqlOut.AppendLine("EXEC " + ProcedureName);

            //GET OUTPARAMETERS FROM OUTBOUND VARIABLES

            //CALL ALL EXECUTABLES

            for (int i = 0; i < thisContent.Parameters.Count; i++)
            {
                //string valueAssignment = "";
                //switch(thisContent.Parameters[i].MSSqlParamDataType)
                //{
                //    case SqlDbType.BigInt:
                //    case SqlDbType.Int:
                //    case SqlDbType.SmallInt:
                //        valueAssignment = thisContent.Parameters[i].ParamValue.ToString();
                //        break;
                //    default:
                //        valueAssignment = "N'" + thisContent.Parameters[i].ParamValue + "'";
                //        break;
                //}

                //Params.Add(" @" + thisContent.Parameters[i].ParamName  + " = " + valueAssignment);

                string thisParam = thisContent.Parameters[i].ParamName;
                thisParam = thisParam.StartsWith("@") ? thisParam : "@" + thisParam;
                thisContent.Parameters[i].ParamName = thisParam;

                AllParams.Add(new ParameterSet
                {
                    ParamName = thisContent.Parameters[i].ParamName,
                    ParamType = thisContent.Parameters[i].MSSqlParamDataType.ToString()
                });

                if (thisContent.Parameters[i].ParamDirection == ParameterDirection.Output)
                {
                    OutParams.Add(new ParameterSet
                    {
                        ParamName = thisContent.Parameters[i].ParamName,
                        ParamType = thisContent.Parameters[i].MSSqlParamDataType.ToString()
                    });
                }
            }

            //for (int i = 0; i < Params.Count(); i++)
            //{		
            //    string End = Params.Count()-1 == i ? "": ", ";
            //    _SqlOut.AppendLine(Params[i] + End); 
            //}


            _this.AllParams = AllParams;
            _this.OutParams = OutParams;

            return _this;
        }

        public static string SQL_PROCEDURE_GET_VALUE(string parameter, DataTable Dataset1)
        {
            ER_Procedure er_proc = new ER_Procedure();

            return er_proc._SQL_PROCEDURE_GET_VALUE(parameter, Dataset1);
        }

        public string _SQL_PROCEDURE_GET_VALUE(string parameter, DataTable Dataset1)
        {
            ER_Env er_env = new ER_Env();

            string returnVar = "";

            if (Dataset1.Columns.Contains("ChildItem"))
            {
                DataTable tempDT = er_env.ProcDataTable();

                tempDT = Dataset1;

                foreach (DataRow i in tempDT.Rows)
                {
                    if (i["ChildItem"].ToString().ToLower() == parameter.ToLower())
                    {
                        returnVar = i["ChildValue"].ToString();
                        return returnVar;
                    }
                    else //error
                    {
                        returnVar = i["ChildValue"].ToString();
                    }
                }
            }
            return returnVar;
        }

        public static byte[] SQL_PROCEDURE_GET_BYTES(string parameter, DataTable Dataset1)
        {
            ER_Procedure er_proc = new ER_Procedure();

            return er_proc._SQL_PROCEDURE_GET_BYTES(parameter, Dataset1);
        }

        public byte[] _SQL_PROCEDURE_GET_BYTES(string parameter, DataTable Dataset1)
        {
            ER_Env er_env = new ER_Env();
            Tools.Box er_tools = new Tools.Box();

            byte[] returnVar = new byte[0];

            if (Dataset1.Columns.Contains("ChildItem"))
            {
                DataTable tempDT = er_env.ProcDataTable();

                tempDT = Dataset1;

                foreach (DataRow i in tempDT.Rows)
                {
                    if (i["ChildItem"].ToString().ToLower() == parameter.ToLower())
                    {
                        returnVar = er_tools.GetBytes((string)i["ChildValue"]);
                        break;
                    }
                    else //error
                    {
                        returnVar = er_tools.GetBytes("No column by that name found.");
                    }
                }
            }
            return returnVar;
        }

        public static DataTable VIRTUAL_PROCEDURE_CALL(IConnectToDB _Connect, VirtualProcedureCall _Procedure)
        {

            ER_Procedure er_proc = new ER_Procedure();

            return er_proc._VIRTUAL_PROCEDURE_CALL(_Connect, _Procedure);
        }

        public DataTable _VIRTUAL_PROCEDURE_CALL(IConnectToDB _Connect, VirtualProcedureCall _Procedure)
        {
            List<DBParameters> EntryProcedureParameters = VirtualCallCore(_Procedure);

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = _Procedure.ProcedureReturnType, ProcedureName = _Procedure.ProcedureName, _dbParameters = EntryProcedureParameters });

            return ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

        }

        public static BIG_CALL _VIRTUAL_TRANSACTION_CALL(IConnectToDB _Connect, List<VirtualProcedureCall> _Procedures)
        {
            ER_Procedure er_proc = new ER_Procedure();

            return er_proc.VIRTUAL_TRANSACTION_CALL(_Connect, _Procedures);
        }

        public BIG_CALL VIRTUAL_TRANSACTION_CALL(IConnectToDB _Connect, List<VirtualProcedureCall> _Procedures)
        {
            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            foreach (var _Procedure in _Procedures)
            {
                List<DBParameters> EntryProcedureParameters = VirtualCallCore(_Procedure);
                RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = _Procedure.ProcedureReturnType, ProcedureName = _Procedure.ProcedureName, _dbParameters = EntryProcedureParameters });
            }

            return RUN;
        }

        private static List<DBParameters> VirtualCallCore(VirtualProcedureCall _Procedure)
        {
            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            foreach (ProcedureParameterStruct _param in _Procedure.ProcedureParams)
            {

                DBParameters tempParm = new DBParameters
                {
                    ParamName = "@" + _param.ParamName,
                    ParamValue = _param.ParamValue
                };


                bool AddParamSize = false;

                switch (_param.MSSqlParamDataType.ToLower())
                {
                    case "char":
                    case "character":   //C# Char
                    case "characters":  //C# String
                        tempParm.MSSqlParamDataType = SqlDbType.Char;
                        AddParamSize = true; //Value 1 to 8000  
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : "";
                        break;
                    case "words":
                    case "varchar":
                    case "string": //C# String
                        tempParm.MSSqlParamDataType = SqlDbType.VarChar;
                        AddParamSize = true; //Value 1 to 8000/-1 for Max
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : "";
                        break;
                    case "date":  //C# Date
                        tempParm.MSSqlParamDataType = SqlDbType.Date;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : DBNull.Value;
                        break;
                    case "datetime":  //C# DateTime
                        tempParm.MSSqlParamDataType = SqlDbType.DateTime;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : DBNull.Value;
                        break;
                    case "datetime2":  //C# DateTime2
                        tempParm.MSSqlParamDataType = SqlDbType.DateTime2;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : DBNull.Value;
                        break;
                    case "datetimeoffset":  //C# DateTime
                        tempParm.MSSqlParamDataType = SqlDbType.DateTimeOffset;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : DBNull.Value;
                        break;
                    case "smalldatetime":  //C# DateTime
                        tempParm.MSSqlParamDataType = SqlDbType.SmallDateTime;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : DBNull.Value;
                        break;
                    case "int16":  //C# Int16
                        tempParm.MSSqlParamDataType = SqlDbType.SmallInt;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : 0;
                        break;
                    case "int32": //C# Int32
                    case "int":  //C# int
                        tempParm.MSSqlParamDataType = SqlDbType.Int;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : 0;
                        break;
                    case "int64":
                    case "bigint":  //C# Int64
                        tempParm.MSSqlParamDataType = SqlDbType.BigInt;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : 0;
                        break;
                    case "numeric":
                    case "decimal": //C# Decimal
                        tempParm.MSSqlParamDataType = SqlDbType.Decimal;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : 0;
                        break;
                    case "money": //C# Decimal
                        tempParm.MSSqlParamDataType = SqlDbType.Money;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : 0;
                        break;
                    case "smallmoney": //C# Decimal
                        tempParm.MSSqlParamDataType = SqlDbType.SmallMoney;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : 0;
                        break;
                    case "float":
                    case "double": //C# Double
                        tempParm.MSSqlParamDataType = SqlDbType.Float;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : 0;
                        AddParamSize = true; //Optional Value 1 to 53 
                        break;
                    case "image": //C# Byte[]   
                        //Data type will be removed in a future version of Microsoft SQL Server.
                        tempParm.MSSqlParamDataType = SqlDbType.Image;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : new byte[0];
                        break;
                    case "filestream": //C# Byte[]
                    case "varbinary": //C# Byte[]                   
                    case "byte[]":
                        tempParm.MSSqlParamDataType = SqlDbType.VarBinary;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : new byte[0];
                        AddParamSize = true; //Value 1 to 8000/-1 for Max
                        break;
                    case "binary": //C# Byte[]
                        tempParm.MSSqlParamDataType = SqlDbType.Binary;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : new byte[0];
                        AddParamSize = true; //Value 1 to 8000
                        break;
                    case "time": //C# TimeSpan
                        tempParm.MSSqlParamDataType = SqlDbType.Time;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : new TimeSpan(0);
                        break;
                    case "xml": //C# XML
                        tempParm.MSSqlParamDataType = SqlDbType.Xml;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : null;
                        //[ CONTENT | DOCUMENT ] xml_schema_collection
                        break;
                    case "nchar": //C# String or Char[]
                        tempParm.MSSqlParamDataType = SqlDbType.NChar;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : "";
                        break;
                    case "nvarchar": //C# String or Char[]
                        tempParm.MSSqlParamDataType = SqlDbType.NVarChar;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : "";
                        break;
                    case "ntext": //C# String or Char[] 
                        //Data type will be removed in a future version of Microsoft SQL Server.
                        tempParm.MSSqlParamDataType = SqlDbType.NText;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : "";
                        break;
                    case "text": //C# String or Char[]
                        //Data type will be removed in a future version of Microsoft SQL Server.
                        tempParm.MSSqlParamDataType = SqlDbType.Text;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : "";
                        break;
                    case "bit":
                    case "bool":
                    case "boolean": //C# Boolean or bool
                        tempParm.MSSqlParamDataType = SqlDbType.Bit;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : null;
                        break;
                    case "real":
                    case "single": //C# Single
                        tempParm.MSSqlParamDataType = SqlDbType.Real;
                        AddParamSize = true; //Optional Value 1 to 53 
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : 0;
                        break;
                    case "rowversion": //C# byte[] Only going out in C# no input.
                        tempParm.MSSqlParamDataType = SqlDbType.Timestamp;
                        break;
                    case "sqlvariant":
                    case "object": //C# object
                        tempParm.MSSqlParamDataType = SqlDbType.Variant;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : new object();
                        break;
                    case "guid": //C# object
                    case "uniqueidentifier":
                        tempParm.MSSqlParamDataType = SqlDbType.UniqueIdentifier;
                        tempParm.ParamValue = _param.ParamValue != null ? _param.ParamValue : new object();
                        break;
                    default:
                        break;
                }

                if (AddParamSize)
                {
                    if (_param.ParamSize == "-1" || _param.ParamSize.ToUpper() == "MAX")
                    {
                        tempParm.ParamSize = -1;
                    }
                    else
                    {
                        try
                        {
                            tempParm.ParamSize = Convert.ToInt32(_param.ParamSize);
                        }
                        catch
                        {
                            tempParm.ParamSize = -1;
                        }
                    }
                }

                switch (_param.ParamDirection.ToLower())
                {
                    case "in":
                        tempParm.ParamDirection = ParameterDirection.Input;
                        break;
                    case "input":
                        tempParm.ParamDirection = ParameterDirection.Input;
                        break;
                    case "out":
                        tempParm.ParamDirection = ParameterDirection.Output;
                        break;
                    case "output":
                        tempParm.ParamDirection = ParameterDirection.Output;
                        break;
                    case "inputoutput":
                        tempParm.ParamDirection = ParameterDirection.InputOutput;
                        break;
                    case "returnvalue":
                        tempParm.ParamDirection = ParameterDirection.ReturnValue;
                        break;
                }

                EntryProcedureParameters.Add(tempParm);
            }

            return EntryProcedureParameters;
        }


    }
}
