using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Revamp.IO.Structs.Models
{
    //public class Object_Value
    //{
    //    public string _String { get; set; }
    //    public long? _Number { get; set; }
    //    public DateTime _Date { get; set; }
    //    public byte[] _File { get; set; }
    //}
    [Serializable]
    public class Object_Value
    {
        public string _String { get; set; }
        public long? _Number { get; set; }
        public DateTime? _Date { get; set; }
        public decimal? _Decimal { get; set; }
        public File_Object _File { get; set; }
    }
    [Serializable]
    public class File_Object
    {
        public byte[] _FileBytes { get; set; }
        public string FILE_NAME { get; set; }
        public long? FILE_SIZE { get; set; }
        public string CONTENT_TYPE { get; set; }
    }

    [Serializable]
    public class InsertObjectModel
    {
        public long? objects_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
    }
    
    [Serializable]
    public class ViewObjectModel
    {
        public long? Objects_ID { get; set; }
        public string enabled { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string Object_Layer { get; set; }
        public string Object_Type { get; set; }

        
    }

    [Serializable]
    public class ViewObjectLayersModel
    {
        public long? Object_Layers_ID { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string Object_Layer { get; set; }


    }


}