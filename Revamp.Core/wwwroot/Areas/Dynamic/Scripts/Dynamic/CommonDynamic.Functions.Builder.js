var clauseBuilder = function clauseBuilder() {

    var StartNew = function (model) {
        $('.clauseContainer').html(ClauseRow(model));

        $('#coreReportClauseModal .clauseContainer li:eq(0) .remove-clause').remove();
    };

    var Append = function (model) {
        $('.clauseContainer').append(ClauseRow(model));
    };

    var Trash = function (model) {
        $('.clauseContainer').append(ClauseRow(model));
    };

    function ClauseRow(model) {
        var clauseRowHTML = '';

        clauseRowHTML += '<li>';
        clauseRowHTML += '  <div class="row">';
        clauseRowHTML += '      <div class="leftsideclause col-md-4"><select name="clauseColumn" style="width:100%;">' + GetColumns(model) + '</select></div>';
        clauseRowHTML += '      <div class="centerclause col-md-1"><select name="clauseConditions[]" style="width: 100%;">' + GetConditionTypesForDBType(model) + '</select></div>';
        clauseRowHTML += '      <div class="rightsideclause col-md-6">' + GetValueInputsBasedOnCondition(model) + '</div>';
        clauseRowHTML += '      <div class="col-md-1"><a class="add-clause"><i class="fa fa-plus"/></a><a class="remove-clause"><i class="fa fa-trash" /></a></div>';
        clauseRowHTML += '  </div>';
        clauseRowHTML += '</li>';

        //$('.clauseContainer').html(clauseRowHTML)

        return clauseRowHTML;
    }

    function GetColumns(model) {
        var DropDownHTML = '';

        if (mGlobal.page[model.page].CurrentBaseFiltersModel != undefined) {
            var theseColumns = mGlobal.page[model.page].CurrentBaseFiltersModel;

            DropDownHTML += '<option value="None" data-dbtype="None">Select Column</option>';
            for (var i = 0; i < theseColumns.length; i++) {
                var thisColumnDBType = ConverDBTypeToText({ thisDBType: theseColumns[i].DBType });
                DropDownHTML += '<option value="' + theseColumns[i].FilterName + '" data-dbtype="' + thisColumnDBType + '">' + theseColumns[i].FilterName + ' (' + thisColumnDBType + ')</option>';
            }
        }

        if (model.leftside != undefined && model.leftside != '') {
            DropDownHTML = DropDownHTML.replace('value="' + model.leftside, 'selected value="' + model.leftside);
        }

        return DropDownHTML;
    }

    function ConverDBTypeToText(model) {
        mGlobal.variable.DBTypes = mGlobal.variable.DBTypes == undefined ? xGetDBTypes() : mGlobal.variable.DBTypes;
        var AllDBTypes = mGlobal.variable.DBTypes;

        var thisDBTypeText = _.filter(AllDBTypes, function (o) { return o.id == model.thisDBType; })[0].dbType;

        return thisDBTypeText;
    }

    function GetConditionTypesForDBType(model) {
        var optionHTML = '';

        model.dbtype = model.dbtype == undefined ? '' : model.dbtype;

        switch (model.dbtype.toLowerCase()) {
            case 'bigint':
            case 'int':
                optionHTML += '<option value="equals"> = </option>';
                optionHTML += '<option value="greater than"> > </option>';
                optionHTML += '<option value="less than"> < </option>';
                optionHTML += '<option value="greater than or equal"> >= </option>';
                optionHTML += '<option value="less than or equal"> <= </option>';
                optionHTML += '<option value="not equal"> != </option>';
                optionHTML += '<option value="in"> in </option>';
                optionHTML += '<option value="not in"> not in </option>';
                optionHTML += '<option value="between"> between </option>';
                break;
            case 'datetime':
                optionHTML += '<option value="equals"> = </option>';
                optionHTML += '<option value="greater than"> > </option>';
                optionHTML += '<option value="less than"> < </option>';
                optionHTML += '<option value="greater than or equal"> >= </option>';
                optionHTML += '<option value="less than or equal"> <= </option>';
                optionHTML += '<option value="not equal"> != </option>';
                optionHTML += '<option value="between"> between </option>';
                break;
            default:
                optionHTML += '<option value="like"> like </option>';
                optionHTML += '<option value="equals"> = </option>';
                optionHTML += '<option value="greater than"> > </option>';
                optionHTML += '<option value="less than"> < </option>';
                optionHTML += '<option value="greater than or equal"> >= </option>';
                optionHTML += '<option value="less than or equal"> <= </option>';
                optionHTML += '<option value="not equal"> != </option>';
                optionHTML += '<option value="in"> in </option>';
                optionHTML += '<option value="not in"> not in </option>';
                optionHTML += '<option value="not like"> not like </option>';
                break;
        }

        if (model.condition != undefined && model.condition != '') {
            optionHTML = optionHTML.replace('value="' + model.condition, 'selected value="' + model.condition);
        }

        return optionHTML;
    }

    function GetValueInputsBasedOnCondition(model) {
        var valueHTML = '';
        model.dbtype = model.dbtype == undefined ? '' : model.dbtype;
        model.condition = model.condition == undefined ? '' : model.condition;

        model.right1 = model.right1 == undefined ? '' : model.right1;
        model.right2 = model.right2 == undefined ? '' : model.right2;

        switch (model.condition.toLowerCase()) {
            default:
                switch (model.dbtype.toLowerCase()) {
                    default:
                        valueHTML += '<div class="col-md-12"><input name="clauseValue[]" style="width: 100%;" value="' + model.right1 + '"></input></div>';
                        break;
                    case 'datetime':
                        valueHTML += '<div class="col-md-12"><input class="date-picker" name="clauseValue[]" style="width: 100%;" value="' + model.right1 + '"></input></div>';
                        break;
                }
                break;
            case 'between':

                switch (model.dbtype.toLowerCase()) {
                    default:
                        valueHTML += '<div class="col-md-5">' +
                            '<input name="clauseValue[]" style="width: 100%;" value="' + model.right1 + '"></input></div>' +
                            '<div class="col-md-2">between</div>' +
                            '<div class="col-md-5">' +
                            '<input name="clauseValue2[]" style="width: 100%;" value="' + model.right2 + '"></input></div>';
                        break;
                    case 'datetime':
                        valueHTML += '<div class="col-md-5">' +
                            '<input class="date-picker" name="clauseValue[]" style="width: 100%;" value="' + model.right1 + '"></input></div>' +
                            '<div class="col-md-2">between</div>' +
                            '<div class="col-md-5">' +
                            '<input class="date-picker" name="clauseValue2[]" style="width: 100%;" value="' + model.right2 + '"></input></div>';
                        break;
                }

                break;
        }

        return valueHTML;
    }

    $('.add-clause').live('click', function (e) {
        var owner = $('#coreReportClauseModal').data('owner');
        Append({ page: owner });
    });

    $('.remove-clause').live('click', function (e) {
        var owner = $('#coreReportClauseModal').data('owner');
        var liIndex = $(this).closest('li').index();

        $('#coreReportClauseModal .clauseContainer li:eq(' + liIndex + ')').remove();

        ResyncWhereStruct({ page: owner });
    });

    $('[name="clauseColumn"]').live('change', function (e) {
        var owner = $('#coreReportClauseModal').data('owner');
        var liIndex = $(this).closest('li').index();
        var thisDBType = $(this).find('option:selected').data('dbtype');

        $('#coreReportClauseModal .clauseContainer li:eq(' + liIndex + ') [name="clauseConditions[]"]').html(GetConditionTypesForDBType({ dbtype: thisDBType }));

        var thisConditionType = $('#coreReportClauseModal .clauseContainer li:eq(' + liIndex + ') [name="clauseConditions[]"]').find('option:selected').val();
        $('#coreReportClauseModal li:eq(' + liIndex + ') .rightsideclause ').html(GetValueInputsBasedOnCondition({ condition: thisConditionType, dbtype: thisDBType }));

        CalendarComponentsPickers.init();
    });

    $('[name="clauseConditions[]"]').live('change', function (e) {
        var owner = $('#coreReportClauseModal').data('owner');
        var liIndex = $(this).closest('li').index();
        var thisConditionType = $(this).find('option:selected').val();
        var thisDBType = $('#coreReportClauseModal li:eq(' + liIndex + ') [name="clauseColumn"] option:selected').data('dbtype');

        $('#coreReportClauseModal li:eq(' + liIndex + ') .rightsideclause ').html(GetValueInputsBasedOnCondition({ condition: thisConditionType, dbtype: thisDBType }));

        CalendarComponentsPickers.init();
    });

    $('#coreReportClauseModal').on('change', function (e) {
        //Update This Page Where Clause

        var thisDTOwner = $(this).data('owner');

        ResyncWhereStruct({ page: thisDTOwner });
    });

    function ResyncWhereStruct(model) {
        mGlobal.page[model.page].whereClause = mGlobal.page[model.page].whereClause == undefined ? [] : [];

        $('[data-owner="' + model.page + '"] .clauseContainer li').each(function (i, element) {
            var leftSide = $(element).find('[name="clauseColumn"] option:selected').val();
            var centerSide = $(element).find('[name="clauseConditions[]"] option:selected').val();
            var rightSide = $(element).find('[name="clauseValue[]"]').val();
            var rightSide2 = $(element).find('[name="clauseValue2[]"]').val();
            var dbtype = $(element).find('[name="clauseColumn"] option:selected').data('dbtype');

            if (leftSide != '' && centerSide != '' && rightSide != '') {
                mGlobal.page[model.page].whereClause.push({ leftside: leftSide, condition: centerSide, right1: rightSide, right2: rightSide2, dbtype: dbtype });
            }

        });

        $('[data-owner="' + model.page + '"][name="ClauseNotificationsCount"]').remove();

        if (mGlobal.page[model.page].whereClause.length > 0) {
            $('.CustomReportClause').append('<span class="badge badge-default" data-owner="' + model.page + '" name="ClauseNotificationsCount">' + mGlobal.page[model.page].whereClause.length + '</span>');
        }
    }

    function BuildAjaxClauseStruct(model, d) {

        var thisClause = mGlobal.page[model.page].whereClause == undefined ? [] : mGlobal.page[model.page].whereClause;
        ResyncWhereStruct({ page: model.page });
        d = d == undefined ? {} : d;

        var textTypes = ['NVarChar', 'VarChar', 'NChar', 'Char', 'Text', 'NText'];
        var dateTypes = ['DateTime', 'SmallDateTime', 'Date', 'Time', 'DateTimeOffset'];

        if (mGlobal.page[model.page].whereClause != undefined) {

            for (var i = 0; i < thisClause.length; i++) {

                var leftSide = thisClause[i].leftside;
                var centerSide = thisClause[i].condition;
                var rightSide = thisClause[i].right1;
                var rightSide2 = thisClause[i].right2;

                var dbtype = thisClause[i].dbtype;


                if (leftSide != '' && leftSide != 'None' && centerSide != '' && rightSide != '') {
                    if (textTypes.indexOf(dbtype) != -1 || dateTypes.indexOf(dbtype) != -1) { //see if the chosen dbtype is text/varchar/etc, or a datetime type

                        if (!rightSide2 || rightSide2 == '') {//there is no second right-side clause

                            if (centerSide == 'in' || centerSide == 'not in') {//and they want to search in/not in

                                //split on commas and wrap each value in quotes
                                var rightVals = rightSide.split(',');

                                var newRightVal = '(';

                                //loop through the values and wrap them, then re-separate with commas
                                for (var i = 0; i < rightVals.length; i++) {
                                    newRightVal += "'" + rightVals[i].trim() + "',";
                                }

                                //remove the trailing comma and append closing paren
                                newRightVal = newRightVal.slice(0, -1) + ')';

                                rightSide = newRightVal;
                            }
                            else { //it's not an 'in/not in', so replace any asterisk with a percent sign in case user is trying a wildcar search
                                rightSide = "'" + rightSide.replace('*', '%') + "'";
                            }
                        }
                        else { //there is a rightSide2 in the clause, so it's can't be a "like/not like" so don't bother replacing wildcards, but do wrap the values in quotes
                            rightSide = "'" + rightSide + "'";
                            rightSide2 = "'" + rightSide2 + "'";
                        }
                    }
                    else { //it isn't a text type or a date type, so no wrapping should be required

                        if (centerSide == 'in' || centerSide == 'not in') {//they want to search in/not in
                            //so let's wrap the right side clause in parens
                            rightSide = '(' + rightSide + ')';
                        }
                    }
                }


                d['_clause.leftSideOfClause[' + i + ']'] = leftSide;
                d['_clause.ClauseConditions[' + i + ']'] = centerSide;
                d['_clause.rightSideOfClause[' + i + ']'] = rightSide;
                d['_clause.rightSideOfClause2[' + i + ']'] = rightSide2;
            }
        }

        return d;
    }

    //$('#RunQueryModalButton').on('click', function (i) {
    //    var DTOwner = $('#coreReportClauseModal').data('owner');

    //    console.log($('#coreReportClauseModal'));

    //    if (DTOwner != undefined && DTOwner != '') {
    //        table[DTOwner].ajax.reload();

    //        console.log(table[DTOwner]);
    //    }
    //});

    $('#RunQueryModalButton').live('click', function (i) {
        var DTOwner = $('#coreReportClauseModal').data('owner');

        if (DTOwner != undefined && DTOwner != '') {
            //table[DTOwner].columns(0).search('').draw();
            //table[DTOwner].ajax.reload();
        }
    });

    function Rebuild(model) {
        if (mGlobal.page[model.page].whereClause != undefined && mGlobal.page[model.page].whereClause.length > 0) {
            for (var i = 0; i < mGlobal.page[model.page].whereClause.length; i++) {

                var thisWhere = mGlobal.page[model.page].whereClause[i];

                if (i == 0) {
                    StartNew({ page: model.page, dbtype: thisWhere.dbtype, condition: thisWhere.condition, leftside: thisWhere.leftside, right1: thisWhere.right1, right2: thisWhere.right2 });
                }
                else {
                    Append({ page: model.page, dbtype: thisWhere.dbtype, condition: thisWhere.condition, leftside: thisWhere.leftside, right1: thisWhere.right1, right2: thisWhere.right2 });
                }

            }

            CalendarComponentsPickers.init();
        }
    }

    return {
        init: function (model) {
            StartNew(model);
        },
        add: function (model) {
            Append(model);
        },
        remove: function (model) {
            Trash(model);
        },
        ajaxData: function (model, d) {
            return BuildAjaxClauseStruct(model, d);
        },
        resume: function (model) {
            Rebuild(model);
        }
    };
}();

var builder = function builder() {

    function clearBuilder(model) {
        if (model.action) {
            var closestfilter = 'FilterDiv_' + mGlobal.page.builder.datatableName;
            var closestWrapper = 'BodyContentWrapper_' + mGlobal.page.builder.datatableName;

            $('#tab_Core_Columns [name="ColumnAvailability"]').empty();
            $('#Filters_Accordion_' + mGlobal.page.builder.datatableName).empty();
            handleCustomReports({
                method: 'POST',
                customreportid: -1,
                selector: '#currentCustomReports'
            });
            $('#dataTableContainer').empty();
            IrisEllipse_Hide();
            showHideBuilderAction({ showhide: 'hide' });

            SideBarManipulation({ action: 'close', closestfilter: closestfilter, closestWrapper: closestWrapper });
        }
    }

    function ReloadBuilder(model) {

        buildBaseReport.callCreate(model);
        $(model.selector + ' .clause').remove();

    }

    function changedBaseReport(model) {
        $('#dataTableContainer').addClass('hide-on-init');
        IrisEllipse_Show();

        var selectedReport = '#currentReports option:selected';

        mGlobal.variable.currentSelectedBaseCustomReportID = $(selectedReport).data('rootreportid');
        mGlobal.variable.currentSelectedBaseReportBaseID = $(selectedReport).data('baserootreportid');
        mGlobal.variable.currentSelectedBaseReportSchema = $(selectedReport).data('templatename');
        mGlobal.variable.currentSelectedRootReportName = $(selectedReport).val();
        $('[name="I_BASE_ROOT_REPORT_ID"]').val(mGlobal.variable.currentSelectedBaseReportBaseID);
        $('[name="I_ROOT_REPORT_ID"]').val(mGlobal.variable.currentSelectedBaseCustomReportID);

        var closestfilter = 'FilterDiv_' + mGlobal.page.builder.datatableName;
        var closestWrapper = 'BodyContentWrapper_' + mGlobal.page.builder.datatableName;

        if (mGlobal.variable.currentSelectedRootReportName != '') {
            ReloadBuilder({
                report: mGlobal.variable.currentSelectedRootReportName,
                filterSelector: '#Filters_Accordion_' + mGlobal.page.builder.datatableName,
                loadDT: true,
                loadAvailableFilters: true,
                callEvent: 'Builder'
            });

            showHideBuilderAction({ showhide: 'show' });
            SideBarManipulation({ action: 'open', closestfilter: closestfilter, closestWrapper: closestWrapper });
        }
        else {

            clearBuilder({ action: true });
        }

        $('#CaptionTitle').text($('#currentReports').val());

    }

    function populateBaseReportList(model) {

        model.TypeOfQuery == undefined ? "Default" : model.TypeOfQuery;

        var _html = '';
        _html += '<option value="">Select a Root Report</option>';

        for (var i = 0; i < model.baseReports.length; i++) {
            var reportname;
            switch (model.TypeOfQuery) {
                default:
                case "Default":
                    reportname = model.baseReports[i].report_name;

                    //var selected = mGlobal.variable.currentSelectedRootReportName != '' && mGlobal.variable.currentSelectedRootReportName == reportname ? 'selected' : '';

                    var preSelectedBaseReport = model.preSelectedBaseReport;

                    var selected = preSelectedBaseReport != undefined && preSelectedBaseReport != '' && reportname == preSelectedBaseReport ? 'selected' : '';

                    _html += '<option ' +
                        'data-rootreportid="' + model.baseReports[i].root_report_id + '" ' +
                        'data-baserootreportid="' + model.baseReports[i].base_root_report_id + '" ' +
                        'data-prevrootreportid="' + model.baseReports[i].prev_root_report_id + '" ' +
                        'data-templateid="' + model.baseReports[i].template_id + '" ' +
                        'data-templatename="' + model.baseReports[i].template_name + '" ' +
                        'value="' + reportname + '" ' + selected + '>' + model.baseReports[i].template_name + ':' + reportname + '</option>';
                    break;
                case "Distinct Root Report":
                    reportname = model.baseReports[i].root_report_name;
                    _html += '<option data-customreportid="' + model.baseReports[i].base_root_report_id + '" value="' + reportname + '">' + model.baseReports[i].template + ':' + reportname + '</option>';
                    break;
            }
        }

        $(model.selector).empty();
        $(model.selector).append(_html);
        $(model.selector).removeClass('dataNotLoaded');
        $(model.selector).addClass('dataLoaded');

        switch (model.callEvent) {
            default:
                break;
            case 'Reload Builder':

                mGlobal.variable.currentSelectedBaseCustomReportID = $('#currentReports option:selected').data('rootreportid');
                mGlobal.variable.currentSelectedBaseReportBaseID = $('#currentReports option:selected').data('baserootreportid');
                mGlobal.variable.currentSelectedRootReportName = $('#currentReports option:selected').val();
                $('[name="I_BASE_ROOT_REPORT_ID"]').val(mGlobal.variable.currentSelectedBaseReportBaseID);
                $('[name="I_ROOT_REPORT_ID"]').val(mGlobal.variable.currentSelectedBaseCustomReportID);

                changedBaseReport({});
                break;
        }
    }

    function populateBaseReportsOnTemplateSelect(model) {
        var selectedTemplate = '#currentTemplates option:selected';
        mGlobal.variable.currentSelectedTemplate = $(selectedTemplate).val();

        clearBuilder({ action: true });

        var Reports = mGlobal.variable.baseReports.filter(function (baseReport) { return (baseReport.template_name == $('#currentTemplates option:selected').val()); });

        model.direction = 'CALLBACK';
        model.baseReports = Reports;
        model.selector = '#currentReports';
        model.handleBaseReportsEvent = 'Populate Root Report List';
        handleBaseReportsList(model);
    }

    function RefreshBuilder(model) {
        var allCalendarsPicked = true;

        $(".sendDates2Server").each(function (i) {

            var thisDate = $('[name="' + $('.sendDates2Server:eq(' + i + ')').attr("name") + '"] .datepicker-years .active').html() +
                pad('00', ($('[name="' + $('.sendDates2Server:eq(' + i + ')').attr("name") + '"] .datepicker-months span.active').index() + 1), true) +
                pad('00', $('[name="' + $('.sendDates2Server:eq(' + i + ')').attr("name") + '"] .datepicker-days .active').html(), true);

            if (thisDate == 'undefined0000') {
                allCalendarsPicked = false;
                return false;
            }

        });

        if (allCalendarsPicked) {
            $('#dataTableContainer').addClass('hide-on-init');
            IrisEllipse_Show();
            table[mGlobal.page.builder.datatableName].ajax.reload();
            $('#CaptionTitle').text($('#currentReports').val());
        }
        else {
            alert("Please select dates for each Calendar!");
        }
    }

    function RebuildBuilder(model) {
        $('#dataTableContainer').addClass('hide-on-init');
        IrisEllipse_Show();
        $('#CaptionTitle').text($('#currentReports').val());

        if (mGlobal.variable.currentSelectedRootReportName != '') {

            ReloadBuilder({
                report: mGlobal.variable.currentSelectedRootReportName,
                filterSelector: '#Filters_Accordion_' + mGlobal.page.builder.datatableName,
                loadDT: true,
                loadAvailableFilters: true,
                callEvent: 'Builder'
            });

            showHideBuilderAction({ showhide: 'show' });
        }
        else {
            $('#dataTableContainer').empty();

            showHideBuilderAction({ showhide: 'hide' });

            IrisEllipse_Hide();
        }
    }

    function showHideBuilderAction(model) {
        switch (model.showhide.toLowerCase()) {
            case 'show':

                $('#SaveReport').show();
                $('#RebuildDT').show();
                $('#SaveRootReport').show();
                break;
            case 'hide':

                $('#SaveReport').hide();
                $('#RebuildDT').hide();
                $('#SaveRootReport').hide();
                break;
        }
    }

    function SaveReport(model) {
        var curentFormValuesArray = $('#SaveReportForm').serializeArray();

        var thisModel = ConvertFromArrayToModel(curentFormValuesArray, 'thisModel');

        thisModel['thisModel.V_ROOT_REPORT_NAME'] = $('#currentReports option:selected').val();

        //Run this when it's an edit
        if (+$('#currentCustomReports option:selected').val() > 0) {
            thisModel['thisModel.I_BASE_CUSTOM_REPORT_ID'] = $('#currentCustomReports option:selected').data('baseid');
            thisModel['thisModel.I_PREV_CUSTOM_REPORT_ID'] = $('#currentCustomReports option:selected').data('id');
        }

        var FilterCount = 0;
        var ColumnCount = 0;

        if (thisModel['thisModel.I_REPORT_NAME'] != ""
            && thisModel['thisModel.I_DESCRIPTION'] != ""
            && thisModel['thisModel.I_REPORT_NAME'] != undefined
            && thisModel['thisModel.I_DESCRIPTION'] != undefined) {

            //DATE FILTERS
            var MandatorySelector = '#Filters_Accordion_' + mGlobal.page.builder.datatableName + ' .panel[data-bound="false"]:not([data-filter="all_filters"])';
            $(MandatorySelector).each(function (i, element) {
                thisModel['thisModel.filters[' + FilterCount + '].I_MANDATORY'] = 'T';
                thisModel['thisModel.filters[' + FilterCount + '].I_FILTER'] = $(element).data('filter');
                var isRequired = $(element).data('required');
                thisModel['thisModel.filters[' + FilterCount + '].I_REQUIRED'] = isRequired == true ? 'T' : 'F';
                FilterCount++;
            });

            //HIDDEN FILTERS
            for (var i = 0; i < mGlobal.page[mGlobal.page.builder.datatableName].HiddenFiltersModel.length; i++) {
                thisModel['thisModel.filters[' + FilterCount + '].I_MANDATORY'] = 'T';
                thisModel['thisModel.filters[' + FilterCount + '].I_FILTER'] = mGlobal.page[mGlobal.page.builder.datatableName].HiddenFiltersModel[i].filterName;
                thisModel['thisModel.filters[' + FilterCount + '].I_REQUIRED'] = 'T';
                FilterCount++;
            }

            //TOGGLE FILTERS
            var ToggleSelector = '#Filters_Accordion_' + mGlobal.page.builder.datatableName + ' .panel.panel-default.clearable.filter.send2server[data-toggle=true]';
            $(ToggleSelector).each(function (i, element) {
                thisModel['thisModel.filters[' + FilterCount + '].I_MANDATORY'] = 'T';
                thisModel['thisModel.filters[' + FilterCount + '].I_FILTER'] = $(element).data('filter');
                thisModel['thisModel.filters[' + FilterCount + '].I_REQUIRED'] = 'F';
                FilterCount++;
            });

            //MultiSelect FILTERS
            var PanelsSelector = '#' + mGlobal.page.builder.datatableName + '_panel_Available_Filters .filter-content input:checked';
            $(PanelsSelector).each(function (i, element) {

                thisModel['thisModel.filters[' + FilterCount + '].I_MANDATORY'] = 'F';
                thisModel['thisModel.filters[' + FilterCount + '].I_FILTER'] = $(element).val();
                var isRequired = $(element).data('required');
                thisModel['thisModel.filters[' + FilterCount + '].I_REQUIRED'] = isRequired == true ? 'T' : 'F';
                FilterCount++;

            });

            $('[name="CustomReportSaveColumns"] li input').each(function (i, element) {

                var thisObject = $('[name="CustomReportSaveColumns"] li input.ShowColumn:eq(' + i + ')');

                if (thisObject.is(':checked')) {
                    thisModel['thisModel.columns[' + ColumnCount + '].I_ORIGINAL_COLUMN'] = $('[name="CustomReportSaveColumns"] input.ColumnArea:eq(' + i + ')').data('original');
                    thisModel['thisModel.columns[' + ColumnCount + '].I_COLUMN_AREA'] = $.trim($('[name="CustomReportSaveColumns"] input.ColumnArea:eq(' + i + ')').val()) == '' ? $('[name="CustomReportSaveColumns"] input.ColumnArea:eq(' + i + ')').data('original') : $.trim($('[name="CustomReportSaveColumns"] input.ColumnArea:eq(' + i + ')').val());
                    thisModel['thisModel.columns[' + ColumnCount + '].I_ALIAS_AREA'] = $.trim($('[name="CustomReportSaveColumns"] input.AliasArea:eq(' + i + ')').val()) == '' ? $('[name="CustomReportSaveColumns"] input.ColumnArea:eq(' + i + ')').data('original') : $.trim($('[name="CustomReportSaveColumns"] input.AliasArea:eq(' + i + ')').val());
                    ColumnCount++;
                }
            });

            mGlobal.page.builder.lastsave = thisModel;
            ajaxUniversal("/dynamic/Create_Core_Report", thisModel, { callCase: 'Save Report', model: thisModel });
            CloseModal({ selector: model.selector });
        }
        else {
            alert('Please enter a Report name and Description!');
        }
    }

    function SaveBaseReport() {
        var ColumnCount = 0;
        var CurrentReportSelected = $('#currentReports option:selected');

        var BaseReportModel = {
            rootreportid: CurrentReportSelected.data('rootreportid'),
            baserootreportid: CurrentReportSelected.data('baserootreportid'),
            prevrootreportid: CurrentReportSelected.data('prevrootreportid'),
            name: CurrentReportSelected.val()
        };

        var baseReport = $.grep(mGlobal.variable.baseReports, function (e) {
            if ((e.root_report_id == BaseReportModel.rootreportid)) { return true; }
            else { return false; }
        });

        if (baseReport.length > 0) {

            var thisModel = {
                'thisModel.I_BASE_ROOT_REPORT_ID': BaseReportModel.baserootreportid,
                'thisModel.I_PREV_ROOT_REPORT_ID': BaseReportModel.rootreportid,
                'thisModel.I_REPORT_NAME': BaseReportModel.name,
                'thisModel.I_PROCEDURE_NAME': baseReport[0].procedure_name,
                'thisModel.I_TEMPLATE_ID': baseReport[0].template_id,
                'thisModel.I_TEMPLATE_NAME': baseReport[0].template_name
            };

            $('[name="ColumnAvailability"] li.AvailableColumnLi').each(function (i) {

                var thisObject = $('[name="ColumnAvailability"] li.AvailableColumnLi input.ShowColumn:eq(' + i + ')');

                var original = $('[name="ColumnAvailability"] li.AvailableColumnLi input.ColumnArea:eq(' + i + ')').data('original');
                var columnArea = $('[name="ColumnAvailability"] li.AvailableColumnLi input.ColumnArea:eq(' + i + ')').val();
                var availableColumn = $('[name="ColumnAvailability"] li.AvailableColumnLi input.AliasArea:eq(' + i + ')').val();

                if (thisObject.is(':checked')) {
                    thisModel['thisModel.columns[' + ColumnCount + '].I_ENABLED'] = 'Y';
                    thisModel['thisModel.columns[' + ColumnCount + '].I_ORIGINAL_COLUMN'] = original;
                    thisModel['thisModel.columns[' + ColumnCount + '].I_COLUMN_AREA'] = $.trim(columnArea) == '' ? original : $.trim(columnArea);
                    thisModel['thisModel.columns[' + ColumnCount + '].I_ALIAS_AREA'] = $.trim(availableColumn) == '' ? original : $.trim(availableColumn);
                    ColumnCount++;
                }
                else {
                    thisModel['thisModel.columns[' + ColumnCount + '].I_ENABLED'] = 'N';
                    thisModel['thisModel.columns[' + ColumnCount + '].I_ORIGINAL_COLUMN'] = original;
                    thisModel['thisModel.columns[' + ColumnCount + '].I_COLUMN_AREA'] = $.trim(columnArea) == '' ? original : $.trim(columnArea);
                    thisModel['thisModel.columns[' + ColumnCount + '].I_ALIAS_AREA'] = $.trim(availableColumn) == '' ? original : $.trim(availableColumn);
                    ColumnCount++;
                }

            });

            var model = {}

            model.ajaxData = model.ajaxData == undefined ? {} : model.ajaxData;
            model.callCase = model.callCase == undefined ? 'Get Root Reports List' : model.callCase;
            model.data = thisModel
            model.options = model.options == undefined ? {} : model.options;
            model.options.async = true;
            model.options.url = "/dynamic/Create_Core_Base_Report?iReport=" + BaseReportModel.name;
            model.options.callBack = function (model) {
                mGlobal.page.builder.LastSavedBaseReport = model.response;

                handleBaseReportsList({
                    direction: 'POST',
                    selector: '#currentReports',
                    TypeOfQuery: 'Default',
                    defaultOption: mGlobal.page.builder.LastSavedRootReportName,
                    callEvent: 'Reload Builder',
                    handleBaseReportsEvent: 'Populate Templates List'
                });

                promptDialog.prompt({
                    promptID: 'Root-Report-Saved-Prompt',
                    body: 'Root Report Updated!',
                    header: 'Builder Save'
                });
            };

            model.notification = model.notification == undefined ? {} : model.notification;
            model.notification.pulse = false;

            ajaxDynamic(model);
        }
    }

    function loadBuilderCustomReport(model) {
        $('#' + mGlobal.page.builder.datatableName + ' tbody').empty();

        mGlobal.variable.currentCustomReportID = $('#currentCustomReports option:selected').val();

        if (mGlobal.variable.currentCustomReportID > 0) {
            buildCustomReport.filters({
                direction: 'POST',
                currentCustomReportID: mGlobal.variable.currentCustomReportID,
                filterSelector: '#Filters_Accordion_' + mGlobal.page.builder.datatableName + '',
                callEvent: 'Builder Custom Report',
                page: mGlobal.page.builder.datatableName,
                rootreportname: mGlobal.variable.currentSelectedRootReportName,
                tableOwner: mGlobal.page.builder.datatableName,
                schema: mGlobal.variable.currentSelectedBaseReportSchema,
                template: mGlobal.variable.currentSelectedBaseReportSchema,
                mandatory_type: 'T,F'
            });
        }
        else {
            ReloadBuilder({
                report: mGlobal.variable.currentSelectedRootReportName,
                filterSelector: '#Filters_Accordion_' + mGlobal.page.builder.datatableName,
                loadDT: true,
                loadAvailableFilters: true,
                callEvent: 'Builder'
            });
        }
    }

    function handleBaseReportsList(model) {

        switch (model.direction) {
            default:
            case 'POST':

                model.ajaxData = model.ajaxData == undefined ? {} : model.ajaxData;
                model.callCase = model.callCase == undefined ? 'Get Root Reports List' : model.callCase;
                model.ajaxData.SearchReport = 'root reports';
                model.ajaxData.P_GET_LATEST = 'T';
                model.ajaxData.P_ENABLED = 'Y';
                model.ajaxData.P_INSTALLED_ = 'Y';
                model.ajaxData.P_VERIFY_ = 'T';
                model.ajaxData.start = 0;
                model.ajaxData.length = -1;
                model.ajaxData.template = 'dynamic';
                model.ajaxData.schema = 'dynamic';
                model.ajaxData.queryType = "query";
                model.ajaxData['order[0][column]'] = 'Report_Name';
                model.ajaxData['order[0][dir]'] = 'asc';

                model.data = model.ajaxData;
                model.options = model.options == undefined ? {} : model.options;
                model.options.async = true;
                model.options.url = "/dynamic/dynosearch?iReport=" + model.ajaxData.SearchReport;
                model.options.callBack = function (model) {
                    //if (typeof model.response.data == 'string' && Base64.isThisEncoded(model.response.data)) {
                    //    model.response.data = JSON.parse(Base64.decode(model.response.data));
                    //}

                    handleBaseReportsList(model);
                };

                model.notification = model.notification == undefined ? {} : model.notification;
                model.notification.pulse = false;

                ajaxDynamic(model);

                break;
            case 'CALLBACK':
                model.baseReports = model.baseReports == undefined ? model.response.data : model.baseReports;

                switch (model.handleBaseReportsEvent) {

                    case 'Populate Root Report List':
                        populateBaseReportList(model);
                        break;
                    default:
                    case 'Populate Templates List':
                        mGlobal.variable.baseReports = model.baseReports;
                        mGlobal.variable.baseTemplates = _.map(_.uniqBy(mGlobal.variable.baseReports, 'template_name'), function (item) { return { template_id: item.template_id, template_name: item.template_name }; });
                        BuildTemplateDropDown({ selector: '#currentTemplates', templateData: mGlobal.variable.baseTemplates, preSelectedTemplate: mGlobal.variable.currentSelectedTemplate });
                        model.preSelectedBaseReport = mGlobal.variable.currentSelectedRootReportName;
                        populateBaseReportsOnTemplateSelect(model);
                        break;
                    case 'Get Data':
                        mGlobal.variable = mGlobal.variable == undefined ? {} : mGlobal.variable;
                        mGlobal.variable[model.variableName] = model.baseReports;

                        createFREDStep2(model.FREDModel);
                        break;
                }

                break;
        }
    }

    function handleCustomReports(model) {
        switch (model.direction) {
            case 'POST':
                model.customreportid == undefined ? "-1" : model.customreportid;

                model.TypeOfQuery == undefined ? "Default" : model.TypeOfQuery;

                model.ajaxData = {
                    TypeOfQuery: model.TypeOfQuery,
                    SearchReport: 'custom reports',
                    start: 0,
                    length: -1,
                    P_GET_LATEST: 'T',
                    P_ENABLED: 'Y',
                    P_VERIFY: 'T',
                    P_BASE_ROOT_REPORT_ID: model.customreportid,
                    template: 'dynamic',
                    schema: 'dynamic',
                    queryType: "query"
                };

                switch (model.TypeOfQuery) {
                    default:
                    case "Default":
                        model.ajaxData['order[0][column]'] = 'Report_Name';
                        model.ajaxData['order[0][dir]'] = 'asc';
                        break;
                    case "Distinct Root Report":
                        model.ajaxData['order[0][column]'] = 'ROOT_REPORT_NAME';
                        model.ajaxData['order[0][dir]'] = 'asc';
                        model.ajaxData.P_DYNO_COL = "distinct ROOT_REPORT_NAME, BASE_ROOT_REPORT_ID, TEMPLATE";
                        break;
                }

                //ajaxUniversal('/dynamic/dynosearch?iReport=' + model.ajaxData.SearchReport, model.ajaxData,
                //{
                //    callCase: 'GetReportsList',
                //    type: model.method,
                //    model: model
                //});

                model.data = model.ajaxData;
                model.options = model.options == undefined ? {} : model.options;
                model.options.async = true;
                model.options.url = '/dynamic/dynosearch?iReport=' + model.ajaxData.SearchReport;
                model.options.callBack = function (model) {
                    model.customReports = model.response.data;
                    handleCustomReports(model);
                };

                model.notification = model.notification == undefined ? {} : model.notification;
                model.notification.pulse = false;

                ajaxDynamic(model);

                break;

            case 'CALLBACK':
                switch (model.TypeOfQuery) {
                    default:
                    case "Default":
                        populateCustomReportList(model);
                        break;
                    case "Distinct Root Report":
                        model.baseReports = model.customReports;
                        populateBaseReportList(model);
                        break;
                }
                break;
        }
    }

    function BuildTemplateDropDown(model) {
        var DropDownHtml = '<option value="">Select a Template</option>';

        for (var i = 0; i < model.templateData.length; i++) {

            var preSelectedTemplate = model.preSelectedTemplate;

            var selected = preSelectedTemplate != undefined && preSelectedTemplate != '' && model.templateData[i].template_name == preSelectedTemplate ? 'selected' : '';

            DropDownHtml += '<option ' +
                'data-templateid="' + model.templateData[i].template_id + '"' +
                'data-templatename="' + model.templateData[i].template_name + '"' +
                'value="' + model.templateData[i].template_name + '" ' + selected + '>' + model.templateData[i].template_name + '</option>';
        }

        $(model.selector).html(DropDownHtml);
    }

    function populateCustomReportList(model) {
        var _html = model.customReports.length == 0 ? '<option value="-1">None Available</option>' : '<option value="-1">Select Report</option>';

        for (var i = 0; i < model.customReports.length; i++) {
            _html += '<option value="' + model.customReports[i].custom_report_id + '" data-baseid="' + model.customReports[i].base_custom_report_id + '" data-id="' + model.customReports[i].custom_report_id + '">' + model.customReports[i].report_name + '</option>';
        }
        $(model.selector).empty();
        $(model.selector).append(_html);
    }

    function handleCustomReportColumns(model) {
        switch (model.direction) {
            case 'POST':
                var currentCustomReportID = model.currentCustomReportID == undefined ? model._customreportid : model.currentCustomReportID;

                if (+currentCustomReportID > 0) {
                    model.data = {
                        SearchReport: 'custom report columns',
                        start: 0,
                        length: -1,
                        P_GET_LATEST: 'T',
                        P_ENABLED: 'Y',
                        P_VERIFY: 'T',
                        P_CUSTOM_REPORT_ID: currentCustomReportID,
                        template: 'dynamic',
                        schema: 'dynamic',
                        queryType: "query"
                    };
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/dynamic/dynosearch?iReport=" + model.data.SearchReport;
                    model.options.callBack = function (model) {

                        model.customReportColumns = model.response.data;
                        handleCustomReportColumns(model);
                    };

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false;

                    ajaxDynamic(model);
                }
                break;
            case 'CALLBACK':
                switch (model.callEvent) {
                    default:
                    case "Builder":
                        break;
                    case 'Builder Custom Report':

                        var ColumnArray = _.map(_.filter(model.customReportColumns, function (cols) { return 'Y' == 'Y'; }), 'original_column');
                        var AliasArray = _.map(_.filter(model.customReportColumns, function (cols) { return 'Y' == 'Y'; }), 'alias_area');

                        ColumnArray = ColumnArray.length == AliasArray.length ? ColumnArray.map(function (item, index) {
                            return item + ' as ' + AliasArray[index];
                        }) : ColumnArray;

                        mGlobal.page[model.page].columns = ColumnArray.length > 0 ? ColumnArray.join(',') : '';

                        ReloadBuilder({
                            report: mGlobal.variable.currentSelectedRootReportName,
                            filterSelector: '#Filters_Accordion_' + mGlobal.page.builder.datatableName,
                            loadDT: true,
                            loadAvailableFilters: true,
                            callEvent: 'Builder Custom Report'
                        });
                        break;
                    case "Reports":

                        var ColumnArray = _.map(_.filter(model.customReportColumns, function (cols) { return cols.enabled == 'Y'; }), 'original_column');
                        var AliasArray = _.map(_.filter(model.customReportColumns, function (cols) { return cols.enabled == 'Y'; }), 'alias_area');

                        ColumnArray = ColumnArray.length == AliasArray.length ? ColumnArray.map(function (item, index) {
                            return item + ' as ' + AliasArray[index];
                        }) : ColumnArray;

                        mGlobal.page[model.page].columns = ColumnArray.length > 0 ? ColumnArray.join(',') : '';

                        model.direction = 'POST';
                        buildCustomReport.columnOrder(model);

                        break;
                    case 'Custom Report':

                        var modelTheseColumn = model.baseReportColumns == undefined ? model.customReportColumns : model.baseReportColumns;

                        var ColumnArray = _.map(_.filter(modelTheseColumn, function (cols) { return cols.enabled == 'Y'; }), 'original_column');

                        return ColumnArray.length > 0 ? ColumnArray.join(',') : '';

                        break;
                }
                break;
        }
    }

    function createFRED(model) {

        theLoader.show({ id: model.page + '_loader' });

        setTimeout(function () {
            handleBaseReportsList({
                //async: false,
                variableName: 'current' + model.page + 'BaseCustomReportID',
                direction: 'POST',
                handleBaseReportsEvent: 'Get Data',
                ajaxData: {
                    'P_TEMPLATE_NAME_': mGlobal.page[model.page].template, 'P_REPORT_NAME_': mGlobal.page[model.page].report
                },
                FREDModel: model
            });
        }, 500);
        //First Step Get Data

    }

    function createFREDStep2(model) {

        $(model.selector).html(buildBaseReport.generateContainer({ page: model.page }));

        var FiltersIndex = model.FilterIndex == undefined ? 0 : model.FilterIndex;

        model.event = 'createfred';
        builder().constructSideBar(model);

        if (mGlobal.variable['current' + model.page + 'BaseCustomReportID'][0] != undefined && mGlobal.variable['current' + model.page + 'BaseCustomReportID'][0].base_root_report_id != undefined) {
            mGlobal.variable['currentSelected' + model.page + 'BaseCustomReportID'] = mGlobal.variable['current' + model.page + 'BaseCustomReportID'][0].base_root_report_id;
        }

        model.method = 'POST';
        model.report = mGlobal.page[model.page].report;
        model.tableName = mGlobal.page[model.page].name;
        model.rawTableName = mGlobal.page[model.page].name;
        model.tableID = '#' + mGlobal.page[model.page].name;
        model.tableContainer = '#dataTableContainer_' + mGlobal.page[model.page].name;
        var defaultURL = '/dynamic/dynosearch';
        var finalUrl = mGlobal.page[model.page].overrideJsonURL != undefined && mGlobal.page[model.page].overrideJsonURL.length > 0 ? mGlobal.page[model.page].overrideJsonURL : defaultURL;
        model.jsonURL = finalUrl + '?iReport=' + mGlobal.page[model.page].report;

        var defaultStructURL = '/dynamic/GetTableStruct';
        model.structURL = mGlobal.page[model.page].overrideStructURL != undefined && mGlobal.page[model.page].overrideStructURL.length > 0 ? mGlobal.page[model.page].overrideStructURL : defaultStructURL;
        model.page = mGlobal.page[model.page].name;
        model.customreportid = mGlobal.variable['currentSelected' + model.page + 'BaseCustomReportID'];
        model.basecustomreportid = mGlobal.variable['currentSelected' + model.page + 'BaseCustomReportID'];
        model.schema = mGlobal.page[model.page].schema;
        model.template = mGlobal.page[model.page].template;
        // model.selector = '#currentCustomReports';
        model.loadDT = true;
        model.tableOwner = mGlobal.page[model.page].name;
        model.rootreportname = mGlobal.page[model.page].report;
        model.scrollY = '450px';
        model.filterSelector = '#Filters_Accordion_' + mGlobal.page[model.page].name;
        //model.CustomReportselector = '#currentCustomReports';
        model.hasCustomReports = false;
        //model.loadAvailableFilters = model.loadAvailableFilters;
        //model.callEvent = model.callEvent;
        model.order = mGlobal.page[model.page].order ? mGlobal.page[model.page].order : [[1, "desc"]];
        model.dynocol = mGlobal.page[model.page].columns;
        model.columnOverrides = mGlobal.page[model.page].columnOverrides;
        model.ajaxAppend = mGlobal.page[model.page].ajaxAppend;
        model.FilterDivBindOverRide = model.FilterDivBindOverRide;
        model.pageLength = mGlobal.page[model.page].pageLength;
        model.hasTableHeightSelector = mGlobal.page[model.page].hasTableHeightSelector == undefined ? true : mGlobal.page[model.page].hasTableHeightSelector;
        model.hasTableZoom = mGlobal.page[model.page].hasTableZoom == undefined ? true : mGlobal.page[model.page].hasTableZoom;
        model.hasSelectedRowData = mGlobal.page[model.page].hasSelectedRowData == undefined ? false : mGlobal.page[model.page].hasSelectedRowData;
        model.hasPsnSearch = mGlobal.page[model.page].hasPsnSearch == undefined ? false : mGlobal.page[model.page].hasPsnSearch;
        model.hasQueryClause = mGlobal.page[model.page].hasQueryClause == undefined ? true : mGlobal.page[model.page].hasQueryClause;
        model.hasExports = mGlobal.page[model.page].hasExports == undefined ? true : mGlobal.page[model.page].hasExports;
        model.inheritFilters = mGlobal.page[model.page].inheritFilters == undefined ? false : mGlobal.page[model.page].inheritFilters;
        model.currentFilters = mGlobal.page[model.page].currentFilters == undefined ? false : mGlobal.page[model.page].currentFilters;
        model.hideDefaults = mGlobal.page[model.page].hideDefaults == undefined ? true : mGlobal.page[model.page].hideDefaults; 
        model.overrideAvailableFiltersTitles = mGlobal.page[model.page].overrideAvailableFiltersTitles == undefined ? true : mGlobal.page[model.page].overrideAvailableFiltersTitles; 
        buildBaseReport.create(model);

        resizeFilterDiv(model);

        //theLoader.hide({ id: model.page + '_loader' });
    }

    function loadBuilder(model) {

        handleBaseReportsList({
            method: 'POST',
            selector: '#currentReports',
            handleBaseReportsEvent: 'Populate Templates List'
        });

        handleCustomReports({
            method: 'POST',
            customreportid: -1,
            selector: '#currentCustomReports'
        });

        model.selector = '#ReportBuilderDiv';
        model.event = 'reportbuilder';
        builder().constructSideBar(model);

        //$('#ReportBuilderDiv').append(sidebarHTML);

        $('.popovers').popover();

        //if ($('body .hide-on-init').length > 0) {

        //    IrisEllipse_Add();

        //    builder.actions({ showhide: 'hide' });
        //}
    }

    function constructSideBar(model) {
        var sideBarModel = {
            navTabs: []
        }; //model.overrideSideBar == undefined ? {} : model.overrideSideBar;

        switch (model.event.toLowerCase()) {
            default:
            case 'createfred':
                sideBarModel.navTabs.push({
                    id: 'tab_Core_Filters_' + mGlobal.page[model.page].name,
                    title: 'FILTERS',
                    innerHtml: '<div class="panel-group accordion" id="' + 'Filters_Accordion_' + mGlobal.page[model.page].name + '"></div>',
                    active: true
                });
            case 'standalone':
                sideBarModel.name = 'FilterDiv_' + mGlobal.page[model.page].name;
                sideBarModel.class = model.customSideBarClass == undefined ? '' : model.customSideBarClass;

                if (mGlobal.page[model.page].sidebar != undefined && mGlobal.page[model.page].sidebar.tabs != undefined) {
                    for (tabAction in mGlobal.page[model.page].sidebar.tabs) {
                        var thisTabAction = tabAction;

                        if (mGlobal.page[model.page].sidebar.tabs[thisTabAction].sections != undefined) {
                            //var returnActions = mGlobal.page[model.page].sidebar.tabs[thisTabAction].actions;
                            for (index in mGlobal.page[model.page].sidebar.tabs[thisTabAction].sections) {
                                var returnActions = commonDynamic.functions.navigation.sidebar.loadActions({ page: model.page, whichTab: thisTabAction, index: index })

                                mGlobal.page[model.page].sidebar.tabs[thisTabAction].sections[index].templateObject.push({
                                    table: model.page,
                                    title: mGlobal.page[model.page].sidebar.tabs[thisTabAction].sections[index].title,
                                    type: mGlobal.page[model.page].sidebar.tabs[thisTabAction].name,
                                    defaultBtns: returnActions.defaultBtns,
                                    selectedBtns: returnActions.selectedBtns
                                });
                            }

                        }
                    }

                    if (model.overrideSideBar == undefined) {
                        model.overrideSideBar = commonDynamic.functions.navigation.sidebar.tabs({ page: model.page });
                    }

                    mGlobal.page[model.page].sidebar.initialState = '';
                }

                mGlobal.page[model.page].selected = mGlobal.page[model.page].selected == undefined ? { row: -1, data: [], index: '' } : mGlobal.page[model.page].selected;

                sideBarModel.navTabs = model.overrideSideBar != undefined && model.overrideSideBar.navTabs != undefined && model.overrideSideBar.navTabs.length > 0 ? sideBarModel.navTabs.concat(model.overrideSideBar.navTabs) : sideBarModel.navTabs;

                for (tab in sideBarModel.navTabs) {
                    sideBarModel.navTabs[tab].class = sideBarModel.navTabs[tab].class == undefined ? (mGlobal.page[model.page].name + ' ' + sideBarModel.navTabs[tab].title.toLowerCase()) : sideBarModel.navTabs[tab].class;
                    sideBarModel.navTabs[tab].table = sideBarModel.navTabs[tab].table == undefined ? mGlobal.page[model.page].name : sideBarModel.navTabs[tab].table;
                    sideBarModel.navTabs[tab].type = sideBarModel.navTabs[tab].type == undefined ? sideBarModel.navTabs[tab].title.toLowerCase() : sideBarModel.navTabs[tab].type;
                }
                break;
            case 'reportbuilder':
                sideBarModel.name = 'FilterDiv_' + mGlobal.page.builder.datatableName;
                sideBarModel.navTabs.push({
                    id: 'tab_Core_Filters_' + mGlobal.page.builder.datatableName,
                    title: 'FILTERS',
                    innerHtml: '<div class="panel-group accordion" id="' + 'Filters_Accordion_' + mGlobal.page.builder.datatableName + '"></div>',
                    active: true
                });
                sideBarModel.navTabs.push({
                    id: 'tab_Core_Columns',
                    title: 'COLUMNS',
                    innerHtml: '<ul name="ColumnAvailability" class="ColumnAvailability"></ul>',
                    active: false,
                    extension_properties: 'data-placement="left" data-container="body" data-trigger="hover" data-content="This is a list of Root Report Columns. Columns can be given aliases, disabled, or reordered by dragging. Changes take effect once you save the root report."',
                    extension_classes: 'popovers'
                });
                sideBarModel.navTabs.push({
                    id: 'tab_Core_Clause',
                    title: 'ADVANCED',
                    innerHtml: '',
                    active: false
                });
                break;
        }

        sideBarModel.navTabs[0].active = true;
        var sidebarHTML = $.fn.revamp.templates.sideBarTemplate(sideBarModel);

        $(model.selector).append(sidebarHTML);

        if (model.class) {
            $(model.selector + ' div.page-quick-filter-sidebar-wrapper').addClass(model.class);
        }
        if (model.event == 'standalone') {
            commonDynamic.functions.navigation.sidebar.populate.actions(model); //model = { page, type, tabID }
        }

        return true;
    }

    return {
        load: function (model) {
            return loadBuilder(model);
        },
        reload: function (model) {
            ReloadBuilder(model);
        },
        clear: function (model) {
            clearBuilder(model);
        },
        changedBaseReport: function (model) {
            changedBaseReport(model);
        },
        loadBaseReportList: function (model) {
            handleBaseReportsList(model);
        },
        loadCustomReportList: function (model) {
            handleCustomReports(model);
        },
        loadCurrentTemplateBaseReports: function (model) {
            populateBaseReportsOnTemplateSelect(model);
        },
        refresh: function (model) {
            RefreshBuilder(model);
        },
        rebuild: function (model) {
            RebuildBuilder(model);
        },
        actions: function (model) {
            showHideBuilderAction(model);
        },
        saveCustomReport: function (model) {
            SaveReport(model);
        },
        saveBaseReport: function (model) {
            SaveBaseReport(model);
        },
        columns: function (model) {
            return handleCustomReportColumns(model);
        },
        loadBuilderCustomReport: function (model) {
            loadBuilderCustomReport(model);
        },
        createFRED: function (model) {
            createFRED(model);
        },
        createFREDStep2: function (model) {
            createFREDStep2(model);
        },
        constructSideBar: function (model) {
            constructSideBar(model)
        }
    };
}();

var buildBaseReport = function buildBaseReport() {

    function handleBaseReportColumns(model) {
        if (model.direction == 'POST') {
            var currentBaseCustomReportID = model.currentBaseCustomReportID; // $('#currentCustomReports').val();

            if (+currentBaseCustomReportID > 0) {

                model = model != undefined ? model : {};

                model.data = {
                    SearchReport: 'root report columns',
                    start: 0,
                    length: -1,
                    P_GET_LATEST: 'T',
                    P_VERIFY: 'T',
                    P_ROOT_REPORT_ID: currentBaseCustomReportID,
                    template: 'dynamic',
                    schema: 'dynamic',
                    queryType: "query"
                }
                model.options = {};
                model.options.async = model.async;
                model.options.url = '/dynamic/dynosearch?iReport=RootReportColumns';
                model.options.callBack = function (model) {
                    model.baseReportColumns = model.response.data;
                    handleBaseReportColumns(model);
                }
                model.notification = model.notification == undefined ? {} : model.notification;
                model.notification.pulse = false;
                ajaxDynamic(model);

            }
        }

        if (model.direction == 'CALLBACK') {

            switch (model.callEvent) {
                default:
                case 'Root Report':

                    var DisabledColumns = _.filter(model.baseReportColumns, function (cols) { return cols.enabled == 'N'; });

                    for (var i = 0; i < DisabledColumns.length; i++) {
                        $('.AvailableColumnLi #col_chbx_' + DisabledColumns[i].original_column).attr('checked', false);
                    }

                    var ColumnArray = _.map(_.filter(model.baseReportColumns, function (cols) { return cols.enabled == 'Y'; }), 'original_column');

                    mGlobal.page[mGlobal.page.builder.datatableName].columns = ColumnArray.length > 0 ? ColumnArray.join(',') : '';

                    break;
                case 'Custom Report':

                    $('[name="CustomReportSaveColumns"]').html($.fn.revamp.templates.li_CustomReport_AvailableColumns(model));

                    //regex implementation for each character. prevents paste
                    commonDynamic.functions.validation.dynamicFormValidation.lockDownObjectInput({ selector: '.availableCustomReportColumns .AliasArea', regex: /^[\w\s]+$/ });

                    $('.availableCustomReportColumns').sortable({ handle: '.grippy' });

                    break;
                case 'Report Builder':

                    //Filter only Enabled Rows, and return array of original column property return.
                    var ColumnArray = _.map(_.filter(model.baseReportColumns, function (cols) { return cols.enabled == 'Y'; }), 'original_column');
                    var AliasArray = _.map(_.filter(model.baseReportColumns, function (cols) { return cols.enabled == 'Y'; }), 'alias_area');

                    ColumnArray = ColumnArray.length == AliasArray.length ? ColumnArray.map(function (item, index) {
                        return item + ' as ' + AliasArray[index];
                    }) : ColumnArray;

                    mGlobal.page[mGlobal.page.builder.datatableName].columns = ColumnArray.length > 0 ? ColumnArray.join(',') : '';

                    $('#tab_Core_Columns [name="ColumnAvailability"]').html($.fn.revamp.templates.li_BuilderReport_AvailableColumns(model));

                    $('.ColumnAvailability').sortable({ handle: '.grippy', items: '.AvailableColumnLi' });

                    break;
            }
        }
    }

    function handleBaseReportFilters(model) {
        switch (model.direction) {
            case 'POST':
                model.async = model.async == undefined ? false : model.async;

                model = model != undefined ? model : {};

                model.data = {
                    report: model.report,
                    template: model.template
                };
                model.options = {};
                model.options.async = true;
                model.options.url = "/dynamic/Get_Base_Report_Structure?iReport=" + model.report;
                model.options.callBack = function (model) {
                    model.AvailableReports = model.response;
                    handleBaseReportFilters(model);
                };
                model.notification = model.notification == undefined ? {} : model.notification;
                model.notification.pulse = false;
                ajaxDynamic(model);

                break;

            case 'CALLBACK':

                model.loadDT = model.loadDT == undefined ? false : model.loadDT;
                mGlobal.page[model.page].CurrentBaseFiltersModel = model.AvailableReports[0].ReportFilters;
                mGlobal.page[model.page].exports = model.AvailableReports[0].CrystalReports;
                mGlobal.page[model.page].structure = model.AvailableReports[0];

                PopulateBaseReportFiltersList(model);

                break;
        }
    }

    function hideDefaults(model) {

        mGlobal.page[model.page].columnOverrides = mGlobal.page[model.page].columnOverrides == undefined ? [] : mGlobal.page[model.page].columnOverrides;

        var others = ['ENABLED', 'DT_UPDATED', 'DT_AVAILABLE', 'DT_END']

        var HideThese = _.filter(mGlobal.page[model.page].CurrentBaseFiltersModel, function (dF) {
            return dF.FilterName.endsWith("_ID") || dF.FilterName.endsWith("_UUID") || others.indexOf(dF.FilterName) > -1
            //return dF.FilterName.endsWith("_ID") || dF.FilterName.endsWith("_UUID") || dF.FilterName.includes("_F_") || others.indexOf(dF.FilterName) > -1
        })

        for (var i = 0; i < HideThese.length; i++) {

            var thisColumnStruct = {
                name: HideThese[i].FilterName.toLowerCase(), visible: false, showFilter: false, orderable: false
            }
            mGlobal.page[model.page].columnOverrides.push(thisColumnStruct);
            
            //Temporary Fix: Make sure Datatable columns are hidden
            if (table[model.page]) {
                //Use redrawCalculations and adjust column sizing and redraw to increase performance
                table[model.page].columns('.css_' + HideThese[i].FilterName.toUpperCase()).visible(false, false);
                var thisAdjustment = _.debounce(table[model.page].columns.adjust().draw, 2000);
            }
        }
    }

    function handleRootReportColumnOrder(model) {
        switch (model.method) {
            case 'POST':
                var currentRootReportID = model.basecustomreportid == undefined ? 0 : model.basecustomreportid;

                if (+currentRootReportID > 0) {
                    var orderList = ajaxUniversal('/dynamic/dynosearch?iReport=RootReportOrder', {
                        SearchReport: 'root report column order',
                        start: 0,
                        length: -1,
                        P_GET_LATEST: 'T',
                        P_ENABLED: 'Y',
                        P_VERIFY: 'T',
                        P_ROOT_REPORT_ID: currentRootReportID,
                        template: 'dynamic',
                        schema: 'dynamic',
                        queryType: "query",
                        'order[0][column]': 'sort_order',
                        'order[0][dir]': 'asc'
                    },
                        {
                            callCase: 'Get Root Report Column Order',
                            type: model.method,
                            model: model,
                            async: false
                        });

                    return orderList.data;
                }
                else {
                    return [];
                }
                break;
            case 'FORMAT':
                switch (model.callEvent) {
                    default:
                    case "Builder":
                    case 'Builder Custom Report':


                        break;

                }
                break;
        }
    }

    function generateDatatableContainer(model) {

        return $.fn.revamp.templates.div_DatatableContainer(model);
    }

    function PopulateBaseReportFiltersList(model) {

        if (model.hideDefaults) {
            hideDefaults({ page: model.rawTableName })
        }

        var _html = '';

        mGlobal.page[model.page].PickListHtml = [];

        switch (model.callEvent) {
            default:
            case 'Builder':
                model.reportFilters = mGlobal.page[model.page].CurrentBaseFiltersModel;
                $(model.filterSelector + ' .clearable').remove();
                break;
            case 'Builder Custom Report':
                $(model.filterSelector + ' .clearable:not([data-filter="all_filters"])').remove();
                $('#collapse_Available_Filters_' + model.page + ' input[type="checkbox"]:not([data-required="true"])').attr('checked', false);
                break;
            case 'Reports':
                $(model.filterSelector + ' .clearable').remove();
                break;
        }

        var RequireCount = 0;

        var filtersCount = 0;
        var Mandatory = [];

        var Required = [];

        Mandatory = model.reportFilters.filter(function (reportFilter) {
            return (reportFilter.Required && !reportFilter.inPickList) ||
                (reportFilter.required == 'T' && reportFilter.in_pick_list == 'F') ||
                (reportFilter.FilterType == '2' || reportFilter.filter_type == 'Toggle') ||
                (reportFilter.FilterType == '4' || reportFilter.filter_type == 'Hidden');
        });

        switch (model.callEvent) {
            default:
                break;
            case 'Builder':
                Required = model.reportFilters.filter(function (reportFilter) { return (reportFilter.Required && reportFilter.inPickList) || (reportFilter.required == 'T' && reportFilter.in_pick_list == 'T'); });
                break;
            case 'Builder Custom Report':
                Required = model.reportFilters.filter(function (reportFilter) { return (reportFilter.inPickList) || (reportFilter.in_pick_list == 'T'); });
                break;
            case 'Reports':
                Required = model.reportFilters.filter(function (reportFilter) { return (reportFilter.inPickList) || (reportFilter.in_pick_list == 'T'); });
                break;
        }

        if (model.loadAvailableFilters) {
            if (model.reportFilters != undefined) {

                mGlobal.variable.DBTypes = mGlobal.variable.DBTypes == undefined ? xGetDBTypes() : mGlobal.variable.DBTypes;
                var AllDBTypes = mGlobal.variable.DBTypes;

                for (var i = 0; i < model.reportFilters.length; i++) {

                    //['FilterType'] = 0 -> MultiSelect
                    //['FilterType'] = 1 -> DatePicker
                    //['FilterType'] = 2 -> Toggle
                    //['FilterType'] = 3 -> Columns
                    //['FilterType'] = 4 -> Hidden
                    var thisFilterType = model.reportFilters[i]['FilterType'] == undefined ? model.reportFilters[i]['filter_type'].toLowerCase() : model.reportFilters[i]['FilterType'];
                    var thisFilterName = model.reportFilters[i]['FilterName'] == undefined ? model.reportFilters[i]['filter_name'] : model.reportFilters[i]['FilterName'];
                    var thisPrettyName = model.reportFilters[i]['PrettyName'] == undefined ? model.reportFilters[i]['pretty_name'] : model.reportFilters[i]['PrettyName'];
                    var thisRequired = model.reportFilters[i]['Required'] == undefined ? model.reportFilters[i]['required'] : model.reportFilters[i]['Required'];
                    var thisPickLst = model.reportFilters[i]['inPickList'] == undefined ? model.reportFilters[i]['inpicklist'] : model.reportFilters[i]['inPickList'];
                    var thisDBType = model.reportFilters[i]['DBType'] == undefined ? model.reportFilters[i]['dbtype'] : model.reportFilters[i]['DBType'];
                    var thisParamValue = model.reportFilters[i]['ParamValue'] == undefined ? model.reportFilters[i]['paramvalue'] : model.reportFilters[i]['ParamValue'];
                    var thisDBTypeText = _.filter(AllDBTypes, function (o) { return o.id == thisDBType; })[0].dbType;

                    var columnAlias = '';

                    if (mGlobal.page[model.page].columnAlias != undefined) {
                        columnAlias = commonDynamic.functions.tabulate.getColumnAlias(mGlobal.page[model.page].columnAlias, thisPrettyName);
                    }

                    var isColVisible = true;
                    var isColFilterVisible = true;
                    var colOverrides = mGlobal.page[model.page].columnOverrides;

                    if (colOverrides != undefined) {
                        var thisCurrentCol = _.filter(colOverrides, function (thisCol) { return thisCol.name.toLowerCase() == thisFilterName.toLowerCase() })

                        if (thisCurrentCol.length > 0 && typeof thisCurrentCol[0].visible == "boolean") {
                            isColVisible = thisCurrentCol[0].visible;
                        }

                        if (thisCurrentCol.length > 0 && typeof thisCurrentCol[0].showFilter == "boolean") {
                            isColFilterVisible = thisCurrentCol[0].showFilter;
                        }
                    }

                    mGlobal.page[model.page].PickListHtml.push = { value: thisFilterName, title: thisPrettyName} 

                    if ((thisFilterType == 0 || thisFilterType == 'multiselect') && thisDBTypeText != 'DateTime') {
                        filtersCount++;

                        var isDisabled = '';
                        var isChecked = '';
                        var boolIsRequired = 'false';

                        if (thisRequired) {
                            isDisabled = 'disabled';
                            isChecked = 'checked';
                            boolIsRequired = 'true';
                        }

                        if ((thisPickLst == true || thisPickLst == 'T') && isColVisible && isColFilterVisible) {
                            var filterText = (columnAlias == '' ? thisPrettyName.toUpperCase() : columnAlias.toUpperCase());
                            _html += '<div class="checkbox-div"><label class="checkbox-container"><input class="iris-filter" ' + isDisabled + ' ' + isChecked + ' data-filtertype="' + thisFilterType + '" data-required="' + boolIsRequired + '" data-rootreportname="' + model.rootreportname + '" data-tableowner="' + model.tableOwner + '" data-template="' + model.template + '" id="chbx_' + model.page + '_' + thisFilterName + '" type="checkbox" value="' + thisFilterName + '"><span class="checkmark"></span></label><div class="checkbox-filter-text" title="' + filterText + '">' + filterText + '</div></div>';
                        }

                    }
                    else if (thisFilterType == 1 || thisFilterType == 'datepicker') {
                        RequireCount++;
                    }
                    else if (thisFilterType == 2 || thisFilterType == 'toggle') {
                        mGlobal.page[model.tableOwner].currentFilters = mGlobal.page[model.tableOwner].currentFilters == undefined ? {} : mGlobal.page[model.tableOwner].currentFilters;
                        mGlobal.page[model.tableOwner].currentFilters[thisFilterName] = { chainEnabled: true, data: [model.reportFilters[i].ParamValue], required: false };
                        //Nothing Yet Specified
                    }
                    else if (thisFilterType == 3 || thisFilterType == 'columns') {
                        //Nothing Yet Specified
                    }
                    else if (thisFilterType == 4 || thisFilterType == 'hidden') {
                        //mGlobal.page[model.tableOwner].currentFilters = mGlobal.page[model.tableOwner].currentFilters == undefined ? {} : mGlobal.page[model.tableOwner].currentFilters;
                        //mGlobal.page[model.tableOwner].currentFilters[thisFilterName] = { chainEnabled: true, data: [thisParamValue], required: false }

                        //promptDialog.prompt({
                        //    promptID: thisRequired + '-Setting-Saved-Prompt',
                        //    body: thisRequired + ' hidden filter was loaded',
                        //    header: thisRequired + ' hidden filter was loaded'
                        //});
                        //Nothing Yet Specified
                    }
                    else {
                        //Nothing Yet Specified
                    }
                }
            }

            var htmlAvailableFiltertsPanel = '';
            //overrideAvailableFiltersTitles
            htmlAvailableFiltertsPanel = CreatePanel({
                href: 'Available_Filters',
                name: 'all_filters',
                title: (model.overrideAvailableFiltersTitles == undefined || model.overrideAvailableFiltersTitles == false) && model.report.length > 0 ? model.report.toUpperCase() : 'AVAILABLE FILTERS',
                content: _html,
                tableOwner: model.tableOwner,
                template: model.template,
                rootreportname: model.rootreportname,
                filterType: thisFilterType,
                sortable: false,
                style: ''
            });

            $(model.filterSelector).append(htmlAvailableFiltertsPanel);

            switch (model.callEvent) {
                default:
                    break;
                case 'Builder':
                case 'Builder Custom Report':
                    $('#' + model.tableOwner + '_panel_Available_Filters a.accordion-toggle.collapsed').trigger('click');
                    break;
            }
        }

        var forcedFilters = Mandatory.concat(Required);

        for (var i = 0; i < forcedFilters.length; i++) {

            //['FilterType'] = 0 -> MultiSelect
            //['FilterType'] = 1 -> DatePicker
            //['FilterType'] = 2 -> Toggle
            //['FilterType'] = 3 -> Columns
            //['FilterType'] = 4 -> Hidden
            var thisFilterType = forcedFilters[i]['FilterType'] == undefined ? forcedFilters[i]['filter_type'].toLowerCase() : forcedFilters[i]['FilterType'];
            var thisFilterName = forcedFilters[i]['FilterName'] == undefined ? forcedFilters[i]['filter_name'] : forcedFilters[i]['FilterName'];
            var thisPrettyName = forcedFilters[i]['PrettyName'] == undefined ? forcedFilters[i]['pretty_name'] : forcedFilters[i]['PrettyName'];
            var thisRequired = forcedFilters[i]['Required'] == undefined ? forcedFilters[i]['required'] : forcedFilters[i]['Required'];


            var thisParamValue = '';
            if (thisFilterType == 4 || thisFilterType == 'hidden') {
                thisParamValue = forcedFilters[i]['ParamValue'] == undefined ? forcedFilters[i]['paramvalue'] : forcedFilters[i]['ParamValue'];
            }

            var thisAlternateTemplate = forcedFilters[i]['AlternateTemplate'] == undefined ? forcedFilters[i]['alternate_template'] : forcedFilters[i]['AlternateTemplate'];
            var thisAlternateReportName = forcedFilters[i]['AlternateReportName'] == undefined ? forcedFilters[i]['alternate_report_name'] : forcedFilters[i]['AlternateReportName'];
            var thisAlternateSchema = forcedFilters[i]['AlternateSchema'] == undefined ? forcedFilters[i]['alternate_schema'] : forcedFilters[i]['AlternateSchema'];
            var thisAlternateSource = forcedFilters[i]['AlternateSource'] == undefined ? forcedFilters[i]['alternate_source'] : forcedFilters[i]['AlternateSource'];

            mGlobal.page[model.page].PickListHtml.push = { value: thisFilterName, title: thisPrettyName } 

            switch (model.callEvent) {
                default:
                    break;
                case 'Builder':

                    break;
                case 'Builder Custom Report':
                    $('#' + model.page + '_panel_Available_Filters [data-tableOwner="' + model.tableOwner + '"][type="checkbox"][value="' + thisFilterName + '"]').attr('checked', true);
                    break;
                case 'Reports':

                    break;
            }

            if (thisFilterType == 0 || thisFilterType == 'multiselect') {
                filtersCount++;

                var isDisabled = '';
                var isChecked = '';
                var boolIsRequired = 'false';

                if (thisRequired) {
                    isDisabled = 'disabled';
                    isChecked = 'checked';
                    boolIsRequired = 'true';
                }

                addFilterPanel({
                    name: thisFilterName,
                    selector: model.filterSelector,
                    rootreportname: model.rootreportname,
                    tableOwner: model.tableOwner,
                    required: thisRequired,
                    template: model.template,
                    dataType: 'multiselect',
                    filterType: thisFilterType,
                    AlternateTemplate: thisAlternateTemplate,
                    AlternateReportName: thisAlternateReportName,
                    AlternateSchema: thisAlternateSchema,
                    AlternateSource: thisAlternateSource,
                    title: thisPrettyName,
                    fullreportid: model.fullreportid
                });

            }
            else if (thisFilterType == 1 || thisFilterType == 'datepicker') {
                RequireCount++;

                var thisFilter = '<div class="clearable" ' + ' data-tableowner="' + model.tableOwner + '" data-rootreportname="' + model.rootreportname + '"  style="width:100%; display:inline-block; vertical-align: top;">';
                thisFilter += '                    <div class="form-body">';
                thisFilter += '                        <div class="form-group">';
                //thisFilter += '                            <div class="alert alert-info" style="text-align:center; margin-bottom: 0; padding: 10px;"><strong>' + model.data[0].ReportFilters[i]['PrettyName'] + '</strong></div>';
                thisFilter += '                            <div class="">';
                thisFilter += '                                <div class="date-picker sendDates2Server"' +
                    ' data-rootreportname="' + model.rootreportname + '" ' +
                    ' data-tableowner="' + model.tableOwner + '" ' +
                    ' data-date-format="mm/dd/yyyy" ' +
                    'name="P_' + thisFilterName + '" ' +
                    'data-date="' + CurrentIsoDate + '" ' +
                    'data-olddate="' + CurrentDate + '" ' +
                    'data-title="' + thisPrettyName + '" ' +
                    'data-filter="' + thisFilterName + '">';
                thisFilter += '                                </div>';
                thisFilter += '                            </div>';
                thisFilter += '                        </div>';
                thisFilter += '                   </div>';
                thisFilter += '               </div>';

                var currentCalendarhtml = CreatePanel({
                    href: thisFilterName,
                    name: thisFilterName,
                    title: thisPrettyName,
                    content: thisFilter,
                    tableOwner: model.tableOwner,
                    template: model.template,
                    rootreportname: model.rootreportname,
                    required: thisRequired,
                    dataType: 'datepicker'
                });

                $(model.filterSelector).append(currentCalendarhtml);
                CalendarComponentsPickers.init();

            }
            else if (thisFilterType == 2 || thisFilterType == 'toggle') {

                var isDisabled = '';
                var isChecked = '';
                var boolIsRequired = 'false';

                if (thisRequired) {
                    isDisabled = 'disabled';
                    isChecked = 'checked';
                    boolIsRequired = 'false';
                }

                addToggleFilterPanel({
                    name: thisFilterName,
                    selector: model.filterSelector,
                    rootreportname: model.rootreportname,
                    tableOwner: model.tableOwner,
                    required: thisRequired,
                    template: model.template,
                    dataType: 'multiselect',
                    filterType: thisFilterType,
                    prettyName: thisPrettyName
                });
                //Nothing Yet Specified
            }
            else if (thisFilterType == 3 || thisFilterType == 'columns') {
                //Nothing Yet Specified
            }
            else if (thisFilterType == 4 || thisFilterType == 'hidden') {
                mGlobal.page[model.tableOwner].currentFilters = mGlobal.page[model.tableOwner].currentFilters == undefined ? {} : mGlobal.page[model.tableOwner].currentFilters;
                mGlobal.page[model.tableOwner].currentFilters[thisFilterName] = { chainEnabled: true, data: [thisParamValue], required: true };

                mGlobal.page[model.tableOwner].HiddenFiltersModel.push({ filterName: thisFilterName, value: [thisParamValue] });
                //Nothing Yet Specified
            }
            else {
                //Nothing Yet Specified
            }
        }

        $('[name="ColumnAvailability"] li').css('width', '100%');
        $(model.filterSelector).sortable({ handle: '.grippy', items: '.thisIsSortable' });
    }

    function CreatePanel(model) {

        var columnAlias = '';

        if (mGlobal.page[model.tableOwner].columnAlias != undefined) {
            columnAlias = commonDynamic.functions.tabulate.getColumnAlias(mGlobal.page[model.tableOwner].columnAlias, model.name);
        }

        //get the 'pretty name' for this column
        var corrFilter = _.find(mGlobal.page[model.tableOwner].CurrentBaseFiltersModel, function (bf) { return bf.FilterName == model.name; });

        //if it's different than the regular name, make that the alias (show it in filter panel)
        if (corrFilter && (model.name != corrFilter.PrettyName)) {
            columnAlias = corrFilter.PrettyName;
        }

        var wrapFilter = '';

        model.bind = model.bind == true || model.bind == false ? model.bind : false;
        var send2server = model.bind == true ? 'send2server' : '';

        model.rootreportname != undefined ? model.rootreportname : '';
        model.tableOwner != undefined ? model.tableOwner : '';
        var isRequired = model.required == 'T' || model.required === true ? 'true' : 'false';
        var showAsterisks = (isRequired == 'true' ? '*' : '');
        var DisplayFullName = columnAlias.length > 0 ? columnAlias : model.title != undefined ? model.title.toUpperCase() : model.name;

        DisplayName = DisplayFullName.length > 8 ? DisplayFullName.substring(0, 6) + showAsterisks + "..." : DisplayFullName + showAsterisks;

        var alternates = '';

        var thisIsSortable = model.sortable != false ? 'thisIsSortable' : '';

        if (commonDynamic.functions.tools.isNotNullEmptyOrUndefined(model.AlternateTemplate) &&
            commonDynamic.functions.tools.isNotNullEmptyOrUndefined(model.AlternateReportName) &&
            commonDynamic.functions.tools.isNotNullEmptyOrUndefined(model.AlternateSchema) /*&&
                model.AlternateSource != undefined*/) {
            alternates = 'data-alternatetemplate="' + model.AlternateTemplate + '" data-alternatereport="' + model.AlternateReportName + '" data-alternateschema="' + model.AlternateSchema + '" data-alternatesource="' + model.AlternateSource + '"';
        }

        wrapFilter += '<div class="filterWrap ' + thisIsSortable + ' panel panel-default clearable filter ' + send2server + '" ' +
            ' style="' + model.style + '" ' +
            ' data-rootreportname="' + model.rootreportname + '" ' +
            ' data-fullreportid="' + model.fullreportid + '" ' +
            ' data-template="' + model.template + '" ' +
            ' data-tableowner="' + model.tableOwner + '" ' +
            ' id="' + model.tableOwner + '_panel_' + model.href + '" ' +
            ' data-filter="' + model.name + '" ' +
            ' data-required="' + isRequired + '" ' +
            ' data-link="false" ' +
            ' data-type="' + (model.dataType != undefined ? model.dataType : 'NA') + '" ' +
            ' data-toggle="false" ' +
            ' data-title="' + DisplayName + '" ' +
            ' data-bound=' + model.bind +
            ' ' + alternates +
            '>';
        wrapFilter += '    <div class="panel-heading" data-filter="' + model.name + '" >';
        wrapFilter += '        <div class="panel-title">';
        wrapFilter += '            <a class="accordion-toggle accordion-toggle-styled collapsed" data-toggle="collapse" href="#collapse_' + model.href + '_' + model.tableOwner + '" data-tableowner="' + model.tableOwner + '"></a>';
        wrapFilter += model.sortable != false ? '<span class="grippy"></span>' : '<span class="non-grippy"></span>';

        wrapFilter += '            <label class="accordion-label" title="' + DisplayFullName + '">';
        wrapFilter += model.sortable != false ? DisplayName : DisplayFullName;
        wrapFilter += '             </label>';
        if (model.bind) {
            wrapFilter += ' <div class="btn-group filter-group">';
            //wrapFilter += ' 				<a type="button" class="dropdown-toggle dropdown-filter" data-toggle="dropdown" data-hover="dropdown" data-delay="1000" data-close-others="true" aria-expanded="false" style="text-decoration:none;">';
            //wrapFilter += ' 				<i class="fa fa-cog"></i>';
            //wrapFilter += ' 				</a>';
            //wrapFilter += ' 				<ul class="dropdown-menu pull-right" role="menu">';

            //wrapFilter += ' 		<li>';
            //wrapFilter += '            <a class="accordion-refresh " data-type="refresh" href="javascript:void(0);" data-tableowner="' + model.tableOwner + '" title="Refresh Filter Data">';
            //wrapFilter += '                <i class="fa fa-refresh"></i> Refresh Filter ';
            //wrapFilter += '            </a>';
            //wrapFilter += ' 		</li>';

            //if (isRequired == 'false') {
            //    wrapFilter += ' 		<li>';
            //    wrapFilter += '            <a class="accordion-link" data-type="refresh" href="javascript:void(0);" data-tableowner="' + model.tableOwner + '" title="Link/Unlink Other Filters">';
            //    wrapFilter += '               <i class="fa fa-chain"></i> Link/Unlink Filter';
            //    wrapFilter += '            </a>';
            //    wrapFilter += ' 		</li>';
            //}

            //wrapFilter += ' 				</ul>';

            wrapFilter += ' 				 <a class="dropdown-filter-remove" data-filter="' + model.name + '" data-tableowner="' + model.tableOwner + '" style="text-decoration:none;">';
            wrapFilter += ' 				<i class="fa fa-trash"></i>';
            wrapFilter += ' 				</a>';

            wrapFilter += ' 			</div>';
        }

        //wrapFilter += '            <a class="accordion-toggle accordion-toggle-styled collapsed" data-toggle="collapse" href="#collapse_' + model.href + '_' + model.tableOwner + '" data-tableowner="' + model.tableOwner + '"></a>';


        wrapFilter += '        </div>';
        wrapFilter += '    </div>';
        wrapFilter += '    <div id="collapse_' + model.href + '_' + model.tableOwner + '" class="panel-collapse collapse" style="min-height:200px;max-height:300px; overflow-y: scroll;">';
        wrapFilter += '        <div class="panel-body">';

        if (model.filterType == 0 || model.filterType == 'multiselect') {
            wrapFilter += '         <input type="search" placeholder="Search..." class="filterSearch" name="focus" required>';

            //we don't want the 'select all' checkbox to appear on the "column" filter that first appears
            if (model.name != 'all_filters') {

                //this creates a 'select all' checkbox to allow user to quickly select/deselect all option values in the filter
                wrapFilter += '<div class="checkbox-div filterSelectAllDiv"><label class="checkbox-container"><input type="checkbox" class="filterSelectAll" /><span class="checkmark"></span></label><div class="checkbox-filter-text">SELECT ALL</div></div>';
            }
        }

        wrapFilter += '         <div class="filter-content ">' + model.content + '</div>';
        wrapFilter += '        </div>';
        wrapFilter += '    </div>';
        wrapFilter += '</div>';

        return wrapFilter;
    }

    function CreateTogglePanel(model) {

        var columnAlias = '';

        if (mGlobal.page[model.tableOwner].columnAlias != undefined) {
            columnAlias = commonDynamic.functions.tabulate.getColumnAlias(mGlobal.page[model.tableOwner].columnAlias, model.name);
        }

        //get the 'pretty name' for this column
        var corrFilter = _.find(mGlobal.page[model.tableOwner].CurrentBaseFiltersModel, function (bf) { return bf.FilterName == model.prettyName; });

        //if it's different than the regular name, make that the alias (show it in filter panel)
        if (corrFilter && (model.prettyName != corrFilter.PrettyName)) {
            columnAlias = corrFilter.PrettyName;
        }

        var wrapFilter = '';

        model.bind = model.bind == true || model.bind == false ? model.bind : false;
        var send2server = model.bind == true ? 'send2server' : '';

        model.rootreportname != undefined ? model.rootreportname : '';
        model.tableOwner != undefined ? model.tableOwner : '';
        var isRequired = model.required == 'T' || model.required === true ? 'true' : 'false';
        var DisplayName = columnAlias.length > 0 ? columnAlias : model.title.toUpperCase();

        wrapFilter += '<div class="panel panel-default clearable filter ' + send2server + '" ' +
            ' data-rootreportname="' + model.rootreportname + '" ' +
            ' data-template="' + model.template + '" ' +
            ' data-tableowner="' + model.tableOwner + '" ' +
            ' id="' + model.tableOwner + '_panel_' + model.href + '" ' +
            ' data-filter="' + model.name + '" ' +
            ' data-required="' + isRequired + '" ' +
            ' data-link="false" ' +
            ' data-type="' + (model.dataType != undefined ? model.dataType : 'NA') + '" ' +
            ' data-toggle="true" ' +
            ' data-title="' + DisplayName + '" ' +
            ' data-bound=' + model.bind + '>';
        wrapFilter += '    <div class="panel-heading" data-filter="' + model.name + '" >';
        wrapFilter += '        <h4 class="panel-title">';
        wrapFilter += '            <label class="accordion-label">';
        wrapFilter += '                 <span class="grippy"></span>';
        wrapFilter += '                ' + DisplayName + (isRequired == 'true' ? '*' : '');
        wrapFilter += '             </label>';
        //wrapFilter += '            <input type="checkbox" class="make-switch" data-size="small">';
        wrapFilter += '             <label class="checkbox-container"><input type="checkbox" class="make-switch" data-size="small" ' + (model.initValue == 'Y' ? 'checked' : '') + '><span class="checkmark"></span></label>';
        wrapFilter += '        </h4>';
        wrapFilter += '    </div>';
        wrapFilter += '</div>';

        return wrapFilter;
    }

    function addFilterPanel(model) {

        var htmlPanel = CreatePanel({
            href: 'Filter_' + model.name,
            name: model.name,
            title: model.title,
            content: $.fn.revamp.templates.div_HourGlass(),
            bind: true,
            template: model.template,
            rootreportname: model.rootreportname,
            tableOwner: model.tableOwner,
            required: model.required,
            dataType: model.dataType,
            filterType: model.filterType,
            AlternateTemplate: model.AlternateTemplate,
            AlternateReportName: model.AlternateReportName,
            AlternateSchema: model.AlternateSchema,
            AlternateSource: model.AlternateSource,
            fullreportid: model.fullreportid
        });

        mGlobal.page[model.tableOwner].currentFilters = mGlobal.page[model.tableOwner].currentFilters == undefined ? {} : mGlobal.page[model.tableOwner].currentFilters;

        mGlobal.page[model.tableOwner].currentFilters[model.name] = mGlobal.page[model.tableOwner].currentFilters[model.name] == undefined ? {} : mGlobal.page[model.tableOwner].currentFilters[model.name];

        mGlobal.page[model.tableOwner].currentFilters[model.name].data = mGlobal.page[model.tableOwner].currentFilters[model.name].data == undefined ? [] : mGlobal.page[model.tableOwner].currentFilters[model.name].data;
        mGlobal.page[model.tableOwner].currentFilters[model.name].chainEnabled = model.required ? false : true;
        mGlobal.page[model.tableOwner].currentFilters[model.name].required = model.required == undefined ? false : model.required;

        $(model.selector).append(htmlPanel);
    }

    function addToggleFilterPanel(model) {


        mGlobal.page[model.tableOwner].currentFilters = mGlobal.page[model.tableOwner].currentFilters == undefined ? {} : mGlobal.page[model.tableOwner].currentFilters;
        mGlobal.page[model.tableOwner].currentFilters[model.name] = mGlobal.page[model.tableOwner].currentFilters[model.name] == undefined ? {} : mGlobal.page[model.tableOwner].currentFilters[model.name];

        var DefaultValues = mGlobal.page[model.tableOwner].AvailableFilters == undefined ? [] : mGlobal.page[model.tableOwner].AvailableFilters.filter(function (Filter) { return Filter.filter_name == model.name; });

        var ToggleDefaultValue = DefaultValues.length == 1 ? DefaultValues[0].paramvalue : 'N';

        mGlobal.page[model.tableOwner].currentFilters[model.name].data = mGlobal.page[model.tableOwner].currentFilters[model.name].data == undefined ? [ToggleDefaultValue] : mGlobal.page[model.tableOwner].currentFilters[model.name].data;

        var htmlPanel = CreateTogglePanel({
            href: 'Filter_' + model.name,
            name: model.name,
            title: model.name,
            content: $.fn.revamp.templates.div_HourGlass(),
            bind: true,
            template: model.template,
            rootreportname: model.rootreportname,
            tableOwner: model.tableOwner,
            required: model.required,
            dataType: model.dataType,
            filterType: model.filterType,
            prettyName: model.prettyName,
            initValue: mGlobal.page[model.tableOwner].currentFilters[model.name].data.length > 0 ? mGlobal.page[model.tableOwner].currentFilters[model.name].data[0] : 'N'
        });

        mGlobal.page[model.tableOwner].currentFilters = mGlobal.page[model.tableOwner].currentFilters == undefined ? {} : mGlobal.page[model.tableOwner].currentFilters;

        mGlobal.page[model.tableOwner].currentFilters[model.name] = mGlobal.page[model.tableOwner].currentFilters[model.name] == undefined ? {} : mGlobal.page[model.tableOwner].currentFilters[model.name];


        mGlobal.page[model.tableOwner].currentFilters[model.name].chainEnabled = model.required ? false : true;
        mGlobal.page[model.tableOwner].currentFilters[model.name].required = model.required == undefined ? false : model.required;

        $(model.selector).append(htmlPanel);
    }

    function removeFilterPanel(model) {
        $('#' + model.tableOwner + '_panel_Filter_' + model.name).remove();

        delete mGlobal.page[model.tableOwner].currentFilters[model.name];
    }

    function loadCurrentFilter(model) {

        mGlobal.page[model.tableOwner].currentFilters[model.name].data = [];

        model.isToggle = model.isToggle == undefined ? false : model.isToggle;

        if (!model.isToggle) {
            $('.panel.filter[data-bound=true][data-filter="' + model.name + '"][data-tableowner="' + model.tableOwner + '"] input:checked').each(function (i, e) {
                if (!$(this).hasClass('filterSelectAll')) { //don't add the 'select all' option value to our list of filters
                    mGlobal.page[model.tableOwner].currentFilters[model.name].data.push($(this).val());
                }
            });
        }
        else {
            if ($('.panel.filter[data-bound=true][data-filter="' + model.name + '"][data-tableowner="' + model.tableOwner + '"] input').is(':checked')) {
                mGlobal.page[model.tableOwner].currentFilters[model.name].data = ['Y'];
            }
            else {
                mGlobal.page[model.tableOwner].currentFilters[model.name].data = ['N'];
            }
        }

        if (model.refreshNextFilter != undefined && model.refreshNextFilter.execute != undefined && model.refreshNextFilter.execute) {
            var currentNum = model.refreshNextFilter.fromPosition + 1;

            if ($('#Filters_Accordion_' + model.fullreportid + ' .panel.filter[data-bound=true][data-toggle=false]:eq(' + currentNum + ')').find('.collapse.in').length > 0) {

                $('#Filters_Accordion_' + model.fullreportid + ' .panel.filter[data-bound=true][data-toggle=false] a[data-type="refresh"].accordion-refresh:eq(' + currentNum + ')').trigger('click');

            }
            else {
                var totalFilterCount = $('#Filters_Accordion_' + model.fullreportid + ' .panel.filter[data-bound=true][data-toggle=false]').length;

                for (var i = (currentNum + 1); i < totalFilterCount; i++) {

                    if ($('#Filters_Accordion_' + model.fullreportid + ' .panel.filter[data-bound=true][data-toggle=false]:eq(' + i + ')').find('.collapse.in').length > 0) {

                        $('#Filters_Accordion_' + model.fullreportid + ' .panel.filter[data-bound=true][data-toggle=false] a[data-type="refresh"].accordion-refresh:eq(' + i + ')').trigger('click');
                        break;
                    }

                }
            }


        }

        commonDynamic.functions.tabulate.reloadTable({ sourceTable: model.tableOwner });
    }

    function GenerateBaseReport(model) {

        if (mGlobal.page == undefined) {
            mGlobal.page = {};
        }

        if (mGlobal.page[model.page] == undefined) {
            mGlobal.page[model.rawTableName] = {};
        }

        mGlobal.page[model.rawTableName].name = model.rawTableName;
        mGlobal.page[model.rawTableName].exports = [];
        mGlobal.page[model.rawTableName].currentJsonData = [];
        mGlobal.page[model.rawTableName].columns = model.dynocol != undefined ? model.dynocol : '';
        mGlobal.page[model.rawTableName].columnOverrides = model.columnOverrides != undefined ? model.columnOverrides : [];
        mGlobal.page[model.rawTableName].exports = [];
        mGlobal.page[model.rawTableName].PickListHtml = [],
            mGlobal.page[model.rawTableName].HiddenFiltersModel = [];
        mGlobal.page[model.rawTableName].CurrentFiltersModel = {};
        mGlobal.page[model.rawTableName].CurrentBaseFiltersModel = {};
        mGlobal.page[model.rawTableName].schema = model.schema;
        mGlobal.page[model.rawTableName].template = model.template;
        mGlobal.page[model.rawTableName].inheritFilters = model.inheritFilters != undefined ? model.inheritFilters : false;
        mGlobal.page[model.rawTableName].currentFilters = model.callEvent == 'Builder Custom Report' ? mGlobal.page[model.rawTableName].currentFilters : mGlobal.page[model.rawTableName].inheritFilters ? mGlobal.page[model.rawTableName].currentFilters : {};

        switch (model.callEvent) {
            default:

            case 'Builder':

                var columnsModel = {
                    direction: 'POST',
                    currentBaseCustomReportID: model.customreportid,
                    callEvent: 'Report Builder',
                    async: false
                };

                handleBaseReportColumns(columnsModel);

                model.direction = 'POST';
                handleBaseReportFilters(model);

                mGlobal.page[model.page].stripTime = true;
                mGlobal.page[model.page].rootReportID = mGlobal.variable.currentSelectedBaseReportBaseID;

                model.method = 'POST';
                mGlobal.page[model.page].columnOrderFromDB = handleRootReportColumnOrder(model);

                commonDynamic.functions.tabulate.convertColumnOrderFromDBtoOrder(model);

                mGlobal.page[model.page].generatedInitCompleteActions = function () {
                    if (model.datatabletype == 'builder') {
                        $('#currentReports').attr('disabled', true);
                    }

                    buildDataTableActions(model);

                    if (model.hasCustomReports) {
                        builder.loadCustomReportList({
                            direction: 'POST',
                            customreportid: mGlobal.page[model.page].rootReportID, //model.basecustomreportid,
                            selector: model.selector
                        }, {
                            });
                    }
                };

                break;
            case 'Builder Custom Report':
                mGlobal.page[model.page].stripTime = true;
                mGlobal.page[model.page].generatedInitCompleteActions = function () {
                    buildDataTableActions(model);
                };
                break;
            case 'Reports':
                mGlobal.page[model.page].generatedInitCompleteActions = function () {

                };
                break;
            case 'Stand Alone':
                if (mGlobal.page[model.page].generatedInitCompleteActions == undefined) {
                    mGlobal.page[model.page].generatedInitCompleteActions = function () {
                        buildDataTableActions(model);
                    };
                }
                break;
            case 'Stand Alone without Toggle':
                if (mGlobal.page[model.page].generatedInitCompleteActions == undefined) {
                    mGlobal.page[model.page].generatedInitCompleteActions = function () {
                        buildDataTableActions(model);
                    };
                }
                break;
            case 'Stand Alone with Filters':
                model.direction = 'POST';
                handleBaseReportFilters(model);


                if (mGlobal.page[model.page].order == undefined || mGlobal.page[model.page].order.length < 1) {
                    commonDynamic.functions.tabulate.convertColumnOrderFromDBtoOrder(model);
                }

                if (mGlobal.page[model.page].generatedInitCompleteActions == undefined) {
                    mGlobal.page[model.page].generatedInitCompleteActions = function () {

                        buildDataTableActions(model);

                    };
                }
                break;
        }

        commonDynamic.functions.tabulate.filters.builderDataTableAjax(model);

        commonDynamic.functions.tabulate.structs.getDataTable({
            report: model.report,
            tableName: model.tableName,
            tableID: model.tableID,
            tableContainer: model.tableContainer,
            jsonURL: model.jsonURL,
            structURL: model.structURL,
            page: model.page,
            customreportid: model.customreportid,
            selector: model.selector,
            CustomReportselector: model.CustomReportselector,
            datatabletype: model.callEvent,
            scrollY: model.scrollY,
            fixedColumns: model.fixedColumns == undefined ? undefined : model.fixedColumns,
            dom: model.dom == undefined ? undefined : model.dom,
            oLanguage: model.oLanguage == undefined ? undefined : model.oLanguage,
            scrollX: model.scrollX == undefined ? true : model.scrollX,
            dynocol: model.dynocol != '' && model.dynocol != undefined ? model.dynocol : mGlobal.page[model.page].columns,
            schema: model.schema,
            template: model.template,
            order: mGlobal.page[model.page].order != undefined ? mGlobal.page[model.page].order : model.order,
            pageLength: model.pageLength,
            hasTableHeightSelector: model.hasTableHeightSelector == undefined ? true : model.hasTableHeightSelector,
            hasTableZoom: model.hasTableZoom == undefined ? true : model.hasTableZoom,
            hasSelectedRowData: model.hasSelectedRowData == undefined ? false : model.hasSelectedRowData,
            hasPsnSearch: model.hasPsnSearch == undefined ? false : model.hasPsnSearch,
            hasOpenSideBar: model.hasOpenSideBar == undefined ? true : model.hasOpenSideBar,
            hasExports: model.hasExports == undefined ? true : model.hasExports,
            customTableClass: model.customTableClass == undefined ? '' : model.customTableClass
        });

        model.currentReport = model.report;

        //No Longer needed
        //buildExport.create(model);

    }

    function callGenerateBaseReport(model) {

        model.method = 'POST';
        model.report = model.report;
        model.tableName = mGlobal.page.builder.datatableName;
        model.rawTableName = mGlobal.page.builder.datatableName;
        model.tableID = '#' + mGlobal.page.builder.datatableName;
        model.tableContainer = '#dataTableContainer';
        var defaultURL = '/dynamic/dynosearch';
        var finalUrl = mGlobal.page[model.page].overrideJsonURL != undefined && mGlobal.page[model.page].overrideJsonURL.length > 0 ? mGlobal.page[model.page].overrideJsonURL : defaultURL;
        model.jsonURL = finalUrl + '?iReport=' + mGlobal.page.builder.datatableName;

        var defaultStructURL = '/dynamic/GetTableStruct';
        model.structURL = mGlobal.page[model.page].overrideStructURL != undefined && mGlobal.page[model.page].overrideStructURL.length > 0 ? mGlobal.page[model.page].overrideStructURL : defaultStructURL;
        model.page = mGlobal.page.builder.datatableName;
        model.customreportid = mGlobal.variable.currentSelectedBaseCustomReportID;
        model.basecustomreportid = mGlobal.variable.currentSelectedBaseReportBaseID;
        model.schema = mGlobal.variable.currentSelectedBaseReportSchema;
        model.template = mGlobal.variable.currentSelectedBaseReportSchema;
        model.selector = '#currentCustomReports';
        model.loadDT = true;
        model.tableOwner = mGlobal.page.builder.datatableName;
        model.rootreportname = $('#currentReports option:selected').val();
        model.scrollY = '450px';
        model.CustomReportselector = '#currentCustomReports';
        model.hasCustomReports = true;
        model.overrideAvailableFiltersTitles = true;
        model.hasOpenSideBar = true;
        model.hasExports = true;

        switch (model.callEvent) {
            case 'Builder Custom Report':
                model.dynocol = mGlobal.page[model.page].columns;
                break;
        }

        mGlobal.page[model.page] = mGlobal.page[model.page] == undefined ? {} : mGlobal.page[model.page];


        mGlobal.page[model.page].generatedInitCompleteActions = function () { };
        mGlobal.page[model.page].initCompleteActions = function () { };
        mGlobal.ajaxData[model.page] = function () { };

        GenerateBaseReport(model);

        resizeFilterDiv(model);
    }

    return {
        generateContainer: function (model) {
            return generateDatatableContainer(model);
        },
        columns: function (model) {
            handleBaseReportColumns(model);
        },
        filters: function (model) {
            handleBaseReportFilters(model);
        },
        loadFilters: function (model) {
            PopulateBaseReportFiltersList(model);
        },
        createFilterPanel: function (model) {
            addFilterPanel(model);
        },
        deleteFilterPanel: function (model) {
            removeFilterPanel(model);
        },
        loadCurrentFilter: function (model) {
            loadCurrentFilter(model);
        },
        create: function (model) {
            GenerateBaseReport(model);
        },
        callCreate: function (model) {
            callGenerateBaseReport(model);
        },
        loadRootReportColumnOrder: function (model) {
            handleRootReportColumnOrder(model);
        },
        hideDefaults: function (model) { hideDefaults(model) }
    };
}();

var buildCustomReport = function buildCustomReport() {

    function GenerateDataTableOnTheFly(model) {

        theLoader.show({ id: model.page + '_loader' });

        setTimeout(function () {
            if (mGlobal.page[model.rawTableName] == undefined) {
                mGlobal.page[model.rawTableName] = {};
            }

            mGlobal.page[model.rawTableName].name = model.rawTableName;
            mGlobal.page[model.rawTableName].exports = [];
            mGlobal.page[model.rawTableName].currentJsonData = [];
            mGlobal.page[model.rawTableName].columns = '';
            mGlobal.page[model.rawTableName].columnOverrides = [];
            mGlobal.page[model.rawTableName].structure = {};
            mGlobal.page[model.rawTableName].exports = [];
            mGlobal.page[model.rawTableName].PickListHtml = [],
                mGlobal.page[model.rawTableName].HiddenFiltersModel = [];
            mGlobal.page[model.rawTableName].currentFilters = {};
            mGlobal.page[model.rawTableName].CurrentFiltersModel = {};
            mGlobal.page[model.rawTableName].CurrentBaseFiltersModel = {};
            mGlobal.page[model.rawTableName].AvailableFilters = [];
            mGlobal.page[model.rawTableName].schema = model.schema;
            mGlobal.page[model.rawTableName].template = model.template;
            mGlobal.page[model.rawTableName].hasTableZoom = model.hasTableZoom;
            mGlobal.page[model.rawTableName].hasTableHeightSelector = model.hasTableHeightSelector;
            mGlobal.page[model.rawTableName].hasQueryClause = model.hasQueryClause;
            mGlobal.page[model.rawTableName].hasExports = model.hasExports;
            mGlobal.page[model.rawTableName].stripTime = model.stripTime == undefined ? true : model.stripTime;

            mGlobal.page[model.rawTableName].drawCallBackActions = function () {

            };

            mGlobal.page[model.rawTableName].generatedInitCompleteActions = function () {
                buildDataTableActions(model);
            };

            model.currentReportID = model._customreportid;

            //TODO: Verify why this line is here. It is handled else where.
            //mGlobal.page[model.rawTableName].columns = builder.columns(model);

            var defaultURL = '/dynamic/dynosearch';
            var finalUrl = model.overrideJsonURL != undefined && model.overrideJsonURL.length > 0 ? model.overrideJsonURL : defaultURL;
            model.jsonURL = finalUrl + '?iReport=' + model.page

            var tabModel = {
                page: model.rawTableName,
                rawTableName: model.rawTableName,
                tableID: '#' + model.rawTableName,
                navTabSelector: model.navTabSelector,
                mainContentSelector: model.mainContentSelector,
                datatableWrapper: model.datatableWrapper,
                tableContainer: '#' + model.datatableWrapper,
                report: model.currentReport,
                currentReport: model.currentReport,
                rootreportname: model.currentReport,
                currentReportID: model._customreportid,
                customreportid: model._customreportid,
                currentCustomReportID: model._customreportid,
                _customreportid: model._customreportid,
                fullreportid: model.customreportid,
                reportidandtitle: model.customreportid,
                tableName: model.rawTableName,
                tableOwner: model.rawTableName,
                reportLocation: model.reportLocation,
                _reporttitle: model._reporttitle,
                reporttitle: model.reporttitle,
                FilterDiv: model.FilterDiv,
                BodyContentWrap: model.BodyContentWrap,
                rowWrapper: model.rowWrapper,
                FilterAccordion: model.FilterAccordion,
                filterSelector: '#' + model.FilterAccordion,
                addToTab: model.addToTab,
                containerid: model.containerid,
                contentWrapper: model.contentWrapper,
                addSidebar: model.addSidebar,
                sideBarWrapperSelector: model.sideBarWrapperSelector,
                dynocol: mGlobal.page[model.page].columns,
                jsonURL: model.jsonURL,
                loadDT: true,
                loadAvailableFilters: false,
                scrollY: '400px',
                method: 'POST',
                callEvent: 'Reports',
                template: model.template,
                schema: model.schema,
                mandatory_type: 'T,F',
                hasExports: model.hasExports,
                hasOpenSideBar: model.hasOpenSideBar,
                hasTableZoom: mGlobal.page[model.page].hasTableZoom,
                hasTableHeightSelector: mGlobal.page[model.page].hasTableHeightSelector,
                hasQueryClause: mGlobal.page[model.page].hasQueryClause,
                headertitle: model.headertitle
            };

            GenerateTabAndContentForReport(tabModel);

            commonDynamic.functions.tabulate.filters.builderDataTableAjax(tabModel);

            tabModel.direction = 'POST';
            //This next call makes nested calls to get Filters, Columns, and Then Generate Datatable.
            handleCustomReportFilters(tabModel);

            // #region Custom Report Columns From DB and Populates Columns property

            //model.direction = 'POST';
            //handleCustomReportColumnOrder(model)
            //mGlobal.page[model.page].columnOrderFromDB = handleCustomReportColumnOrder(model);

            //commonDynamic.functions.tabulate.convertColumnOrderFromDBtoOrder(model);

            // #endregion


        }, 300);
    }

    function GenerateTabAndContentForReport(model) {

        if (model.addToTab) {
            var navTab = $.fn.revamp.templates.li_NavTab(model);

            $(model.navTabSelector).append(navTab);

            var tabPane = $.fn.revamp.templates.div_TabPane(model);

            $(model.mainContentSelector).append(tabPane);
        }
        else {
            var contentWrapper = $.fn.revamp.templates.div_TabPane(model);

            $(model.mainContentSelector).append(contentWrapper);
        }

        if (model.addSidebar) {

            var sideBarModel = {};
            sideBarModel.name = model.FilterDiv;
            sideBarModel.navTabs = [];
            sideBarModel.navTabs.push({
                id: 'tab_Core_Filters_' + model.fullreportid,
                title: 'FILTERS',
                innerHtml: '<div class="panel-group accordion" id="' + model.FilterAccordion + '"></div>',
                active: true
            });

            var sidebarHTML = $.fn.revamp.templates.sideBarTemplate(sideBarModel);

            $(model.sideBarWrapperSelector).append(sidebarHTML);

            resizeFilterDiv({ page: model.reportidandtitle });
        }

    }

    function handleCustomReportFilters(model) {
        switch (model.direction) {
            case 'POST':

                var currentCustomReportID = model.currentCustomReportID; // $('#currentCustomReports').val();

                if (+currentCustomReportID > 0) {

                    model.data = {
                        SearchReport: 'custom report filters',
                        P_CUSTOM_REPORT_ID_: currentCustomReportID,
                        //P_MANDATORY: model.mandatory_type == undefined ? 'F' : model.mandatory_type,
                        P_VERIFY: 'T',
                        P_GET_LATEST: 'F',
                        template: 'dynamic',
                        schema: 'dynamic',
                        queryType: "query",
                        start: 0,
                        length: -1,
                        'order[0][column]': 'Report_Name',
                        'order[0][dir]': 'asc',
                        'order[1][column]': 'Filter_Order',
                        'order[1][dir]': 'asc'
                    };
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/dynamic/dynosearch?iReport=" + model.data.SearchReport;
                    model.options.callBack = function (model) {
                        handleCustomReportFilters(model);
                    };

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false;

                    ajaxDynamic(model);
                }

                break;
            case 'CALLBACK':

                mGlobal.page[model.page].AvailableFilters = model.response.data;
                model.reportFilters = model.response.data;

                switch (model.callEvent) {
                    default:
                    case "Builder":
                        loadFilters(model);
                        break;
                    case 'Builder Custom Report':
                        loadFilters(model);
                        break;
                    case 'Reports':
                        loadFilters(model);
                        break;
                }

                break;
        }
    }

    function loadFilters(model) {
        switch (model.callEvent) {
            default:
            case 'Builder':
                $(model.filterSelector + ' .clearable').remove();
                break;
            case 'Builder Custom Report':
                $(model.filterSelector + ' .clearable:not([data-filter="all_filters"])').remove();
                break;
            case 'Reports':
                $(model.filterSelector + ' .clearable').remove();
                break;
        }

        buildBaseReport.loadFilters(model);

        switch (model.callEvent) {
            default:
            case 'Builder':

                break;
            case 'Builder Custom Report':

                model.currentCustomReportID = mGlobal.variable.currentCustomReportID;
                model.direction = 'POST',

                    builder.columns(model);

                break;
            case 'Reports':
                model.currentCustomReportID = model.currentCustomReportID;
                model.direction = 'POST';

                builder.columns(model);
                break;
        }
    }

    function handleCustomReportColumnOrder(model) {
        switch (model.direction) {
            case 'POST':
                var currentCustomReportID = model.currentReportID == undefined ? 0 : model.currentReportID;

                if (+currentCustomReportID > 0) {

                    model.data = {
                        SearchReport: 'custom report order',
                        start: 0,
                        length: -1,
                        P_GET_LATEST: 'T',
                        P_ENABLED: 'Y',
                        P_VERIFY: 'T',
                        P_CUSTOM_REPORT_ID: currentCustomReportID,
                        template: 'dynamic',
                        schema: 'dynamic',
                        queryType: "query",
                        'order[0][column]': 'sort_order',
                        'order[0][dir]': 'asc'
                    };
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/dynamic/dynosearch?iReport=" + model.data.SearchReport;
                    model.options.callBack = function (model) {
                        handleCustomReportColumnOrder(model);
                    };

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false;

                    ajaxDynamic(model);
                }
                else {
                    return [];
                }
                break;
            case 'CALLBACK':
                switch (model.callEvent) {
                    default:
                        break;
                    case 'Reports':
                        mGlobal.page[model.page].columnOrderFromDB = model.response.data;
                        commonDynamic.functions.tabulate.convertColumnOrderFromDBtoOrder(model);

                        commonDynamic.functions.tabulate.structs.getDataTable({
                            report: model.report,
                            tableName: model.tableName,
                            tableID: model.tableID,
                            tableContainer: model.tableContainer,
                            structURL: model.structURL,
                            jsonURL: model.jsonURL,
                            page: model.page,
                            customreportid: model.customreportid,
                            selector: model.selector,
                            CustomReportselector: model.CustomReportselector,
                            datatabletype: 'builder',
                            scrollY: model.scrollY,
                            dynocol: model.dynocol != '' && model.dynocol != undefined ? model.dynocol : mGlobal.page[model.page].columns,
                            async: true,
                            template: model.template,
                            schema: model.schema,
                            hasTableZoom: model.hasTableZoom,
                            hasTableHeightSelector: model.hasTableHeightSelector,
                            hasQueryClause: model.hasQueryClause,
                            order: mGlobal.page[model.page].order
                        });

                        if (model.hasExports) {
                            buildExport.create(model);
                        }

                        if (model.hasOpenSideBar) {
                            SideBarManipulation({ action: 'open', closestfilter: model.FilterDiv, closestWrapper: model.BodyContentWrap });
                        }
                        break;
                }
                break;
            case 'FORMAT':
                switch (model.callEvent) {
                    default:
                    case "Builder":
                    case 'Builder Custom Report':


                        break;

                }
                break;
        }
    }

    return {
        create: function (model) {
            GenerateDataTableOnTheFly(model);
        },
        filters: function (model) {
            handleCustomReportFilters(model);
        },
        columnOrder: function (model) {
            handleCustomReportColumnOrder(model);
        }
    };
}();

var buildExport = function buildExport() {

    function BuildExportsCustomReport(model) {

        var FilterModel = {};
        FilterModel = model;
        FilterModel.direction = 'POST';
        FilterModel.callEvent = model.callEvent;

        handleBaseReportExports(FilterModel);
    }

    function handleBaseReportExports(model) {
        switch (model.direction) {
            case 'POST':

                model.data = {
                    report: model.currentReport,
                    template: model.template
                    //schema: model.schema
                };
                model.options = model.options == undefined ? {} : model.options;
                model.options.async = true;
                model.options.url = "/dynamic/Get_Base_Report_Structure?iReport=" + model.data.report;
                model.options.callBack = function (model) {
                    handleBaseReportExports(model);
                };

                model.notification = model.notification == undefined ? {} : model.notification;
                model.notification.pulse = false;

                ajaxDynamic(model);

                break;
            case 'CALLBACK':

                mGlobal.page[model.page].exports = model.response[0].CrystalReports;
                mGlobal.page[model.page].structure = model.response[0];

                break;
        }
    }

    return {
        create: function (model) {
            BuildExportsCustomReport(model);
        },
        callback: function (model) {
            handleBaseReportExports(model);
        }

    };
}();

function ajaxUniversal(url, data, switchMechanism) {
    var ajaxData;

    var _async = (switchMechanism.async == undefined || switchMechanism.async == null) ? true : switchMechanism.async;
    var _type = (switchMechanism.type == undefined || switchMechanism.type == null) ? 'POST' : switchMechanism.type;
    //var returnBypass = (switchMechanism.returnBypass == undefined || switchMechanism.returnBypass == null) ? false : switchMechanism.returnBypass;

    $.ajax({
        url: url,
        async: _async,
        type: _type,
        headers: {
            __RequestVerificationToken: currentRequestVerificationToken
        },
        data: data,
        dataType: 'json',
        success: function (response) {

            var callCase = (switchMechanism.callCase == undefined || switchMechanism.callCase == null) ? switchMechanism : switchMechanism.callCase;

            switchMechanism.model = switchMechanism.model == undefined ? {} : switchMechanism.model;

            switch (callCase) {
                case 'Get DB Types':
                    break;

                case 'Save Report':
                    mGlobal.page.builder.LastSavedReport = response;
                    builder.loadCustomReportList({
                        direction: 'POST',
                        customreportid: mGlobal.variable.currentSelectedBaseReportBaseID,
                        selector: '#currentCustomReports'
                    }, {});

                    var RootReportName = switchMechanism.model["thisModel.V_ROOT_REPORT_NAME"];
                    var CustomReportName = switchMechanism.model["thisModel.I_REPORT_NAME"];
                    var SuccessMsg = 'Root Report : ' + RootReportName + '<br/> Custom Report: ' + CustomReportName;

                    toastr[ToastConstants.genericSuccess.type](SuccessMsg, 'Custom Report Saved');
                    break;


                    break;
            }

            if (!_async) {
                ajaxData = response;
            }
        },
        error: function (result) {
            if (result.status == 403) {
                toastr[ToastConstants.noPrivilege.type](ToastConstants.noPrivilege.msg, ToastConstants.noPrivilege.title);
            }
            else {
                toastr[ToastConstants.genericError.type](ToastConstants.genericError.msg, ToastConstants.genericError.title);
            }
        }
    });

    return ajaxData;
}

function xGetDBTypes(model) {

    var dbTypes = ajaxUniversal("/dynamic/gettypes", {
    }, {
            callCase: 'Get DB Types',
            async: false,
            model: model
        });

    if (typeof dbTypes == 'string' && Base64.isThisEncoded(dbTypes)) {
        dbTypes = JSON.parse(Base64.decode(dbTypes));
    }

    return dbTypes;
}

function SideBarManipulation(model) {

    switch (model.action) {

        case 'open':
            
            //$('[name="' + model.closestfilter + '"]').addClass('page-quick-filter-sidebar-wrapper-open');
            $('[name="' + model.closestfilter + '"]').css({ 'border-left-color': '#ddd', 'border-left-width': '1px', 'border-left-style': 'solid' });
     
                $('[name="' + model.closestWrapper + '"]').removeClass('col-md-12');
                $('[name="' + model.closestWrapper + '"]').addClass('col-md-10');
                $('[name="' + model.closestfilter + '"]').show("slide", { direction: "right" }, 0);
       

            break;

        case 'close':

            
            //$('[name="' + model.closestfilter + '"]').removeClass('page-quick-filter-sidebar-wrapper-open');
            $('[name="' + model.closestfilter + '"]').css({ 'border-left-color': '', 'border-left-width': '', 'border-left-style': '' });
            $('[name="' + model.closestfilter + '"]').hide("slide", { direction: "right" }, 0);
            
                $('[name="' + model.closestWrapper + '"]').removeClass('col-md-10');
                $('[name="' + model.closestWrapper + '"]').addClass('col-md-12');
            
            
            

            break;


        case 'expand_parent':

            $('[name="FilterDiv_' + model.tableName + '"] .accordion-toggle').trigger('click');
            //$('#' + model.tableName + ' #collapse_Available_Filters_' + model.tableName).removeClass('collapse');
            //$('#' + model.tableName + ' #collapse_Available_Filters_' + model.tableName).addClass('expand');

            break;

        case 'collapse_parent':

            $('#' + model.tableName + ' #collapse_Available_Filters_' + model.tableName).removeClass('expand');
            $('#' + model.tableName + ' #collapse_Available_Filters_' + model.tableName).addClass('collapse');

            break;

        case 'expand_children':

            if (model.thisTable == model.tableName) {

                for (var i = 0; i < model.filter.length; i++) {

                    $('[name="FilterDiv_' + model.tableName + '"] #chbx_' + model.tableName + '_' + model.filter[i]).trigger('click');
                    //$($('#' + model.tableName + ' input[type="checkbox"]').filter(function () { return this.value == model.filter[i]; })).trigger('click');
                    //$('#' + model.tableName + ' #' + model.tableName + '_panel_Filter_' + model.filter[i] + ' .accordion-toggle').trigger('click');
                }
            }

            break;

        case 'apply_filters':

            $.each(model.filter, function (index, p) {

                $($('#collapse_Filter_' + p.key + '_' + model.tableName + ' input').filter(function () { return this.value == p.value; })).trigger('click');
            });

            break;

        case 'refresh':

            for (var i = 0; i < model.filter.length; i++) {

                if ($('#' + model.tableName + '_panel_Filter_' + model.filter[i] + ' .accordion-refresh').length > 0) {

                    $('#' + model.tableName + '_panel_Filter_' + model.filter[i] + ' .accordion-refresh').trigger('click');
                }
            }

            break;
    }
}

function buildDataTableActions(model) {
    var actionItemHTML = $.fn.revamp.templates.a_CustomReportExport(model);
    var actionItemPrint = $.fn.revamp.templates.a_CustomReportPrint(model);
    var filterSelector = $('#' + model.page + '_filter');

    model.hasExports = model.hasExports != undefined && (model.hasExports == true || model.hasExports == false) ? model.hasExports : true;

    if (model.hasExports) {
        filterSelector.append(actionItemHTML);
    }

    filterSelector.append(actionItemPrint);

    var actionItemHTML2 = $.fn.revamp.templates.a_CustomReportClause(model);

    model.hasQueryClause = model.hasQueryClause != undefined && (model.hasQueryClause == true || model.hasQueryClause == false) ? model.hasQueryClause : true;

    if (model.hasQueryClause && model.callEvent != undefined && model.callEvent.indexOf('without Toggle') == -1) {
        filterSelector.append(actionItemHTML2);
    }

    var FilterName = model.FilterDivBindOverRide != undefined ? model.FilterDivBindOverRide : 'FilterDiv_' + model.page;
    var BodyContentWrapper = 'BodyContentWrapper_' + model.page;

    var sideBarState = 'open';
    if (!model.hasOpenSideBar) {
        sideBarState = 'close';
    }

    var actionItemRefresh = $.fn.revamp.templates.a_CustomReportRefresh(model);

    filterSelector.append(actionItemRefresh);


    if (model.callEvent != undefined && model.callEvent.indexOf('without Toggle') == -1) {

        var action = $.fn.revamp.templates.div_ToggleSidebar({ sideBarState: sideBarState, FilterName: FilterName, BodyContentWrapper: BodyContentWrapper, page: model.page });

        filterSelector.append(action);
    }

    if (model.customTableClass != undefined) {
        filterSelector.addClass(model.customTableClass);
    }

    if (model.customTableClass != undefined) {
        filterSelector.addClass(model.customTableClass);
    }

    $('[name="BodyContentWrapper_' + model.page + '"] .dtQuickActionPopover').popover({
        container: 'body',
        trigger: 'hover',
        placement : 'bottom'
    })
}

//Loads Custom Report Filters

function resizeFilterDiv(model) {

    try {

        var footerHeight = +$('.page-footer').css('height').replace('px', ''); //Footer Height
        var topHeight = +$('.FilterDiv').css('top').replace('px', '');
        var contentHeight = +$('.page-content').css('min-height').replace('px', '');
        var topOffset = $('.navbar-fixed-top').css('height').replace('px', '');
        var NavTabHeight = $('.FilterDiv .nav-tabs').css('height').replace('px', '');

        var FilterHeight = contentHeight - (topHeight - topOffset);
        // var FilterHeight = contentHeight;
        // var TabPainHeight = FilterHeight - NavTabHeight - footerHeight;
        var TabPainHeight = FilterHeight - 37 - 4;

        var TuneAdjustment = -8;

        FilterHeight = $('.page-footer').position().top < $(window).height() ? FilterHeight + TuneAdjustment : FilterHeight + footerHeight + TuneAdjustment;
        TabPainHeight = $('.page-footer').position().top < $(window).height() ? TabPainHeight + TuneAdjustment : TabPainHeight + footerHeight + TuneAdjustment;

        $('.FilterDiv').css('height', FilterHeight + 'px');
        $('.FilterDiv .tab-pane').css('height', TabPainHeight + 'px');
        $('.FilterDiv .tab-pane').css('overflow-y', 'scroll');

    }
    catch (e) {

    }
}

function callGenerateStandAloneDT(model) {
    toastr.info("Please wait while the datatable loads.")
    builder.createFRED(model);
}

var sidebar = { fn: {} }

sidebar.fn = (function () {

    return {

        open: function (model) {

            if (model !== undefined && model.closestfilter !== undefined && model.closestWrapper !== undefined) {

                $('[name="' + model.closestfilter + '"]').addClass('page-quick-filter-sidebar-wrapper-open');
                $('[name="' + model.closestfilter + '"]').css({ 'border-left-color': '#ddd', 'border-left-width': '1px', 'border-left-style': 'solid' });
                $('[name="' + model.closestWrapper + '"]').removeClass('col-md-12');
                $('[name="' + model.closestWrapper + '"]').addClass('col-md-10');
            }
        },

        close: function (model) {

            if (model !== undefined && model.closestfilter !== undefined && model.closestWrapper !== undefined) {

                $('[name="' + model.closestfilter + '"]').removeClass('page-quick-filter-sidebar-wrapper-open');
                $('[name="' + model.closestfilter + '"]').css({ 'border-left-color': '', 'border-left-width': '', 'border-left-style': '' });
                $('[name="' + model.closestWrapper + '"]').removeClass('col-md-10');
                $('[name="' + model.closestWrapper + '"]').addClass('col-md-12');
            }
        },

        expandParent: function (model) {

            if (model !== undefined && model.tableName !== undefined) {

                $('[name="FilterDiv_' + model.tableName + '"] .accordion-toggle').trigger('click');

                //$('#' + model.tableName + ' #collapse_Available_Filters_' + model.tableName).removeClass('collapse');
                //$('#' + model.tableName + ' #collapse_Available_Filters_' + model.tableName).addClass('expand');
            }
        },

        expandChildren: function (model) {

            if (model !== undefined && model.tableName !== undefined && model.filter !== undefined) {

                for (var i = 0; i < model.filter.length; i++) {

                    $('[name="FilterDiv_' + model.tableName + '"] #chbx_' + model.tableName + '_' + model.filter[i]).trigger('click');

                    //$($('#' + model.tableName + ' input[type="checkbox"]').filter(function () { return this.value == model.filter[i]; })).trigger('click');
                    //$('#' + model.tableName + ' #' + model.tableName + '_panel_Filter_' + model.filter[i] + ' .accordion-toggle').trigger('click');
                }
            }
        },

        collapseParent: function (model) {

            if (model !== undefined && model.tableName !== undefined) {

                $('#' + model.tableName + ' #collapse_Available_Filters_' + model.tableName).removeClass('expand');
                $('#' + model.tableName + ' #collapse_Available_Filters_' + model.tableName).addClass('collapse');
            }
        },

        applyFilters: function (model) {

            if (model !== undefined && model.tableName !== undefined && model.filter !== undefined) {

                $.each(model.filter, function (index, p) {

                    $($('#collapse_Filter_' + p.key + '_' + model.tableName + ' input').filter(function () { return this.value == p.value; })).trigger('click');
                });
            }
        },

        refresh: function (model) {

            if (model !== undefined && model.tableName !== undefined && model.filter !== undefined) {

                for (var i = 0; i < model.filter.length; i++) {

                    if ($('#' + model.tableName + '_panel_Filter_' + model.filter[i] + ' .accordion-refresh').length > 0) {

                        $('#' + model.tableName + '_panel_Filter_' + model.filter[i] + ' .accordion-refresh').trigger('click');
                    }
                }
            }
        }
    }
}());
