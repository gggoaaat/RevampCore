using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Revamp.IO.DB.Bridge;
using System.Data;
using Revamp.IO.Structs.Enums;
using Revamp.IO.Structs.Models;

namespace Revamp.IO.DB.Bridge
{
    public class TransactionCall<T,V>
    {
        public SQLProcedureModels.BIG_CALL GetParams(T thisModel, V thisModelParamClone)
        {
            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();
            PropertyInfo[] properties = thisModel.GetType().GetProperties();
            PropertyInfo[] propertiesClone = thisModelParamClone.GetType().GetProperties();
            Type modelType = properties.GetType();
            Type modelTypeClone = propertiesClone.GetType();
            string procedure = properties[0].Name;

            string[] variableList = new string[] { "V_", "I_", "O_" };
            int cloneI = 0;
            foreach (PropertyInfo property in properties.Where(p => variableList.Contains(p.Name.Substring(0, 2))))
            {
                string name = property.Name;
                object value = property.GetValue(thisModel, null);

                if (name == "V_PROCEDURE_NAME")
                {
                    procedure = value.ToString();
                }
                else if (name.Substring(0, 2) == "I_" || name.Substring(0, 2) == "O_")
                {
                    var cloneType = thisModelParamClone.GetType().GetProperty(name)?.PropertyType;
                    string propertyType = (cloneType == typeof(String) || cloneType == typeof(byte[]) || cloneType == typeof(Decimal)) ? cloneType.Name : cloneType.GenericTypeArguments[0].Name;
                    ParameterDirection direction = name.Substring(0, 2) == "I_" ? ParameterDirection.Input : ParameterDirection.Output;
                    SqlDbType dbType = new SqlDbType();

                    switch (propertyType)
                    {
                        default:
                        //'varchar' then 'string'
                        case "String":
                            dbType = SqlDbType.VarChar;
                            break;
                        //'char' then 'char?'
                        case "Char":
                            dbType = SqlDbType.Char;
                            break;
                        //'int' then 'int'
                        case "Int32":
                            dbType = SqlDbType.Int;
                            break;
                        //'bigint' then 'long?'
                        case "Int64":
                            dbType = SqlDbType.BigInt;
                            break;
                        //'numeric' then 'Decimal'
                        //'decimal' then 'Decimal'
                        case "Decimal":
                            dbType = SqlDbType.Decimal;
                            break;
                        //'date' then 'DateTime?'
                        //'smalldatetime' then 'DateTime?'
                        //'datetime' then 'DateTime?'
                        case "DateTime":
                            dbType = SqlDbType.DateTime;
                            break;
                        //'varbinary' then 'byte[]'
                        case "Byte[]":
                            dbType = SqlDbType.VarBinary;
                            break;
                        //'bit' then 'bool'
                        case "Boolean":
                            dbType = SqlDbType.Bit;
                            break;
                        case "Guid":
                            dbType = SqlDbType.UniqueIdentifier;
                            break;
                    }

                    EntryProcedureParameters.Add(new DBParameters { ParamName = "@" + name, ParamDirection = direction, MSSqlParamDataType = dbType, ParamSize = -1, ParamValue = value });
                }

                cloneI++;
            }

            string ProcedureName = procedure;
            string ProcedureReturnType = "value";
            string outputParam = properties[properties.Length - 1].Name;
            string ReturnValueFor = "@" + outputParam;
          //  ER_DML _dml = new ER_DML();

            SQLProcedureModels.BIG_CALL RUN = new SQLProcedureModels.BIG_CALL();

            RUN.COMMANDS = new List<SQLProcedureModels.SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQLProcedureModels.SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });
            return RUN;
        }

    }
    public class Universal_Call<T>
    {
        public T GenericInputProcedure(IConnectToDB _Connect, T thisModel)
        {
            T ok = GenericInputProcedureParamsAndCall(_Connect, thisModel);

            return ok;
        }

        public T GenericInputProcedureParamsAndCall(IConnectToDB _Connect, T thisModel)
        {   //START T GenericInputProcedure
            SQLProcedureModels.BIG_CALL RUN = GetParams(thisModel);

            try
            {
                RunGeneratedCommand(_Connect, thisModel, RUN);
                //thisModel.GetType().GetProperty(outputParam).SetValue(thisModel, Convert.ToInt64(_result.Rows[properties.Length - 2].ItemArray[3]), null);                
            }
            catch (Exception ex)
            {
                //ER_Tools._WriteEventLog(string.Format("Caught exception: {0} \r\n Stack Trace: {1}", ex.Message, ex.StackTrace), EventLogType.exception);
            }

            return thisModel;
        }

        public static SQLProcedureModels.BIG_CALL GetParams(T thisModel)
        {
            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();
            PropertyInfo[] properties = thisModel.GetType().GetProperties();
            Type modelType = properties.GetType();
            string procedure = properties[0].Name;

            string[] variableList = new string[] { "V_", "I_", "O_" };
            foreach (PropertyInfo property in properties.Where(p => variableList.Contains(p.Name.Substring(0, 2))))
            {
                string name = property.Name;
                object value = property.GetValue(thisModel, null);

                if (name == "V_PROCEDURE_NAME")
                {
                    procedure = value.ToString();
                }
                else if (name.Substring(0, 2) == "I_" || name.Substring(0, 2) == "O_")
                {
                    string propertyType = (property.PropertyType == typeof(String) || property.PropertyType == typeof(byte[]) || property.PropertyType == typeof(Decimal)) ? property.PropertyType.Name : property.PropertyType.GenericTypeArguments[0].Name;
                    ParameterDirection direction = name.Substring(0, 2) == "I_" ? ParameterDirection.Input : ParameterDirection.Output;
                    SqlDbType dbType = new SqlDbType();

                    switch (propertyType)
                    {
                        default:
                        //'varchar' then 'string'
                        case "String":
                            dbType = SqlDbType.VarChar;
                            break;
                        //'char' then 'char?'
                        case "Char":
                            dbType = SqlDbType.Char;
                            break;
                        //'int' then 'int'
                        case "Int32":
                            dbType = SqlDbType.Int;
                            break;
                        //'bigint' then 'long?'
                        case "Int64":
                            dbType = SqlDbType.BigInt;
                            break;
                        //'numeric' then 'Decimal'
                        //'decimal' then 'Decimal'
                        case "Decimal":
                            dbType = SqlDbType.Decimal;
                            break;
                        //'date' then 'DateTime?'
                        //'smalldatetime' then 'DateTime?'
                        //'datetime' then 'DateTime?'
                        case "DateTime":
                            dbType = SqlDbType.DateTime;
                            break;
                        //'varbinary' then 'byte[]'
                        case "Byte[]":
                            dbType = SqlDbType.VarBinary;
                            break;
                        //'bit' then 'bool'
                        case "Boolean":
                            dbType = SqlDbType.Bit;
                            break;
                        case "Guid":
                            dbType = SqlDbType.UniqueIdentifier;
                            break;
                    }

                    EntryProcedureParameters.Add(new DBParameters { ParamName = "@" + name, ParamDirection = direction, MSSqlParamDataType = dbType, ParamSize = -1, ParamValue = value });
                }
            }

            string ProcedureName = procedure;
            string ProcedureReturnType = "value";
            string outputParam = properties[properties.Length - 1].Name;
            string ReturnValueFor = "@" + outputParam;
           // ER_DML _dml = new ER_DML();

            SQLProcedureModels.BIG_CALL RUN = new SQLProcedureModels.BIG_CALL();

            RUN.COMMANDS = new List<SQLProcedureModels.SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQLProcedureModels.SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });
            return RUN;
        }

        private static void RunGeneratedCommand(IConnectToDB _Connect, T thisModel, SQLProcedureModels.BIG_CALL RUN)
        {
            DataTable _result = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS[0].result._DataTable;

            //DataRow[] outputRow = _result.Select("ID = " + outputParam);

            string attemptedSQL = _result.Rows.Count > 0 && _result.Rows[0].Field<string>("ChildSQL") != null ? _result.Rows[0].Field<string>("ChildSQL").ToString() : "";

            bool isError = _result.Rows.Count > 0 && _result.Rows[0].Field<string>("ChildType") != null && _result.Rows[0].Field<string>("ChildType").ToString() == "Error";

            if (!String.IsNullOrWhiteSpace(attemptedSQL))
            {
                thisModel.GetType().GetProperty("V_ATTEMPTED_SQL")?.SetValue(thisModel, attemptedSQL, null);
            }

            if (isError)
            {
                try
                {
                    thisModel.GetType().GetProperty("O_ERR_MESS")?.SetValue(thisModel, _result.Rows[0].Field<string>("ChildValue").ToString(), null);
                }
                catch (Exception)
                {

                    
                }
            }
            else
            { 
                foreach (DataRow row in _result.Rows)
                {
                    var modelProperty = row["ChildItem"].ToString().Replace("@", "");
                    var thisValue = row["ChildValue"];
                    if (modelProperty.Substring(0, 2) == "O_")
                    {
                        var propertyType = thisModel.GetType().GetProperty(modelProperty)?.PropertyType;
                        object propertyVal = row["ChildValue"];
                        if (propertyType != null)
                        {
                            //propertyVal = Convert.ChangeType(thisValue, propertyType);
                            propertyVal = ChangeType(propertyVal, propertyType);
                        }

                        if (propertyVal != null)
                        {
                            thisModel.GetType().GetProperty(modelProperty)?.SetValue(thisModel, propertyVal, null);
                        }
                    }

                    //if (row.ItemArray[2].ToString() == ReturnValueFor && Int64.TryParse(row.ItemArray[3].ToString(), out outputid))
                    //{
                    //    thisModel.GetType().GetProperty(outputParam).SetValue(thisModel, outputid, null);
                    //}
                } 
            }
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            object tempResult = t.Name.ToUpper() == "GUID" ? Tools.Box.ConvertToGuid(value.ToString()) : Convert.ChangeType(value, t);
            
            return tempResult;
        }

        public SQLTrasaction GenericInputSQLTransaction(IConnectToDB _Connect, T thisModel)
        {   //START T GenericInputProcedure
            SQLTrasaction SQLTran = new SQLTrasaction();

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();
            PropertyInfo[] properties = thisModel.GetType().GetProperties();
            Type modelType = properties.GetType();
            string procedure = properties[0].Name;

            foreach (PropertyInfo property in properties.Where(p => !p.PropertyType.IsClass || p.PropertyType == typeof(String)))
            {
                string name = property.Name;
                object value = property.GetValue(thisModel, null);

                if (name == "V_PROCEDURE_NAME")
                {
                    procedure = value.ToString();
                    SQLTran.ProcedureName = value.ToString();
                }
                else if (name.Substring(0, 2) == "I_" || name.Substring(0, 2) == "O_")
                {
                    string propertyType = (property.PropertyType == typeof(String) || property.PropertyType == typeof(Decimal)) ? property.PropertyType.Name : property.PropertyType.GenericTypeArguments[0].Name;
                    ParameterDirection direction = name.Substring(0, 2) == "I_" ? ParameterDirection.Input : ParameterDirection.Output;
                    SqlDbType dbType = new SqlDbType();

                    switch (propertyType)
                    {
                        default:
                        //'varchar' then 'string'
                        case "String":
                            dbType = SqlDbType.VarChar;
                            break;
                        //'char' then 'char?'
                        case "Char":
                            dbType = SqlDbType.Char;
                            break;
                        //'int' then 'int'
                        case "Int32":
                            dbType = SqlDbType.Int;
                            break;
                        //'bigint' then 'long?'
                        case "Int64":
                            dbType = SqlDbType.BigInt;
                            break;
                        //'numeric' then 'Decimal'
                        //'decimal' then 'Decimal'
                        case "Decimal":
                            dbType = SqlDbType.Decimal;
                            break;
                        //'date' then 'DateTime?'
                        //'smalldatetime' then 'DateTime?'
                        //'datetime' then 'DateTime?'
                        case "DateTime":
                            dbType = SqlDbType.DateTime;
                            break;
                        //'varbinary' then 'byte[]'
                        case "Byte[]":
                            dbType = SqlDbType.VarBinary;
                            break;
                        //'bit' then 'bool'
                        case "Boolean":
                            dbType = SqlDbType.Bit;
                            break;
                        case "Guid":
                            dbType = SqlDbType.UniqueIdentifier;
                            break;
                    }

                    EntryProcedureParameters.Add(new DBParameters { ParamName = name, ParamDirection = direction, MSSqlParamDataType = dbType, ParamSize = -1, ParamValue = value });
                }
            }

            SQLTran.EntryProcedureParameters = EntryProcedureParameters;

            return SQLTran;
        }
    }
}
