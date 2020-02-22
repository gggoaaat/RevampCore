using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Globalization;
//using System.Web.Security;
using System.Data;


namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class ContainersModel
    {
        public List<StageModel> stages { get; set; }
        public ContainerStruct Container { get; set; }
        public ViewContainerModel ViewContainer { get; set; }

        [Serializable]
        public class ContainerActivity
        {
            public DateTime DT_created { get; set; }
            public long?count { get; set; }

        }

        [Serializable]
        public class ContainerActivityDates
        {
            public DateTime start { get; set; }
            public DateTime end { get; set; }
            public long?app_id { get; set; }
        }

        [Serializable]
        public class ContainerSubmissions
        {
            public long?draw { get; set; }
            public long?recordsTotal { get; set; }
            public long?recordsFiltered { get; set; }
            public List<string> data { get; set; }

        }

        [Serializable]
        public class ContainerModels
        {
           
            public long?containers_id { get; set; }

            public ContainerTableModel ContainerTable { get; set; }
            public ViewContainerModel ContainerView { get; set; }

            public List<ContainerActivity> ActivityCounts { get; set; }

            public virtual List<StageModels> stages { get; set; }
            public NotificationsModel SessionNotification { get; set; }

        }

        [Serializable]
        public class ContainerTableModel
        {
            public long?containers_id { get; set; }
            public long?container_name { get; set; }
            public string enabled { get; set; }
            public DateTime dt_created { get; set; }
            public DateTime? dt_updated { get; set; }
            public DateTime? dt_available { get; set; }
            public DateTime? dt_end { get; set; }
            public string object_type { get; set; }
            
           
        }

        [Serializable]
        public class CreateContainerModel
        {
            [Required]
            public string db_platform { get; set; }
            public string connOwner { get; set; }
           
        }

        [Serializable]
        public class ViewContainerModel
        {
            public long?containers_id { get; set; }
            public DateTime? dt_created { get; set; }
            public DateTime? dt_available { get; set; }
            public DateTime? dt_end { get; set; }
            public string object_type { get; set; }
            public string container_name { get; set; }
            public string object_layer { get; set; }
            public string enabled { get; set; }

        }

        [Serializable]
        public class ContainerCharModel
        {
            public long?containers_dat_char_id { get; set; }
            public string enabled { get; set; }
            public DateTime dt_created { get; set; }
            public DateTime dt_updated { get; set; }
            public DateTime dt_available { get; set; }
            public DateTime dt_end { get; set; }
            public long?rendition { get; set; }
            public long?Containers_id { get; set; }
            public long?Applications_id { get; set; }
            public long?stages_id { get; set; }
            public long?grips_id { get; set; }
            public long?object_sets_id { get; set; }
            public long?obj_prop_sets_id { get; set; }
            public long?identities_id { get; set; }
            public string value { get; set; }
        }

        [Serializable]
        public class ContainerNumbModel
        {
            public long?containers_dat_numb_id { get; set; }
            public string enabled { get; set; }
            public DateTime dt_created { get; set; }
            public DateTime dt_updated { get; set; }
            public DateTime dt_available { get; set; }
            public DateTime dt_end { get; set; }
            public long?Applications_id { get; set; }
            public long?Containers_id { get; set; }
            public long?stages_id { get; set; }
            public long?grips_id { get; set; }
            public long?object_sets_id { get; set; }
            public long?obj_prop_sets_id { get; set; }
            public long?identities_id { get; set; }
            public long?value { get; set; }
        }

        [Serializable]
        public class ContainerDateModel
        {

            public long?containers_dat_date_id { get; set; }
            public string enabled { get; set; }
            public DateTime dt_created { get; set; }
            public DateTime dt_updated { get; set; }
            public DateTime dt_available { get; set; }
            public DateTime dt_end { get; set; }
            public long?Applications_id { get; set; }
            public long?Containers_id { get; set; }
            public long?stages_id { get; set; }
            public long?grips_id { get; set; }
            public long?object_sets_id { get; set; }
            public long?obj_prop_sets_id { get; set; }
            public long?identities_id { get; set; }
            public DateTime value { get; set; }
        }

        [Serializable]
        public class ContainerStruct
        {

            public ContainerTableModel container { get; set; }
            public ContainerCharModel containerCharMeta { get; set; }
            public ContainerNumbModel containerNumbMeta { get; set; }
            public ContainerDateModel containerDateMeta { get; set; }
            public ViewContainerModel ContainerView { get; set; }         
            public ObjectSetModels containerObjectSets { get; set; }

           

        }
    }
}