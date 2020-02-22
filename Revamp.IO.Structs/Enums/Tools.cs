using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Enums
{
    [Serializable]
    public enum EventLogType
    {
        error, exception, information, general, failureaudit, successaudit, success, warning
    }

    public enum AuditEventType
    {
        Create,
        Edit,
        Delete,
        AccessForbidden,
        Search
    }

    public enum FolderStatus
    {
        Success,
        Failed,
        Exists
    }

    public enum PermissionsRequestType
    {
        ActionResult,
        JsonResult
    }
}
