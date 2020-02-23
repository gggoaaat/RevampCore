using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revamp.Core.Services
{
    public interface IMvcApplication 
    {
        Task<string> ReturnViewToStringAsync(CommonModels.MVCGetPartial thisModel);
    }
}
