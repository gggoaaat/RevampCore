using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{



    public class WorkflowModels
    {
    }

    [Serializable]
    public class flowObject
    {
        public Guid? workflows_uuid { get; set; }
        public Guid? base_workflows_uuid { get; set; }
        public Guid? prev_workflows_uuid { get; set; }
        public Guid? cores_uuid { get; set; }
        public Guid? applications_uuid { get; set; }
        public Guid? base_applications_uuid { get; set; }
        public List<flow> flow { get; set; }
    }

    [Serializable]
    public class flow
    {
        public string caller { get; set; }

        public List<actions> actions { get; set; }
    }

    [Serializable]
    public class actions
    {
        public string type { get; set; }
        public flowProps flow { get; set; }

    }

    [Serializable]
    public class flowProps
    {
        public string submission { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string name { get; set; }

        public string navigate { get; set; }

        public List<string> notification { get; set; }

        public string validate { get; set; }

        public string audit { get; set; }

        public List<Guid?> people { get; set; } = new List<Guid?>();
    }
}
