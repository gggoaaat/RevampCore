dynamic.functions.reports = (function () {
    return {

        getPortletsDDL: function (model) {

            switch (model.direction) {
                case 'POST':

                    model.data = model.data;
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/dynamic/Get_All_Portlets";
                    model.options.callBack = function (model) { dynamic.functions.reports.getPortletsDDL(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false

                    ajaxDynamic(model);

                    break;
                case 'CALLBACK':

                    mGlobal.page.reports.currentPortletsDDL = model.response;

                    var portletsDdlMarkup = '';

                    for (var i = 0; i < mGlobal.page.reports.currentPortletsDDL.length; i++) {
                        var thisData = mGlobal.page.reports.currentPortletsDDL[i];

                        portletsDdlMarkup += '<option value=' + thisData.base_portlet_id + '>' + thisData.title + '</option>'
                    }

                    $('#ddlPrivPortlets').html(portletsDdlMarkup);

                    break;
            }
        },//getPortletsDDL

        savePortlet: function (model) {
            switch (model.direction) {
                case 'POST':

                    model.title = $('#portletName').val();
                    model.data = {
                        base_portlet_id: $('.portlet.editThisSectionRow').data('baseportletid'),
                        prev_portlet_id: $('.portlet.editThisSectionRow').data('portletid')
                    }

                    var _html = '';

                    $('[name="containersSelectList[]"]').each(function (i) {

                        _html += dynamic.functions.reports.createSectionContainer({
                            title: $('[name="containerName[]"]:eq(' + i + ')').val(),
                            size: $('[name="containersSelectList[]"]:eq(' + i + ') option:selected').data('col')
                        })
                    });

                    $('.editThisSectionRow .portlet-body .scroller').html(_html);

                    var portletSaveModel = {
                        I_ENABLED: 'Y',
                        I_APPLICATION_ID: sGlobal.application.id,
                        I_TITLE: model.title,
                        I_BASE_PORTLET_ID: model.data != undefined && model.data.base_portlet_id != undefined ? model.data.base_portlet_id : undefined,
                        I_PREV_PORTLET_ID: model.data != undefined && model.data.prev_portlet_id != undefined ? model.data.prev_portlet_id : undefined,
                        O_PORTLET_ID: model.data != undefined && model.data.portlet_id != undefined ? model.data.portlet_id : undefined
                    };

                    $('#PortletContainers tr').each(function (i) {
                        portletSaveModel['Containers[' + i + '].I_TITLE'] = $(this).data('containername');
                        portletSaveModel['Containers[' + i + '].I_SIZE'] = $(this).data('containersize');
                        portletSaveModel['Containers[' + i + '].I_BASE_CONTAINER_ID'] = $(this).data('basecontainerid');
                        portletSaveModel['Containers[' + i + '].I_PREV_CONTAINER_ID'] = $(this).data('containerid');
                    });

                    model.data = portletSaveModel;
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/dynamic/Create_Core_Portlet";
                    model.options.callBack = function (model) { dynamic.functions.reports.savePortlet(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false

                    ajaxDynamic(model);

                    break;

                case 'CALLBACK':

                    var data = model.response;

                    //$('.coreSections').empty();
                    dynamic.functions.reports.getPortlets({
                        direction: 'POST',
                        data: {
                            P_PORTLET_ID: model.data.O_PORTLET_ID
                        },
                        selector: $('.editThisSectionRow'),
                        callType: "SINGLE"
                    });

                    $('#corePortletConfigModal').modal('hide');
                    break;
            }
        },//savePortlet

        getPortlets: function (model) {

            switch (model.direction) {
                case 'POST':

                    model.data = { P_APPLICATION_ID: sGlobal.application.id };
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/dynamic/Get_Core_Portlet";
                    model.options.callBack = function (model) { dynamic.functions.reports.getPortlets(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false

                    ajaxDynamic(model);

                    break;
                case 'CALLBACK':

                    mGlobal.page.reports.currentPortlets = model.response;
                    model.DisplayData = mGlobal.page.reports.currentPortlets;
                    dynamic.functions.reports.displaySectionRows(model);
                    dynamic.functions.reports.getContainer({ direction: 'POST', portlets: mGlobal.page.reports.currentPortlets });
                    break;
            }
        },//getPortlets

        refreshPortlet: function (model) {
            var model = model != undefined ? model : {};

            model.direction = 'POST';
            model.data = {
                base_portlet_id: $('.portletContainer.editThisContainer').data('baseportletid'),
                //prev_portlet_id: $('.portlet.editThisSectionRow').data('prevPortletID'),
                prev_portlet_id: $('.portletContainer.editThisContainer').data('portletid')
                //    portlet_id: $('.portlet.editThisSectionRow').data('portletID')

            }
            model.selector = $('.editThisSectionRow');

            dynamic.functions.reports.getPortlets(model);
            dynamic.functions.reports.getPortletsDDL(model);
        },//refreshPortlet

        updateSelectedPrivPortlet: function (model) {
            switch (model.direction) {
                case 'POST':

                    var basePortletID = $('#ddlPrivPortlets option:selected').val();

                    $('#PRIV_BASE_PORTLET_ID').val(basePortletID);

                    var selectedPortlet = $.grep(mGlobal.page.reports.currentPortlets, function (e) {
                        if ((e.base_portlet_id == basePortletID))
                        { return true; }
                        else
                        { return false; }
                    });

                    dynamic.functions.reports.getPortletPrivileges({
                        data: { basePortletID: basePortletID }
                    });

                    break;
            }
        },//updateSelectedPrivPortlet

        getContainer: function (model) {
            switch (model.direction) {
                case 'POST':

                    tempArray = [];

                    for (var i = 0; i < model.portlets.length; i++) {
                        tempArray.push(model.portlets[i].portlet_id);
                    }

                    var getContainersModel = {
                        P_PORTLET_ID: tempArray.join(',')
                    };

                    model.data = getContainersModel;
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/dynamic/Get_Portlet_Containers";
                    model.options.callBack = function (model) { dynamic.functions.reports.getContainer(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false
                    model.callEvent = 'load container';

                    ajaxDynamic(model);

                    break;
                case 'CALLBACK':
                    mGlobal.page.reports.currentContainers = model.response;

                    for (var i = 0; i < mGlobal.page.reports.currentPortlets.length; i++) {

                        var tempPortlertContainers = [];

                        tempPortlertContainers = $.grep(mGlobal.page.reports.currentContainers, function (e) {
                            if ((e.base_portlet_id == mGlobal.page.reports.currentPortlets[i].base_portlet_id))
                            { return true; }
                            else
                            { return false; }
                        });

                        var _HTML = '';

                        for (var c = 0; c < tempPortlertContainers.length; c++) {
                            _HTML += dynamic.functions.reports.createSectionContainer(tempPortlertContainers[c]);
                        }

                        $('[data-baseportletid="' + mGlobal.page.reports.currentPortlets[i].base_portlet_id + '"] .portlet-body .scroller').append(_HTML);
                    }

                    dynamic.functions.reports.getUsedReports({ direction: 'POST' });
                    break;
            }
        },//getContainer

        getUsedReports: function (model) {
            switch (model.direction) {
                case 'POST':
                    model.data = model.data == undefined ? {} : model.data;

                    model.data.SearchReport = 'utilized custom report permissions';
                    model.data.start = 0;
                    model.data.length = -1;
                    model.data.P_DYNO_COL = '[PORTLET_CONTAINER_CUSTOM_REPORT_ID],[BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID],[PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID],[ENABLED],[DT_CREATED],[DT_UPDATED],[PORTLET_ID],[BASE_PORTLET_ID],[PREV_PORTLET_ID],[PORTLET_TITLE],[CONTAINER_ID],[BASE_CONTAINER_ID],[PREV_CONTAINER_ID],[CONTAINER_TITLE],[ROOT_REPORT_ID],[BASE_ROOT_REPORT_ID],[PREV_ROOT_REPORT_ID],[TEMPLATE_ID],[TEMPLATE_NAME],[CUSTOM_REPORT_ID],[BASE_CUSTOM_REPORT_ID],[PREV_CUSTOM_REPORT_ID],[CUSTOM_REPORT_NAME],[TITLE],[APPLICATION_ID],[APPLICATION_NAME],[ROOT_REPORT_NAME],[REPORT_NAME]';
                    model.data.P_VERIFY = 'T';
                    model.data['_clause.leftSideOfClause[0]'] = 'Identity_id';
                    model.data['_clause.ClauseConditions[0]'] = '=';
                    model.data['_clause.rightSideOfClause[0]'] = sGlobal.identityID.id + ' or a.IDENTITY_ID is null';
                    model.data.template = 'dynamic';
                    model.data.schema = 'dynamic';
                    model.data.queryType = 'query';
                    model.data['order[0][column]'] = 'BASE_CONTAINER_ID';
                    model.data['order[0][dir]'] = 'asc';
                    model.data['order[1][column]'] = 'PORTLET_CONTAINER_CUSTOM_REPORT_ID';
                    model.data['order[1][dir]'] = 'asc';

                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/dynamic/dynosearch?iReport=" + model.data.SearchReport;
                    model.options.callBack = function (model) { dynamic.functions.reports.getUsedReports(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false
                    model.callEvent = 'load container';

                    ajaxDynamic(model);

                    break;
                case 'CALLBACK':
                    switch (model.callEvent) {
                        default:
                        case 'load container':
                            mGlobal.page.reports.currentReports = model.response.data;

                            for (var i = 0; i < mGlobal.page.reports.currentContainers.length; i++) {

                                var currentReportsForContainer = $.grep(mGlobal.page.reports.currentReports, function (e) {
                                    if ((e.base_container_id == mGlobal.page.reports.currentContainers[i].base_container_id))
                                    { return true; }
                                    else
                                    { return false; }
                                });

                                var _html = '';

                                if (currentReportsForContainer.length > 0) {
                                    for (var j = 0; j < currentReportsForContainer.length; j++) {

                                        currentReportsForContainer[j].title_mod = currentReportsForContainer[j].title.replace(/[^\w\s]/gi, '_').replace(/\s/g, "_");
                                    }

                                    _html = $.fn.revamp.templates.li_currentReportsForContainer({ editReports: canUser('Access Report Builder'), currentReportsForContainer: currentReportsForContainer })
                                }

                                $('[data-basecontainerid="' + mGlobal.page.reports.currentContainers[i].base_container_id + '"] .portlet-body ul').html(_html);
                            }

                            if (model.forceRefresh) {
                                dynamic.functions.reports.refreshPortlet();
                            }
                            break;
                    }

                    break;
            }
        },//getUsedReports

        displaySectionRows: function (model) {

            var _html = '';

            for (var i = 0; i < model.DisplayData.length; i++) {

                var data = model.DisplayData[i];

                var baseID = data != undefined && data.base_portlet_id != undefined ? ' data-basePortletID="' + data.base_portlet_id + '" ' : '';
                var portletID = data != undefined && data.portlet_id != undefined ? ' data-portletID="' + data.portlet_id + '" ' : '';
                var prevPortletID = data != undefined && data.prev_portlet_id != undefined ? ' data-prevPortletID="' + data.prev_portlet_id + '" ' : '';

                var isThereAPortletTitle = data != undefined && data.title != undefined ? data.title : '';
                var vieworEdit = mGlobal.page.reports.currentPageState == 'edit' ? 'editsectionRow' : '';

                _html += $.fn.revamp.templates.div_portlet({
                    baseID: baseID,
                    portletID: portletID,
                    prevPortletID: prevPortletID,
                    isThereAPortletTitle: isThereAPortletTitle,
                    vieworEdit: vieworEdit,
                    canEdit: canUser('Access Report Builder')
                });
            }

            var thisSelector = model.selector == undefined ? $('.coreSections') : model.selector;

            if (model.selector == undefined) {
                thisSelector.append(_html);
            }
            else {
                thisSelector.replaceWith(_html);
            }

        },//displaySectionRows

        createSectionRow: function (model) {
            dynamic.functions.reports.displaySectionRows({
                DisplayData: [{}]
            })
        },//createSectionRow

        containerConfigOption: function (model) {

            if (model == undefined) {
                model = {}
            }

            model.title = model != undefined && model.title != undefined ? model.title : '';
            model.size = model != undefined && model.size != undefined ? model.size : '4';

            model.options = [];

            for (var i = 1; i <= 12; i++) {

                model.options.push({ value: i, selected: i == +model.size ? true : false });
            }

            return $.fn.revamp.templates.tr_containerConfigOption(model);
        },//containerConfigOption

        createSectionContainer: function (model) {

            model.size = model.size != undefined && model.size != '' ? model.size : '4';
            model.canEdit = canUser('Access Report Builder');

            return $.fn.revamp.templates.div_portletContainer(model);
        },//createSectionContainer

        toggleEdit: function (action) {
            switch (action) {
                case 'edit':
                    dynamic.functions.reports.makeEditable();
                    mGlobal.page.reports.currentPageState = 'edit';
                    break;
                case 'view':
                    dynamic.functions.reports.makeViewable()
                    mGlobal.page.reports.currentPageState = 'view';
                    break;
            }
        },//toggleEdit

        makeEditable: function () {
            $('.sectionRow').addClass('editsectionRow');
        },

        makeViewable: function () {
            $('.sectionRow').removeClass('editsectionRow');
        },

        loadReport2Container: function () {

            var iteration = 0;

            if ($('[name="customBaseReportsSelectList[]"]:last').index() >= 0) {
                iteration = +$('[name="customBaseReportsSelectList[]"]:last').data('iteration') + 1;
            }
            else {
                iteration = 1;
            }

            return $.fn.revamp.templates.tr_loadReport2Container({ iteration: iteration });
        },//loadReport2Container

        configurationLaunchLogic: function (model) {
            switch (model.thisObject.attr('href')) {
                case '#portlet-config':
                    var thisPortlet = model.thisObject.closest('.sectionRow');

                    $('.portlet').removeClass('editThisSectionRow');

                    $(thisPortlet).addClass('editThisSectionRow');

                    var portletID = $('.portlet.editThisSectionRow').data('portletid');
                    var basePortletID = $('.portlet.editThisSectionRow').data('baseportletid');
                    var prevPortletID = $('.portlet.editThisSectionRow').data('prevportletid');

                    $('#CURRENT_PORTLET_ID').val(portletID);
                    $('#BASE_PORTLET_ID').val(basePortletID);
                    $('#PREV_PORTLET_ID').val(prevPortletID);

                    var selectedPortlet = $.grep(mGlobal.page.reports.currentPortlets, function (e) {
                        if ((e.base_portlet_id == basePortletID))
                        { return true; }
                        else
                        { return false; }
                    });

                    if (selectedPortlet[0] != undefined) {
                        $('#portletName').val(selectedPortlet[0].title)
                    }

                    var selectedContainers = $.grep(mGlobal.page.reports.currentContainers, function (e) {
                        if ((e.base_portlet_id == basePortletID))
                        { return true; }
                        else
                        { return false; }
                    });

                    var _htmlContainers = '';
                    for (var i = 0; i < selectedContainers.length; i++) {
                        _htmlContainers += dynamic.functions.reports.containerConfigOption(selectedContainers[i]);
                    }

                    $('#PortletContainers').append(_htmlContainers)

                    $('#corePortletConfigModal').modal('show');
                    break;
                case '#container-config':

                    var thisContainer = model.thisObject.closest('.portletContainer');

                    $('.portletContainer').removeClass('editThisContainer');

                    $(thisContainer).addClass('editThisContainer');

                    $('.portletContainer.editThisContainer').closest('.sectionRow').addClass('editThisSectionRow');

                    var currentPortletName = model.thisObject.closest('.portletContainer').data('portletname');
                    var currentPortletID = model.thisObject.closest('.portletContainer').data('containerid');

                    $('[name="CONTAINER_ID"]').val(currentPortletID);

                    //get the reports that are assigned to this portlet/container
                    var currentReportsForContainer = $.grep(mGlobal.page.reports.currentReports, function (e) {
                        if ((e.base_container_id == currentPortletID))
                        { return true; }
                        else
                        { return false; }
                    });

                    ////loop through reports on this container, displaying each on the config modal
                    var _html = $.fn.revamp.templates.tr_currentReportsForContainer({ currentReportsForContainer: currentReportsForContainer });

                    $('#ReportContainers tbody').append(_html);

                    $('#AddReportToContainer').modal('show');

                    break;
                case '#portlet-privileges':
                    var thisPortlet = model.thisObject.closest('.sectionRow');

                    $('.portlet').removeClass('editThisSectionRow');

                    $(thisPortlet).addClass('editThisSectionRow');

                    var basePortletID = $('.portlet.editThisSectionRow').data('baseportletid');

                    $('#PRIV_BASE_PORTLET_ID').val(basePortletID);

                    var selectedPortlet = $.grep(mGlobal.page.reports.currentPortlets, function (e) {
                        if ((e.base_portlet_id == basePortletID))
                        { return true; }
                        else
                        { return false; }
                    });

                    $('#ddlPrivPortlets option[value="' + basePortletID + '"]').attr('selected', 'selected');


                    dynamic.functions.reports.getPortletPrivileges({
                        callEvent: 'show modal',
                        data: { basePortletID: basePortletID }
                    });

                    break;
                default:
                    break;
            }
        },//configurationLaunchLogic

        modalAllPortletsPrivs: function () {
            $('#ddlPrivPortlets').val($('#ddlPrivPortlets option:first').val());
            var basePortletID = $('#ddlPrivPortlets option:selected').val();

            $('#PRIV_BASE_PORTLET_ID').val(basePortletID);

            var selectedPortlet = $.grep(mGlobal.page.reports.currentPortlets, function (e) {
                if ((e.base_portlet_id == basePortletID))
                { return true; }
                else
                { return false; }
            });


            dynamic.functions.reports.getPortletPrivileges({
                callEvent: 'show modal',
                data: { basePortletID: basePortletID }
            });


        },//modalAllPortletsPrivs

        getPortletPrivileges: function (model) {
            switch (model.direction) {
                case 'POST':
                default:

                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = '/dynamic/GetPortletPrivileges';
                    model.options.callBack = function (model) { dynamic.functions.reports.getPortletPrivileges(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false
                    model.callEvent = 'show modal'; // 'load container';

                    ajaxDynamic(model);

                    break;

                case 'CALLBACK':

                    mGlobal.page.reports.currentPortletPrivileges = model.response;

                    if (mGlobal.page.reports.currentPortletPrivileges.success == true) {
                        var portPrivHTML = '';

                        var privs = mGlobal.page.reports.currentPortletPrivileges.data;

                        for (var i = 0; i < privs.length; i++) {

                            var isSelected = privs[i].on_portlet ? 'selected' : '';
                            portPrivHTML += '<option value="' + privs[i].privilege_id + '" ' + isSelected + '>' + privs[i].privilege_name + '</option>';
                        }

                        $('#privs_multi_select').empty();
                        $('#privs_multi_select').append(portPrivHTML);

                        dynamicMultiSelectComponent.rebuild({ selector: '#privs_multi_select', formSelector: '#PortletPrivForm', multiselectOwner: 'privs_multi_select' });
                        dynamicMultiSelectComponent.init({ selector: '#privs_multi_select', formSelector: '#PortletPrivForm', multiselectOwner: 'privs_multi_select' });
                    }

                    switch (model.callEvent) {
                        case 'show modal':
                            $('#corePortletPrivilegesModal').modal('show');

                            break;
                    }

                    break;
            }
        },//getPortletPrivileges       

        setPortletPrivileges: function (model) {
            switch (model.direction) {
                case 'POST':
                    var thisModel = {
                    };

                    thisModel.base_portlet_id = $('#PRIV_BASE_PORTLET_ID').val();
                    thisModel.portlet_privilege_type_id = 1;//todo: fix $('#PRIV_BASE_PORTLET_ID').val();
                    thisModel.privilege_ids = [];

                    $('#privs_multi_select option:selected').each(function (i, element) {
                        thisModel.privilege_ids.push($(element).val());
                    });

                    model.data = thisModel;
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = '/dynamic/SetPortletPrivileges';
                    model.options.callBack = function (model) { dynamic.functions.reports.setPortletPrivileges(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false

                    ajaxDynamic(model);

                    break;
                case 'CALLBACK':
                    if (model.response.success == true) {
                        $('#corePortletPrivilegesModal').modal('hide');
                        toastr[ToastConstants.genericSuccess.type](ToastConstants.genericSuccess.msg, ToastConstants.genericSuccess.title);

                        $('.coreSections').html('');
                        //Look to move this method out of CORe and Into Dynamic
                        core.functions.main.loadReports();
                    }
                    else {
                        toastr[ToastConstants.genericError.type](ToastConstants.genericError.msg, ToastConstants.genericError.title);
                    }
                    break;
            }
        },//setPortletPrivileges

        removeReportFromContainer: function (model) {
            ///<summary>this will remove the selected report from the container (both db call and ui)</summary>
            ///<param name="model" type="object">
            ///     incoming example in response to user input (about to make the db call) would be
            ///     model { thisObject = the <li> of the report to be removed
            ///             direction = 'POST' (indicating this is the first call in response to user action) }
            ///
            ///     incoming example in response to callback (db ajax call completed) would be
            ///     model { direction = 'CALLBACK' (indicating this is the call made after ajax response from server)
            ///             dbsuccess = true/false whether the call succeeded
            ///             recordid = id of the record that was deleted }
            ///</param>

            switch (model.direction) {
                case 'POST':

                    var containerReportId = model.thisObject.data('portletcontainercustomreportid');

                    var ContainerReportToDelete = {};
                    ContainerReportToDelete['thisModel.O_PORTLET_CONTAINER_CUSTOM_REPORT_ID'] = containerReportId;

                    model.data = ContainerReportToDelete;
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = '/dynamic/Remove_Report_From_Container';
                    model.options.callBack = function (model) { dynamic.functions.reports.removeReportFromContainer(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false

                    ajaxDynamic(model);

                    break;
                case 'CALLBACK':

                    model.dbsuccess = model.response.dbsuccess;
                    model.recordId = model.response.recordId;

                    //there was some issue with deleting it, don't remove from screen
                    if (!model.dbsuccess) {
                        toastr[ToastConstants.deletionError.type](ToastConstants.deletionError.msg, ToastConstants.deletionError.title);
                        return;
                    }

                    //remove the <li> containing the report from the portlet/container
                    $('ul.reports_ul li[data-portletcontainercustomreportid=' + model.recordId + ']').remove();

                    //remove the <tr> containing the report from the config modal
                    $('#ReportContainers tbody tr[data-portletcontainercustomreportid=' + model.recordId + ']').remove();

                    //remove the report from mGlobal currentReports
                    mGlobal.page.reports.currentReports = _.reject(mGlobal.page.reports.currentReports, function (el) { return el.portlet_container_custom_report_id == model.recordId });

                    toastr[ToastConstants.deletionSuccess.type](ToastConstants.deletionSuccess.msg, ToastConstants.deletionSuccess.title);

                    break;
            }
        },//removeReportFromContainer

        saveReportToContainer: function (model) {
            switch (model.direction) {
                default:
                case 'POST':

                    var AddReports = []
                    var Container = $('[name="CONTAINER_ID"]').val();
                    var HTML = '';
                    var thisContainer = $('[name="CONTAINER_ID"]').val()

                    $('#ReportContainers tbody tr').each(function (i) {

                        var thisReportid = $('#ReportContainers tbody tr:eq(' + i + ') [name="customBaseReportsSelectList[]"] option:selected').data('customreportid');
                        var thisCustomReportid = $('#ReportContainers tbody tr:eq(' + i + ') [name="customReportsSelectList[]"] option:selected').data('id');
                        var thisTitle = $('#ReportContainers tbody tr:eq(' + i + ') [name="customReportsTitle[]"]').val();

                        if (thisReportid != undefined && thisCustomReportid != undefined && (thisTitle != undefined && thisTitle != '')) {
                            AddReports.push({
                                BaseCustomReportID: thisReportid,
                                CustomReportID: thisCustomReportid,
                                Title: thisTitle
                            });
                        }
                    });

                    var Report2Container = {}
                    for (var i = 0; i < AddReports.length; i++) {
                        Report2Container['theseModels[' + i + '].I_BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID'] = 0;
                        Report2Container['theseModels[' + i + '].I_PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID'] = 0;
                        Report2Container['theseModels[' + i + '].I_CONTAINER_ID'] = thisContainer;
                        Report2Container['theseModels[' + i + '].I_CUSTOM_REPORT_ID'] = AddReports[i].CustomReportID
                        Report2Container['theseModels[' + i + '].I_TITLE'] = AddReports[i].Title
                    }

                    model.data = Report2Container;
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = '/dynamic/Create_Core_Container_Report';
                    model.options.callBack = function (model) { dynamic.functions.reports.saveReportToContainer(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false

                    ajaxDynamic(model);

                    break;
                case 'CALLBACK':

                    dynamic.functions.reports.getUsedReports({ direction: 'POST', forceRefresh: true });
                    $('#AddReportToContainer').modal('hide');

                    break;
            }
        },//saveReportToContainer

        callLoadReport: function (model) {

            model.reporttitle = model._reporttitle.toString().indexOf(' ') > 0 ? model._reporttitle.toString().split(' ').join('_') : model._reporttitle.toString();
            model.customreportid = model.containerid + '_' + model._customreportid + '_' + model.reporttitle;

            var theReportModel = {
                currentReport: model.currentReport,
                containerid: model.containerid,
                _customreportid: model._customreportid,
                customreportid: model.customreportid,
                reportLocation: model.prefix + '_' + model.containerid + '_' + model._customreportid,
                _reporttitle: model._reporttitle,
                reporttitle: model.reporttitle,
                rawTableName: model.prefix + '_' + model.customreportid,
                page: model.prefix + '_' + model.customreportid,

                BodyContentWrap: 'BodyContentWrapper_' + model.prefix + '_' + model.customreportid,
                rowWrapper: 'ReportBuilderDiv_' + model.customreportid + '',
                datatableWrapper: 'dataTableContainer_' + model.customreportid + '',

                contentWrapper: 'tab_collection_' + model.customreportid,
                mainContentSelector: '#tab_reports .tab-content#ReportsTabContent',

                addToTab: model.addToTab,
                navTabSelector: '[name="ReportCollectionTabs"]',

                addSidebar: model.addSidebar,
                FilterDiv: 'FilterDiv_' + model.prefix + '_' + model.customreportid,
                sideBarWrapperSelector: '#' + 'tab_collection_' + model.customreportid,
                FilterAccordion: 'Filters_Accordion_' + model.customreportid,
                template: model.template,
                schema: model.schema,
                hasExports: true,
                hasOpenSideBar: true,
                hasQueryClause: false,
                hasTableZoom: true,
                hasTableHeightSelector: true,
                headertitle: model.headertitle,
                jsonURL : model.jsonURL
            }

            if ($('[data-contentwrapper="' + theReportModel.contentWrapper + '"]').length == 0) {

                buildCustomReport.create(theReportModel);

                mGlobal.states[theReportModel.page + "_Interval"] = window.setInterval(
                    function () {
                        if (mGlobal.page[theReportModel.page] != undefined
                            && mGlobal.page[theReportModel.page].currentJsonData != undefined
                            && mGlobal.page[theReportModel.page].currentJsonData.data != undefined
                            && mGlobal.page[theReportModel.page].currentJsonData.data.length >= 0) {
                            clearInterval(mGlobal.states[theReportModel.page + "_Interval"]);

                            dynamic.functions.reports.clickOpenTab(theReportModel);
                        }
                    }, 200
                );
            }
            else {
                dynamic.functions.reports.clickOpenTab(theReportModel)

            }
        },//callLoadReport

        clickOpenTab: function (model) {
            $('[data-contentwrapper="' + model.contentWrapper + '"] .closeTab').addClass('closeTabDisable').removeClass('closeTab');
            $('[data-contentwrapper="' + model.contentWrapper + '"]').trigger('click');
            $('[data-contentwrapper="' + model.contentWrapper + '"] .closeTabDisable').addClass('closeTab').removeClass('closeTabDisable');
        },//clickOpenTab

        resizeReportWrapper: function () {

            if ($('.ReportsWrapper').length) {
                var thisHeight = $('.ReportsWrapper').offset().top;
                $('.ReportsWrapper').hide();
                Layout.fixContentHeight();

                $('.ReportsWrapper').css('height', (($('.page-content').height() + +$('.page-content').css('padding-bottom').replace('px', '') + +$('.page-content').css('padding-top').replace('px', '')) - thisHeight) + 'px');
                $('.ReportsWrapper').show()
            }

        },//resizeReportWrapper

        customReportSelectedChange: function (el) {
            var rptTitle = $(el).find('option:selected').text();

            if (rptTitle == 'Select Report') {
                $(el).parent().parent().find('input[name="customReportsTitle[]"]').val('');
            }
            else {
                $(el).parent().parent().find('input[name="customReportsTitle[]"]').val(rptTitle);
            }
        },

        closeCollectionReport: function (model) {
            var thisData = model.currentObject.data();

            if (table[thisData.table] != undefined) {
                table[thisData.table].destroy();
            }

            delete table[thisData.table];
            delete mGlobal.page[thisData.table];

            $('#' + thisData.contentwrapper).remove();

            var thisNavLi = model.currentObject.parent('a').parent('li');

            if (thisNavLi.hasClass('active')) {

                thisNavLi.remove();

                $('.nav-tabs [href="#tab_collection"]').trigger('click');
            }
            else {
                thisNavLi.remove();
            }
        },
        reDrawColumns: function (model) {
            var thisTable = model.currentObject.data('tabcollection');

            window.setTimeout(
                function () {
                    //table[thisTable].columns.adjust();
                    // window.dispatchEvent(new Event('resize'))
                    $('#' + thisTable).resize()
                }, 150)
        },
        removeReport: function (model) {
            var thisObject = model.currentObject;

            promptDialog.prompt({
                promptID: 'remove-report',
                body: 'Are you sure you want to remove this report from the container?',
                header: 'Remove Report',
                forceBackDrop: true,
                buttons: [
                    {
                        text: 'No',
                        click: function () {
                            $($(this).parents('.modal')).modal('hide');
                        }
                    },
                    {
                        text: 'Yes',
                        click: function () {
                            dynamic.functions.reports.removeReportFromContainer({ thisObject: thisObject, direction: 'POST' }); //'post' is for the initial call, called method will accept callback to display final result
                            $($(this).parents('.modal')).modal('hide');
                        },
                        selected: true
                    }]
            });
        },
        launchReport: function (model) {
            var thisObject = model.currentObject;

            dynamic.functions.reports.callLoadReport({
                // thisObject: thisObject,
                currentReport: thisObject.data('rootreportname'),
                containerid: thisObject.closest('.portletContainer ').data('basecontainerid'),
                template: thisObject.data('templatename'),
                schema: thisObject.data('templatename'),
                prefix: 'thisDataTable',
                _customreportid: thisObject.data('customreportid'),
                _reporttitle: thisObject.data('reporttitle'),
                headertitle: thisObject.data('headertitle'),
                addToTab: true,
                addSidebar: true, 
                jsonURL : undefined
            });
        },
        trackChange: function (model) {
            var thisObject = model.currentObject;

            var _tr = thisObject.closest('tr');

            switch (thisObject.attr('name')) {
                case "containerName[]":
                    _tr.attr('data-containername', thisObject.val());
                    break;
                case "containersSelectList[]":
                    _tr.attr('data-containersize', thisObject.val());
                    break;
            }
        },
        launchReportSettings: function (model) {
            dynamic.functions.reports.modalCustomReportPrivs(model)
        },
        modalCustomReportPrivs: function (model) {
            //   $('#ddlPrivReports').val($('#ddlPrivReports option:first').val());
            //var customReportID = $('#ddlPrivReports option:selected').val();

            var customReportID = model.currentObject.data().customreportid;

            $('#I_CUSTOM_REPORT_ID').val(customReportID);

            $('#spanPrivReports').html(model.currentObject.data().headertitle);

            //var selectedPortlet = $.grep(mGlobal.page.reports.currentPortlets, function (e) {
            //    if ((e.base_portlet_id == basePortletID))
            //    { return true; }
            //    else
            //    { return false; }
            //});


            dynamic.functions.reports.getReportPrivileges({
                callEvent: 'show modal',
                data: { customReportID: customReportID }
            });
        },
        getReportPrivileges: function (model) {
            switch (model.direction) {
                case 'POST':
                default:

                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = '/dynamic/GetCustomReportPrivileges';
                    model.options.callBack = function (model) { dynamic.functions.reports.getReportPrivileges(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false
                    model.callEvent = 'show modal'; // 'load container';

                    ajaxDynamic(model);

                    break;

                case 'CALLBACK':

                    mGlobal.page.reports.currentCustomReportPrivileges = model.response;

                    if (mGlobal.page.reports.currentCustomReportPrivileges.success == true) {
                        var portPrivHTML = '';

                        var privs = mGlobal.page.reports.currentCustomReportPrivileges.data;

                        for (var i = 0; i < privs.length; i++) {

                            var isSelected = privs[i].selected ? 'selected' : '';
                            portPrivHTML += '<option value="' + privs[i].privilege_id + '" ' + isSelected + '>' + privs[i].privilege_name + '</option>';
                        }

                        $('#custom_report_privs_multi_select').empty();
                        $('#custom_report_privs_multi_select').append(portPrivHTML);

                        dynamicMultiSelectComponent.rebuild({ selector: '#custom_report_privs_multi_select', formSelector: '#ReportPrivForm', multiselectOwner: 'custom_report_privs_multi_select' });
                        dynamicMultiSelectComponent.init({ selector: '#custom_report_privs_multi_select', formSelector: '#ReportPrivForm', multiselectOwner: 'custom_report_privs_multi_select' });
                    }

                    switch (model.callEvent) {
                        case 'show modal':
                            $('#dynamicReportPrivilegesModal').modal('show');

                            break;
                    }

                    break;
            }
        },//getPortletPrivileges
        setReportPrivileges: function (model) {
            switch (model.direction) {
                case 'POST':
                default:
                    
                    model.data = {
                        'theseModels[0].I_PRIVILEGE_IDS': $('#custom_report_privs_multi_select').val().join(),
                        'theseModels[0].I_CUSTOM_REPORT_ID': $('#I_CUSTOM_REPORT_ID').val()
                    }
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = '/dynamic/Create_Custom_Report_Priv';
                    model.options.callBack = function (model) { dynamic.functions.reports.setReportPrivileges(model) }

                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false
                    model.callEvent = 'show modal'; // 'load container';

                    ajaxDynamic(model);

                    break;

                case 'CALLBACK':

                    promptDialog.prompt({
                        promptID: 'permissions_updated',
                        body: 'Permissions Updated',
                        header: 'ermissions Updated',
                        forceBackDrop: true
                    });

                    break;
            }
        },//getPortletPrivileges
    }
}());


