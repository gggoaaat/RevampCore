$(document).on('click', '.CustomReportExport', function (e) {
    $('#coreReportExportModal').attr('data-owner', $(this).data('owner'));
    $('#coreReportExportModal').data('owner', $(this).data('owner'));
    $('#coreReportExportModal').attr('data-customreportid', $(this).data('customreportid'));
    $('#coreReportExportModal').data('customreportid', $(this).data('customreportid'));

    var thisExportModel = mGlobal.page[$(this).data('owner')].exports;
    var ExportDDHtml = '';
    if (thisExportModel != undefined && thisExportModel.length > 0) {

        $('[name="ExportTypeDropDown"] option[data-crystal="true"]').show();
        $('[name="crystalReportDisplay"]').show();
        $('#ExportListDropDown').show();

        if ($('#ExportTypeDropDown option[values="PDF"]').css('display') != 'none') {
            $('#ExportTypeDropDown option[values="PDF"]').attr('selected', 'selected')
        }
        else {
            $('#ExportTypeDropDown option[values=""]').attr('selected', 'selected')
        }

        for (var i = 0; i < thisExportModel.length; i++) {

            var theseColumns = thisExportModel[i].Columns != undefined && thisExportModel[i].Columns.length > 0 ? thisExportModel[i].Columns : '';
            ExportDDHtml += '<option data-path="' + thisExportModel[i].FilePath + '" data-order="' + thisExportModel[i].OrderBY + '" data-name="' + thisExportModel[i].Name + '" data-columns="' + theseColumns + '">' + thisExportModel[i].Name + '</option>'
        }
    }
    else {

        $('[name="ExportTypeDropDown"] option[data-crystal="true"]').hide();
        $('[name="crystalReportDisplay"]').hide();
        $('#ExportListDropDown').hide();
    }

    $('#ExportListDropDown').html(ExportDDHtml)

    $('#coreReportExportModal').modal('show');
});

$('.CustomReportPrint').live('click', function (e) {
    printDataTable();
});

$('#coreReportExportModal').live('show.bs.modal', function () {

});

$('#ReportExportModalButton').live('click', function () {

    var export_source = $('#coreReportExportModal').data('owner');
    var export_reportid = $('#coreReportExportModal').data('customreportid');
    var export_doctype = $('#ExportTypeDropDown option:selected').data('doctype');
    var export_name = $('#ExportListDropDown option:selected').data('name');
    var export_path = $('#ExportListDropDown option:selected').data('path');
    var export_order = $('#ExportListDropDown option:selected').data('order');
    var export_columns = $('#ExportListDropDown option:selected').data('columns');

    var thisParam = table[export_source].ajax.params();
    var Params = $.param(thisParam);
    var encoded = Base64.encode(Params);

    $('#ExportForm [name="Param"]').val(encoded);
    $('#ExportForm [name="DocType"]').val(export_doctype);
    $('#ExportForm [name="ExportReport"]').val(export_name);
    $('#ExportForm [name="ExportPath"]').val(export_path);
    $('#ExportForm [name="ExportOrder"]').val(export_order);
    $('#ExportForm [name="ExportColumns"]').val(export_columns);
    $('#ExportForm [name="Schema"]').val(mGlobal.page[export_source].schema);
    $('#ExportForm [name="Template"]').val(mGlobal.page[export_source].template);
    $('#ExportForm [name="stripTime"]').val(mGlobal.page[export_source].stripTime == undefined ? false : mGlobal.page[export_source].stripTime);

    $('#ExportForm').submit();
});

$('.CustomReportClause').live('click', function (e) {

    $('#coreReportClauseModal').attr('data-owner', $(this).data('owner'));
    $('#coreReportClauseModal').data('owner', $(this).data('owner'));

    var owner = $(this).data('owner')
    if (mGlobal.page[owner].whereClause != undefined && mGlobal.page[owner].whereClause.length > 0) {
        clauseBuilder.resume({ page: owner });
    }
    else {
        clauseBuilder.init({ page: owner });
    }
    $('#coreReportClauseModal').modal('show');
    $('#coreReportClauseModal').draggable();
});

window.addEventListener('resize', function () {

    $('body').find('.dataTables_scrollBody:visible table').not('div.no-resize-reload table').each(function (index, element) {

        var thisID = $(element)[0].id;

        if (table[thisID] != undefined) {
            table[thisID].columns.adjust();
        }
    });

}, true);

$(document).on('click','a[data-toggle=tab]', function () {
    var run = $(this).hasClass('no-reload') ? false : true
    var thisData = $(this).data();
    if (run) {
        if (typeof mGlobal == 'object' && mGlobal.page && mGlobal.page[thisData.uuid] && mGlobal.page[thisData.uuid].isLoaded == true) {
            window.setTimeout(function () {
                $('body').find('.dataTables_scrollBody:visible table').not('div.no-tab-reload table').each(function (index, element) {

                    var thisID = $(element)[0].id;

                    if (table[thisID] != undefined) {

                        table[thisID].ajax.reload(null, false);

                    }
                });
            }, 200);
        }
        else {
            if (typeof mGlobal == 'object' && mGlobal.page && mGlobal.page[thisData.uuid]) {
                callGenerateStandAloneDT({ page: thisData.uuid, selector: mGlobal.page[thisData.uuid].tableContainer, loadAvailableFilters: true, callEvent: 'Stand Alone with Filters' });
            }
        }
    }
});

$('a[data-istable="true"]').live('click', function (e) {

    var thisTable = $(this).data('table');

    if (table[thisTable] && table[thisTable].columns) {
        window.setTimeout(function () {
            table[thisTable] != undefined ? table[thisTable].columns.adjust() : undefined
        }, 150);
    }
});

$('a[name="toggleFilters"]').live('click', function () {

    //alert($('a[name="toggleFilters"]').data('toggle'));

    var closestfilter = $(this).data('filterdiv');
    var closestWrapper = $(this).data('wrapper');

    resizeFilterDiv({ page: $(this).data('owner') });

    var sidebarVisible = $('[name="' + closestfilter + '"]').is(':visible')
    var toggle = sidebarVisible ? "close" : "open";
    switch (toggle) {
        case 'open':
            $(this).data('toggle', 'close');
            //$(this).find('span').addClass('glyphicon glyphicon-filter');
            //$(this).find('span').removeClass('glyphicon glyphicon-filter');

            SideBarManipulation({ action: 'open', closestfilter: closestfilter, closestWrapper: closestWrapper });
            break;
        case 'close':
            $(this).data('toggle', 'open');
            //$(this).find('span').removeClass('glyphicon glyphicon-filter');
            //$(this).find('span').addClass('glyphicon glyphicon-filter');

            SideBarManipulation({ action: 'close', closestfilter: closestfilter, closestWrapper: closestWrapper });
            break;
    }

    table[$(this).data('owner')].columns.adjust();
});

$('.actionRefresh').live('click', function () {
    table[$(this).data('owner')].ajax.reload();
})

$('a.dropdown-filter-remove').live('click', function () {
    var table = $(this).data('tableowner');
    var filter = $(this).data('filter');

    selectAllCheckbox = $('#collapse_Filter_' + filter + '_' + table).find('.filterSelectAllDiv input[type=checkbox]');

    clearFilters(table, filter);
})

$('[data-filter="all_filters"] .filter-content label input').live('click', function () {

    if ($(this).is(':checked')) {
        buildBaseReport.createFilterPanel({
            name: $(this).val(),
            selector: '#Filters_Accordion_' + $(this).data('tableowner'),
            rootreportname: $(this).data('rootreportname'),
            tableOwner: $(this).data('tableowner'),
            template: $(this).data('template'),
            filterType: $(this).data('filtertype')
        });
    }
    else {
        clearFilters($(this).data('tableowner'), $(this).val());
    }
});

$('.panel.filter[data-bound=true] a.accordion-toggle').live('click', function () {
    var parentObj = $(this).parent().parent().parent().data('filter');

    var thisFilterData = $(this).closest('[data-rootreportname]').data();

    var thisReport = thisFilterData.rootreportname;
    var thisTableOwner = thisFilterData.tableowner;
    var template = thisFilterData.template;


    if ($(this).hasClass('collapsed')) {
        // loadFilterData(parentObj, parentObj, '');
    }
    else {
        if ($(this).parents('.panel').find('.panel-collapse').find('.filter-content label').length == 0) {
            $(this).closest('.send2server').find('.filter-content').html('' +
                '<div style="margin-top: 25%;">' +
                '   <div class="fa fa-spin" style=" height: 100%; width: 100%; text-align: center; margin: 0 auto; font-weight:bold;">' +
                '       <span aria-hidden="true" class="icon-hourglass"></span>' +
                '   </div>' +
                '</div>');

            commonDynamic.functions.tabulate.checkComponentCall({
                name: parentObj,
                prettyname: parentObj,
                filterType: '',
                rootreportname: thisFilterData.alternatereport == undefined ? thisReport : thisFilterData.alternatereport,
                fullreportid: thisFilterData.fullreportid,
                tableOwner: thisTableOwner,
                page: thisTableOwner,
                schema: thisFilterData.alternateschema == undefined ? template : thisFilterData.alternateschema,
                template: thisFilterData.alternatetemplate == undefined ? template : thisFilterData.alternatetemplate,
            });
        }
    }
});

$('.panel.filter[data-bound=true] a.accordion-refresh').live('click', function () {
    var parentObj = $(this).parents('.panel-heading').data('filter');

    var thisFilterData = $(this).closest('[data-rootreportname]').data();

    var thisReport = thisFilterData.rootreportname;
    var thisTableOwner = thisFilterData.tableowner;
    var template = thisFilterData.template;

    $(this).find('i').removeClass('fa-refresh')
    $(this).find('i').addClass('fa-spin fa-refresh')

    $(this).closest('.send2server').find('.filter-content').html('' +
        '<div style="margin-top: 25%;">' +
        '   <div class="fa fa-spin" style=" height: 100%; width: 100%; text-align: center; margin: 0 auto; font-weight:bold;">' +
        '       <span aria-hidden="true" class="icon-hourglass"></span>' +
        '   </div>' +
        '</div>');

    //ReloadTable({ sourceTable: thisTableOwner });

    commonDynamic.functions.tabulate.checkComponentCall({
        name: parentObj,
        prettyname: parentObj,
        filterType: '',
        rootreportname: thisFilterData.alternatereport == undefined ? thisReport : thisFilterData.alternatereport,
        fullreportid: thisFilterData.fullreportid,
        tableOwner: thisTableOwner,
        page: thisTableOwner,
        schema: thisFilterData.alternateschema == undefined ? template : thisFilterData.alternateschema,
        template: thisFilterData.alternatetemplate == undefined ? template : thisFilterData.alternatetemplate,
        isThisRefresh: true
    });
});

$('.panel.filter[data-bound=true] input:not(".filterSearch")').live('click', _.debounce(function (event, ui) {

    var model = {};

    //if user clicks 'select all,' set all other options in this filter to the checked/unchecked value user wants
    if ($(this).hasClass('filterSelectAll')) {
        var isChecked = $(this).prop('checked') ? $(this).prop('checked') : false;
        var chks = $(this).closest('div.panel-body').find('div.filter-content input[type="checkbox"]');//.each(function () { $(this).prop('selected','') });
        chks.prop('checked', isChecked);
        model.isToggle = false;
    }

    if ($(this).hasClass('make-switch')) {
        //var isChecked = $(this).prop('checked') ? $(this).prop('checked') : false;
        //var chks = $(this).closest('div.panel-body').find('div.filter-content input[type="checkbox"]');//.each(function () { $(this).prop('selected','') });
        //chks.prop('checked', isChecked);
        model.isToggle = true;
    }


    model.tableOwner = $(this).closest('[data-tableowner]').data('tableowner');
    model.name = $(this).closest('[data-filter]').data('filter');

    buildBaseReport.loadCurrentFilter(model);

}, 1000));

$('.panel.filter[data-bound=true] a.accordion-link').live('click', function () {
    //var parentObj = $(this).parent().parent().parent().data('filter');
    var parentObj = $(this).closest('.panel-heading').data('filter');

    var thisFilterData = $(this).closest('[data-rootreportname]').data();

    var thisReport = thisFilterData.rootreportname;
    var thisTableOwner = thisFilterData.tableowner;
    var template = thisFilterData.template;

    $(this).closest('.send2server').find('.filter-content').html('' +
        '<div style="margin-top: 25%;">' +
        '   <div class="fa fa-spin" style=" height: 100%; width: 100%; text-align: center; margin: 0 auto; font-weight:bold;">' +
        '       <span aria-hidden="true" class="icon-hourglass"></span>' +
        '   </div>' +
        '</div>');

    var chainEnabled = false;

    if ($(this).find('i').hasClass('fa-chain')) {
        $(this).find('i').removeClass('fa-chain');
        $(this).find('i').addClass('fa-chain-broken');
        $('#' + thisTableOwner + '_panel_Filter_' + parentObj + '').data('link', true);
        chainEnabled = false;
    }
    else if ($(this).find('i').hasClass('fa-chain-broken')) {
        $(this).find('i').removeClass('fa-chain-broken');
        $(this).find('i').addClass('fa-chain');
        $('#' + thisTableOwner + '_panel_Filter_' + parentObj + '').data('link', false);
        chainEnabled = true;
    }

    mGlobal.page[thisTableOwner].currentFilters[parentObj].chainEnabled = chainEnabled;

    //ReloadTable({ sourceTable: thisTableOwner });

    commonDynamic.functions.tabulate.checkComponentCall({
        name: parentObj,
        prettyname: parentObj,
        filterType: '',
        rootreportname: thisFilterData.alternatereport == undefined ? thisReport : thisFilterData.alternatereport,
        tableOwner: thisTableOwner,
        page: thisTableOwner,
        schema: thisFilterData.alternateschema == undefined ? template : thisFilterData.alternateschema,
        template: thisFilterData.alternatetemplate == undefined ? template : thisFilterData.alternatetemplate,
        isThisRefresh: true,
        breakChain: chainEnabled
    });
});

$(document).on('input', '.filterSearch', function () {
    var target = $(this);
    var search = target.val().toLowerCase();
    var filterContainer = target.parent();
    var containerType = filterContainer.find('.checkbox-div:first-child').length == 1 ? '.checkbox-div' : '.checkbox-container';
    var filterItemContainer = filterContainer.find(containerType);
    var filterItem = filterItemContainer.find('input[type="checkbox"]');

    if (!search) {
        filterItemContainer.show();
        filterContainer.find('.filterSelectAllDiv').show();
        return false;
    }
    else {
        filterContainer.find('.filterSelectAllDiv').hide();
    }

    $(filterItem).each(function () {
        var filterItemName = $(this).val().toLowerCase();
        var match = filterItemName.indexOf(search) > -1;

        if (!match) {
            $(this).closest(containerType).hide();
        }
        else {
            $(this).closest(containerType).show();
        }
    });
});

function clearFilters(table, filter) {
    var selectAllCheckbox = $('#collapse_Filter_' + filter + '_' + table).find('.filterSelectAllDiv input[type=checkbox]');
    var selectedFilter = $('#chbx_' + table + '_' + filter);

    //Clear filter
    $(document).trigger({
        type: 'click',
        target: selectAllCheckbox[0]
    });

    //Remove Filter Panel
    setTimeout(function () {
        buildBaseReport.deleteFilterPanel({
            name: filter,
            tableOwner: table
        });
    }, 1000);

    //Remove Filter
    selectedFilter.prop('checked', false);

    //Live click remove filter
    //$(document).trigger({
    //    type: 'click',
    //    target: selectedFilter[0]
    //});
}

window.addEventListener('resize', _.debounce(function (event, ui) {

    resizeFilterDiv({});
    // var footerHeight = +$('.page-footer').css('height').replace('px', ''); //Footer Height
    // var topHeight = +$('.FilterDiv').css('top').replace('px', '');
    // var contentHeight = +$('.page-content').css('min-height').replace('px', '');
    // var topOffset = $('.navbar-fixed-top').css('height').replace('px', '');
    // var NavTabHeight = $('.FilterDiv .nav-tabs').css('height').replace('px', '');

    // var FilterHeight = contentHeight - (topHeight - topOffset);
    //// var FilterHeight = contentHeight;
    // // var TabPainHeight = FilterHeight - NavTabHeight - footerHeight;
    // var TabPainHeight = FilterHeight - NavTabHeight;

    // FilterHeight = $('.page-footer').position().top < $(window).height() ? FilterHeight : FilterHeight + footerHeight;

    // $('.FilterDiv').css('height', FilterHeight + 'px');
    // $('.FilterDiv .tab-pane').css('height', TabPainHeight + 'px');
    // $('.FilterDiv .tab-pane').css('overflow-y', 'scroll');

}, 200), true);

//Print using modal window
function printDataTable() {
    //showBSModal({
    //    remote: 'dashboard/html/popup-window.html'
    //});

    //showBSModal({
    //    title: "Print Preview",
    //    body: "PrintBody",
    //    size: "small"
    //});

    var table = $('div[id^="dataTableContainer_"] div.dataTables_scrollBody table:visible').clone().removeAttr('class style');

    //Remove datatable styles
    table.find("[style*='display:none']").remove();
    table.find('*').removeAttr('class style');

    //Add print style
    table.addClass('all-print');

    var printTable = formatPage(table);

    showBSModal({
        title: "Print Preview",
        body: printTable.prop('outerHTML'),
        onShow: function (e) {

            //Hide background page
            //$('.content-wrapper.main-content').css('display', 'none');

            //Format modal window
            $('.all-print').parent('.modal-body:visible').css({
                "height": "500px",
                "overflow-y": "auto",
            });

            $('.all-print').closest('.modal-dialog').find('[class|="modal"]').css({
                "border": "none !important",
            });

            //Print modal window
            window.document.close();
            window.focus();
            window.print();
        },
        onHide: function (e) {
            //$('.content-wrapper.main-content').css('display', 'block');
        },
        size: "large"
    });

}

function printDashboardRecord() {
    //Print modal window
    window.document.close();
    window.focus();
    window.print();
}

function formatPage(table) {
    var tableData = table.find('tbody td');
    var count = 0;
    var columns = table.find('thead th');

    totalColumns = columns.length;

    tableData.each(function (index, element) {
        if (count > (totalColumns - 1)) {
            count = 0
        }

        var column = columns[count].innerText;
        var title = '<em><strong>' + column + '</strong></em>: ' + $(this).html();

        $(this).html(title);

        count++;
    });

    return table;
}