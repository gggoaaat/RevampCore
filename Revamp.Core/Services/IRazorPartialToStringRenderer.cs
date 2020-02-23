using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revamp.Core.Services
{
    public interface IRazorPartialToStringRenderer
    {
        Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model);
    }
}
