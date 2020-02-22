using System;
using System.Collections.Generic;

namespace Revamp.IO.Structs.Models.RevampSystem
{
    public static class Dictionary
    {
        [Serializable]
        public class AddTable
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "ER_TABLES";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public long? I_BASE_ER_TABLES_ID { get; set; } = 0;
            public long? I_PREV_ER_TABLES_ID { get; set; } = 0;
            public Guid? I_BASE_ER_TABLES_UUID { get; set; }
            public Guid? I_PREV_ER_TABLES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_TABLE_SCHEMA { get; set; }
            public string I_TABLE_NAME { get; set; }
            public string I_TABLE_TYPE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_TABLES_ID { get; set; }
            public Guid? O_ER_TABLES_UUID { get; set; }

        }

        [Serializable]
        public class AddPrimaryKey
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "ER_PRIMARY_KEYS";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_ER_PRIMARY_KEYS_ID { get; set; } = 0;
            public long? I_PREV_ER_PRIMARY_KEYS_ID { get; set; } = 0;
            public Guid? I_BASE_ER_PRIMARY_KEYS_UUID { get; set; }
            public Guid? I_PREV_ER_PRIMARY_KEYS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_KEY_NAME { get; set; }
            public string I_TABLE_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_PRIMARY_KEYS_ID { get; set; }
            public Guid? O_ER_PRIMARY_KEYS_UUID { get; set; }

            public List<AddPrimaryKeyColumns> Columns { get; set; }
        }

        [Serializable]
        public class AddPrimaryKeyColumns
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "ER_PK_COLUMNS";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_ER_PK_COLUMNS_ID { get; set; } = 0;
            public long? I_PREV_ER_PK_COLUMNS_ID { get; set; } = 0;
            public Guid? I_BASE_ER_PK_COLUMNS_UUID { get; set; }
            public Guid? I_PREV_ER_PK_COLUMNS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public long? I_ER_PRIMARY_KEYS_ID { get; set; }
            public string I_COLUMN_NAME { get; set; }
            public string I_COLUMN_DATATYPE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_PK_COLUMNS_ID { get; set; }
            public Guid? O_ER_PK_COLUMNS_UUID { get; set; }
        }

        [Serializable]
        public class AddForeignKey
        {

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_ER_FOREIGN_KEYS";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 



            public long? I_BASE_ER_FOREIGN_KEYS_ID { get; set; } = 0;
            public long? I_PREV_ER_FOREIGN_KEYS_ID { get; set; } = 0;
            public Guid? I_BASE_ER_FOREIGN_KEYS_UUID { get; set; }
            public Guid? I_PREV_ER_FOREIGN_KEYS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_KEY_NAME { get; set; }
            public string I_TABLE_SCHEMA{ get; set; }
            public string I_TABLE_NAME { get; set; }
            public string I_PARENT_SCHEMA { get; set; }
            public string I_PARENT_TABLE_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_FOREIGN_KEYS_ID { get; set; }
            public Guid? O_ER_FOREIGN_KEYS_UUID { get; set; }

            public List<AddForeignKeyColumn> V_FK_ColumnsList1 { get; set; }
        }

        [Serializable]
        public class AddForeignKeyColumn
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "ER_FK_COLUMNS";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_ER_FK_COLUMNS_ID { get; set; } = 0;
            public long? I_PREV_ER_FK_COLUMNS_ID { get; set; } = 0;
            public Guid? I_BASE_ER_FK_COLUMNS_UUID { get; set; }
            public Guid? I_PREV_ER_FK_COLUMNS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public long? I_ER_FOREIGN_KEYS_ID { get; set; }
            public string I_COLUMN_NAME { get; set; }
            public string I_PARENT_COLUMN_NAME { get; set; }
            public string I_COLUMN_DATATYPE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_FK_COLUMNS_ID { get; set; }
            public Guid? O_ER_FK_COLUMNS_UUID { get; set; }
        }

        [Serializable]
        public class AddUniqueKey
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_ER_UNIQUE_KEYS";
            public long? I_BASE_ER_UNIQUE_KEYS_ID { get; set; } = 0;
            public long? I_PREV_ER_UNIQUE_KEYS_ID { get; set; } = 0;
            public Guid? I_BASE_ER_UNIQUE_KEYS_UUID { get; set; }
            public Guid? I_PREV_ER_UNIQUE_KEYS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_KEY_NAME { get; set; }
            public string I_TABLE_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_UNIQUE_KEYS_ID { get; set; }
            public Guid? O_ER_UNIQUE_KEYS_UUID { get; set; }

            public List<AddUniqueKeyColumn> V_UK_ColumnsList1 { get; set; }
        }

        [Serializable]
        public class AddUniqueKeyColumn
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_ER_UK_COLUMNS";
            public long? I_BASE_ER_UK_COLUMNS_ID { get; set; } = 0;
            public long? I_PREV_ER_UK_COLUMNS_ID { get; set; } = 0;
            public Guid? I_BASE_ER_UK_COLUMNS_UUID { get; set; }
            public Guid? I_PREV_ER_UK_COLUMNS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public long? I_ER_UNIQUE_KEYS_ID { get; set; }
            public string I_COLUMN_NAME { get; set; }
            public string I_COLUMN_DATATYPE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_UK_COLUMNS_ID { get; set; }
            public Guid? O_ER_UK_COLUMNS_UUID { get; set; }

        }

        [Serializable]
        public class AddView
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_ER_VIEWS";
            public long? I_BASE_ER_VIEWS_ID { get; set; } = 0;
            public long? I_PREV_ER_VIEWS_ID { get; set; } = 0;
            public Guid? I_BASE_ER_VIEWS_UUID { get; set; }
            public Guid? I_PREV_ER_VIEWS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_VIEW_NAME { get; set; }
            public string I_VIEW_TYPE { get; set; }
            public string I_VIEWDATA { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_VIEWS_ID { get; set; }
            public Guid? O_ER_VIEWS_UUID { get; set; }
        }

        [Serializable]
        public class AddIndex
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_ER_INDEXES";
            public long? I_BASE_ER_INDEXES_ID { get; set; } = 0;
            public long? I_PREV_ER_INDEXES_ID { get; set; } = 0;
            public Guid? I_BASE_ER_INDEXES_UUID { get; set; }
            public Guid? I_PREV_ER_INDEXES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_INDEX_NAME { get; set; }
            public string I_INDEX_TYPE { get; set; }
            public string I_SOURCE_NAME { get; set; }
            public string I_SOURCE_TYPE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_INDEXES_ID { get; set; }
            public Guid? O_ER_INDEXES_UUID { get; set; }
        }

        [Serializable]
        public class DropView
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_D_ER_VIEWS";
            public long? I_ER_VIEWS_ID { get; set; }
            public long? O_ER_VIEWS_ID { get; set; }
        }
    }
}
