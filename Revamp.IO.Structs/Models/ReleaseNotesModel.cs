using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    class ReleaseNotesModel
    {
    }

    [Serializable]
    public class _FullReleasesModel
    {
        public List<_ReleaseNotesModel> Releases { get; set; }
    }

    [Serializable]
    public class _ReleaseNotesModel
    {
        public string _ReleaseTitle { get; set; }

        public string _VersionNum { get; set; }

        public string _ReleaseDate { get; set; }

        public List<string> _ApplicationsAffected { get; set; }

        public List<_ReleaseChangesModel> _ReleaseChanges { get; set; }
    }

     public enum _Change_Type { bug_fix, enhancement, feature, other, initial_release }

     public enum _Application { TIMMS, MAA, BSP, CSA, PROBE, COMMAND_PLAN, CORE, IDTS, IRIS, COMMON, DBOPS, OPB, LINKS, G1ANNUALAWARDS, G1CIVILIANAWARDS, G1CONGRESSIONALS, G1FOREIGNDECORATIONS, G1MILITARYAWARDS, G1TDY, G1UPO, G1UNITRECOGNITIONAWARDS }

    [Serializable]
    public class _ReleaseChangesModel
    {
        public _Application _application { get; set; }

        public string _change { get; set; }

        public string _date { get; set;}

        public _Change_Type _ChangeType { get; set; }
       
    }
   
}
