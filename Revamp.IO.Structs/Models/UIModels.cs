using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Revamp.IO.Structs.Models
{
    public class UIModels
    {
        [Serializable]
        public class SideBarFilter
        {
            public string name { get; set; }
            public List<NavTabs> navTabs { get; set; }

        }

        [Serializable]
        public class NavTabs
        {
            public string id { get; set; }
            public string title { get; set; }
            public string innerHtml { get; set; }
        }

        [Serializable]
        public class HtmlControlAttributes
        {
            public string id { get; set; }
            public string name { get; set; }
            public string label { get; set; }
            public string defaultValue { get; set; }
            public string propertyValue { get; set; }
            public string propertyType { get; set; }
            public byte[] image { get; set; }
            public string url { get; set; }
            public List<SelectListItem> multiItemList { get; set; }
        }
    }
}
