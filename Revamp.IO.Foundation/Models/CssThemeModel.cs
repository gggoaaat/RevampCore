using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Foundation.Models
{
    public class CssThemeModel
    {
        public CssThemeModels _Theme { get; set; }
    }


    [Serializable]
    public class CssThemeModels
    {
        public Body _Body { get; set;}
        public theme_panel _ThemePanel { get; set; }
        public Page_Header _PageHeader { get; set; }

        public class Body
        {
            public List<CSSProperty> _Properties { get; set; }
        }

        public List<CSSStruct> _CustomCSS { get; set; }

        

        public class theme_panel
        {
            public List<CSSProperty> _Properties { get; set; }
            public Toggler __toggler { get; set; }
        }

        public class portlet_title
        {
            public tag_a _a {get; set;}
        }

        public class tag_a
        {
            public List<CSSProperty> _Properties { get; set; }
        }

        public class Page_Header
        {
            public List<CSSProperty> _Properties { get; set; }
            public class NavBar
            {
                public List<CSSProperty> _Properties { get; set; }
            }
        }

        public class PageSidebar
        {

        }

        public class QuickSidebar
        {
             

            public class Content
            {

            }
        }

       public class Footer
       {

       }

       public class Toggler
       {
           public class hover
           {
               public List<CSSProperty> _Properties { get; set; }
           }
       }

       
    }

    public class CSSProperty
    {
        public string type { get; set; }
        public string value { get; set; }

        public bool important { get; set; }
    }

    public class CSSStruct
    {
        public string objectSelector { get; set; }

        public List<CSSProperty> objectProperties { get; set; }
    }
}
