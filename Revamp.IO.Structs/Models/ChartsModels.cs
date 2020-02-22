using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class ERChartsModels
    {
        public List<DynamicChartModel> Charts { get; set; }
    }

    [Serializable]
    public class DynamicChartModel
    {
        public string spanLength { get; set; }
        public string ChartType { get; set; }
        public string ChartTitle { get; set; }
        public string IconPath { get; set; }
        public string DivId { get; set; }
        public int TotalPoints { get; set; }
        public string JSONURL { get; set; }
        public string JSONURLUpdate { get; set; }
        public ChartLines lines { get; set; }
        public YAxis YAxis { get; set; }
        public XAxis XAxis { get; set; }
        public int rowstosplice { get; set; }
        public string tickFormatter  { get; set; }
    }

    [Serializable]
    public class ChartLines {
        public bool show { get; set; }
        public decimal linewidth { get; set; }
        public bool fillpublic  { get; set; }         
    }

    [Serializable]
    public class YAxis
    {
        public bool show { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public string tickColor { get; set; }
        public string tickFormater { get; set; }

    }

    [Serializable]
    public class XAxis
    {
        public bool show { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int ticksize { get; set; }
    }
}