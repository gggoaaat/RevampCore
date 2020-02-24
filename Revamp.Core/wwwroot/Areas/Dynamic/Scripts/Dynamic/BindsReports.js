window.addEventListener('resize', function () { dynamic.functions.reports.resizeReportWrapper(); }, true);

$('.trackChange').live('keyup change', function () {

    var _tr = $(this).closest('tr');

    switch ($(this).attr('name')) {
        case "containerName[]":
            _tr.attr('data-containername', $(this).val());
            break;
        case "containersSelectList[]":
            _tr.attr('data-containersize', $(this).val());
            break;
    }

})

$('.config').live('click', function () {

    var thisObject = $(this);

    dynamic.functions.reports.configurationLaunchLogic({ thisObject: thisObject });
});

$('.portletPriv').live('click', function () {

    var thisObject = $(this);

    dynamic.functions.reports.configurationLaunchLogic({ thisObject: thisObject });
});

$('#allPortletsPrivs').live('click', function () {

    dynamic.functions.reports.modalAllPortletsPrivs();
});

$('#createSectionRow').live('click', function () {
    dynamic.functions.reports.createSectionRow();
});

$('#editPage').live('click', function () {
    dynamic.functions.reports.toggleEdit('edit');
});

$('#savePage').live('click', function () {
    dynamic.functions.reports.toggleEdit('view');
});

$('#undoPage').live('click', function () {
    dynamic.functions.reports.toggleEdit('view');
});

$('#portletName').on('keyup', function () {
    $('.editThisSectionRow [name="portletTitle"]').html($(this).val());
});

$('#CommitConfigSave').live('click', function () {

    dynamic.functions.reports.savePortlet({
        method: 'POST'
        // data: { base_portlet_id: $('.editThisSectionRow').data('baseportletid') }
    });

});

$('#addContainer2Section').live('click', function () {
    $('#PortletContainers').append(dynamic.functions.reports.containerConfigOption());
});

$('#addReport2Container').live('click', function () {
    $('#ReportContainers tbody').append(dynamic.functions.reports.loadReport2Container());

    builder.loadCustomReportList(
           {
               method: 'POST',
               selector: '.dataNotLoaded[name="customBaseReportsSelectList[]"]',
               TypeOfQuery: 'Distinct Base Report'
           },
           {

           }
    );

});

$('[name="removeContainer"]').live('click', function () {
    $(this).closest('tr').remove();
});

$('#collapsePortlet').live('click', function () {
    if ($('#collapsePortlet').hasClass('expand')) {
        $('#collapsePortlet i').removeClass('fa-chevron-down');
        $('#collapsePortlet i').addClass('fa-chevron-up');
    }

    if ($('#collapsePortlet').hasClass('collapse')) {
        $('#collapsePortlet i').removeClass('fa-chevron-up');
        $('#collapsePortlet i').addClass('fa-chevron-down');
    }
});

$('#collapseFullscreen').live('click', function () {
    if ($('#collapseFullscreen').hasClass('on')) {
        $('#collapseFullscreen i').removeClass('fa-expand');
        $('#collapseFullscreen i').addClass('fa-compress');
    }
    else {
        $('#collapseFullscreen i').removeClass('fa-compress');
        $('#collapseFullscreen i').addClass('fa-expand');
    }
});

$('[name="customBaseReportsSelectList[]"]').live('change', function () {
    var iteration = $(this).data('iteration');

    var customreportid = $('[name="customBaseReportsSelectList[]"][data-iteration="' + iteration + '"] option:selected').data('customreportid');

    customreportid = customreportid == undefined ? -1 : customreportid;

    builder.loadCustomReportList({
        method: 'POST',
        customreportid: customreportid,
        selector: '[name="customReportsSelectList[]"][data-iteration="' + iteration + '"]'
    }, {});
});

$('#SaveContainerReports').on('click', function (i) {
    dynamic.functions.reports.saveReportToContainer({});
});

$('.reportLink').live('click', function (i) {

    var thisObject = $(this);

    dynamic.functions.reports.callLoadReport({
        // thisObject: thisObject,
        currentReport: thisObject.data('basereportname'),
        containerid: thisObject.closest('.portletContainer ').data('basecontainerid'),
        template: thisObject.data('templatename'),
        schema: thisObject.data('templatename'),
        prefix: 'thisDataTable',
        _customreportid: thisObject.data('customreportid'),
        _reporttitle: thisObject.data('reporttitle'),
        headertitle: thisObject.data('headertitle'),
        addToTab: true,
        addSidebar: true
    });
})

//binds the "minus sign" next to a displayed report to allow you to remove it from the container it's displayed on
$('.removeReport').live('click', function (i) {

    var thisObject = $(this);

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
                dynamic.functions.reports.removeReportFromContainer({ thisObject: thisObject, method: 'POST' }); //'post' is for the initial call, called method will accept callback to display final result
                $($(this).parents('.modal')).modal('hide');
            },
            selected: true
        }]
    });
})

$('select[name="customReportsSelectList[]"]').live('change', function () {
    dynamic.functions.reports.customReportSelectedChange(this);
})

$('.reDrawColumns').live('click', function (i) {
    var thisTable = $(this).data('tabcollection');

    window.setTimeout(
        function () {
            //table[thisTable].columns.adjust();
            // window.dispatchEvent(new Event('resize'))
            $('#' + thisTable).resize()
        }, 150)


});

$('#CommitPrivilegesSave').on('click', function () {

    dynamic.functions.reports.setPortletPrivileges({ method: 'POST' });

});

$('#ddlPrivPortlets').on('change', function () {
    dynamic.functions.reports.updateSelectedPrivPortlet({ method: 'POST' });
});

$('[name="ReportCollectionTabs"] .closeTab').live('click', function () {

    var thisData = $(this).data();
    
    if (table[thisData.table] != undefined) {
        table[thisData.table].destroy();
    }

    delete table[thisData.table];
    delete mGlobal.page[thisData.table];

    $('#' + thisData.contentwrapper).remove();
   
    var thisNavLi = $(this).parent('a').parent('li');

    if (thisNavLi.hasClass('active')) {

        thisNavLi.remove();

        $('.nav-tabs [href="#tab_collection"]').trigger('click');
    }
    else
    {
        thisNavLi.remove();
    }
});