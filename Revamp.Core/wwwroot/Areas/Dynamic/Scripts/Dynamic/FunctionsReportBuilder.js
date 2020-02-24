//Start Group 1
function loadClauseSelect() {
    var clauseString = '';
    $('#AvailableFiltersSelect option')

    var innerHtml = ''
    for (var i = 0; i < mGlobal.page.builder.PickListHtml.length; i++) {
        var thisCurrentObject = mGlobal.page.builder.PickListHtml[i];
        innerHtml += '<option value="' + thisCurrentObject.value + '">' + thisCurrentObject.title + '</option>';
    }

    if ($('.clause').length == 0) {

        clauseString = '<div class="clause">' +
        '  <i class="fa fa-times-circle removeClause"></i>' +
        '  <select style="width: 30%;" name="leftSideOfClause[]">' + innerHtml + '</select>' +
        '    <select style="width: 25%;" name="ClauseConditions[]">' +
        '        <option>=</option>' +
        '        <option>></option>' +
        '        <option>>=</option>' +
        '        <option><</option>' +
        '        <option><=</option>' +
        '        <option>!=</option>' +
        '        <option>in</option>' +
        '        <option>not in</option>' +
        '        <option>like</option>' +
        '        <option>not like</option>' +
        '        <option>between</option>' +
        '    </select>' +
        '    <input style="width:50%;" name="rightSideOfClause[]"></input>' +
        '</div>';
    }
    else {
        clauseString = '<div class="clause">' +
        '  <i class="fa fa-times-circle removeClause"></i>' +
        '    <select style="width: 17%;" name="preConditions[]">' +
        '        <option>AND</option>' +
        '        <option>OR</option>' +
        '    </select>' +
        '  <select style="width: 20%;" name="leftSideOfClause[]">' + innerHtml + '</select>' +
        '    <select style="width: 15%;" name="ClauseConditions[]">' +
        '        <option>=</option>' +
        '        <option>></option>' +
        '        <option>>=</option>' +
        '        <option><</option>' +
        '        <option><=</option>' +
        '        <option>in</option>' +
        '        <option>not in</option>' +
        '        <option>like</option>' +
        '    </select>' +
        '    <input style="width:42%;" name="rightSideOfClause[]"></input>' +
        '</div>';
    }
    $('.DynoFilter').append(clauseString);
}

function loadClauseArray() {
    mGlobal.page.builder.ClauseArray = [];
    $(".clause").each(function (i) {
        try {


            if (i == 0
                && $('.clause:eq(' + i + ') [name="rightSideOfClause[]"]input').val().length > 0
                && $('.clause:eq(' + i + ') [name="leftSideOfClause[]"]').val().length > 0
                && $('.clause:eq(' + i + ') [name="ClauseConditions[]"]').val().length > 0
                ) {


                mGlobal.page.builder.ClauseArray.push({
                    leftSideOfClause: $('.clause:eq(' + i + ') [name="leftSideOfClause[]"]').val(),
                    ClauseConditions: $('.clause:eq(' + i + ') [name="ClauseConditions[]"]').val(),
                    rightSideOfClause: $('.clause:eq(' + i + ') [name="rightSideOfClause[]"]input').val()
                });
            }
            else if (i > 0
                && $('.clause:eq(0) [name="rightSideOfClause[]"]input').val().length > 0
                && $('.clause:eq(0) [name="leftSideOfClause[]"]').val().length > 0
                && $('.clause:eq(0) [name="ClauseConditions[]"]').val().length > 0
                && $('.clause:eq(' + i + ') [name="preConditions[]"]').val().length > 0
                && $('.clause:eq(' + i + ') [name="rightSideOfClause[]"]input').val().length > 0
                && $('.clause:eq(' + i + ') [name="leftSideOfClause[]"]').val().length > 0
                && $('.clause:eq(' + i + ') [name="ClauseConditions[]"]').val().length > 0) {

                mGlobal.page.builder.ClauseArray.push({
                    preConditions: $('.clause:eq(' + i + ') [name="preConditions[]"]').val(),
                    leftSideOfClause: $('.clause:eq(' + i + ') [name="leftSideOfClause[]"]').val(),
                    ClauseConditions: $('.clause:eq(' + i + ') [name="ClauseConditions[]"]').val(),
                    rightSideOfClause: $('.clause:eq(' + i + ') [name="rightSideOfClause[]"]input').val()
                });
            }
            else {

            }
        }
        catch (e) {

        }
    });
}

//End Group 1

//Start Group 2
function ConvertFromArrayToModel(thisArray, destinationName) {
    var thisModel = {};

    for (var i = 0; i < thisArray.length; i++) {

        if (destinationName != undefined && destinationName != '') {
            thisModel[destinationName + '.' + thisArray[i].name] = thisArray[i].value;
        }
        else {
            thisModel[thisArray[i].name] = thisArray[i].value;
        }
    }

    return thisModel;
}

function DisplaySaveReportModal(model) {

    var mandatoryfilterHTML = '';
    var filterHTML = '';
    var clauseHTML = '';
    $('#FiltersList').empty();

    $('[name="V_ROOT_REPORT_NAME"]').val($('#currentReports option:selected').val());

    $('[name="FilterDiv_' + mGlobal.page.builder.datatableName + '"] .sendDates2Server').each(function (i, element) {
        mandatoryfilterHTML += '<li><div class="filter-box-display"><i class="fa fa-calendar"/> <label>Date: ' + $('[name="' + $(element).attr("name") + '"]').data('title') + '*</label></div></li>'
    });

    $('[name="FilterDiv_' + mGlobal.page.builder.datatableName + '"] .panel.panel-default.clearable.filter.send2server[data-toggle=true]').each(function (i, element) {
        mandatoryfilterHTML += '<li><div class="filter-box-display"><i class="fa fa-check-square-o"/>  <label>Toggle: ' + $(element).data('title') + '</label></div></li>'
    });


    for (var i = 0; i < mGlobal.page[mGlobal.page.builder.datatableName].HiddenFiltersModel.length; i++) {
        mandatoryfilterHTML += '<li><div class="filter-box-display"><i class="fa fa-puzzle-piece"/> <label>Hidden: ' + mGlobal.page[mGlobal.page.builder.datatableName].HiddenFiltersModel[i].filterName + '</label></div></li>'
    }

    $('#FiltersList').append(mandatoryfilterHTML);

    $('#' + mGlobal.page.builder.datatableName + '_panel_Available_Filters .filter-content input:checked').each(function (i, element) {
        var isRequired = $(element).data('required') == "true" || $(element).data('required') == true ? "*" : "";
        filterHTML += '<li><div class="filter-box-display"><i class="fa fa-filter"/> <label>Multi-Select: ' + $(element).val() + isRequired + '</label></div></li>'
    });

    if (mandatoryfilterHTML.length == 0 && filterHTML.length == 0) {
        filterHTML += '<li>No Filters have been identified. Please add filters through the builder.</li>';
    }

    $('#FiltersList').append(filterHTML);

    loadClauseArray();

    $('#ClausesList').empty();

    for (var i = 0; i < mGlobal.page.builder.ClauseArray.length; i++) {
        var tempPre = mGlobal.page.builder.ClauseArray[i].preConditions == undefined ? "" : mGlobal.page.builder.ClauseArray[i].preConditions;

        clauseHTML += '<li>' + tempPre + ' ' + mGlobal.page.builder.ClauseArray[i].leftSideOfClause + ' ' + mGlobal.page.builder.ClauseArray[i].ClauseConditions + ' ' + mGlobal.page.builder.ClauseArray[i].rightSideOfClause + ' ' + '</li>'
    }

    $('#ClausesList').append(clauseHTML);

    var columnsModel = {
        direction: 'POST',
        currentBaseCustomReportID: $('#currentReports option:selected').data('rootreportid'),
        callEvent: 'Custom Report',
        page: mGlobal.page.builder.datatableName,
        async: false
    }

    buildBaseReport.columns(columnsModel);

    $('#SaveReportModal').modal('show');
}

function CloseModal(model) {
    $(model.selector).modal('hide');
}
//End Group 2

//Start Group 3
function TriggerCustomReportSaveModel() {
    var close = false;

    switch ($(this).data('cmd').toLowerCase()) {
        case 'cancel':
            close = true;
            break;
        case 'submit':
            switch ($(this).data('src').toLowerCase()) {
                case 'savereportmodal':

                    if ($('#SaveReportForm').validate().form()) {
                        builder.saveCustomReport({ selector: '#' + $(this).data('src') });
                    }
                    else {
                        toastr[ToastConstants.genericError.type]('Fix form values', 'Failed to submit form.');
                    }
                    close = false;
                    break;
            }
            break;
        default:
            break;
    }

    if (close) {
        CloseModal({ selector: '#' + $(this).data('src') });
    }
}
//End Group 3


function deleteCustomReport(model) {

    model.data = model.data == undefined ? {} : model.data;
    model.options = model.options == undefined ? {} : model.options;
    model.options.async = model.async;
    model.options.url = "/dynamic/DeleteCustomReport";
    model.options.callBack = function (model) {
        if (model.response.success == true) {
            toastr[ToastConstants.genericSuccess.type](ToastConstants.genericSuccess.msg, ToastConstants.genericSuccess.title);
            $('#currentReports').trigger('change');
        }
        else {
            toastr[ToastConstants.genericError.type](ToastConstants.genericError.msg, ToastConstants.genericError.title);
        }
    };

    model.notification = model.notification == undefined ? {} : model.notification;
    model.notification.pulse = false;

    ajaxDynamic(model);

}
