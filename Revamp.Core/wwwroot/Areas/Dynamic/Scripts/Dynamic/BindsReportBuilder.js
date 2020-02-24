//For Report Builder
$('.day').live('click', function (event) {

    var tempValue = $(this).html();
    // table["this_DataTable"].ajax.reload();

});

$('#currentTemplates').on('change', function (e) {
    builder.loadCurrentTemplateBaseReports({});

    $('#DeleteReport').hide();
})

$('#currentReports').live('change', function () {
    builder.changedBaseReport({});

    $('#DeleteReport').hide();
});

$('#currentCustomReports').on('change', function () {
    builder.loadBuilderCustomReport({});
    
    if ($('#currentCustomReports').prop('selectedIndex') > 0) {
        $('#DeleteReport').show();
    }
    else {
        $('#DeleteReport').hide();
    }
});

$('[name="AddClause"]').live('click', function () {

    loadClauseSelect();
});

$('.removeClause').live('click', function () {

    $(this).parent('.clause').remove()

    if ($('.clause:eq(0) [name="preConditions[]"]').length > 0) {
        $('.clause:eq(0) [name="preConditions[]"]').remove();
        $('.clause:eq(0) [name="leftSideOfClause[]"]').css('width', '30%');
        $('.clause:eq(0) [name="rightSideOfClause[]"]').css('width', '50%');
    }
});

$('.dyno_filters').live('click', function () {

    $('#FilterModal').modal('show');
});


$('#RebuildDT').on('click', function () {

    builder.rebuild({});
});

$('#SaveReport').on('click', function () {

    DisplaySaveReportModal({});
});

$('#DeleteReport').on('click', function () {

    promptDialog.prompt({
        promptID: 'delete-custom-report',
        body: 'Are you sure you want to delete this custom report from the system?',
        header: 'Delete Custom Report',
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
                    
                    var baseId = $('#currentCustomReports option:selected').data('baseid');
                    var customReportId = $('#currentCustomReports option:selected').data('id');

                    var model = { method: 'POST', data: { baseId : baseId, customReportId: customReportId }, async: true };

                    deleteCustomReport(model);
                    $($(this).parents('.modal')).modal('hide');
                },
                selected: true
            }]
    });
});

$('#SaveRootReport').on('click', function () {

    builder.saveBaseReport();
});

$('[name="FilterManagerLink"]').on('click', function () {

    var thisObject = $(this).parent('li');

    //window.setTimeout(function () {
    //    var isItOpen = thisObject.hasClass('open')
    //    if (isItOpen) { thisObject.find('.sub-menu').show(); }
    //}, 150)
});

$('.btn-bind').on('click', TriggerCustomReportSaveModel);

$('.NavigationButton').on('click', function (e) {

    $('.NavigationButton').each(function (i) {
        var thisOwner = $('.NavigationButton:eq(' + i + ')').data('owner')

        $('#' + thisOwner).hide();
    });

    var thisOwner = $(this).data('owner');

    $('#' + thisOwner).show();
})

$('.ColumnAvailability input[type="checkbox"]').live('click', function () {

    $(this).is(':checked') ? $(this).closest('li').find('.form-control').attr('disabled', false) : $(this).closest('li').find('.form-control').attr('disabled', true);

});
