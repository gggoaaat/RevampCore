using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class SecurityApplicationRoles
    {
        public long? applications_sec_role_id { get; set; }
        public long? base_applications_sec_role_id { get; set; }
        public long? prev_applications_sec_role_id { get; set; }
        public Guid? applications_sec_role_uuid { get; set; }
        public Guid? base_applications_sec_role_uuid { get; set; }
        public Guid? prev_applications_sec_role_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? applications_id { get; set; }
        public long? roles_id { get; set; }
        public long? roles_id_f_roles { get; set; }
        public long? base_roles_id { get; set; }
        public long? prev_roles_id { get; set; }
        public Guid? roles_uuid { get; set; }
        public Guid? base_roles_uuid { get; set; }
        public Guid? prev_roles_uuid { get; set; }
        public long? identities_id_f_roles { get; set; }
        public string enabled_f_roles { get; set; }
        public DateTime? dt_created_f_roles { get; set; }
        public DateTime? dt_updated_f_roles { get; set; }
        public DateTime? dt_available_f_roles { get; set; }
        public DateTime? dt_end_f_roles { get; set; }
        public string object_type_f_roles { get; set; }
        public long? cores_id { get; set; }
        public string role_name { get; set; }
        public long? objects_id { get; set; }
        public long? base_objects_id { get; set; }
        public long? prev_objects_id { get; set; }
        public Guid? objects_uuid { get; set; }
        public Guid? base_objects_uuid { get; set; }
        public Guid? prev_objects_uuid { get; set; }
        public long? identities_id_f_objects { get; set; }
        public string enabled_f_objects { get; set; }
        public DateTime? dt_created_f_objects { get; set; }
        public DateTime? dt_updated_f_objects { get; set; }
        public DateTime? dt_available_f_objects { get; set; }
        public DateTime? dt_end_f_objects { get; set; }
        public string object_type_f_objects { get; set; }
        public string object_layer { get; set; }
        public long? applications_id_f_applications { get; set; }
        public long? base_applications_id { get; set; }
        public long? prev_applications_id { get; set; }
        public Guid? applications_uuid { get; set; }
        public Guid? base_applications_uuid { get; set; }
        public Guid? prev_applications_uuid { get; set; }
        public long? identities_id_f_applications { get; set; }
        public string enabled_f_applications { get; set; }
        public DateTime? dt_created_f_applications { get; set; }
        public DateTime? dt_updated_f_applications { get; set; }
        public DateTime? dt_available_f_applications { get; set; }
        public DateTime? dt_end_f_applications { get; set; }
        public string object_type_f_applications { get; set; }
        public long? cores_id_f_applications { get; set; }
        public string application_name { get; set; }
        public string rendition { get; set; }
        public string root_application { get; set; }
        public string application_link { get; set; }
        public string application_description { get; set; }
    }

    [Serializable]
    public class SecurityStageRoles
    {
        public long? stages_sec_role_id { get; set; }
        public long? base_stages_sec_role_id { get; set; }
        public long? prev_stages_sec_role_id { get; set; }
        public Guid? stages_sec_role_uuid { get; set; }
        public Guid? base_stages_sec_role_uuid { get; set; }
        public Guid? prev_stages_sec_role_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? stages_id { get; set; }
        public long? roles_id { get; set; }
        public long? roles_id_f_roles { get; set; }
        public long? base_roles_id { get; set; }
        public long? prev_roles_id { get; set; }
        public Guid? roles_uuid { get; set; }
        public Guid? base_roles_uuid { get; set; }
        public Guid? prev_roles_uuid { get; set; }
        public long? identities_id_f_roles { get; set; }
        public string enabled_f_roles { get; set; }
        public DateTime? dt_created_f_roles { get; set; }
        public DateTime? dt_updated_f_roles { get; set; }
        public DateTime? dt_available_f_roles { get; set; }
        public DateTime? dt_end_f_roles { get; set; }
        public string object_type_f_roles { get; set; }
        public long? cores_id { get; set; }
        public string role_name { get; set; }
        public long? objects_id { get; set; }
        public long? base_objects_id { get; set; }
        public long? prev_objects_id { get; set; }
        public Guid? objects_uuid { get; set; }
        public Guid? base_objects_uuid { get; set; }
        public Guid? prev_objects_uuid { get; set; }
        public long? identities_id_f_objects { get; set; }
        public string enabled_f_objects { get; set; }
        public DateTime? dt_created_f_objects { get; set; }
        public DateTime? dt_updated_f_objects { get; set; }
        public DateTime? dt_available_f_objects { get; set; }
        public DateTime? dt_end_f_objects { get; set; }
        public string object_type_f_objects { get; set; }
        public string object_layer { get; set; }
        public long? stages_id_f_stages { get; set; }
        public long? base_stages_id { get; set; }
        public long? prev_stages_id { get; set; }
        public Guid? stages_uuid { get; set; }
        public Guid? base_stages_uuid { get; set; }
        public Guid? prev_stages_uuid { get; set; }
        public long? identities_id_f_stages { get; set; }
        public string enabled_f_stages { get; set; }
        public DateTime? dt_created_f_stages { get; set; }
        public DateTime? dt_updated_f_stages { get; set; }
        public DateTime? dt_available_f_stages { get; set; }
        public DateTime? dt_end_f_stages { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public long? containers_id { get; set; }
        public string stage_link { get; set; }
    }

    [Serializable]
    public class IdentityRoles
    {
        public long? identities_roles_id { get; set; }
        public long? base_identities_roles_id { get; set; }
        public long? prev_identities_roles_id { get; set; }
        public Guid? identities_roles_uuid { get; set; }
        public Guid? base_identities_roles_uuid { get; set; }
        public Guid? prev_identities_roles_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public Guid? roles_uuid { get; set; }
        public long? objects_id { get; set; }
        public long? base_objects_id { get; set; }
        public long? prev_objects_id { get; set; }
        public Guid? objects_uuid { get; set; }
        public Guid? base_objects_uuid { get; set; }
        public Guid? prev_objects_uuid { get; set; }
        public long? identities_id_f_objects { get; set; }
        public string enabled_f_objects { get; set; }
        public DateTime? dt_created_f_objects { get; set; }
        public DateTime? dt_updated_f_objects { get; set; }
        public DateTime? dt_available_f_objects { get; set; }
        public DateTime? dt_end_f_objects { get; set; }
        public string object_type_f_objects { get; set; }
        public string object_layer { get; set; }
        public long? roles_id { get; set; }
        public long? base_roles_id { get; set; }
        public long? prev_roles_id { get; set; }
        public Guid? roles_uuid_f_roles { get; set; }
        public Guid? base_roles_uuid { get; set; }
        public Guid? prev_roles_uuid { get; set; }
        public long? identities_id_f_roles { get; set; }
        public string enabled_f_roles { get; set; }
        public DateTime? dt_created_f_roles { get; set; }
        public DateTime? dt_updated_f_roles { get; set; }
        public DateTime? dt_available_f_roles { get; set; }
        public DateTime? dt_end_f_roles { get; set; }
        public string object_type_f_roles { get; set; }
        public Guid? cores_uuid { get; set; }
        public long? cores_id { get; set; }
        public string role_name { get; set; }
        public long? identities_id_f_identities { get; set; }
        public long? base_identities_id { get; set; }
        public long? prev_identities_id { get; set; }
        public Guid? identities_uuid { get; set; }
        public Guid? base_identities_uuid { get; set; }
        public Guid? prev_identities_uuid { get; set; }
        public long? identities__id { get; set; }
        public string enabled_f_identities { get; set; }
        public DateTime? dt_created_f_identities { get; set; }
        public DateTime? dt_updated_f_identities { get; set; }
        public DateTime? dt_available_f_identities { get; set; }
        public DateTime? dt_end_f_identities { get; set; }
        public string object_type_f_identities { get; set; }
        public string user_name { get; set; }
        public string edipi { get; set; }
        public string email { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
    }
}
