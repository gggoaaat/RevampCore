using Microsoft.Data.SqlClient;
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
    public static class ToolsEXT
    {
        public static Boolean ToBooleanOrDefault(this String s, Boolean Default)
        {
            return ToBooleanOrDefault((Object)s, Default);
        }

        public static Boolean ToBooleanOrDefault(this Object o, Boolean Default)
        {
            Boolean ReturnVal = Default;
            try
            {
                if (o != null)
                {
                    switch (o.ToString().ToLower())
                    {
                        case "yes":
                        case "true":
                        case "ok":
                        case "y":
                            ReturnVal = true;
                            break;
                        case "no":
                        case "false":
                        case "n":
                            ReturnVal = false;
                            break;
                        default:
                            ReturnVal = Boolean.Parse(o.ToString());
                            break;
                    }
                }
            }
            catch
            {
            }
            return ReturnVal;
        }

        /// <summary>
        /// Removes all non-alphanumeric characters from a string except for periods and underscores
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static String ParameterValueForSQL(this SqlParameter sp)
        {
            String retval = "";

            switch (sp.SqlDbType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.Time:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                    retval = "'" + sp.Value.ToString().Replace("'", "''") + "'";
                    break;

                case SqlDbType.Bit:
                    retval = (sp.Value.ToBooleanOrDefault(false)) ? "1" : "0";
                    break;

                default:
                    retval = sp.Value.ToString().Replace("'", "''");
                    break;
            }

            return retval;
        }

        public static String CommandAsSql(this SqlCommand sc)
        {
            StringBuilder sql = new StringBuilder();
            Boolean FirstParam = true;

            ArrayList outputColumns = new ArrayList();

            sql.AppendLine("use " + sc.Connection.Database + ";");
            switch (sc.CommandType)
            {
                case CommandType.StoredProcedure:
                    sql.AppendLine("declare @return_value int;");

                    foreach (SqlParameter sp in sc.Parameters)
                    {
                        if ((sp.Direction == ParameterDirection.InputOutput) || (sp.Direction == ParameterDirection.Output))
                        {
                            var thisTempVal = sp.SqlDbType.ToString() == "VarChar" ? sp.SqlDbType.ToString() + "(max)" : sp.SqlDbType.ToString();
                            sql.Append("declare " + sp.ParameterName + "\t" + thisTempVal + "\t= ");

                            sql.AppendLine(((sp.Direction == ParameterDirection.Output) ? "null" : sp.ParameterValueForSQL()) + "");

                            if (sp.Direction == ParameterDirection.Output)
                            {
                                outputColumns.Add(sp.ParameterName);
                            }
                        }
                    }

                    sql.AppendLine("exec " + sc.CommandText + "");

                    foreach (SqlParameter sp in sc.Parameters)
                    {
                        if (sp.Direction != ParameterDirection.ReturnValue)
                        {
                            sql.Append((FirstParam) ? "\t" : "\t, ");

                            if (FirstParam) FirstParam = false;

                            if (sp.Direction == ParameterDirection.Input)
                            {
                                var ok = sp.ParameterValueForSQL();
                                var thisValue = string.IsNullOrWhiteSpace(ok) ? "null" : sp.ParameterValueForSQL();
                                sql.AppendLine(sp.ParameterName + " = " + thisValue);
                            }
                            else
                            {
                                sql.AppendLine(sp.ParameterName + " = " + sp.ParameterName + " output");
                            }
                        }
                    }
                    sql.AppendLine("");
                    sql.AppendLine("");

                    var tempString = string.Join(",", outputColumns.ToArray());
                    tempString = string.IsNullOrWhiteSpace(tempString) ? "" : "," + tempString;
                    sql.AppendLine("select 'Return Value' = convert(varchar, @return_value)" + tempString);

                    /*foreach (SqlParameter sp in sc.Parameters)
                    {
                        if ((sp.Direction == ParameterDirection.InputOutput) || (sp.Direction == ParameterDirection.Output))
                        {
                            sql.AppendLine("select '" + sp.ParameterName + "' = convert(varchar, " + sp.ParameterName + ");");
                        }
                    }*/
                    break;
                case CommandType.Text:
                    sql.AppendLine(sc.CommandText);
                    break;
            }

            return sql.ToString();
        }

    }
}
