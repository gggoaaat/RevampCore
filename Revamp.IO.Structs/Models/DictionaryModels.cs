using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class ERDictionaryModels
    {
        public List<TablesDictionary> tables { get; set; }
        public List<ViewTablesDictionary> tablesView { get; set; }
        public List<ViewsDictionary> views { get; set; }
        public List<ViewViewsDictionary> viewsView { get; set; }
        public List<PrimaryKeysDictionary> pkeys { get; set; }
        public List<ViewPrimaryKeysDictionary> pkeysViews { get; set; }
        public List<UniqueKeysDictionary> ukeys { get; set; }
        public List<ViewUniqueKeysDictionary> ukeysViews { get; set; }
        public List<ForeignKeysDictionary> fkeys { get; set; }
        public List<ViewForeignKeysDictionary> fkeysViews { get; set; }
        public List<ViewIndexesDictionary> indexes { get; set; }

    }

    [Serializable]
    public class TablesDictionary
    {
        public long? er_tables_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }     
        public DateTime? dt_end { get; set; }     
        public string table_name  { get; set; }
        public string table_type { get; set; }

    }

    [Serializable]
    public class ViewTablesDictionary
    {
        public long? er_tables_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }     
        public DateTime? dt_end { get; set; }     
        public string table_name  { get; set; }
        public string table_type { get; set; }

    }

    [Serializable]
    public class ViewsDictionary
    {
        public long? er_views_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string view_name { get; set; }
        public string view_type { get; set; }
    }

    [Serializable]
    public class ViewViewsDictionary
    {
        public long? er_views_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string view_name { get; set; }
        public string view_type { get; set; }
    }

    [Serializable]
    public class IndexesDictionary
    {
        public long? er_indexes_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string index_name { get; set; }
        public string index_type { get; set; }
        public string source_name { get; set; }
        public string source_type { get; set; }

        //public virtual List<PKColumnsDictionary> columns { get; set; }
    }

    [Serializable]
    public class ViewIndexesDictionary
    {
        public long? er_indexes_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string index_name { get; set; }
        public string index_type { get; set; }
        public string source_name { get; set; }
        public string source_type { get; set; }

        //public virtual List<PKColumnsDictionary> columns { get; set; }
    }

    [Serializable]
    public class PrimaryKeysDictionary
    {
        public long? er_primary_keys_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string key_name { get; set; }
        public string table_name { get; set; }

        public virtual List<PKColumnsDictionary> columns { get; set; }
    }

    [Serializable]
    public class ViewPrimaryKeysDictionary
    {
        public long? er_primary_keys_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string key_name { get; set; }
        public string table_name { get; set; }

        public virtual List<ViewPKColumnsDictionary> columns { get; set; }
    }

    [Serializable]
    public class PKColumnsDictionary
    {
        public long? er_pk_columns_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? er_primary_keys_id { get; set; }
        public string column_name { get; set; }
        public string column_datatype { get; set; }
    }

    [Serializable]
    public class ViewPKColumnsDictionary
    {
        public long? er_pk_columns_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? er_primary_keys_id { get; set; }
        public string column_name { get; set; }
        public string column_datatype { get; set; }
    }

    [Serializable]
    public class UniqueKeysDictionary
    {
        public long? er_unique_keys_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string key_name { get; set; }
        public string table_name { get; set; }

        public virtual List<UKColumnsDictionary> columns { get; set; }
    }

    [Serializable]
    public class ViewUniqueKeysDictionary
    {
        public long? er_unique_keys_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string key_name { get; set; }
        public string table_name { get; set; }

        public virtual List<ViewUKColumnsDictionary> columns { get; set; }
    }

    [Serializable]
    public class UKColumnsDictionary
    {
        public long? er_uk_columns_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? er_unique_keys_id { get; set; }
        public string column_name { get; set; }
        public string column_datatype { get; set; }
    }

    [Serializable]
    public class ViewUKColumnsDictionary
    {
        public long? er_uk_columns_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? er_unique_keys_id { get; set; }
        public string column_name { get; set; }
        public string column_datatype { get; set; }
    }

    [Serializable]
    public class ForeignKeysDictionary
    {
        public long? er_foreign_keys_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string key_name { get; set; }
        public string table_name { get; set; }
        public string parent_table_name { get; set; }

        public virtual List<FKColumnsDictionary> columns { get; set; }
    }

    [Serializable]
    public class ViewForeignKeysDictionary
    {
        public long? er_foreign_keys_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string key_name { get; set; }
        public string table_name { get; set; }
        public string parent_table_name { get; set; }

        public virtual List<ViewFKColumnsDictionary> columns { get; set; }
    }

    [Serializable]
    public class FKColumnsDictionary
    {
        public long? er_fk_columns_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public DateTime? er_foreign_keys_id { get; set; }
        public string column_name { get; set; }
        public string parent_column_name { get; set; }
        public string column_datatype { get; set; }
    }

    [Serializable]
    public class ViewFKColumnsDictionary
    {
        public long? er_fk_columns_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public DateTime? er_foreign_keys_id { get; set; }
        public string column_name { get; set; }
        public string parent_column_name { get; set; }
        public string column_datatype { get; set; }
    }

}