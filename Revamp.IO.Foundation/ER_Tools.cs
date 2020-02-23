using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Data;
using System.Web;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Enums;
using System.Configuration;
using Revamp.IO.Structs.Models;
using Revamp.IO.Structs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using TextFieldParserCore;
using Microsoft.Extensions.Options;

namespace Revamp.IO.Foundation
{
    public class ER_Tools
    {

        // START TOOL FORMATTING
        //        
        private RevampCoreSettings RevampCoreSettings { get; set; }
        public ER_Tools(IOptions<RevampCoreSettings> settings)
        {
            RevampCoreSettings = settings.Value;
        }


        public string MaxNameLength(string name, int maxLength)
        {
            Tools.Box ddl = new Tools.Box();

            return ddl.MaxNameLength(name, maxLength);
        }

        public string CreateFilePath(string path1)
        {
            return path1.Replace(@"\", @"\\");
        }

        //START TOOL UNIVERSAL 
        //

        // START DB SELECTS FROM DICTIONARY VIEWS
        //

        

        private static Random random = new Random((int)DateTime.Now.Ticks);

        //private static Random random2 = new Random();
        public static string RandomAlphaString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public int RandomNumber(int lowRange, int highRange)
        {

            Random r = new Random();
            int rInt = r.Next(lowRange, highRange); //for ints

            return rInt;
        }

        public string RandomNameGenerator(string NameType, string subType)
        {
            List<string> stringsList = new List<string>();
            int ListLength = 0;

            switch (NameType.ToLower())
            {
                case "person":
                    switch (subType.ToLower())
                    {
                        case "first name":
                            stringsList.Add("Abraham");
                            stringsList.Add("Adam");
                            stringsList.Add("Josephine");
                            stringsList.Add("Joseph");
                            stringsList.Add("Victor");
                            stringsList.Add("Phillip");
                            stringsList.Add("Benjamin");
                            stringsList.Add("Jose");
                            stringsList.Add("Eyaluta");
                            stringsList.Add("Isaac");
                            stringsList.Add("Jesse");
                            stringsList.Add("Crystal");
                            stringsList.Add("Christie");
                            stringsList.Add("Edwin");
                            stringsList.Add("Eully");
                            stringsList.Add("Valentina");
                            stringsList.Add("Francesco");
                            break;
                        case "middle name":
                            stringsList.Add("Antonio");
                            stringsList.Add("Adam");
                            stringsList.Add("Lee");
                            stringsList.Add("Joseph");
                            stringsList.Add("Kody");
                            stringsList.Add("Phillip");
                            stringsList.Add("Benjamin");
                            stringsList.Add("Marcus");
                            break;
                        case "last name":
                            stringsList.Add("Risi");
                            stringsList.Add("Adams");
                            stringsList.Add("Lions");
                            stringsList.Add("Barnes");
                            stringsList.Add("Vick");
                            stringsList.Add("Rossburg");
                            stringsList.Add("Billups");
                            stringsList.Add("Fried");
                            break;
                    }
                    break;
                case "business":
                    switch (subType.ToLower())
                    {
                        case "company name":
                            stringsList.Add("Eminent IT");
                            stringsList.Add("Google");
                            stringsList.Add("Celtic Tech");
                            stringsList.Add("Microsoft");
                            stringsList.Add("McRonalds");
                            break;
                        case "industry":
                            stringsList.Add("Consulting");
                            stringsList.Add("Food");
                            stringsList.Add("Search Engine");
                            stringsList.Add("Gaming");
                            stringsList.Add("Contracting");
                            break;
                        case "year founded":
                            stringsList.Add("2000");
                            stringsList.Add("2012");
                            stringsList.Add("2009");
                            stringsList.Add("1975");
                            stringsList.Add("1983");
                            break;
                    }
                    break;
                case "platoon":
                    switch (subType.ToLower())
                    {
                        case "platoon name":
                            stringsList.Add("1090");
                            stringsList.Add("1091");
                            stringsList.Add("1092");
                            stringsList.Add("1690");
                            stringsList.Add("1893");
                            break;
                        case "company name":
                            stringsList.Add("Alpha");
                            stringsList.Add("Bravo");
                            stringsList.Add("Charlie");
                            stringsList.Add("Delta");
                            stringsList.Add("Echo");
                            break;
                        case "year founded":
                            stringsList.Add("2000");
                            stringsList.Add("2012");
                            stringsList.Add("2009");
                            stringsList.Add("1975");
                            stringsList.Add("1983");
                            break;
                    }
                    break;
                default:
                    break;
            }


            ListLength = stringsList.Count() - 1;
            return stringsList[RandomNumber(0, stringsList.Count())].ToString();
        }

        private Boolean ArethereErrors(string DB_Platform, List<string> Arraylist, string ErrorClue)
        {
            Boolean DoesThisHaveErrors = false;
            string tempstring = "";

            foreach (string i in Arraylist)
            {
                tempstring = i.ToString();
                if (i.ToString().Contains(ErrorClue))
                {
                    DoesThisHaveErrors = true;
                    break;
                }


            }

            return DoesThisHaveErrors;
        }

        private string ConvertFiletoByteString(string pathToFile)
        {
            var bytes = File.ReadAllBytes(pathToFile);

            return bytes.ToString();
        }

        public byte[] ConvertFiletoByte(string pathToFile)
        {
            byte[] bytes = File.ReadAllBytes(pathToFile);

            return bytes;
        }




        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes").ToString();
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }

        public string ProperCase(string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text.ToLower(), @"\b[a-z]", m => m.Value.ToUpper());


        }

        public Boolean CheckForString(List<string> Logger, string _Contains)
        {
            foreach (string x in Logger)
            {
                if (x.Contains(_Contains))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetSymbolbyID(IConnectToDB _Connect, string SymbolName)
        {
            string sqlstatement = "Select * from CSA.SYMBOLS where LOWER(SYMBOL_NAME) = @SYMBOL_NAME";
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sqlstatement,
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "SYMBOL_NAME", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = SymbolName.ToLower() } }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            if (_DT.Rows.Count > 0 && DCC.Contains("SYMBOLS_ID"))
            {
                return _DT.Rows[0]["SYMBOLS_ID"].ToString();
            }
            else
            {
                return "1000";
            }
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
                ER_Tools._WriteEventLog(string.Format("Caught exception: {0} \r\n Stack Trace: {1}", ex.Message, ex.StackTrace), EventLogType.exception);
                throw;
            }
        }

        public static byte[] _GetBytes(string str)
        {
            ER_Tools tools = new ER_Tools();

            return tools.GetBytes(str);
        }

        public string GetString(byte[] bytes)
        {
            try
            {
                int arraylength = bytes.Length % 2 != 0 ? bytes.Length + 1 : bytes.Length;

                char[] chars = new char[arraylength / sizeof(char)];
                System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
                return new string(chars);
            }
            catch (Exception ex)
            {
                ER_Tools._WriteEventLog(string.Format("Caught exception: {0} \r\n Stack Trace: {1}", ex.Message, ex.StackTrace), EventLogType.error);
                return "Error getting string bytes. " + ex.ToString(); ;
            }
        }

        public static string ConvertObjectToString(byte[] bytes)
        {
            ER_Tools tools = new ER_Tools();

            return tools.GetString(bytes);
        }

        public string getStringFromBytes(byte[] bytes)
        {
            char[] chars = new char[0];
            chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);

            return new string(chars);
        }

        public dynamic BytesToDynamicObject(dynamic ListType, byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (Stream ms = new MemoryStream(bytes))
            {
                ListType = (dynamic)bf.Deserialize(ms);
            }

            return ListType;
        }

        public Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }

        

        #region Event Writing
        public Boolean WriteEventLog(string message, EventLogType eventlogtype)
        {
            Boolean _Result = false;
            bool enableEventLogging = RevampCoreSettings.EnableEventLogging;

            try
            {

                if (!enableEventLogging)
                {
                    return false;
                }

                if (!EventLog.SourceExists("IRIS"))
                {
                    EventLog.CreateEventSource("IRIS", "Application");
                }

                EventLogEntryType eventType = new EventLogEntryType();

                switch (eventlogtype.ToString().ToLower())
                {
                    case "error":
                    case "exception":
                        eventType = EventLogEntryType.Error;
                        break;
                    case "information":
                    case "general":
                        eventType = EventLogEntryType.Information;
                        break;
                    case "failureaudit":
                        eventType = EventLogEntryType.FailureAudit;
                        break;
                    case "successaudit":
                    case "success":
                        eventType = EventLogEntryType.SuccessAudit;
                        break;
                    case "warning":
                    case "default":
                        eventType = EventLogEntryType.Warning;
                        break;
                    default:
                        eventType = EventLogEntryType.Warning;
                        break;
                }

                EventLog.WriteEntry("IRIS", message, eventType, 12839);

                _Result = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                _Result = false;
                //Can't write to Eventlog
            }

            return _Result;
        }

        public static Boolean _WriteEventLog(string message, EventLogType eventlogtype)
        {
            ER_Tools _tools = new ER_Tools();

            return _tools.WriteEventLog(message, eventlogtype);
        }

        #endregion

        public List<string> DelimitedStringToList(string _text, string delimiter)
        {
            char[] splitdelimiter = { Convert.ToChar(delimiter) };

            List<string> list = new List<string>();

            foreach (string s in _text.Split(splitdelimiter))
            {
                list.Add(s);
            }

            return list;
        }

        public List<object> GetListFromDataTable(DataTable DT)
        {
            List<object> list = new List<object>();

            foreach (DataRow _Row in DT.Rows)
            {
                List<string> myAL = new List<string>();

                foreach (DataColumn _DC in DT.Columns)
                {
                    myAL.Add(_Row[_DC].ToString());
                }

                list.Add(myAL);
            }

            return list;
        }

        public List<string> ConvertDataTableToArrayList(DataTable dtTable)
        {
            List<string> myArrayList = new List<string>();
            for (int i = 0; i <= dtTable.Rows.Count - 1; i++)
            {
                List<string> myArrayList2 = new List<string>();
                for (int j = 0; j <= dtTable.Columns.Count - 1; j++)
                {
                    myArrayList2.Add(dtTable.Rows[i][j].ToString());
                }
                myArrayList.Add(myArrayList2.ToString());
            }

            return myArrayList;
        }

        public static List<string> _ConvertDataTableToArrayList(DataTable dtTable)
        {
            ER_Tools _tools = new ER_Tools();

            return _tools.ConvertDataTableToArrayList(dtTable);
        }

        public static DataTable ConvertCSVtoDataTable(byte[] csv_File, string[] delimiter)
        {
            ER_Tools _tools = new ER_Tools();

            //Stream stream = File.OpenRead("C:\\Users\\jose\\Source\\Repos\\revamp\\ER\\Files\\Test.csv");

            Stream stream = new MemoryStream(csv_File);

            DataTable table = new DataTable("ReturnData");
            DataColumn column;

            try
            {
                //using (TextFieldParser parser = new TextFieldParser("C:\\Users\\jose\\Source\\Repos\\revamp\\ER\\Files\\Test.csv"))
                //TODO: Verify .Net Core Port
                using (TextFieldParser parser = new TextFieldParser(stream))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(delimiter);

                    int count = 0;
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        string[] fields = parser.ReadFields();
                        int fieldsCount = fields.Length;
                        if (count == 0)
                        {
                            // Create first column and add to the DataTable.
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.Int32");
                            column.ColumnName = "CVS_ID";
                            column.AutoIncrement = true;
                            column.AutoIncrementSeed = 0;
                            column.AutoIncrementStep = 1;
                            column.Caption = "ID";
                            column.ReadOnly = true;
                            column.Unique = true;
                            table.Columns.Add(column);
                            count++;


                            for (int i = 0; i < fieldsCount; i++)
                            {
                                column = new DataColumn();
                                column.DataType = System.Type.GetType("System.String");
                                column.ColumnName = fields[i];
                                column.AutoIncrement = false;
                                column.Caption = fields[i];
                                column.ReadOnly = false;
                                column.Unique = false;
                                table.Columns.Add(column);
                                //TODO: Process field
                            }
                        }

                        DataRow _row = table.NewRow();

                        for (int i = 0; i < fieldsCount; i++)
                        {
                            _row[i + 1] = fields[i];
                            //TODO: Process field
                        }

                        table.Rows.Add(_row);
                    }
                }
            }
            catch (Exception e)
            {
                table = new DataTable("Error");
                // Create fourth column and add to the DataTable.
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "Exception";
                column.AutoIncrement = false;
                column.Caption = "Exception";
                column.ReadOnly = false;
                column.Unique = false;
                table.Columns.Add(column);


                DataRow row = table.NewRow();
                row["Exception"] = e;
                table.Rows.Add(row);
            }
            //stream = File.OpenRead("C:\\Users\\jose\\Source\\Repos\\revamp\\ER\\Files\\Test.csv");

            //using (StreamReader sr = new StreamReader("C:\\Users\\jose\\Source\\Repos\\revamp\\ER\\Files\\Test.csv"))
            //using (StreamReader sr = new StreamReader(stream))
            //{
            //    string currentLine;
            //    // currentLine will be null when the StreamReader reaches the end of file
            //    while ((currentLine = sr.ReadLine()) != null)
            //    {
            //        // Search, case insensitive, if the currentLine contains the searched keyword
            //        StringReader csv_reader = new StringReader(currentLine.ToString());

            //        TextFieldParser csv_parser = new TextFieldParser(csv_reader);
            //        csv_parser.SetDelimiters(delimiter);  
            //        csv_parser.HasFieldsEnclosedInQuotes = true;
            //        string[] csv_array = csv_parser.ReadFields();

            //    }
            //}

            return table;
        }

        public List<object> GetObjectListFromDataTable(List<object> list, DataTable DT)
        {
            Int64 numRows = DT.Rows.Count;
            int current;

            for (current = 0; current < numRows; current++)
            {
                Dictionary<string, string> myDict = new Dictionary<string, string>();

                Int64 numCols = DT.Columns.Count;

                for (int currentCol = 0; currentCol < numCols; currentCol++)
                {
                    myDict.Add(DT.Columns[currentCol].ToString().ToLower(), DT.Rows[current][DT.Columns[currentCol]].ToString());
                }

                list.Add(myDict);
            }

            return list;
        }

        public static List<object> _GetObjectListFromDataTable(List<object> list, DataTable DT)
        {
            ER_Tools _tools = new ER_Tools();

            return _tools.GetObjectListFromDataTable(list, DT);
        }

        public ArrayList GetObjectListFromDataTable(ArrayList list, DataTable DT)
        {
            Int64 numRows = DT.Rows.Count;

            //foreach (DataRow _Row in DT.Rows)
            for (int current = 0; current < numRows; current++)
            {
                Dictionary<string, object> myDict = new Dictionary<string, object>();

                Int64 numCols = DT.Columns.Count;
                //foreach (DataColumn _DC in DT.Columns)
                for (int currentCol = 0; currentCol < numCols; currentCol++)
                {
                    var tempValue = DT.Rows[current][DT.Columns[currentCol]];
                    var finalValue = new object();
                    switch (tempValue.GetType().ToString())
                    {
                        case "System.Byte[]":
                            finalValue = tempValue != null ? ConvertObjectToString(((byte[])tempValue)) : null;
                            break;
                        default:
                            finalValue = tempValue.ToString();
                            break;
                    }

                    myDict.Add(DT.Columns[currentCol].ToString().ToLower(), finalValue);
                }

                list.Add(myDict);
            }

            return list;
        }
        public static ArrayList _GetObjectListFromDataTable(ArrayList list, DataTable DT)
        {
            ER_Tools _tools = new ER_Tools();

            return _tools.GetObjectListFromDataTable(list, DT);
        }
        public static long ConvertToInt64(object AttemptedValue)
        {
            try
            {
                long temp;
                if (long.TryParse(AttemptedValue.ToString(), out temp))
                {
                    return temp;
                }
                else
                {
                    var ok = long.MinValue;
                    return ok;
                }
            }
            catch (Exception)
            {

                return long.MinValue;
            }
        }

        public static Int32 ConvertToInt32(string AttemptedValue)
        {
            Int32 temp;
            if (Int32.TryParse(AttemptedValue, out temp))
            {
                return temp;
            }
            else
            {
                return Int32.MinValue;
            }
        }

        public static Guid? ConvertToGuid(string AttemptedValue)
        {
            Guid temp;
            if (Guid.TryParse(AttemptedValue, out temp))
            {
                return temp;
            }
            else
            {
                return null;
            }
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

        public static List<CommandResult> CREATE_OBJECT_FROM_FILE(IConnectToDB _NewConnect, sqlCreateObject thisModel, List<CommandResult> _Results)
        {
            string _objectName = thisModel.objectName;
            string _Schema = _NewConnect.Schema;
            string SQLFilePath = thisModel.SqlFilePath;
            string ServerPath = SQLFilePath;

            ER_DB _dbio = new ER_DB();

            StringBuilder _sqlIn = new StringBuilder();
            CommandResult _Result = new CommandResult();

            if (!thisModel.registerObjectToDictionary || !ER_Tools.checkIfTableExist(_NewConnect, _NewConnect.Schema, _objectName))
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


        public static CommandResult RUN_SQL_FILE(IConnectToDB _NewConnect, string ServerPath, string ResultMessage, StringBuilder _sqlIn)
        {
            _sqlIn = Tools.Box.convertStringArray(System.IO.File.ReadAllLines(ServerPath), _sqlIn);

            CommandResult _Result = new CommandResult();

            _Result._StartTime = DateTime.Now;
            _Result._Response = ER_Query._RUN_NON_QUERY(_NewConnect, _sqlIn.ToString(), ResultMessage);
            _Result._Successful = _Result._Response.IndexOf("Success") != -1 ? true : false;
            _Result._EndTime = DateTime.Now;

            return _Result;
        }


        public static string ConvertSerializabaleObjectToString(object ThisObject)
        {
            var json = JsonConvert.SerializeObject(ThisObject);

            return json;
        }

        //public static string GetPathLocation(string SQLFilePath, string appSettingName = "sqlFileTarget")
        //{
           
        //    bool sqlFileTarget = ConfigurationManager.AppSettings.Get(appSettingName) != null ? Convert.ToBoolean(ConfigurationManager.AppSettings.Get(appSettingName)) : false;

        //    string RootPath = sqlFileTarget ? "" : string.Join("\\", System.AppDomain.CurrentDomain.BaseDirectory.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Reverse().Skip(1).Reverse().ToArray()) + "\\";
            
        //    //TODO: Verify .Net Core Port
        //    string ServerPath = sqlFileTarget ? System.IO.Directory.GetCurrentDirectory() + SQLFilePath : RootPath + SQLFilePath;
        //    return ServerPath;
        //}

        public static bool ConvertCheckBoxtoBool(string value)
        {
            bool val = false;
            if (!string.IsNullOrWhiteSpace(value) && value.ToLower().Contains("on"))
            { val = true; }

            return val;
        }

        public static class ObjectCopier
        {
            /// <summary>
            /// Perform a deep Copy of the object.
            /// </summary>
            /// <typeparam name="T">The type of object being copied.</typeparam>
            /// <param name="source">The object instance to copy.</param>
            /// <returns>The copied object.</returns>
            public static T Clone<T>(T source)
            {
                if (!typeof(T).IsSerializable)
                {
                    throw new ArgumentException("The type must be serializable.", "source");
                }

                // Don't serialize a null object, simply return the default for that object
                if (Object.ReferenceEquals(source, null))
                {
                    return default(T);
                }

                IFormatter formatter = new BinaryFormatter();
                Stream stream = new MemoryStream();
                using (stream)
                {
                    formatter.Serialize(stream, source);
                    stream.Seek(0, SeekOrigin.Begin);
                    return (T)formatter.Deserialize(stream);
                }
            }
        }
    }
}
