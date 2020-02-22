using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class SharedModels
    {
    }

    [Serializable]
    public class DatatablePartialView
    {
        public string typeofView { get; set; }
        public string nameofView { get; set; }
    }

    [Serializable]
    public class LeadCaptureForm
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string jobtitle { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string employees { get; set; }
        public string appidea { get; set; }
        public string company { get; set; }
    }
}