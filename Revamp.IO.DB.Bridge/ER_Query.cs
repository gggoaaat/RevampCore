using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Revamp.IO.DB.Bridge
{
    public class ER_Query
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

        public string DB_ERROR_FORMATTER(IConnectToDB _Connect, SqlException ex)
        {
            StringBuilder errorMessages = new StringBuilder();
            //If set to true returns Microst formatted Error
            Boolean runMicrosoft = false;

            StringBuilder returnString = new StringBuilder("");

            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":
                    runMicrosoft = true;
                    break;
                default:
                    break;
            }

            returnString.Append("Error-");

            if (runMicrosoft)
            {
                //nothing yet.
                // returnString.Append(ExtractString(DB_ERROR.ToString(), "System.Data.SqlClient.SqlException", "at System.Data.SqlClient.SqlInternalConnection.OnError"));

                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                //Console.WriteLine(errorMessages.ToString());
            }

            return errorMessages.ToString();
        }

        // START  DB SQL Executes, USER for DDL, and DML

        //Only for None Query SQL statements.

        public bool CanIdentityConnect(IConnectToDB _Connect)
        {

            switch (_Connect.Platform)
            {

                case "Microsoft":
                case "MICROSOFT":
                    using (SqlConnection connection = new SqlConnection(_Connect.DBConnString))
                    {
                        try
                        {
                            connection.Open();

                            return true;
                        }
                        catch (SqlException ex)
                        {
                            string err = ex.ToString();

                            return false;
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }

                default:
                    return false;

            }

        }

        public static string _RUN_NON_QUERY(IConnectToDB _Connect, string SQLin, string SuccessMessage)
        {
            ER_Query query = new ER_Query();

            return query.RUN_NON_QUERY(_Connect, SQLin, SuccessMessage);
        }

        public string RUN_NON_QUERY(IConnectToDB _Connect, string SQLin, string SuccessMessage)
        {
            switch (_Connect.Platform)
            {

                case "Microsoft":
                case "MICROSOFT":
                    using (SqlConnection connection = new SqlConnection(_Connect.DBConnString))
                    {
                        try
                        {
                            connection.Open();

                            SqlCommand sqlplus = connection.CreateCommand();

                            StringBuilder sqlStatement = new StringBuilder();

                            sqlStatement.Append(SQLin);

                            sqlplus.CommandText = sqlStatement.ToString();

                            int Result = sqlplus.ExecuteNonQuery();



                            return (SuccessMessage);
                        }
                        catch (SqlException ex)
                        {
                            return DB_ERROR_FORMATTER(_Connect, ex);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }

                default:
                    return "Invalid DB Platform";

            }

        }

        public DataTable RUN_QUERY(IConnectToDB _Connect, string SQLin)
        {
            DataTable dt = new DataTable("ReturnData");
            DataColumn column;
            DataRow row;

            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":
                    using (SqlConnection connection = new SqlConnection(_Connect.DBConnString))
                    {
                        try
                        {
                            connection.Open();

                            SqlCommand sqlplus = connection.CreateCommand();

                            StringBuilder sqlStatement = new StringBuilder();

                            sqlStatement.Append(SQLin);

                            sqlplus.CommandText = sqlStatement.ToString();

                            SqlDataReader dr = sqlplus.ExecuteReader();

                            dt.Load(dr);
                        }
                        catch (SqlException ex)
                        {
                            //Return Oracle Error.
                            //return ex.ToString();

                            // Create first column and add to the DataTable.
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.Int32");
                            column.ColumnName = "ChildID";
                            column.AutoIncrement = true;
                            column.AutoIncrementSeed = 0;
                            column.AutoIncrementStep = 1;
                            column.Caption = "ID";
                            column.ReadOnly = true;
                            column.Unique = true;
                            dt.Columns.Add(column);

                            // Create second column and add to the DataTable.
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.String");
                            column.ColumnName = "ChildType";
                            column.AutoIncrement = false;
                            column.Caption = "ChildType";
                            column.ReadOnly = false;
                            column.Unique = false;
                            dt.Columns.Add(column);

                            // Create third column and add to the DataTable.
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.String");
                            column.ColumnName = "ChildItem";
                            column.AutoIncrement = false;
                            column.Caption = "ChildItem";
                            column.ReadOnly = false;
                            column.Unique = false;
                            dt.Columns.Add(column);

                            // Create fourth column and add to the DataTable.
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.String");
                            column.ColumnName = "ChildValue";
                            column.AutoIncrement = false;
                            column.Caption = "ChildValue";
                            column.ReadOnly = false;
                            column.Unique = false;
                            dt.Columns.Add(column);

                            row = dt.NewRow();
                            row["ChildType"] = "RETURN";
                            row["ChildItem"] = "ERROR";
                            row["ChildValue"] = DB_ERROR_FORMATTER(_Connect, ex);
                            dt.Rows.Add(row);

                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }

                        return dt;
                    }

                default:
                    return dt;

            }
        }

        public static DataTable _RUN_QUERY(IConnectToDB _Connect, string SQLin)
        {
            ER_Query er_query = new ER_Query();

            return er_query.RUN_QUERY(_Connect, SQLin);
        }

        public List<DataTable> RUN_NON_QUERY(IConnectToDB _Connect, string SQLin, string SuccessMessage, List<DataTable> DTs)
        {

            switch (_Connect.Platform)
            {

                case "Microsoft":
                case "MICROSOFT":
                    // Todo : Rename DBConnString to SourceDBString
                    using (SqlConnection connection = new SqlConnection(_Connect.DBConnString))
                    {
                        try
                        {
                            connection.Open();

                            SqlCommand sqlplus = connection.CreateCommand();

                            StringBuilder sqlStatement = new StringBuilder();

                            sqlStatement.Append(SQLin);

                            sqlplus.CommandText = sqlStatement.ToString();

                            //int Result = sqlplus.ExecuteNonQuery();

                            SqlDataReader dr = sqlplus.ExecuteReader();

                            bool Exit = true;
                            //while (Exit)
                            //{
                            //    try
                            //    {
                            //        DataTable dt = new DataTable();
                            //        dt.Load(dr);
                            //        DTs.Add(dt);
                            //        // Exit = dr.NextResult();
                            //    }
                            //    catch (Exception)
                            //    {
                            //        Exit = false;
                            //    }
                            //}

                            do
                            {
                                DataTable dt = new DataTable();
                                dt.Load(dr);
                                DTs.Add(dt);

                            } while (!dr.IsClosed);

                            dr.Close();

                            //return (SuccessMessage);
                        }
                        catch (SqlException)
                        {
                            // return er_tools.DB_ERROR_FORMATTER(_Connect, ex.ToString());
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }

                    return DTs;

                default:
                    return DTs;

            }

        }
        public string RUN_COMMAND_WITH_PARAMS(IConnectToDB _Connect, string SQLin, string SuccessMessage, SqlParameterCollection[] dbParams, List<DataTable> DTs)
        {
            using (SqlConnection connection = new SqlConnection(_Connect.DBConnString))
            {
                connection.Open();

                using (SqlTransaction dbTrans = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand dbCommand = new SqlCommand(SQLin, connection))
                        {
                            dbCommand.Transaction = dbTrans;

                            if (dbParams != null && dbParams.Count() > 0)
                            {
                                dbCommand.Parameters.AddRange(dbParams);
                            }
                            //dbCommand.Parameters.Add("id", SqlDbType.VarChar).Value = id;
                            //dbCommand.Parameters.Add("accountid", SqlDbType.VarChar).Value = accountId;
                            //dbCommand.Parameters.Add("firstname", SqlDbType.VarChar).Value = firstName;
                            //dbCommand.Parameters.Add("lastname", SqlDbType.VarChar).Value = lastName;
                            //dbCommand.Parameters.Add("jobtitle", SqlDbType.VarChar).Value = jobTitle;
                            //dbCommand.Parameters.Add("phonenumber", SqlDbType.VarChar).Value = phoneNumber;

                            dbCommand.ExecuteNonQuery();
                        }

                        dbTrans.Commit();
                    }
                    catch (SqlException e)
                    {
                        dbTrans.Rollback();
                        SuccessMessage = e.ToString();

                        //throw; // bubble up the exception and preserve the stack trace
                    }
                }

                connection.Close();
            }

            return SuccessMessage;
        }

        [Serializable]
        public class Parameter_Run
        {
            public string sqlIn { get; set; }
            public List<DBParameters> _dbParameters { get; set; } = new List<DBParameters>();
        }

        public static DataTable _RUN_PARAMETER_QUERY(IConnectToDB _Connect, Parameter_Run ParamCall)
        {
            ER_Query eq = new ER_Query();

            return eq.RUN_PARAMETER_QUERY(_Connect, ParamCall);
        }

        public DataTable RUN_PARAMETER_QUERY(IConnectToDB _Connect, Parameter_Run ParamCall)
        {

            DataTable dt = new DataTable("ReturnData");
            DataColumn column;
            DataRow row;

            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":
                    using (SqlConnection connection = new SqlConnection(_Connect.DBConnString))
                    {
                        try
                        {
                            connection.Open();

                            SqlCommand sqlplus = connection.CreateCommand();

                            sqlplus.CommandText = ParamCall.sqlIn;
                            foreach (var item in ParamCall._dbParameters)
                            {
                                sqlplus.Parameters.AddWithValue("@" + item.ParamName, item.ParamValue);
                            }

                            SqlDataReader dr = sqlplus.ExecuteReader();

                            dt.Load(dr);
                        }
                        catch (SqlException ex)
                        {
                            //Return Oracle Error.
                            //return ex.ToString();

                            // Create first column and add to the DataTable.
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.Int32");
                            column.ColumnName = "ChildID";
                            column.AutoIncrement = true;
                            column.AutoIncrementSeed = 0;
                            column.AutoIncrementStep = 1;
                            column.Caption = "ID";
                            column.ReadOnly = true;
                            column.Unique = true;
                            dt.Columns.Add(column);

                            // Create second column and add to the DataTable.
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.String");
                            column.ColumnName = "ChildType";
                            column.AutoIncrement = false;
                            column.Caption = "ChildType";
                            column.ReadOnly = false;
                            column.Unique = false;
                            dt.Columns.Add(column);

                            // Create third column and add to the DataTable.
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.String");
                            column.ColumnName = "ChildItem";
                            column.AutoIncrement = false;
                            column.Caption = "ChildItem";
                            column.ReadOnly = false;
                            column.Unique = false;
                            dt.Columns.Add(column);

                            // Create fourth column and add to the DataTable.
                            column = new DataColumn();
                            column.DataType = System.Type.GetType("System.String");
                            column.ColumnName = "ChildValue";
                            column.AutoIncrement = false;
                            column.Caption = "ChildValue";
                            column.ReadOnly = false;
                            column.Unique = false;
                            dt.Columns.Add(column);

                            row = dt.NewRow();
                            row["ChildType"] = "RETURN";
                            row["ChildItem"] = "ERROR";
                            row["ChildValue"] = DB_ERROR_FORMATTER(_Connect, ex);
                            dt.Rows.Add(row);

                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }

                        return dt;
                    }

                default:
                    return dt;

            }
        }
    }
}
