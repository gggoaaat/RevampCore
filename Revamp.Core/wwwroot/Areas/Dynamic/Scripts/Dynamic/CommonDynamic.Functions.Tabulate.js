var table = [];
var defaultTabPrefix = 'tab_Core_Action_'

commonDynamic.functions.tabulate = (function () {
    return {
        getColumnIndex: function (model) {
            if (model != undefined && model.dataSet != undefined && commonDynamic.functions.tools.isNotNullEmptyOrUndefined(model.column)) {

                var FirstCondition = _.findIndex(model.dataSet, function (columnItem) { return columnItem == model.column; });

                var FinalCondition = FirstCondition >= 0 ? FirstCondition : _.findIndex(model.dataSet, function (columnItem) { return columnItem.indexOf(model.column) !== -1; });

                return FinalCondition + 1;
            }
            else {
                return 1;
            }
        },
        convertColumnOrderFromDBtoOrder: function (model) {
            mGlobal.page[model.page].order = [];

            var tempDBColumnOrder = mGlobal.page[model.page].columnOrderFromDB ? mGlobal.page[model.page].columnOrderFromDB : [];
            for (var i = 0; i < tempDBColumnOrder.length; i++) {
                var thisIndex = commonDynamic.functions.tabulate.getColumnIndex({ dataSet: mGlobal.page[model.page].columns.split(','), column: tempDBColumnOrder[i].sort_column });
                if (thisIndex > 0) {
                    mGlobal.page[model.page].order.push([thisIndex, tempDBColumnOrder[i].sort_direction]);
                }
            }
        },
        formatColHeader: function (model) {

            for (var i = 0; i < model.columnAlias.length; i++) {
                $('thead .css_' + model.columnAlias[i].key + '.css_' + model.tableName + ':eq(0)').html(model.columnAlias[i].value);
            }
        },
        getColumnAlias: function (model, value) {

            var emptyString = '';
            var currentAlias = [];

            if (model != undefined) {
                currentAlias = model.filter(function (Alias) { return Alias.key == value; });
                if (currentAlias.length > 0) {
                    return currentAlias[0].value;
                }
            }

            return emptyString;
        },
        checkSearchvar: function (model) {

            // commonDynamic.functions.tabulate.dbCall(model);
            table[model.sourceTable].ajax.reload();

            return true;
        },
        reloadTable: function (model) {

            try {
                commonDynamic.functions.tabulate.checkSearchvar(model);
            }
            catch (e) {
                var tempTable = model.sourceTable != undefined ? model.sourceTable : 'N/A';
                alert('Error Reloading Table:tempTable Exception:' + e.toString());
            }
        },
        checkExistingData: function (model) {
            if (mGlobal.page[model.page].currentFilters[model.name] != undefined && mGlobal.page[model.page].currentFilters[model.name].data != undefined) {
                for (var i = 0; i < mGlobal.page[model.page].currentFilters[model.name].data.length; i++) {
                    var thisValue = mGlobal.page[model.page].currentFilters[model.name].data[i];

                    $('#' + model.page + '_panel_Filter_' + model.name + ' input[type="checkbox"][value="' + thisValue + '"').prop('checked', true);
                }
            }
        },
        loadFilterData: function (model) {

            var thisDataValues = model.filterData;

            thisDataValues.filtername = model.name;

            $('.panel.filter[data-filter="' + model.name + '"][data-tableowner="' + model.tableOwner + '"] .filter-content').html($.fn.revamp.templates.filterOptions(thisDataValues));
            $('.panel.filter[data-filter="' + model.name + '"][data-tableowner="' + model.tableOwner + '"] .filter-content input[value=""]').parents('label').remove();

            $('.panel.filter[data-filter="' + model.name + '"][data-tableowner="' + model.tableOwner + '"] .filter-content label').addClass('checkbox-container');
            $('.panel.filter[data-filter="' + model.name + '"][data-tableowner="' + model.tableOwner + '"] .filter-content label').append('<span class="checkmark"></span>');
            $('.panel.filter[data-filter="' + model.name + '"][data-tableowner="' + model.tableOwner + '"] .filter-content label').addClass('checkbox-filterdata');

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
            if (model.isThisRefresh) {
                $('#' + model.page + '_panel_Filter_' + model.name + ' .accordion-refresh i').removeClass('fa-spin');
            }

            commonDynamic.functions.tabulate.checkExistingData(model);
        },
        checkComponentCall: function (model) {

            model.rootreportname = model.rootreportname != undefined ? model.rootreportname : $('#currentReports').val();
            model.report = model.rootreportname;

            model.data = commonDynamic.functions.tabulate.filters.dataTableFilterAjax(model);
            model.options = model.options == undefined ? {} : model.options;
            model.options.async = true;

            var defaultURL = '/dynamic/dynosearch';
            var finalUrl = mGlobal.page[model.page].overrideJsonURL != undefined && mGlobal.page[model.page].overrideJsonURL.length > 0 ? mGlobal.page[model.page].overrideJsonURL : defaultURL;
            model.options.url = finalUrl + '?iReport=' + model.rootreportname + '&iFilter=' + model.name
            model.data.schema = mGlobal.page[model.page].overrideJsonURL != undefined && mGlobal.page[model.page].overrideJsonURL.length > 0 ? mGlobal.page[model.page].schema : model.data.schema;
            // model.options.url = '/dynamic/dynosearch?iReport=' + model.rootreportname + '&iFilter=' + model.name;
            model.options.callBack = function (model) {
                //if (typeof model.response.data == 'string' && Base64.isThisEncoded(model.response.data)) {
                //    model.response.data = JSON.parse(Base64.decode(model.response.data));
                //}
                model.filterData = model.response;
                commonDynamic.functions.tabulate.loadFilterData(model);
            };

            model.notification = model.notification == undefined ? {} : model.notification;
            model.notification.pulse = false;

            ajaxDynamic(model);
        },
        dbCall: function (model) {

            clearInterval(mGlobal.page[model.sourceTable].searchInterval);
            mGlobal.page[model.sourceTable].searchInterval = undefined;
        },
        filters: (function () {
            return {
                checkIfFiltersValid: function (model, d) {
                    var returnVal = false;

                    var runLogic = d.verify == undefined ? true : false;

                    if (runLogic) {
                        var thisReportSet = model.reportidandtitle == undefined ? model.page : model.reportidandtitle;

                        var hasDatePicker = $('#tab_Core_Filters_' + thisReportSet + ' [data-type="datepicker"]').length > 0 ? true : false;
                        var numberOfDatePickers = $('#tab_Core_Filters_' + thisReportSet + ' [data-type="datepicker"]').length;
                        var numberOfSelectedDatePickers = $('#tab_Core_Filters_' + thisReportSet + ' .active.day').length;

                        var hasRequiredFilters = $('#tab_Core_Filters_' + thisReportSet + ' [data-required="true"]:not([data-bound="false"])').length > 0 ? true : false;
                        var numberOfRequiredFilters = $('#tab_Core_Filters_' + thisReportSet + ' [data-required="true"]:not([data-bound="false"])').length;

                        var checkedRequiredFilters = ($('#tab_Core_Filters_' + thisReportSet + ' [data-required="true"] input:checked').length > 0 ? true : false);
                        var checkedFilters = ($('#tab_Core_Filters_' + thisReportSet + ' [data-bound="true"] input:checked').length > 0 ? true : false);

                        var canUseDate = (hasDatePicker == true && numberOfDatePickers == numberOfSelectedDatePickers) ? true : false;

                        if (hasDatePicker == true) {

                            if (canUseDate == true && hasRequiredFilters == true) {
                                if (checkedRequiredFilters) {
                                    returnVal = true;
                                }
                                else {
                                    returnVal = false;
                                }
                            }
                            //else if (canUseDate == true && hasRequiredFilters == false) {
                            //    returnVal = true;
                            //}
                            else {
                                returnVal = true;
                            }
                        }
                        //else if (hasDatePicker == false) {
                        //    if (hasRequiredFilters == true && checkedRequiredFilters == true) {
                        //        returnVal = true;
                        //    }
                        //    else
                        //    {
                        //        returnVal = false;
                        //    }
                        //}
                        else if (hasRequiredFilters == true) {
                            if (checkedRequiredFilters) {
                                returnVal = true;
                            }
                            else {
                                returnVal = false;
                            }
                        }
                        //else if (hasRequiredFilters == false) {
                        //    if (checkedFilters == true) {
                        //        returnVal = true;
                        //    }
                        //    else {
                        //        returnVal = true;
                        //    }
                        //}
                        else {
                            returnVal = true;
                        }
                    }
                    else {
                        returnVal = d.verify == 'T' ? true : false;
                    }

                    return returnVal ? 'T' : 'F';

                    //$('#tab_Core_Filters_' + model.page + ' [data-required="true"]:not([data-bound="false"])').length > 0 ? ($('#tab_Core_Filters_' + model.page + ' [data-required="true"] input:checked').length > 0 ? 'T' : 'F') : ($('#tab_Core_Filters_' + model.page + ' input:checked').length > 0 ? 'T' : 'F')
                },
                commonAjaxStruct: function (model, d) {
                    d.SearchReport = model.report;
                    d.schema = model.schema;
                    d.template = model.template;
                    $('.sendDates2Server[data-tableowner="' + model.tableOwner + '"]').each(function (i, element) {

                        var thisDate = '';
                        if ($(element).find('.datepicker-days .active').length > 0) {
                            thisDate = $(element).find('.datepicker-years .active').html() +
                                pad('00', ($(element).find('.datepicker-months span.active').index() + 1), true) +
                                pad('00', $(element).find('.datepicker-days .active').html(), true);
                        }
                        else {
                            var todayIs = new Date();
                            thisDate = todayIs.toISOString().slice(0, 10).replace(/-/g, "");
                        }
                        d[$(element).attr("name")] = thisDate;

                    });

                    d = commonDynamic.functions.tabulate.filters.appendChainFilterData(model, d);
                    //d = commonDynamic.functions.tabulate.filters.appendChainFilters(model, d);
                    //d = model.ajaxAppend == undefined ? d : model.ajaxAppend(d);
                    d = mGlobal.page[model.page].ajaxAppend == undefined ? d : mGlobal.page[model.page].ajaxAppend(d);
                    d = clauseBuilder.ajaxData(model, d);
                    d.stripTime = mGlobal.page[model.page].stripTime == undefined ? false : mGlobal.page[model.page].stripTime;

                    return d;
                },
                dataTableFilterAjax: function (model) {
                    var d = {};

                    model.breakChain = model.breakChain == undefined ? false : model.breakChain;

                    d.FilterColumn = model.prettyname;
                    d.ExportType = "checkbox";
                    d.queryType = "filter";

                    //If You have required?
                    if ($('#tab_Core_Filters_' + model.fullreportid + ' [data-required="true"]').length > 0) {

                        //if this is a required field?
                        if ($('[data-tableowner="' + model.tableOwner + '"][data-filter="' + model.name + '"][data-required="true"]').length > 0) {
                            d.VERIFY = 'T';
                        }
                        else {
                            //check if required fields are checked
                            if ($('#tab_Core_Filters_' + model.fullreportid + ' [data-required="true"] input:checked').length > 0 ||
                                $('#tab_Core_Filters_' + model.fullreportid + ' [data-required="true"] .active.day').length == $('#tab_Core_Filters_' + model.fullreportid + ' [data-required="true"] .date-picker').length
                            ) {
                                d.VERIFY = 'T';
                            }
                            else {
                                d.VERIFY = 'F';
                            }

                        }

                        //d.VERIFY = $('#tab_Core_Filters_' + model.fullreportid + ' [data-required="true"] input:checked').length > 0 ? 'T' : 'F'
                    }
                    //If you don't have required
                    else {
                        d.VERIFY = 'T'; //($('#tab_Core_Filters_' + model.fullreportid + ' input:checked').length > 0 ? 'T' : 'F')
                    }


                    d = commonDynamic.functions.tabulate.filters.commonAjaxStruct(model, d);

                    return d;
                },
                builderDataTableAjax: function (model) {
                    mGlobal.ajaxData = mGlobal.ajaxData == undefined ? {} : mGlobal.ajaxData;
                    mGlobal.ajaxData[model.page] = function (d) {

                        d.P_DYNO_COL = mGlobal.page[model.page].columns, // model.dynocol != undefined ? model.dynocol : '';
                            d.queryType = "query";
                        d.VERIFY = commonDynamic.functions.tabulate.filters.checkIfFiltersValid(model, d);
                        d = commonDynamic.functions.tabulate.filters.commonAjaxStruct(model, d);
                    };
                },
                appendChainFilters: function (model, d) {
                    if (mGlobal.page[model.page].requiredFilters != undefined) {

                        for (i in mGlobal.page[model.page].requiredFilters) {
                            if (model.breakChain) {
                                //if (mGlobal.page[model.page].requiredFilters[i].Required) {
                                //    d['P_' + mGlobal.page[model.page].requiredFilters[i].key] = mGlobal.page[model.page].requiredFilters[i].value
                                //}
                            }
                            else {
                                d['P_' + mGlobal.page[model.page].requiredFilters[i].key] = mGlobal.page[model.page].requiredFilters[i].value;
                            }
                        }
                    }

                    return d;
                },
                appendChainFilterData: function (model, d) {
                    if (model.tableOwner != undefined && mGlobal.page[model.tableOwner].currentFilters != undefined) {

                        for (thisFilter in mGlobal.page[model.tableOwner].currentFilters) {
                            var currentNode = thisFilter;
                            var currentDelimiter = searchQueryDelimiter;

                            d['P_DELIMITER'] = currentDelimiter;

                            if (d.queryType == "filter") { //Run when Filter

                                if (currentNode != model.name) {
                                    if (mGlobal.page[model.tableOwner].currentFilters[model.name].chainEnabled) {
                                        //if (mGlobal.page[model.tableOwner].currentFilters[currentNode].data != undefined) {

                                        if (mGlobal.page[model.tableOwner].currentFilters[currentNode].data.length > 0) {
                                            d['P_' + currentNode] = mGlobal.page[model.tableOwner].currentFilters[currentNode].data.join(currentDelimiter);
                                        }
                                        // }
                                    }
                                    else {
                                        if (mGlobal.page[model.tableOwner].currentFilters[currentNode].required && mGlobal.page[model.tableOwner].currentFilters[currentNode].data != undefined) {

                                            if (mGlobal.page[model.tableOwner].currentFilters[currentNode].data.length > 0) {
                                                d['P_' + currentNode] = mGlobal.page[model.tableOwner].currentFilters[currentNode].data.join(currentDelimiter);
                                            }
                                        }
                                    }
                                }
                            }
                            else //Run when Query
                            {
                                if (mGlobal.page[model.tableOwner].currentFilters[currentNode] != undefined
                                    && mGlobal.page[model.tableOwner].currentFilters[currentNode].data != undefined
                                    && mGlobal.page[model.tableOwner].currentFilters[currentNode].data.length > 0) {
                                    d['P_' + currentNode] = mGlobal.page[model.tableOwner].currentFilters[currentNode].data.join(currentDelimiter);
                                }
                            }
                        }
                    }
                    return d;
                }
            }
        })(),
        structs: (function () {
            return {
                getDataTable: function (model) {

                    model = model != undefined ? model : {};

                    model.data = {
                        report: model.report,
                        tableName: model.tableName,
                        dynocol: model.dynocol != undefined ? model.dynocol : '',
                        async: model.async,
                        queryType: "query",
                        schema: model.schema,
                        template: model.template
                    }
                    model.options = {};
                    model.options.async = true;
                    model.options.url = model.structURL //"/dynamic/GetTableStruct?iReport=" + model.report;
                    model.options.callBack = function (model) {

                        model.html = model.response;
                        commonDynamic.functions.tabulate.structs.loadDataTable(model);
                    }
                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false;
                    ajaxDynamic(model);

                },
                loadDataTable: function (model) {

                    function handleAjaxError(xhr, textStatus, error) {
                        if (textStatus === 'timeout') {
                            alert('The server took too long to send the data.');
                        }
                        else {
                            alert('An error occurred on the server. Please try again in a minute.');
                        }
                        myDataTable.fnProcessingIndicator(false);
                    }

                    mGlobal.ajaxData = mGlobal.ajaxData == undefined ? {} : mGlobal.ajaxData;
                    mGlobal.tableColumns = mGlobal.tableColumns == undefined ? {} : mGlobal.tableColumns;

                    if (mGlobal.ajaxData[model.page] == undefined) {

                        commonDynamic.functions.tabulate.filters.builderDataTableAjax(model);
                    }

                    $(model.tableContainer).empty();
                    $(model.tableContainer).append(Base64.decode(model.html));

                    mGlobal.tableColumns[model.tableName] = {
                        ColumnsModel: commonDynamic.functions.tabulate.buildtableColumns(model)
                    };

                    var tableName = model.tableID == undefined ? '#' + model.tableName : model.tableID;
                    var modelObjPath = model.path == undefined ? 'page' : model.path;

                    function searchBind(me) {
                        var thisObjectData = me;
                        if (thisObjectData && thisObjectData.tableName && table && table[thisObjectData.tableName]) {
                            table[model.tableName].search($('#' + model.tableName + '_filter input[type="search"]').val()).draw()
                        }
                    }

                    table[model.tableName] = $(tableName).on('preXhr.dt', function (e, settings, data) {
                        data.columns = [];

                    }).on('xhr.dt', function (e, settings, json, xhr) {
                        //concole.log('An error has been reported by DataTables: ', message);
                        if (json == "Serious Server Error") {
                            $('.dataTables_processing').hide();
                            $('#currentReports').attr('disabled', false);
                            alert('Invalid Query, please check your query and try again!');
                        }

                        if (json == undefined) {
                            $('.the-loader').remove()
                            $.fn.dataTable.ext.errMode = 'none';
                            json = {};
                            json.data == [];
                            promptDialog.prompt({
                                promptID: 'Delete-Resp-Role',
                                body: 'Looks like there something wrong with your session!',
                                header: 'Session Warning',
                                buttons: [
                                    {
                                        text: "Refresh",
                                        click: function () {
                                            $($(this).parents('.modal')).modal('hide');
                                            window.location.reload();

                                        }
                                    }]
                            });

                        }

                        //if (typeof json.data == 'string' && Base64.isThisEncoded(json.data)) {
                        //    json.data = JSON.parse(Base64.decode(json.data));
                        //}


                        if (mGlobal[modelObjPath][model.page] != undefined) {
                            mGlobal[modelObjPath][model.page].currentJsonData = json;
                        }
                        currentJsonData = json;

                        if (mGlobal[modelObjPath][model.page].xhr != undefined && typeof mGlobal[modelObjPath][model.page].xhr == 'function') {
                            mGlobal[modelObjPath][model.page].xhr(json.data);
                        }

                        IrisEllipse_Hide();
                    }).on('error.dt', function (e, settings, techNote, message) {
                        mGlobal[modelObjPath][model.page].error = message;
                    }).on('processing.dt', function (e, settings, processing) {
                        $('.dataTables_processing').remove();
                        //$('#processingIndicator').css('display', processing ? 'block' : 'none');
                        if (processing) {
                            theLoader.show({ id: model.page + '_loader' });
                        }
                        else {
                            //theLoader.hide({ id: 'Ajax' + model.page });
                            theLoader.hide({ id: model.page + '_loader' });
                        }
                    }).DataTable({
                        //"fixedHeader": false,
                        //"fixedColumns": { leftColumns: 3 },
                        "error": handleAjaxError,
                        "responsive": true,
                        "scrollX": '100%',
                        "scrollY": model.scrollY != undefined && model.scrollY != '' ? model.scrollY : '600px',
                        "scroller": true,
                        //"stateSave": true,
                        "searchDelay": 1500,
                        "autoWidth": false,
                        "scrollCollapse": true,
                        "processing": true,
                        "serverSide": true,
                        "pagingType": "full_numbers",
                        "paging": true,
                        "stateSave": false,
                        "dom": model.dom != undefined ? model.dom : "<'row'<'col-sm-6'l><'col-sm-6'f>><'row'<'col-sm-12'tr>><'row'<'col-sm-5'i><'col-sm-7'p>>",
                        "fixedColumns": model.fixedColumns != undefined ? model.fixedColumns : undefined,
                        "lengthMenu": [[10, 25, 50, 100, 200, -1], [10, 25, 50, 100, 200, 'ALL']],
                        "pageLength": model.pageLength != undefined && model.pageLength != '' ? model.pageLength : 100,
                        "searching": true,
                        "oLanguage": model.oLanguage != undefined ? model.oLanguage : { "sLengthMenu": "_MENU_", "sSearch": "_INPUT_", "sSearchPlaceholder": "Search..." },
                        "ajax": {
                            headers: {
                                __RequestVerificationToken: currentRequestVerificationToken
                            },
                            "url": model.jsonURL,
                            "type": "POST",
                            "data": mGlobal['ajaxData'][model.page]
                            //"dataType": "json",
                            //"contentType": "application/x-www-form-urlencoded;charset=utf-8",
                            //"async": true,
                            //"cache": false,
                        },
                        "fnPreDrawCallback": function (oSettings, json) {
                            if (mGlobal[modelObjPath][model.page].preDrawCallBackActions != undefined) {
                                mGlobal[modelObjPath][model.page].preDrawCallBackActions();
                            }
                        },
                        "fnDrawCallback": function (oSettings, json) {

                            setTimeout(function () {
                                $('#' + model.page + '_wrapper .dataTables_scrollHeadInner .table:hidden, #' + model.page + '_wrapper .dataTables_scrollHeadInner:hidden').css('width', '100%');
                            }, 1000);

                            if ($('.popovers').length > 0) {

                                $('.popover.in').hide();
                                $('.popovers').popover();
                            }

                            if (table[mGlobal[modelObjPath][model.page].tableName] != undefined) {

                                if (table[mGlobal[modelObjPath][model.page].tableName]) { table[mGlobal[modelObjPath][model.page].tableName].columns.adjust(); };
                            }

                            var navExt = mGlobal[modelObjPath][model.page].navExtension;
                            irisLoadingPanel.hide({ id: model.page });

                            setTimeout(function () {

                                var thisTempTableName = model.tableName != undefined ? model.tableName : model.page;

                                iGlobal.pager.init({
                                    tableId: thisTempTableName,
                                    divID: '#' + thisTempTableName,
                                    currentJsonData: mGlobal[modelObjPath][model.page].currentJsonData,
                                    params: table[thisTempTableName].ajax.params(),
                                    recordType: ''
                                });

                                if (model.hasTableHeightSelector && $('#' + thisTempTableName + ' tbody tr td:eq(0)')) {
                                    irisTableHeightSelect.init({
                                        tableName: model.page,
                                        //value: ['195px', '295px', '395px', '495px', '595px', '695px'],
                                        value: mGlobal[modelObjPath][model.page].table_height_options == undefined ? [.25, .5, .75, 1, 1.25, 1.5] : mGlobal[modelObjPath][model.page].table_height_options,
                                        max_height: mGlobal[modelObjPath][model.page].max_height == undefined ? $('.page-content').height() - $('#' + thisTempTableName + ' tbody tr td:eq(0)').offset().top : mGlobal[modelObjPath][model.page].max_height,
                                        defaultValue: mGlobal[modelObjPath][model.page].table_height_index == undefined ? 2 : mGlobal[modelObjPath][model.page].table_height_index,
                                        width: '60px',
                                        sort: 'd'
                                    });
                                }
                                if (model.hasTableZoom) {
                                    irisTableZoom.init({
                                        tableName: model.page,
                                        name: model.page
                                    });
                                }

                                if (model.hasSelectedRowData && mGlobal[modelObjPath][model.page].currentJsonData.recordsFiltered > 0) {
                                    irisSelectedRowData.init({
                                        page: model.page,
                                        col: mGlobal[modelObjPath][model.page].SelectedRowData.col,
                                        width: mGlobal[modelObjPath][model.page].SelectedRowData.width,
                                        delimiter: mGlobal[modelObjPath][model.page].SelectedRowData.delimiter,
                                        selectorType: mGlobal[modelObjPath][model.page].SelectedRowData.selectorType,
                                        tabName: mGlobal[modelObjPath][model.page].SelectedRowData.tabName,
                                        hasTabs: mGlobal[modelObjPath][model.page].SelectedRowData.hasTabs
                                    });
                                    //$('.' + irisSelectedRowData.get_name()).html(' ');
                                    $('#' + model.page + ' tbody tr:eq(0)').trigger('click');
                                }
                            }, 100);


                            $('.form-control.input-sm').attr('title', 'Search');
                            $('select.form-control.input-sm').attr('title', 'Rows Per Page');
                            $('.iris-pager-nav-select').attr('title', 'Current Page');
                            $('.dataTables_scrollHeadInner table').css('display', 'table');
                            $('.dataTables_scrollBody table').css('display', 'table');
                            $('.dataTables_wrapper .dataTables_paginate .paginate_button').css('padding', '2px');
                            $('.dataTables_scrollBody').addClass('NoBottomBorder ' + model.customTableClass);

                            $(tableName).css('display', 'table');
                            $(tableName).css('float', 'left');

                            $('#currentReports').attr('disabled', false);

                            $('<style type="text/css"> .css_' + model.tableName + ' { min-width: 150px;} </style>').appendTo('head');

                            commonDynamic.functions.tabulate.buildColumns(model.tableName);

                            $(model.tableContainer).removeClass('hide-on-init');
                            $('[name="FilterDiv_' + model.page + '"]').removeClass('hide-on-init');

                            if (model.showHover == undefined || !model.showHover) {
                                $(model.tableID).removeClass('table-hover');
                            }

                            if (mGlobal.drawCallBackActions != undefined && mGlobal.drawCallBackActions[model.page] != undefined) {
                                mGlobal.drawCallBackActions[model.page]();
                            }

                            if (mGlobal[modelObjPath][model.page].drawCallBackActions != undefined) {
                                mGlobal[modelObjPath][model.page].drawCallBackActions();
                            }

                            if (mGlobal[modelObjPath][model.page].drawCallBackActionsWait != undefined) {
                                setTimeout(function () {
                                    mGlobal[modelObjPath][model.page].drawCallBackActionsWait();
                                }, 200);
                            }

                            //if (mGlobal[modelObjPath][model.page].scrollTop !== undefined && mGlobal[modelObjPath][model.page].scrollLeft !== undefined) {

                            //    $(this).parent().scrollTop(mGlobal[modelObjPath][model.page].scrollTop);
                            //    $(this).parent().scrollLeft(mGlobal[modelObjPath][model.page].scrollLeft);
                            //}
                        },
                        "fnInitComplete": function () {
                            //if (hasSelectedRowData) {
                            //    irisSelectedRowData.get_myFunction()
                            //}

                            $('#' + model.page + '_filter input[type="search"]').unbind();
                            $('#' + model.page + '_filter input[type="search"]').data('owner', model.page);

                            //$('#' + model.page + '_filter input[type="search"]').on('change keyup search',
                            //    _.debounce(function () {
                            //        var thisObjectData = $(this).data();
                            //        if (thisObjectData && thisObjectData.owner && table && table[thisObjectData.owner]) {
                            //            table[model.page].search($(this).val()).draw()
                            //        }
                            //    }, 1000)
                            //);    		

                            //$('#' + model.page + '_filter input[type="search"]').unbind('change keyup search', _.debounce(function () { searchBind(model) }, 1000));
                            $('#' + model.page + '_filter input[type="search"]').bind('change keyup search', _.debounce(function () { searchBind(model) }, 1500));

                            setTimeout(function () {
                                $('#' + model.page + '_wrapper .dataTables_scrollHeadInner .table, #' + model.page + '_wrapper .dataTables_scrollHeadInner').css('width', '100%');
                            }, 1000)


                            sidebar.fn.open({ closestfilter: 'FilterDiv_' + model.page, closestWrapper: 'BodyContentWrapper_' + model.page });
                            sidebar.fn.expandParent({ tableName: model.page });
                            $('#' + model.page + '_wrapper .dataTables_paginate.paging_full_numbers, #' + model.page + '_wrapper .dataTables_info').hide();
                            $('body').find('.dataTables_scrollBody').addClass('scrollbar');
                            $(model.tableContainer).removeClass('hide-on-init');
                            $('[name="FilterDiv_' + model.page + '"]').removeClass('hide-on-init');


                            if (model.showHover == undefined || !model.showHover) {
                                $(model.tableID).removeClass('table-hover');
                            }

                            if (mGlobal.page[model.page].generatedInitCompleteActions != undefined) {
                                mGlobal.page[model.page].generatedInitCompleteActions();
                            }

                            if (mGlobal.initCompleteActions != undefined && mGlobal.initCompleteActions[model.page] != undefined) {
                                mGlobal.initCompleteActions[model.page]();
                            }

                            if (mGlobal[modelObjPath][model.page].initCompleteActions != undefined) {
                                mGlobal[modelObjPath][model.page].initCompleteActions();
                            }

                            $('#SaveReport').attr('disabled', false);
                        },
                        "colReorder": model.colReorder != undefined && model.colReorder != '' ? model.colReorder : false,
                        "columns": mGlobal.tableColumns[model.tableName].ColumnsModel.Columns,
                        "columnDefs": mGlobal.tableColumns[model.tableName].ColumnsModel.ColumnDefs,
                        "order": model.order != undefined && model.order != '' ? model.order : [[1, "asc"]]
                    });



                    $(tableName + ' tbody').on('click', 'tr', function () {
                        table[model.tableName].$('tr.selected').removeClass('selected');
                        $(this).addClass('selected');
                        var row = $(this).index();
                    });

                    if (typeof $('[name="CoreTabs"] li') !== 'undefined') {

                        $('[name="CoreTabs"] li').on('click', function () {

                            if ($('[name="CoreTabs"] li.active').hasClass('has-iris-srd') && $('.iris-srd') != undefined) {

                                $('.iris-srd').remove();
                            }
                        });
                    }


                    //theLoader.hide({ id: model.page + '_loader' });
                    mGlobal.page[model.page].isLoaded = true;
                    //$(this).parent().scrollTop(0);
                }
            }
        })(),
        drawCallBack: function (model) {


            if (model.default) {
                commonDynamic.functions.navigation.sidebar.css({ page: model.name });
            }
            else {
                mGlobal.sidebar.css({ page: model.name });
            }

            if (model.row != -1) {
                var selectedID = (model.id && model.index) ? (table[model.name].rows().data()[model.row][model.index] ? table[model.name].rows().data()[model.row][model.index] : '') : 'default';

                if (selectedID == 'default' || selectedID == model.id) {
                    $('#' + model.name + '_wrapper .dataTables_scrollBody tbody tr:eq(' + model.row + ')').addClass('selected');
                    $('#dd-item-' + model.name + '-actions-selected').show();
                }
            }
            else {
                $('#' + model.name + '_wrapper .dataTables_scrollBody tbody tr').removeClass('selected');
                $('#dd-item-' + model.name + '-actions-selected').hide();
            }
        },
        buildColumns: function (tableName) {
            $('#' + tableName + '_wrapper tr th').each(function (i) {

                var thisColumn = $('#' + tableName + '_wrapper tr th:eq(' + i + ')').data('columnname');

                $('#' + tableName + '_wrapper tr td:nth-child(' + (i + 1) + '), #'
                    + tableName + '_wrapper tr th:nth-child(' + (i + 1) + ')').addClass('css_' + thisColumn + ' css_' + tableName).data('column', thisColumn);

                //get the pretty name for this column
                var corrFilter = _.find(mGlobal.page[tableName].CurrentBaseFiltersModel, function (bf) { return bf.FilterName == thisColumn; });

                //if it's different than the regular name, set an attribute on the th and change the displayed text
                if (corrFilter && corrFilter.PrettyName != thisColumn) {
                    $('#' + tableName + '_wrapper tr:eq(0) th:eq(' + i + ')').attr('data-pretty-name', corrFilter.PrettyName);
                    $('#' + tableName + '_wrapper tr:eq(0) th:eq(' + i + ')').text(corrFilter.PrettyName);
                }
            });
        },
        saveScrollPosition: function (model) {
            if (table[model.page] != undefined && mGlobal.page[model.page].stateSave) {
                mGlobal.page[model.page].scrollTop = $('div.dataTables_scrollBody:visible').scrollTop();
                mGlobal.page[model.page].scrollLeft = $('div.dataTables_scrollBody:visible').scrollLeft();
            }
            else {
                mGlobal.page[model.page].scrollTop = 0;
                mGlobal.page[model.page].scrollLeft = 0;
            }

            mGlobal.page[model.page].stateSave = true;
        },
        buildtableColumns: function (model) {
            var temptableColumns = [];
            var temptableColumnDefs = [];

            $('#' + model.tableName + ' thead th').each(function (i) {
                var name = $('#' + model.tableName + ' thead th:eq(' + i + ')').data('columnname');
                if (i == 0) {
                    temptableColumns.push({ data: name.toLowerCase(), orderable: false });
                    temptableColumnDefs.push({ 'visible': false, 'width': '100px', 'targets': i });
                }
                else {
                    var neverFoundMatch = true;

                    if (mGlobal.page[model.page] != undefined) {
                        if (mGlobal.page[model.page].columnOverrides != undefined && mGlobal.page[model.page].columnOverrides.length > 0) {

                            var j = 0;
                            var foundMatch = false;

                            do {

                                mGlobal.page[model.page].columnOverrides[j].name = mGlobal.page[model.page].columnOverrides[j].name == undefined && mGlobal.page[model.page].columnOverrides[j].data != undefined ? mGlobal.page[model.page].columnOverrides[j].data : mGlobal.page[model.page].columnOverrides[j].name;

                                if (name.toLowerCase() == mGlobal.page[model.page].columnOverrides[j].name.toLowerCase()) {
                                    foundMatch = true;
                                    neverFoundMatch = false;

                                    temptableColumns.push({
                                        data: name.toLowerCase(),
                                        orderable: mGlobal.page[model.page].columnOverrides[j].orderable != undefined ? mGlobal.page[model.page].columnOverrides[j].orderable : true,
                                        render: mGlobal.page[model.page].columnOverrides[j].render != undefined ? mGlobal.page[model.page].columnOverrides[j].render : undefined,
                                    });
                                    temptableColumnDefs.push({
                                        'visible': mGlobal.page[model.page].columnOverrides[j].visible != undefined ? mGlobal.page[model.page].columnOverrides[j].visible : true,
                                        'width': mGlobal.page[model.page].columnOverrides[j].width != undefined ? mGlobal.page[model.page].columnOverrides[j].width : '100px',
                                        'targets': i
                                    });
                                }

                                j++;
                            } while (!foundMatch && mGlobal.page[model.page].columnOverrides.length > j);
                        }

                    }
                    else {
                        neverFoundMatch = true;
                    }

                    if (neverFoundMatch) {
                        temptableColumns.push({ data: name.toLowerCase(), orderable: true });
                        temptableColumnDefs.push({ 'visible': true, 'width': '100px', 'targets': i });
                    }
                }
            });

            return { Columns: temptableColumns, ColumnDefs: temptableColumnDefs }
        },
        addTab: function (model) {
            $('[name="FilterDiv_' + model.page + '"] ul.nav-tabs').append('<li class=""><a href="#' + defaultTabPrefix + model.page + '" data-toggle="tab" class="filterTab no-reload">' + model.name + '</a></li>');
            $('[name="FilterDiv_' + model.page + '"] .tab-content').append('<div class="tab-pane" id="' + defaultTabPrefix + model.page + '">' + model.content + '</div>');
        },
        hideMetaCol: function (m) {
            $('#' + m.page + ' thead th').each(function (thisIndex, thisElement) {
                var name = $(thisElement).data('columnname');

                var hideColumn = ["OBJECT_TYPE", "ACTIVE", "VERIFIED", "ENABLED"];

                if (m.extraColumns != undefined && m.extraColumns.length > 0) {
                    hideColumn = hideColumn.concat(m.extraColumns);
                }
                //if (name.endsWith("_ID") || name.endsWith("_UUID") || name.includes('_F_') || hideColumn.indexOf(name.toUpperCase()) > -1) {
                if (name.endsWith("_ID") || name.endsWith("_UUID") || hideColumn.indexOf(name.toUpperCase()) > -1) {
                    $('.css_' + name).hide();

                    //var column = table[m.page].column(thisIndex);
                    //column.visible(false);
                    //console.log('hide ' + m.page + '   ' + name)
                }

                table[m.page].columns.adjust();
            });
        }
    }
})();
