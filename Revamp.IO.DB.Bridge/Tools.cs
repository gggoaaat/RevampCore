using Revamp.IO.Structs;
using Revamp.IO.Structs.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.DB.Bridge
{
    public class DBTools
    {
        public string ExtractString(string s, string tag, string tag2)
        {
            // You should check for errors in real-world code, omitted for brevity
            try
            {
                var startTag = tag;
                int startIndex = s.IndexOf(startTag) + startTag.Length;
                var startTag2 = tag2;
                int startIndex2 = s.IndexOf(startTag2) - s.IndexOf(startTag);

                string temp = s.Substring(startIndex, startIndex2);
                return s.Substring(startIndex, startIndex2);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string GET_DATATYPE(IConnectToDB _Connect, string column)
        {
            string tempColumn = column.ToLower(); //ExtractString(column, "", "(");

            if (column.ToLower().Contains("characters") || column.ToLower().Contains("varchar"))
                tempColumn = ExtractString(column, "", "(");

            switch (_Connect.Platform)
            {
                //case "Oracle":
                //case "ORACLE":

                //    switch (tempColumn.ToLower())
                //    {
                //        case "characters":
                //        case "varchar2":
                //        case "varchar":
                //            return "varchar2(" + ExtractString(column, "(", ")");
                //        case "numbers":
                //        case "number":
                //        case "integer":
                //        case "numbs":
                //        case "int":
                //        case "ints":
                //            return "number";
                //        case "dates":
                //        case "date":
                //            return "date";
                //        case "times":
                //        case "timestamp":
                //            return "timestamp";
                //        case "file":
                //        case "blob":
                //        case "raw":
                //            return "blob";
                //        default:
                //            return "Invalid Column Type";
                //    }

                case "Microsoft":
                case "MICROSOFT":

                    switch (tempColumn.ToLower())
                    {
                        case "characters":
                        case "varchar2":
                        case "varchar":
                            return "varchar(" + ExtractString(column, "(", ")");
                        case "numbers":
                        case "number":
                        case "integer":
                        case "numbs":
                        case "int":
                        case "ints":
                        case "int32":
                            return "integer";
                        case "big":
                        case "bigint":
                        case "int64":
                        case "bignumbers":
                            return "bigint";
                        case "decimal":
                            return "decimal(" + ExtractString(column, "(", ")");
                        case "money":
                            return "decimal(20,2)"; //Up to a Trillions
                        case "dates":
                        case "date":
                        case "datetime":
                        case "datetime2":
                            return "datetime2";
                        case "times":
                        case "time":
                            return "time(7)";                       
                        case "timestamp":
                            return "timestamp";
                        case "file":
                        case "blob":
                        case "raw":
                        case "varbinary":
                            return "VARBINARY(MAX)";
                        case "guid":
                        case "uniqueidentifier":
                            return "uniqueidentifier";
                        default:
                            return "Invalid Column Type";
                    }

                default:
                    return "Invalid DB Platform";
            }
        }



        public static string GetSchema(IConnectToDB _Connect)
        {
            string _Schema = (_Connect.Schema == "" || _Connect.Schema == null ? (_Connect.SourceDBOwner == "" || _Connect.SourceDBOwner == null ? _Connect.RevampSystemName : _Connect.SourceDBOwner) : _Connect.Schema);
            return _Schema;
        }
        public static List<CommandResult> CREATE_OBJECT_FROM_FILE(IConnectToDB _NewConnect, sqlCreateObject thisModel, List<CommandResult> _Results)
        {
            string _objectName = thisModel.objectName;
            string _Schema = _NewConnect.Schema;
            string SQLFilePath = thisModel.SqlFilePath;
            string ServerPath = SQLFilePath;

            StringBuilder _sqlIn = new StringBuilder();
            CommandResult _Result = new CommandResult();

            if (!thisModel.registerObjectToDictionary || !checkIfTableExist(_NewConnect, _NewConnect.Schema, _objectName))
            {
                _sqlIn = new StringBuilder();

                _Result = RUN_SQL_FILE(_NewConnect, ServerPath, "Success - " + _objectName + " SQL Ran.", _sqlIn);
                _Results.Add(_Result);

                //TODO: FIx this to make insert into DB. Might need type of object as well.
                //_CSAInputProcedures CSA_IPH = new _CSAInputProcedures();

                //if (thisModel.registerObjectToDictionary)
                //{
                //    VDbModel this_InstallModel = CSA_IPH.InitVApp(_Schema, thisModel.DBOBJTypeID, _objectName, "Y", "N", thisModel._selectedTemplateID);
                //    _Results.AddRange(CSA_IPH.InsertInstallation(_NewConnect, this_InstallModel));
                //}
            }
            else
            {
                _Result = new CommandResult();
                _Result._StartTime = DateTime.Now;
                _Result._Response = "Warning - " + _objectName + " Object already exists.";
                _Result._Successful = _Result._Response.IndexOf("Warning") != -1 ? true : false;
                _Result._EndTime = DateTime.Now;
                _Results.Add(_Result);
            }

            return _Results;
        }

        public static bool checkIfTableExist(IConnectToDB _Connect, string schema, string tablename)
        {
            string _sqlIn = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @TABLE_SCHEMA AND  TABLE_NAME = @TABLE_NAME";
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = _sqlIn,
                _dbParameters = new List<DBParameters> {
                    new DBParameters { ParamName = "TABLE_SCHEMA", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = schema },
                    new DBParameters { ParamName = "TABLE_NAME", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = tablename }
                }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            return _DT.Rows.Count > 0 && DCC.Contains("TABLE_SCHEMA") ? true : false;
        }

        public static CommandResult RUN_SQL_FILE(IConnectToDB _NewConnect, string ServerPath, string ResultMessage, StringBuilder _sqlIn)
        {
            _sqlIn = convertStringArray(System.IO.File.ReadAllLines(ServerPath), _sqlIn);

            CommandResult _Result = new CommandResult();

            _Result._StartTime = DateTime.Now;
            _Result._Response = ER_Query._RUN_NON_QUERY(_NewConnect, _sqlIn.ToString(), ResultMessage);
            _Result._Successful = _Result._Response.IndexOf("Success") != -1 ? true : false;
            _Result._EndTime = DateTime.Now;

            return _Result;
        }

        public static StringBuilder convertStringArray(string[] thisArray, StringBuilder thisStringBuilder)
        {
            foreach (string item in thisArray)
            {
                thisStringBuilder.AppendLine(item);
            }

            return thisStringBuilder;
        }

        public byte[] GetBytes(string str)
        {
            try
            {
                byte[] bytes = new byte[str.Length * sizeof(char)];
                System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
                return bytes;
            }
            catch (Exception ex)
            {              
                throw;
            }
        }

        public static byte[] _GetBytes(string str)
        {
            DBTools tools = new DBTools();

            return tools.GetBytes(str);
        }
    }
}
