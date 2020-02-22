using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs
{
    [Serializable]
    public class FormWizardModel
    {
        [Serializable]
        public enum CallType
        {
            JavaScript, Razor
        }

        public CallType callType { get; set; }
        public string name { get; set; }
        public List<ListStructure> listStructure { get; set; }
    }

    [Serializable]
    public class UITabModel
    {
        [Serializable]
        public enum CallType
        {
            JavaScript, Razor
        }

        public CallType callType { get; set; }
        public string name { get; set; }
        public string portletTitle { get; set; }
        public List<ListStructure> listStructure { get; set; }
    }

    [Serializable]
    public class ListStructure
    {
        public string name { get; set; }
        public string prettyName { get; set; }
        public string symbolName { get; set; }
        public string spanText { get; set; }
        public string content { get; set; }
        public bool activeContent { get; set; }
    }
}
