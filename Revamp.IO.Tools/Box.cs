using Revamp.IO.Structs.Enums;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Revamp.IO.Tools
{
    public class Box
    {
        public static FolderStatus CreateDir(string Path)
        {
            FolderStatus tempStatus = FolderStatus.Failed;

            bool exists = System.IO.Directory.Exists(Path);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(Path);
                tempStatus = System.IO.Directory.Exists(Path) ? FolderStatus.Success : FolderStatus.Failed;
            }
            else
            {
                tempStatus = FolderStatus.Exists;
            }

            return tempStatus;
        }

        public Boolean WriteEventLog(string message, EventLogType eventlogtype)
        {
            Boolean _Result = false;
            bool enableEventLogging = true;

            try
            {
                if (ConfigurationManager.AppSettings["EnableEventLogging"] != null)
                {
                    bool.TryParse(ConfigurationManager.AppSettings["EnableEventLogging"].ToString(), out enableEventLogging);
                }

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
            Box _tools = new Box();

            return _tools.WriteEventLog(message, eventlogtype);
        }

        public static System.Text.StringBuilder convertStringArray(string[] thisArray, System.Text.StringBuilder thisStringBuilder)
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
                // ER_Tools._WriteEventLog(string.Format("Caught exception: {0} \r\n Stack Trace: {1}", ex.Message, ex.StackTrace), EventLogType.exception);
                throw;
            }
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

        public string MaxNameLength(string name, int maxLength)
        {
            string returnString = name;

            if (name.Length > maxLength)
                returnString = name.Substring(0, maxLength);

            return returnString;

            // END TOOL FORMATTING
            //
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool IsBase64String(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;

            s = s.Trim();
            return (s.Length % 4 == 0) && System.Text.RegularExpressions.Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", System.Text.RegularExpressions.RegexOptions.None);

        }

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

        public byte[] ListToBytes(object AnyObject)
        {
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, AnyObject);

            //This gives you the byte array.
            return mStream.ToArray();
        }

        public static byte[] ObjectToByteArray(object AnyObject)
        {
            try
            {
                var binFormatter = new BinaryFormatter();
                var mStream = new MemoryStream();
                binFormatter.Serialize(mStream, AnyObject);

                //This gives you the byte array.
                return mStream.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
    }
}
