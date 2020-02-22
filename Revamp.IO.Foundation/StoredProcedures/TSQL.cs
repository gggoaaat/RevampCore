using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Foundation.StoredProcedures
{
    public class TSQL
    {
        public List<CommandResult> GET_FORM_VALUES(IConnectToDB _Connect, List<CommandResult> _results)
        {
            ER_Query er_query = new ER_Query();
            string _Schema = ER_DB.GetSchema(_Connect);
            StringBuilder SQLin = new StringBuilder();

            SQLin.AppendLine("CREATE PROCEDURE " + _Schema + ".FORMS_GET_ENTRY_VALUES");
            SQLin.AppendLine("	@P_FORM_ID bigint ");
            SQLin.AppendLine("AS");
            SQLin.AppendLine("BEGIN");
            SQLin.AppendLine("	-- SET NOCOUNT ON added to prevent extra result sets from");
            SQLin.AppendLine("	-- interfering with SELECT statements.");
            SQLin.AppendLine("	SET NOCOUNT ON;");
            SQLin.AppendLine("");
            SQLin.AppendLine("	select * from (");
            SQLin.AppendLine("	    select 'FORMS_DAT_CHAR' source, fdc.FORMS_ID, fdc.GRIPS_ID, fdc.STAGES_ID, fdc.OBJ_PROP_SETS_ID, fdc.RENDITION, fdc.VALUE char_value, null numb_value,  null date_value, null deci_value, null opt_value, null file_value from " + _Schema + ".VW__FORMS_DAT_CHAR fdc where fdc.FORMS_ID = @P_FORM_ID");
            SQLin.AppendLine("	    union");
            SQLin.AppendLine("	    select 'FORMS_DAT_NUMB' source, fdn.FORMS_ID, fdn.GRIPS_ID, fdn.STAGES_ID, fdn.OBJ_PROP_SETS_ID, fdn.RENDITION, null char_value,  fdn.VALUE numb_value,  null date_value,null deci_value, null opt_value, null file_value from " + _Schema + ".VW__FORMS_DAT_NUMB fdn where fdn.FORMS_ID = @P_FORM_ID");
            SQLin.AppendLine("	    union");
            SQLin.AppendLine("	    select 'FORM_DAT_DATE'  source, fdd.FORMS_ID, fdd.GRIPS_ID, fdd.STAGES_ID, fdd.OBJ_PROP_SETS_ID, fdd.RENDITION, null char_value, null numb_value, fdd.VALUE date_value, null deci_value, null opt_value, null file_value from " + _Schema + ".VW__FORMS_DAT_DATE fdd where fdd.FORMS_ID = @P_FORM_ID");
            SQLin.AppendLine("	    union");
            SQLin.AppendLine("	    select 'FORM_DATE_DECI' source, fddc.FORMS_ID, fddc.GRIPS_ID, fddc.STAGES_ID, fddc.OBJ_PROP_SETS_ID, fddc.RENDITION, null char_value, null numb_value, null date_value, fddc.VALUE deci_value, null opt_value, null file_value from " + _Schema + ".VW__FORMS_DAT_DECI fddc where fddc.FORMS_ID = @P_FORM_ID");
            SQLin.AppendLine("	    union ");
            SQLin.AppendLine("	    select 'FORMS_DAT_OPT'  source, fdo.FORMS_ID, fdo.GRIPS_ID, fdo.STAGES_ID, fdo.OBJ_PROP_SETS_ID, fdo.RENDITION, null char_value, null numb_value, null date_value, null deci_value, fdo.VALUE opt_value, null file_value from " + _Schema + ".VW__FORMS_DAT_OPT fdo where fdo.FORMS_ID = @P_FORM_ID");
            SQLin.AppendLine("	    union ");
            SQLin.AppendLine("	    select 'FORMS_DAT_FILE'  source, fdf.FORMS_ID, fdf.GRIPS_ID, fdf.STAGES_ID, fdf.OBJ_PROP_SETS_ID, fdf.RENDITION, null char_value, null numb_value, null date_value, null deci_value, null opt_value, fdf.VALUE file_value from " + _Schema + ".VW__FORMS_DAT_FILE fdf where fdf.FORMS_ID = @P_FORM_ID");
            SQLin.AppendLine("	) ");
            SQLin.AppendLine("	ALL_VALUES");
            SQLin.AppendLine("END");

            CommandResult _result = new CommandResult();

            _result._StartTime = DateTime.Now;
            _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLin.ToString(), "Procedure " + _Schema + "." + "FORMS_GET_ENTRY_VALUES" + " created");
            _result._Successful = _result._Response.Contains("created") ? true : false;
            _result._EndTime = DateTime.Now;

            _results.Add(_result);

            return _results;
        }
    }
}
