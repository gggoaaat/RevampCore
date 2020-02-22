using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Foundation;
using static Revamp.IO.Structs.Models.SQLProcedureModels;
using Revamp.IO.Structs.Models;

namespace Revamp.IO.DB.Binds.IO.Dynamic
{
    public class _DynamicOutputProcedures : I_DynamicOutputProcedures
    {
        #region Important Dyno Core Reports Methods
        public DataTable GetTableStruct(IConnectToDB _Connect, string ProcedureName)
        {
            string ProcedureReturnType = "table";


            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_TableorCount",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = -1,
                ParamValue = "Default Query"
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_VERIFY",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Char,
                ParamSize = 1,
                ParamValue = "F"
            });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable ReturnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            return ReturnSet;
        }

        public DataTable GetTableStruct(IConnectToDB _Connect, string ProcedureName, string DynoCol)
        {
            string ProcedureReturnType = "table";


            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_TableorCount",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = -1,
                ParamValue = "Custom Query"
            });

            if (DynoCol != null && DynoCol != String.Empty)
            {
                EntryProcedureParameters.Add(new DBParameters
                {
                    ParamName = "P_DYNO_COL",
                    ParamDirection = ParameterDirection.Input,
                    MSSqlParamDataType = SqlDbType.VarChar,
                    ParamSize = -1,
                    ParamValue = DynoCol
                });
            }

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_VERIFY",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Char,
                ParamSize = 1,
                ParamValue = "F"
            });

           // return _dml.CALL_DB_PROCEDURE(_Connect, ProcedureName, ProcedureReturnType, EntryProcedureParameters);

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            return ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS[0].result._DataTable;
        }

        public static DataTable _DynoProcSearch(IConnectToDB _Connect, string QueryType, string ProcedureName, DataTableDotNetModelMetaData Meta, List<DynamicModels.RootReportFilter> _Filters)
        {
            _DynamicOutputProcedures DOP = new _DynamicOutputProcedures();

            return DOP.DynoProcSearch(_Connect, QueryType, ProcedureName, Meta, _Filters);
        }


        public DataTable DynoProcSearch(IConnectToDB _Connect, string QueryType, string ProcedureName, DataTableDotNetModelMetaData Meta, List<DynamicModels.RootReportFilter> _Filters)
        {
            string ProcedureReturnType = "table";

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_TableorCount",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = -1,
                ParamValue = QueryType
            });

            if (Meta != null && !string.IsNullOrWhiteSpace(Meta.columns))
            {
                EntryProcedureParameters.Add(new DBParameters
                {
                    ParamName = "P_DYNO_COL",
                    ParamDirection = ParameterDirection.Input,
                    MSSqlParamDataType = SqlDbType.VarChar,
                    ParamSize = -1,
                    ParamValue = Meta.columns
                });
            }

            if (Meta != null && !string.IsNullOrWhiteSpace(Meta.search))
            {
                EntryProcedureParameters.Add(new DBParameters
                {
                    ParamName = "P_SEARCH",
                    ParamDirection = ParameterDirection.Input,
                    MSSqlParamDataType = SqlDbType.VarChar,
                    ParamSize = -1,
                    ParamValue = Meta.search
                });
            }

            if (Meta != null && Meta.start != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                {
                    ParamName = "P_STARTING_ROW",
                    ParamDirection = ParameterDirection.Input,
                    MSSqlParamDataType = SqlDbType.BigInt,
                    ParamValue = Meta.start
                });
            }

            if (Meta != null && Meta.length != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                {
                    ParamName = "P_LENGTH_OF_SET",
                    ParamDirection = ParameterDirection.Input,
                    MSSqlParamDataType = SqlDbType.BigInt,
                    ParamValue = Meta != null && Meta.length != null ? Meta.length : 10
                });
            }

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_ORDER_BY",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = -1,
                ParamValue = Meta != null && !string.IsNullOrEmpty(Meta.order) ? Meta.order : "1 asc"
            });

            //if (Meta != null && !string.IsNullOrEmpty(Meta.columns))
            //{
            //    EntryProcedureParameters.Add(new DBParameters
            //    {
            //        ParamName = "P_DYNO_COL",
            //        ParamDirection = ParameterDirection.Input,
            //        MSSqlParamDataType = SqlDbType.VarChar,
            //        ParamSize = -1,
            //        ParamValue = Meta.columns
            //    });
            //}

            foreach (DynamicModels.RootReportFilter _thisFilter in _Filters)
            {
                if ((_thisFilter.ParamValue != null) &&
                    (_thisFilter.FilterName != null || _thisFilter.FilterName != string.Empty)
                    )
                {
                    DBParameters thisParam = new DBParameters
                    {
                        ParamName = "P_" + _thisFilter.FilterName,
                        ParamDirection = ParameterDirection.Input,
                        MSSqlParamDataType = _thisFilter.DBType,
                        ParamSize = _thisFilter.ParamSize,
                        ParamValue = _thisFilter.DBType == SqlDbType.VarChar ? _thisFilter.ParamValue.ToString() : _thisFilter.ParamValue
                    };

                    EntryProcedureParameters.Add(thisParam);
                }
            }

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_VERIFY",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Char,
                ParamSize = 1,
                ParamValue = Meta != null && Meta.verify != null && Meta.verify != String.Empty ? Meta.verify : "T"
            });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            return returnSet;
        }

        public Int64 DynoProcSearchCount(IConnectToDB _Connect, string ProcedureName, DataTableDotNetModelMetaData Meta, List<DynamicModels.RootReportFilter> _Filters)
        {
            string ProcedureReturnType = "table";

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_TableorCount",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = -1,
                ParamValue = "Count Query"
            });

            if (Meta != null && Meta.search != null && Meta.search != string.Empty)
            {
                EntryProcedureParameters.Add(new DBParameters
                {
                    ParamName = "P_SEARCH",
                    ParamDirection = ParameterDirection.Input,
                    MSSqlParamDataType = SqlDbType.VarChar,
                    ParamSize = -1,
                    ParamValue = Meta.search
                });
            }

            foreach (DynamicModels.RootReportFilter _thisFilter in _Filters)
            {
                if ((_thisFilter.ParamValue != null) &&
                    (_thisFilter.FilterName != null || _thisFilter.FilterName != string.Empty)
                    )
                {
                    DBParameters thisParam = new DBParameters
                    {
                        ParamName = "P_" + _thisFilter.FilterName,
                        ParamDirection = ParameterDirection.Input,
                        MSSqlParamDataType = _thisFilter.DBType,
                        ParamSize = _thisFilter.ParamSize,
                        ParamValue = _thisFilter.ParamValue
                    };

                    EntryProcedureParameters.Add(thisParam);
                }
            }

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_VERIFY",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Char,
                ParamSize = 1,
                ParamValue = "T"
            });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            return Convert.ToInt64(returnSet.Rows[0]["Count"].ToString());
        }
        #endregion

        public static DataTable _GetPortletsSearch(IConnectToDB _Connect, DynamicModels.Output.Portlet_Search thisModel)
        {
            _DynamicOutputProcedures DOP = new _DynamicOutputProcedures();

            return DOP.GetPortletsSearch(_Connect, thisModel);
        }

        public DataTable GetPortletsSearch(IConnectToDB _Connect, DynamicModels.Output.Portlet_Search thisModel)
        {
            string ProcedureReturnType = "table";
            string ProcedureName = "SP_S_PORTLETS_SEARCH";

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            if (thisModel.P_TableorCount != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_TableorCount", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_TableorCount });
            }
            if (thisModel.P_DYNO_COL != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_DYNO_COL", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_DYNO_COL });
            }
            if (thisModel.P_SEARCH != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_SEARCH", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_SEARCH });
            }
            if (thisModel.P_WHERE != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_WHERE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_WHERE });
            }
            if (thisModel.P_STARTING_ROW != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_STARTING_ROW", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.P_STARTING_ROW });
            }
            if (thisModel.P_LENGTH_OF_SET != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_LENGTH_OF_SET", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.P_LENGTH_OF_SET });
            }
            if (thisModel.P_ORDER_BY != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_ORDER_BY", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_ORDER_BY });
            }
            if (thisModel.P_GET_LATEST != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_GET_LATEST", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.P_GET_LATEST });
            }
            if (thisModel.P_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_PORTLET_ID });
            }
            if (thisModel.P_EXCLUDE_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_EXCLUDE_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_PORTLET_ID });
            }
            if (thisModel.P_BASE_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_BASE_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_BASE_PORTLET_ID });
            }
            if (thisModel.P_EXCLUDE_BASE_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_EXCLUDE_BASE_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_BASE_PORTLET_ID });
            }
            if (thisModel.P_PREV_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_PREV_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_PREV_PORTLET_ID });
            }
            if (thisModel.P_EXCLUDE_PREV_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_EXCLUDE_PREV_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_PREV_PORTLET_ID });
            }
            if (thisModel.P_ENABLED != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_ENABLED });
            }
            if (thisModel.P_EXCLUDE_ENABLED != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_EXCLUDE_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_ENABLED });
            }
            if (thisModel.P_DT_CREATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_DT_CREATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_DT_CREATED });
            }
            if (thisModel.P_EXCLUDE_DT_CREATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_EXCLUDE_DT_CREATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_DT_CREATED });
            }
            if (thisModel.P_DT_UPDATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_DT_UPDATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_DT_UPDATED });
            }
            if (thisModel.P_EXCLUDE_DT_UPDATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_EXCLUDE_DT_UPDATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_DT_UPDATED });
            }
            if (thisModel.P_IDENTITY_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_IDENTITY_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_IDENTITY_ID });
            }
            if (thisModel.P_EXCLUDE_IDENTITY_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_EXCLUDE_IDENTITY_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_IDENTITY_ID });
            }
            if (thisModel.P_APPLICATION_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_APPLICATION_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_APPLICATION_ID });
            }
            if (thisModel.P_EXCLUDE_APPLICATION_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_EXCLUDE_APPLICATION_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_APPLICATION_ID });
            }
            if (thisModel.P_TITLE != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_TITLE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_TITLE });
            }
            if (thisModel.P_EXCLUDE_TITLE != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_EXCLUDE_TITLE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_TITLE });
            }
            if (thisModel.P_VERIFY != null)
            {
                EntryProcedureParameters.Add(new DBParameters { ParamName = "P_VERIFY", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.P_VERIFY });
            }

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            return returnSet;
        }

        public static DataTable _GetPortletPrivilegesSearch(IConnectToDB _Connect, long? base_portlet_id)
        {
            _DynamicOutputProcedures DOP = new _DynamicOutputProcedures();

            return DOP.GetPortletPrivilegesSearch(_Connect, base_portlet_id);
        }

        public DataTable GetPortletPrivilegesSearch(IConnectToDB _Connect, long? base_portlet_id)
        {
            string ProcedureReturnType = "table";
            string ProcedureName = "SP_S_VIEW_PORTLET_PRIVILEGES_SEARCH";

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            if (base_portlet_id != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_BASE_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = base_portlet_id });
            }
          
            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            return returnSet;
        }

        public static DataTable _GetPortletsSearchContainers(IConnectToDB _Connect, DynamicModels.Output.Portlet_Containers_Search thisModel)
        {
            _DynamicOutputProcedures COP = new _DynamicOutputProcedures();

            return COP.GetPortletsSearchContainers(_Connect, thisModel);
        }

        public DataTable GetPortletsSearchContainers(IConnectToDB _Connect, DynamicModels.Output.Portlet_Containers_Search thisModel)
        {
            string ProcedureReturnType = "table";
            string ProcedureName = "SP_S_PORTLET_CONTAINERS_SEARCH";

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            if (thisModel.P_TableorCount != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_TableorCount", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_TableorCount });
            }
            if (thisModel.P_DYNO_COL != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_DYNO_COL", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_DYNO_COL });
            }
            if (thisModel.P_SEARCH != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_SEARCH", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_SEARCH });
            }
            if (thisModel.P_WHERE != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_WHERE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_WHERE });
            }
            if (thisModel.P_STARTING_ROW != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_STARTING_ROW", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.P_STARTING_ROW });
            }
            if (thisModel.P_LENGTH_OF_SET != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_LENGTH_OF_SET", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.P_LENGTH_OF_SET });
            }
            if (thisModel.P_ORDER_BY != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_ORDER_BY", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_ORDER_BY });
            }
            if (thisModel.P_GET_LATEST != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_GET_LATEST", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.P_GET_LATEST });
            }
            if (thisModel.P_CONTAINER_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_CONTAINER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_CONTAINER_ID });
            }
            if (thisModel.P_EXCLUDE_CONTAINER_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_CONTAINER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_CONTAINER_ID });
            }
            if (thisModel.P_BASE_CONTAINER_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_BASE_CONTAINER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_BASE_CONTAINER_ID });
            }
            if (thisModel.P_EXCLUDE_BASE_CONTAINER_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_BASE_CONTAINER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_BASE_CONTAINER_ID });
            }
            if (thisModel.P_PREV_CONTAINER_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_PREV_CONTAINER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_PREV_CONTAINER_ID });
            }
            if (thisModel.P_EXCLUDE_PREV_CONTAINER_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_PREV_CONTAINER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_PREV_CONTAINER_ID });
            }
            if (thisModel.P_ENABLED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_ENABLED });
            }
            if (thisModel.P_EXCLUDE_ENABLED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_ENABLED });
            }
            if (thisModel.P_DT_CREATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_DT_CREATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_DT_CREATED });
            }
            if (thisModel.P_EXCLUDE_DT_CREATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_DT_CREATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_DT_CREATED });
            }
            if (thisModel.P_DT_UPDATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_DT_UPDATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_DT_UPDATED });
            }
            if (thisModel.P_EXCLUDE_DT_UPDATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_DT_UPDATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_DT_UPDATED });
            }
            if (thisModel.P_IDENTITY_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_IDENTITY_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_IDENTITY_ID });
            }
            if (thisModel.P_EXCLUDE_IDENTITY_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_IDENTITY_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_IDENTITY_ID });
            }
            if (thisModel.P_BASE_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_BASE_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_BASE_PORTLET_ID });
            }
            if (thisModel.P_EXCLUDE_BASE_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_BASE_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_BASE_PORTLET_ID });
            }
            if (thisModel.P_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_PORTLET_ID });
            }
            if (thisModel.P_EXCLUDE_PORTLET_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_PORTLET_ID });
            }
            if (thisModel.P_TITLE != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_TITLE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_TITLE });
            }
            if (thisModel.P_EXCLUDE_TITLE != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_TITLE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_TITLE });
            }
            if (thisModel.P_SIZE != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_SIZE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_SIZE });
            }
            if (thisModel.P_EXCLUDE_SIZE != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_SIZE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_SIZE });
            }
            if (thisModel.P_VERIFY != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_VERIFY", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.P_VERIFY });
            }

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            return returnSet;
        }
        
        public static DataTable _GetBaseReportsSearch(IConnectToDB _Connect, DynamicModels.Output.Root_Reports_Search thisModel)
        {
            _DynamicOutputProcedures COP = new _DynamicOutputProcedures();

            return COP.GetBaseReportsSearch(_Connect, thisModel);
        }
        public DataTable GetBaseReportsSearch(IConnectToDB _Connect, DynamicModels.Output.Root_Reports_Search thisModel)
        {
            string ProcedureReturnType = "table";
            string ProcedureName = "SP_S_ROOT_REPORTS_SEARCH";

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            if (thisModel.P_TableorCount != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_TableorCount", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_TableorCount });
            }
            if (thisModel.P_DYNO_COL != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_DYNO_COL", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_DYNO_COL });
            }
            if (thisModel.P_SEARCH != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_SEARCH", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_SEARCH });
            }
            if (thisModel.P_WHERE != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_WHERE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_WHERE });
            }
            if (thisModel.P_STARTING_ROW != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_STARTING_ROW", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.P_STARTING_ROW });
            }
            if (thisModel.P_LENGTH_OF_SET != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_LENGTH_OF_SET", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.P_LENGTH_OF_SET });
            }
            if (thisModel.P_ORDER_BY != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_ORDER_BY", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_ORDER_BY });
            }
            if (thisModel.P_GET_LATEST != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_GET_LATEST", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.P_GET_LATEST });
            }
            if (thisModel.P_ROOT_REPORT_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_ROOT_REPORT_ID });
            }
            if (thisModel.P_EXCLUDE_ROOT_REPORT_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_ROOT_REPORT_ID });
            }
            if (thisModel.P_BASE_ROOT_REPORT_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_BASE_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_BASE_ROOT_REPORT_ID });
            }
            if (thisModel.P_EXCLUDE_BASE_ROOT_REPORT_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_BASE_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_BASE_ROOT_REPORT_ID });
            }
            if (thisModel.P_PREV_ROOT_REPORT_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_PREV_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_PREV_ROOT_REPORT_ID });
            }
            if (thisModel.P_EXCLUDE_PREV_ROOT_REPORT_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_PREV_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_PREV_ROOT_REPORT_ID });
            }
            if (thisModel.P_ENABLED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_ENABLED });
            }
            if (thisModel.P_EXCLUDE_ENABLED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_ENABLED });
            }
            if (thisModel.P_DT_CREATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_DT_CREATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_DT_CREATED });
            }
            if (thisModel.P_EXCLUDE_DT_CREATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_DT_CREATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_DT_CREATED });
            }
            if (thisModel.P_DT_UPDATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_DT_UPDATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_DT_UPDATED });
            }
            if (thisModel.P_EXCLUDE_DT_UPDATED != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_DT_UPDATED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_DT_UPDATED });
            }
            if (thisModel.P_IDENTITY_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_IDENTITY_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_IDENTITY_ID });
            }
            if (thisModel.P_EXCLUDE_IDENTITY_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_IDENTITY_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_IDENTITY_ID });
            }
            if (thisModel.P_TEMPLATE_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_TEMPLATE_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_TEMPLATE_ID });
            }
            if (thisModel.P_EXCLUDE_TEMPLATE_ID != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_TEMPLATE_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_TEMPLATE_ID });
            }
            if (thisModel.P_TEMPLATE_NAME != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_TEMPLATE_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_TEMPLATE_NAME });
            }
            if (thisModel.P_EXCLUDE_TEMPLATE_NAME != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_TEMPLATE_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_TEMPLATE_NAME });
            }
            if (thisModel.P_REPORT_NAME != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_REPORT_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_REPORT_NAME });
            }
            if (thisModel.P_EXCLUDE_REPORT_NAME != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_REPORT_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_REPORT_NAME });
            }
            if (thisModel.P_PROCEDURE_NAME != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_PROCEDURE_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_PROCEDURE_NAME });
            }
            if (thisModel.P_EXCLUDE_PROCEDURE_NAME != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_EXCLUDE_PROCEDURE_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.P_EXCLUDE_PROCEDURE_NAME });
            }
            if (thisModel.P_VERIFY != null)
            {
                EntryProcedureParameters.Add(new DBParameters
                { ParamName = "P_VERIFY", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.P_VERIFY });
            }

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            return returnSet;
        }
       
    }
}
