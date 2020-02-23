using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revamp.Core.Services
{
    public interface IViewRenderService
    {
        Task<string> ReturnViewToStringAsync(CommonModels.MVCGetPartial thisModel);
    }
}
