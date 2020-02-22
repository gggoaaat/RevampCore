using Revamp.IO.Helpers.DataCompositions;
using Revamp.IO.Helpers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Helpers.DataCompositions.Factory
{
    public class DataCompositionFactory
    {
        public static IDCHelper Create(string templateName)
        {
            switch (templateName.ToLower())
            {
                case "csa":
                case "system":
                    //1
                    return new SystemDC();
                case "dynamic":
                    //100000000
                    return new DynamicDC();
                case "castgoop":
                    return new CastGoopDC();
                default:
                    throw new NotImplementedException(string.Format("You have requested an unknown type: ({0}) from ReportFactory", templateName));
            }
        }
    }
}
