using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;

[Serializable]
public class CreateDBStruct
{
    public string DB_PLATFORM { get; set; }
    public string connRoot { get; set; }
    public string connAuth { get; set; }
    public string connOwner { get; set; }
    public string SystemName { get; set; }
    public string AuthorizedUser { get; set; }
    public string Password { get; set; }
    public string DBServerName { get; set; }
    public string Debug { get; set; }
    public string ServerMapPath { get; set; }
    public string InstallType { get; set; }
    public bool IntegratedSecurity { get; set; }
    public bool useExistingUser { get; set; }
}

[Serializable]
public class sqlSelectStructure
{
    //private Boolean __HasFrom = false;
    public Boolean _HasFrom { get; set; }
    public string _TableName { get; set; }
    public string _ChildTableName { get; set; }
    public string _TableAlias { get; set; }
    //private Boolean __HasAggregateFunc = false;
    public Boolean _HasAggregateFunc { get; set; }
    public string _AggregateFunctions { get; set; }
    public List<ColumnStructure> _IncludeColumns { get; set; }
    public List<ColumnStructure> _ExcludeColumns { get; set; }
    //private Boolean __HasJoin = false;
    public Boolean _HasJoin { get; set; }
    public string _JoinClause { get; set; }
    public string _JoinOn { get; set; }
    //private Boolean __HasWhere = false;
    public Boolean _HasWhere { get; set; }
    public List<WhereStructure> _WhereClause { get; set; }
    //private Boolean __HasGroupBy = false;
    public Boolean _HasGroupBy { get; set; }
    public string _GroupByClause { get; set; }
    //private Boolean __HasHaving = false;
    public Boolean _HasHaving { get; set; }
    public string _HavingClause { get; set; }
}

[Serializable]
public class WhereStructure
{
    public string OpenParentheses { get; set; }
    public string WhereClause { get; set; }
    public string CloseParentheses { get; set; }
    public string ContinuingOperator { get; set; }
}

[Serializable]
public class DBParameters
{
    public string ParamName { get; set; }
    //public OracleDbType OracleParamDataType { get; set; }
    public SqlDbType MSSqlParamDataType { get; set; }
    public ParameterDirection ParamDirection { get; set; }
    public int ParamSize { get; set; } = -1;
    public object ParamValue { get; set; }
    public Boolean ParamReturn { get; set; }

    public Boolean DynamicallyAssign { get; set; }

    public string DynamicPartner { get; set; }

    public Boolean throwInHolder { get; set; }
    public bool AvoidAntiXss { get; set; }
}

[Serializable]
public class ColumnStructure
{
    public string _Name { get; set; } = "";
    public string _DataType { get; set; }
    public string _DefaultValue { get; set; }
    public Boolean _IsNull { get; set; }
    public string _Alias { get; set; } = "";
    public string _Table { get; set; }
    public string _LiteralBefore { get; set; }
    public string _LiteralAfter { get; set; }
    public string _SelectSQL { get; set; }
}

[Serializable]
public class TableStructure
{
    public List<ColumnStructure> ColumnStructure { get; set; }
    public List<ColumnStructure> RelationshipColumnStructure { get; set; }
    public string _TableName { get; set; }
}

[Serializable]
public class sqlProcedureStructure
{
    public string _ProcedureName { get; set; }
    public List<sqlProcedureParameterStructure> _Parameters { get; set; }
    public List<sqlProcedureLineStructure> _Declare { get; set; }
    public List<sqlProcedureLineStructure> _Body { get; set; }
    public List<sqlProcedureLineStructure> _Exception { get; set; }

}

[Serializable]
public class sqlProcedureParameterStructure
{
    public string _Name { get; set; }
    public ParameterDirection _Direction { get; set; }
    public string _DataType { get; set; }
    public string _DefaultValue { get; set; }
}

[Serializable]
public class sqlProcedureLineStructure
{
    public string LineEntry { get; set; }
}

[Serializable]
public class ER_Env
{
    public DataTable ProcDataTable()
    {
        // Create a new DataTable.
        DataTable table = new DataTable("ReturnData");
        DataColumn column;

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
        table.Columns.Add(column);

        // Create second column and add to the DataTable.
        column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "ChildType";
        column.AutoIncrement = false;
        column.Caption = "ChildType";
        column.ReadOnly = false;
        column.Unique = false;
        table.Columns.Add(column);

        // Create third column and add to the DataTable.
        column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "ChildItem";
        column.AutoIncrement = false;
        column.Caption = "ChildItem";
        column.ReadOnly = false;
        column.Unique = false;
        table.Columns.Add(column);

        // Create fourth column and add to the DataTable.
        column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "ChildValue";
        column.AutoIncrement = false;
        column.Caption = "ChildValue";
        column.ReadOnly = false;
        column.Unique = false;
        table.Columns.Add(column);

        column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "ChildSQL";
        column.AutoIncrement = false;
        column.Caption = "ChildSQL";
        column.ReadOnly = false;
        column.Unique = false;
        table.Columns.Add(column);
        

        return table;
    }


}
