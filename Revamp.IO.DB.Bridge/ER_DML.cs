using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs;
using Revamp.IO.Structs.Models;
using Revamp.IO.Structs.Models.DataEntry;
using Revamp.IO.Structs.Models.RevampSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Revamp.IO.Structs.Models.SQLProcedureModels;

//using ER.Helpers;

namespace Revamp.IO.DB.Bridge
{
    public class ER_DML
    {
        public string PROCEDURE_ACTION(IConnectToDB _Connect, string ProcedureName, List<DBParameters> EntryProcedureParameters, string _ReturnParameter)
        {
            List<DBParameters> ParamListDynamic = new List<DBParameters>();

            switch (_Connect.Platform.ToLower())
            {

                case "microsoft":

                    Paramatizer(EntryProcedureParameters, ParamListDynamic);

                    BIG_CALL RUN = new BIG_CALL();

                    RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

                    RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = "value", ProcedureName = ProcedureName, _dbParameters = ParamListDynamic });

                    DataTable _result = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS[0].result._DataTable;

                    return ER_Procedure.SQL_PROCEDURE_GET_VALUE("@" + _ReturnParameter, _result);


                default:

                    return "Invalid DB Platform";

            }
        }

        private static void Paramatizer(List<DBParameters> EntryProcedureParameters, List<DBParameters> ParamListDynamic)
        {
            long? count = EntryProcedureParameters.Count;

            for (int i = 0; i < count; i++)
            {
                //if (i.ParamValue != null || i.ParamValue != "")

                switch (EntryProcedureParameters[i].MSSqlParamDataType)
                {
                    case SqlDbType.VarChar:
                    case SqlDbType.Char:
                        ParamListDynamic.Add(new DBParameters
                        {
                            ParamName = "@" + EntryProcedureParameters[i].ParamName,
                            ParamDirection = EntryProcedureParameters[i].ParamDirection,
                            MSSqlParamDataType = EntryProcedureParameters[i].MSSqlParamDataType,
                            ParamSize = EntryProcedureParameters[i].ParamSize,
                            ParamValue = EntryProcedureParameters[i].ParamValue
                        }
                        );
                        break;
                    case SqlDbType.Decimal:
                    case SqlDbType.Int:
                    case SqlDbType.BigInt:
                    case SqlDbType.Date:
                    case SqlDbType.DateTime:
                    case SqlDbType.Timestamp:
                    case SqlDbType.SmallDateTime:
                    case SqlDbType.UniqueIdentifier:
                        ParamListDynamic.Add(new DBParameters
                        {
                            ParamName = "@" + EntryProcedureParameters[i].ParamName,
                            ParamDirection = EntryProcedureParameters[i].ParamDirection,
                            MSSqlParamDataType = EntryProcedureParameters[i].MSSqlParamDataType,
                            ParamValue = EntryProcedureParameters[i].ParamValue
                        }
                        );
                        break;
                    case SqlDbType.VarBinary:
                        ParamListDynamic.Add(new DBParameters
                        {
                            ParamName = "@" + EntryProcedureParameters[i].ParamName,
                            ParamDirection = EntryProcedureParameters[i].ParamDirection,
                            MSSqlParamDataType = EntryProcedureParameters[i].MSSqlParamDataType,
                            ParamSize = EntryProcedureParameters[i].ParamSize,
                            ParamValue = EntryProcedureParameters[i].ParamValue
                        }
                        );
                        break;
                }

            }
        }

        public RevampNucleus.AddOwnerSeed ADD_Owner_Seed(IConnectToDB _Connect, RevampNucleus.AddOwnerSeed thisModel)
        {
            Universal_Call<RevampNucleus.AddOwnerSeed> universalCall = new Universal_Call<RevampNucleus.AddOwnerSeed>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Dictionary.AddTable ADD_Dictionary_Table(IConnectToDB _Connect, Dictionary.AddTable thisModel)
        {
            Universal_Call<Dictionary.AddTable> universalCall = new Universal_Call<Dictionary.AddTable>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Dictionary.AddPrimaryKey ADD_Dictionary_PK(IConnectToDB _Connect, Dictionary.AddPrimaryKey thisModel)
        {
            Universal_Call<Dictionary.AddPrimaryKey> universalCall = new Universal_Call<Dictionary.AddPrimaryKey>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            if (thisModel.O_ER_PRIMARY_KEYS_ID > 0)
            {
                for (int i = 0; i < thisModel.Columns.Count; i++)
                {
                    thisModel.Columns[i].I_ER_PRIMARY_KEYS_ID = thisModel.O_ER_PRIMARY_KEYS_ID;
                    thisModel.Columns[i] = ADD_Dictionary_PK_Columns(_Connect, thisModel.Columns[i]);
                }
            }

            return thisModel;
        }

        public Dictionary.AddPrimaryKeyColumns ADD_Dictionary_PK_Columns(IConnectToDB _Connect, Dictionary.AddPrimaryKeyColumns thisModel)
        {
            Universal_Call<Dictionary.AddPrimaryKeyColumns> universalCall = new Universal_Call<Dictionary.AddPrimaryKeyColumns>();
            thisModel = universalCall.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Dictionary.AddForeignKey ADD_Dictionary_FK(IConnectToDB _Connect, Dictionary.AddForeignKey thisModel)
        {
            Universal_Call<Dictionary.AddForeignKey> _uFKKey = new Universal_Call<Dictionary.AddForeignKey>();

            thisModel = _uFKKey.GenericInputProcedure(_Connect, thisModel);

            if (thisModel.O_ER_FOREIGN_KEYS_ID > 0)
            {
                foreach (Dictionary.AddForeignKeyColumn i in thisModel.V_FK_ColumnsList1)
                {
                    i.I_ER_FOREIGN_KEYS_ID = thisModel.O_ER_FOREIGN_KEYS_ID;
                    ADD_Dictionary_FK_Column(_Connect, i);
                }
            }

            return thisModel;
        }

        public Dictionary.AddForeignKeyColumn ADD_Dictionary_FK_Column(IConnectToDB _Connect, Dictionary.AddForeignKeyColumn thisModel)
        {
            Universal_Call<Dictionary.AddForeignKeyColumn> _uFKKey = new Universal_Call<Dictionary.AddForeignKeyColumn>();
            thisModel = _uFKKey.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Dictionary.AddUniqueKeyColumn ADD_Dictionary_UK_Column(IConnectToDB _Connect, Dictionary.AddUniqueKeyColumn thisModel)
        {
            Universal_Call<Dictionary.AddUniqueKeyColumn> _uFKKey = new Universal_Call<Dictionary.AddUniqueKeyColumn>();
            thisModel = _uFKKey.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Dictionary.AddUniqueKey ADD_Dictionary_UK(IConnectToDB _Connect, Dictionary.AddUniqueKey thisModel)
        {

            Universal_Call<Dictionary.AddUniqueKey> _uFKKey = new Universal_Call<Dictionary.AddUniqueKey>();
            thisModel = _uFKKey.GenericInputProcedure(_Connect, thisModel);

            if (thisModel.O_ER_UNIQUE_KEYS_ID > 0 && thisModel.V_UK_ColumnsList1 != null)
            {
                foreach (Dictionary.AddUniqueKeyColumn i in thisModel.V_UK_ColumnsList1)
                {
                    i.I_ER_UNIQUE_KEYS_ID = thisModel.O_ER_UNIQUE_KEYS_ID;
                    ADD_Dictionary_UK_Column(_Connect, i);
                }
            }

            return thisModel;
        }      

        public Dictionary.AddView ADD_Dictionary_View(IConnectToDB _Connect, Dictionary.AddView thisModel) //, string ViewName, string ViewType, string VIEWDATA
        {
            Universal_Call<Dictionary.AddView> _uFKKey = new Universal_Call<Dictionary.AddView>();
            thisModel = _uFKKey.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public Dictionary.DropView DROP_Dictionary_View(IConnectToDB _Connect, string ViewName)
        {
            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = "Select * from CSA.ER_VIEWS where UPPER(VIEW_NAME) = @VIEW_NAME",
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "VIEW_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = ViewName.ToUpper() } }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            Dictionary.DropView thisModel = new Dictionary.DropView();

            if (_DT.Rows.Count > 0 && DCC.Contains("ER_VIEWS_ID"))
            {
                thisModel = new Dictionary.DropView { I_ER_VIEWS_ID = _DT.Rows[0].Field<long?>("ER_VIEWS_ID") };
                Universal_Call<Dictionary.DropView> dv = new Universal_Call<Dictionary.DropView>();
                thisModel = dv.GenericInputProcedure(_Connect, thisModel);              
            }

            return thisModel;

        }

        public Dictionary.AddIndex ADD_Dictionary_Index(IConnectToDB _Connect, Dictionary.AddIndex thisModel) //, string index_name, string index_type, string source_name, string source_type
        {
            Universal_Call<Dictionary.AddIndex> _uFKKey = new Universal_Call<Dictionary.AddIndex>();
            thisModel = _uFKKey.GenericInputProcedure(_Connect, thisModel);

            return thisModel;
        }

        public string TOGGLE_OBJECT(IConnectToDB _Connect, string _source, long? _id, string toggleYN)
        {
            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_SOURCE",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 30,
                ParamValue = _source
            });


            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_ID",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Int,
                ParamValue = _id
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_ENABLED",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 1,
                ParamValue = toggleYN.ToUpper()
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "O_ID",
                ParamDirection = ParameterDirection.Output,
                MSSqlParamDataType = SqlDbType.Int
            });

            return PROCEDURE_ACTION(_Connect, "_TOGGLE_OBJECT__RW", EntryProcedureParameters, "R_ID");
        }

        public string TOGGLE_FORM_OBJECTS(IConnectToDB _Connect, long? _stages_id, long? _forms_id, string toggleYN)
        {
            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_FORMS_ID",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Int,
                ParamValue = _forms_id
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_STAGES_ID",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Int,
                ParamValue = _stages_id
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_ENABLED",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 1,
                ParamValue = toggleYN.ToUpper()
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "R_ID",
                ParamDirection = ParameterDirection.Output,
                MSSqlParamDataType = SqlDbType.Int
            });

            return PROCEDURE_ACTION(_Connect, "TOGGLE_FORM_OBJECTS__RW", EntryProcedureParameters, "R_ID");
        }

        public string OBJECT_DML(IConnectToDB _Connect, string _Action, string _Source, string _SourceField, long? _SourceID, Object_Value _Value)
        {
            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_Action",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                //ParamSize = 255,
                ParamValue = _Action
            });


            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_SOURCE",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                // ParamSize = 255,
                ParamValue = _Source
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_SOURCEFIELD",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                // ParamSize = 7,
                ParamValue = _SourceField
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_SOURCEID",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Int,
                ParamValue = _SourceID
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_VARVALUE",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamValue = _Value._String
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_NUMBVALUE",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Int,
                ParamValue = _Value._Number
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_DATEVALUE",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.DateTime,
                ParamValue = _Value._Date
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "I_DECIVALUE",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Decimal,
                ParamValue = _Value._Decimal
            });

            try
            {
                EntryProcedureParameters.Add(new DBParameters
                {
                    ParamName = "I_RAWVALUE",
                    ParamDirection = ParameterDirection.Input,
                    MSSqlParamDataType = SqlDbType.VarBinary,
                    ParamValue = _Value._File._FileBytes == null ? new Byte[0] : _Value._File._FileBytes
                });
            }
            catch
            {
                EntryProcedureParameters.Add(new DBParameters
                {
                    ParamName = "I_RAWVALUE",
                    ParamDirection = ParameterDirection.Input,
                    MSSqlParamDataType = SqlDbType.VarBinary,
                    ParamValue = null
                });
            }

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "O_ID",
                ParamDirection = ParameterDirection.Output,
                MSSqlParamDataType = SqlDbType.Int
            });

            //return PROCEDURE_ACTION(_Connect, _Source + "_DML", EntryProcedureParameters, "R_ID");
            return PROCEDURE_ACTION(_Connect, "SP_U_Mod_" + _Source, EntryProcedureParameters, "R_ID");
        }

        public DataTable GET_COLUMNS_VIA_TABLENAME(IConnectToDB _Connect, string TableName, Boolean _isSeqID)
        {
            StringBuilder SQLin = new StringBuilder();
            

            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":
                    SQLin.Append("Select COLUMN_NAME, DATA_TYPE, ");
                    SQLin.Append("CHARACTER_OCTET_LENGTH DATA_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE ");
                    SQLin.Append("from INFORMATION_SCHEMA.COLUMNS ");
                    SQLin.Append("where upper(TABLE_NAME) = upper(@TABLE_NAME) ");
                    if (_isSeqID == false)
                    {
                        SQLin.Append("and ORDINAL_POSITION > 12 ");
                    }
                    else
                    {
                        SQLin.Append("and ORDINAL_POSITION = 1 ");
                    }
                    SQLin.Append("order by ORDINAL_POSITION ASC ");

                    ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                    {
                        sqlIn = SQLin.ToString(),
                        _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = TableName } }
                    };

                    return ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                default:
                    return new DataTable();
            }
        }

        public DataTable GET_COLUMNS_VIA_TABLENAME(IConnectToDB _Connect, string TableName)
        {

            StringBuilder SQLin = new StringBuilder();
            SQLin.Append("Select COLUMN_NAME, DATA_TYPE, ");

            switch (_Connect.Platform)
            {
                case "Microsoft":
                case "MICROSOFT":
                    SQLin.Append("CHARACTER_OCTET_LENGTH DATA_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE ");
                    SQLin.Append("from INFORMATION_SCHEMA.COLUMNS ");
                    SQLin.Append("where upper(TABLE_NAME) = upper(@TABLE_NAME) ");
                    SQLin.Append("order by ORDINAL_POSITION ASC ");

                    ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                    {
                        sqlIn = SQLin.ToString(),
                        _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = TableName } }
                    };

                    return ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                default:
                    return new DataTable();
            }

        }
    }
}
