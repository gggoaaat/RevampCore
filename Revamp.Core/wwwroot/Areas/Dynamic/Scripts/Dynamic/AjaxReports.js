function ajaxCoreReports(url, data, switchMechanism) {
    var ajaxData;
    var _async = (switchMechanism.async == undefined || switchMechanism.async == null) ? true : switchMechanism.async;
    var _type = (switchMechanism.type == undefined || switchMechanism.type == null) ? 'POST' : switchMechanism.type;

    $.ajax({
        url: url,
        async: _async,
        type: _type,
        data: data,
        dataType: 'json',
        success: function (response) {

            var callCase = (switchMechanism.callCase == undefined || switchMechanism.callCase == null) ? switchMechanism : switchMechanism.callCase;

            switch (callCase) {
                //Load Functions
                case 'create portlet':
                    switchMechanism.model.method = 'CALLBACK';
                    switchMechanism.model.data = response;
                    dynamic.functions.reports.savePortlet(switchMechanism.model)
                    break;
                case 'get portlets':
                    switchMechanism.model.method = 'CALLBACK';
                    switchMechanism.model.data = response;
                    mGlobal.page.reports.currentPortlets = response;
                    dynamic.functions.reports.getPortlets(switchMechanism.model);
                    break;
                case 'get containers':
                    switchMechanism.model.method = 'CALLBACK';
                    switchMechanism.model.portlets = response;
                   dynamic.functions.reports.getContainer(switchMechanism.model);
                    break;
                case 'refresh portlets':
                    mGlobal.page.reports.currentPortlets = response;
                    break;
                case 'refresh containers':
                    mGlobal.page.reports.currentContainers = response;
                    break;
                case 'assign reports to container':
                    //Do Nothing
                   dynamic.functions.reports.refreshPortlet();
                    break;
                case 'remove report from container':
                    switchMechanism.model.method = 'CALLBACK';
                    switchMechanism.model.dbsuccess = response.dbsuccess;
                    switchMechanism.model.recordId = response.recordId;
                    dynamic.functions.reports.removeReportFromContainer(switchMechanism.model);
                    break;
                case 'get all used custom reports':
                    switchMechanism.model.method = 'CALLBACK';
                    switchMechanism.model.currentReports = response.data;
                    dynamic.functions.reports.getUsedReports(switchMechanism.model)
                    break;
                case 'get portlet privileges':
                    switchMechanism.model.method = 'CALLBACK';
                    switchMechanism.model.data = response;
                    dynamic.functions.reports.getPortletPrivileges(switchMechanism.model);
                    break;
                case 'set portlet privileges':
                    switchMechanism.model.method = 'CALLBACK';
                    switchMechanism.model.data = response;
                    dynamic.functions.reports.setPortletPrivileges(switchMechanism.model);
                    break;
                case 'get portlets ddl':
                    switchMechanism.model.method = 'CALLBACK';
                    switchMechanism.model.data = response;
                    dynamic.functions.reports.getPortletsDDL(switchMechanism.model);
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