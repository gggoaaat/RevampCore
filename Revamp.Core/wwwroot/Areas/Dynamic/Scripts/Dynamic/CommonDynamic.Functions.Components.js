Date.prototype.isValid = function () {
    // An invalid date object returns NaN for getTime() and NaN is the only
    // object not strictly equal to itself.
    return this.getTime() === this.getTime();
};



function IrisPagerInit(pagerModel) {
    //pagerModel.tableId, pagerModel.divID, pagerModel.currentJsonData, pagerModel.params, pagerModel.recordType, pagerModel.NavExtension
    pagerModel.recordType = pagerModel.recordType == '' || pagerModel.recordType == undefined || pagerModel.recordType == null ? 'rows' : pagerModel.recordType;
    var id = pagerModel.divID.substring(1, pagerModel.divID.length);
    var total = pagerModel.currentJsonData.recordsFiltered;
    var stop = pagerModel.params.start + pagerModel.currentJsonData.data.length;
    var start = stop > 0 ? (pagerModel.params.start + 1) : 0;
    var isFirstPage = pagerModel.params.start === 0 ? true : false;
    //var isLastPage = pagerModel.params.start + pagerModel.params.length >= pagerModel.currentJsonData.recordsTotal + 1 ? true : false;
    var isLastPage = pagerModel.params.start + pagerModel.params.length >= total + 1 ? true : false;
    var isMoreThanOnePage = isFirstPage && isLastPage || pagerModel.params.length < 0 ? false : true;
    var btnFirst = '';
    var btnPrev = '';
    var btnNext = '';
    var btnLast = '';
    var disPageSelect = '';
    var disabled = 'disabled="disabled"';
    var showPageSelectOption = true;
    var btnStatus = '';

    if (isFirstPage && isMoreThanOnePage) {
        //disable first and prev
        btnFirst = disabled;
        btnPrev = disabled;
        btnNext = '';
        btnLast = '';
    }
    else if (isLastPage && isMoreThanOnePage) {
        //disable last and next
        btnFirst = '';
        btnPrev = '';
        btnNext = disabled;
        btnLast = disabled;
    }
    else if (!isMoreThanOnePage) {
        //disable all
        btnFirst = disabled;
        btnPrev = disabled;
        btnNext = disabled;
        btnLast = disabled;
        disPageSelect = disabled;
        showPageSelectOption = false;
    }
    else if (!isFirstPage && !isLastPage && isMoreThanOnePage) {
        //disable none
        btnFirst = '';
        btnPrev = '';
        btnNext = '';
        btnLast = '';
    }

    var pageTotal = Math.ceil(pagerModel.currentJsonData.recordsFiltered / table[pagerModel.tableId].ajax.params().length);
    var currentPageSelected = (table[pagerModel.tableId].ajax.params().start + table[pagerModel.tableId].ajax.params().length) / table[pagerModel.tableId].ajax.params().length;
    var pageHtml = '';

    if (showPageSelectOption) {
        for (i = 1; i < pageTotal + 1; i++) {
            if (currentPageSelected == i) {
                pageHtml += '<option value="' + (i) + '" selected>' + (i) + '</option>';
            }
            else {
                pageHtml += '<option value="' + (i) + '">' + (i) + '</option>';
            }
        }
    }

    var btnGoto = '';

    if (btnStatus == '' || btnStatus == undefined || btnStatus == null)
        btnGoto = '<select data-owner="' + id + '" data-cmd="goto" name="' + id + '_pageSelect" aria-controls="' + id + '" class="btn btn-default iris-pager-nav-select" title="Current Page" style="border-right:1px!important;display: inline-block !important; height: 34px !important; width: 70px !important; border: 1px solid rgb(229, 229, 229) !important;" ' + disPageSelect + '>' + pageHtml + '</select>';
    else {

        btnGoto = '<select data-owner="' + id + '" data-cmd="goto" name="' + id + '_pageSelect" aria-controls="' + id + '" class="btn btn-default iris-pager-nav-select" title="Current Page" style="border-right:1px!important;display: inline-block !important; height: 34px !important; width: 70px !important; border: 1px solid rgb(229, 229, 229) !important;"' + disPageSelect + '>' + pageHtml + '</select>';
    }

    var navHtml =
        '<a type="button" data-owner="' + id + '" data-cmd="first" name="btnPageFirst" id="' + id + '_btnPageFirst" class="btn btn-default iris-pager-nav" style="border-left: 0px; border-right: 1px solid #e5e5e5; border-top: 1px solid #e5e5e5; height: 34px !important; border-bottom: 1px solid #e5e5e5;display: inline-block !important" title="First Page" ' + btnFirst + '>' +
        '<i class="fa fa-fast-backward"></i>' +
        '</a>' +
        '<a type="button" data-owner="' + id + '" data-cmd="previous" name="btnPagePrev" id="' + id + '_btnPagePrev" class="btn btn-default iris-pager-nav" style="border:1px solid #e5e5e5;display: inline-block !important; height: 34px !important;" title="Previous Page" ' + btnPrev + '>' +
        '<i class="fa fa-backward"></i>' +
        '</a>' +
        '<a type="button" data-owner="' + id + '" data-cmd="next" name="btnPageNext" id="' + id + '_btnPageNext" class="btn btn-default iris-pager-nav" style="border:1px solid #e5e5e5;display: inline-block !important; height: 34px !important;" title="Next Page" ' + btnNext + '>' +
        '<i class="fa fa-forward"></i>' +
        '</a>' +
        '<a type="button" data-owner="' + id + '" data-cmd="last" name="btnPageLast" id="' + id + '_btnPageLast" class="btn btn-default iris-pager-nav" style="border-right: 0px; border-left: 1px solid #e5e5e5; border-top: 1px solid #e5e5e5; border-bottom: 1px solid #e5e5e5; display: inline-block !important; height: 34px !important;" title="Last Page" ' + btnLast + '>' +
        '<i class="fa fa-fast-forward"></i>' +
        '</a>' +
        btnGoto;

    var ExtensionHTML = '';

    if (pagerModel.NavExtension != undefined) {
        for (var i = 0; i < pagerModel.NavExtension.length; i++) {
            if (pagerModel.NavExtension[i].HTML != undefined) {
                ExtensionHTML += pagerModel.NavExtension[i].HTML(id);
            }
        }
    }

    $(pagerModel.divID + '_wrapper .iris-pager').remove();

    $(pagerModel.divID + '_wrapper .row:eq(0)').children().prop('class', 'col-sm-6');
    $(pagerModel.divID + '_wrapper .row:eq(0) .col-sm-6:eq(0)').append(
        '<div id="' + id + '_pager" class="iris-pager">' +
        '<div class="btn-group iris-pager" style="padding-bottom: 3px; padding-left: 0px; padding-right: 0px; display: inline-block !important">' +
        navHtml +
        ExtensionHTML +
        '</div>' +
        '<div class="btn btn-default dataTables_info" name="PageFirst" id="' + id + '_pagerSummary" role="status" aria-live="polite" style="display: inline-block !important; padding-left: 10px; padding-top: 5.5px; height: 34px; float: none !important; border: 0;" disabled>' +
        //'Showing ' + start + ' to ' + stop + ' of ' + total + ' ' + pagerModel.recordType +
        start + ' to ' + stop + ' of ' + total +
        '</div>' +
        '</div>');

    if (pagerModel.NavExtension != undefined) {
        for (var i = 0; i < pagerModel.NavExtension.length; i++) {
            if (pagerModel.NavExtension[i].FUNCTION != undefined) {
                pagerModel.NavExtension[i].FUNCTION(id);
            }
        }
    }

    if (pagerModel.NavExtension != undefined) {
        for (var i = 0; i < pagerModel.NavExtension.length; i++) {
            if (pagerModel.NavExtension[i].BIND != undefined) {
                pagerModel.NavExtension[i].BIND(id);
            }
        }
    }

    if (pagerModel.NavExtension != undefined) {
        for (var i = 0; i < pagerModel.NavExtension.length; i++) {
            if (pagerModel.NavExtension[i].CSS != undefined) {
                pagerModel.NavExtension[i].CSS(id);
            }
        }
    }

    $(pagerModel.divID + '_wrapper .col-sm-6:eq(0) .form-control.input-sm').addClass('iris-pager-ddl');

    $(pagerModel.divID + '_info').parent().parent().hide();
}

$('.iris-pager-nav').live('click', function () {

    var btnSelected = $(this).attr('name');
    var ownerId = $(this).data('owner');
    var currentCommand = $(this).data('cmd');

    if ($($('body').find('#' + ownerId + '_processing')).length > 0)
        $($('body').find('#' + ownerId + '_processing')).remove();

    switch (currentCommand) {

        case 'snap_left':

            $('#' + ownerId).parent().scrollLeft(-$('#' + ownerId).css('width').replace('px', ''));

            break;

        case 'snap_right':

            $('#' + ownerId).parent().scrollLeft(+$('#' + ownerId).css('width').replace('px', ''));

            break;

        default:

            irisLoadingPanel.show({
                id: ownerId,
                parentDiv: $($('#' + ownerId).parent().parent())
            });

            table[ownerId].page(currentCommand).draw('page');

            break;
    }

    //if(typeof mGlobal !== 'undefined'){

    //    if (mGlobal.application.template == 'TIMMS2') {

    //        switch(ownerId){

    //            case 'documents':
    //            case 'paragraphs':
    //            case 'paragraphHistory':
    //            case 'details':
    //            case 'detailHistory':

    //                irisLoadingPanel.show({
    //                    id: ownerId,
    //                    parentDiv: $($('#' + ownerId).parent().parent())
    //                })

    //                table[ownerId].page(currentCommand).draw('page');

    //                break;

    //            default:
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        //IrisEllipse_Show();

    //        switch(currentCommand){

    //            case 'snap_left':

    //                //$('.dataTables_scrollBody.scrollbar').scrollLeft(-5000);
    //                $('#' + ownerId).parent().scrollLeft(-5000);

    //                break;

    //            case 'snap_right':

    //                //$('.dataTables_scrollBody.scrollbar').scrollLeft(5000);
    //                $('#' + ownerId).parent().scrollLeft(5000);

    //                break;

    //            default:

    //                irisLoadingPanel.show({
    //                    id: ownerId,
    //                    parentDiv: $($('#' + ownerId).parent().parent())
    //                })

    //                table[ownerId].page(currentCommand).draw('page');

    //                break;
    //        }


    //    }
    //}
    //else {
    //    //IrisEllipse_Show();

    //    irisLoadingPanel.show({
    //        id: ownerId,
    //        parentDiv: $($('#' + ownerId).parent().parent())
    //    })

    //    table[ownerId].page(currentCommand).draw('page');
    //}
});

$('.iris-pager-nav-select').live('change', function () {

    var ownerId = $(this).data('owner');

    if ($($('body').find('#' + ownerId + '_processing')).length > 0)
        $($('body').find('#' + ownerId + '_processing')).remove();

    if (typeof mGlobal !== 'undefined') {
        table[ownerId].page(+$(this).val() - 1).draw('page');

        //mGlobal.page[ownerId].template == 'TIMMS2'?
        //if (mGlobal.application.template == 'TIMMS2') {

        //    switch (ownerId) {

        //        case 'documents':

        //            table[ownerId].page(+$(this).val() - 1).draw('page');

        //            if (table.paragraphs != undefined) {
        //                table.paragraphs.ajax.reload(null, false);
        //            }

        //            if (table.details != undefined) {
        //                table.details.ajax.reload(null, false);
        //            }

        //            break;

        //        case 'paragraphs':

        //            table[ownerId].page(+$(this).val() - 1).draw('page');

        //            if (table.details != undefined) {
        //                table.details.ajax.reload(null, false);
        //            }

        //            break;

        //        case 'details':

        //        default:

        //            table[ownerId].page(+$(this).val() - 1).draw('page');

        //            break;
        //    }
        //}
        //else {
        //    table[ownerId].page(+$(this).val() - 1).draw('page');
        //}

    }
    else {
        //IrisEllipse_Show();
        irisLoadingPanel.show({
            id: ownerId,
            parentDiv: $($('#' + ownerId).parent().parent())
        });
        table[ownerId].page(+$(this).val() - 1).draw('page');
    }

    irisLoadingPanel.show({
        id: ownerId,
        parentDiv: $($('#' + ownerId).parent().parent())
    });
});

$('.dataTables_length').live('change', function () {

    if ($('body').find('.iris-pager').length > 0) {

        var ownerId = $(this).attr('id').replace('_length', '');

        if ($($('body').find('#' + ownerId + '_processing')).length > 0)
            $($('body').find('#' + ownerId + '_processing')).remove();

        //if (typeof mGlobal !== 'undefined') {
        //    if (mGlobal.application.template == 'TIMMS2') {
        //        irisLoadingPanel.show({
        //            id: ownerId,
        //            parentDiv: $($('#' + ownerId).parent().parent())
        //        })
        //    }
        //}
        //else {
        //    IrisEllipse_Show();
        //}
        irisLoadingPanel.show({
            id: ownerId,
            parentDiv: $($('#' + ownerId).parent().parent())
        });
    }
});


var irisLoadingPanel = function IrisLoadingPanel() {

    var name = 'iris-lp';

    //var getname = function(){
    //    return name;
    //}

    var _html = function (model) {
        get_html(model);
    };

    function get_html(model) {
        var id = name + '-' + model.id;
        var ___html = '';
        if ($('.dataTables_scroll .dataTables_scrollBody table#' + model.id).length > 0) {
            var h1 = $($('.dataTables_scroll .dataTables_scrollBody table#' + model.id).parent().parent()).css('height').replace('px', '');
            var h2 = .4 * h1;
            ___html += '<div id="' + id + '" class="modal-backdrop in" style="display: none; z-index: 999999; position: absolute; bottom: 10px; margin-bottom: 0px; left: 15px; right: 15px;">';
            ___html += '<div id="circleG" style="margin-top: ' + h2 + 'px">';
            ___html += '<div id="circleG_1" class="circleG"></div>';
            ___html += '<div id="circleG_2" class="circleG"></div>';
            ___html += '<div id="circleG_3" class="circleG"></div>';
            ___html += '</div>';
            ___html += '</div>';
        }
        else {
            ___html += '<div id="' + id + '" class="modal-backdrop in" style="display: none; z-index: 999999; position: absolute; margin-bottom: 0px; left: 45px;">';
            ___html += '<div id="circleG">';
            ___html += '<div id="circleG_1" class="circleG"></div>';
            ___html += '<div id="circleG_2" class="circleG"></div>';
            ___html += '<div id="circleG_3" class="circleG"></div>';
            ___html += '</div>';
            ___html += '</div>';
        }
        return ___html;
    }

    //function returnHTMLwoModel (model){
    //    var id = model.id + '_' + name;
    //    var ___html = '';
    //    if ($('.dataTables_scroll').length > 0) {
    //        var h1 = $($('.dataTables_scroll .dataTables_scrollBody table#' + model.id).parent().parent()).css('height').replace('px','');
    //        var h2 = .4 * h1;
    //        _//__html += '<div id="' + id + '" class="modal-backdrop in" style="display: none; z-index: 999999; position: absolute; bottom: 10px; margin-bottom: 0px; left: 15px; right: 15px;">';
    //        ___html +=     '<div id="circleG" style="margin-top: ' + h2 + 'px">';
    //        ___html +=         '<div id="circleG_1" class="circleG"></div>';
    //        ___html +=         '<div id="circleG_2" class="circleG"></div>';
    //        ___html +=         '<div id="circleG_3" class="circleG"></div>';
    //        ___html +=     '</div>';
    //        //___html += '</div>';
    //    }
    //    else {
    //        //  ___html += '<div id="' + id + '" class="modal-backdrop in" style="display: none; z-index: 999999; position: absolute; margin-bottom: 0px; left: 45px;">';
    //        ___html +=     '<div id="circleG">';
    //        ___html +=         '<div id="circleG_1" class="circleG"></div>';
    //        ___html +=         '<div id="circleG_2" class="circleG"></div>';
    //        ___html +=         '<div id="circleG_3" class="circleG"></div>';
    //        ___html +=     '</div>';
    //        // ___html += '</div>';
    //    }
    //    return ___html;
    //};

    var _show = function (model) {
        var id = '#' + name + '-' + model.id;

        if ($(id).length > 0 && ($(id).css('display') == 'none') && $('.dataTables_scroll .dataTables_scrollBody table#' + model.id).length > 0) {
            var height = $($('.dataTables_scroll .dataTables_scrollBody table#' + model.id).parent().parent()).css('height').replace('px', '') * .4;
            $(id).css('opacity', 0);
            $(id + ' #circleG').css('margin-top', height + 'px');
            $(id).show();
            irisLoadingPanel.unfade($(id));
        }
        else {
            irisLoadingPanel.add(model);
            $(id).css('opacity', 0);
            $(id).show();
            irisLoadingPanel.unfade($(id));
        }
    };
    var _hide = function (model) {
        var id = '#' + name + '-' + model.id;

        if ($(id).length > 0) {
            $(id).hide();
            $(id).css('opacity', 0);
        }
    };
    var _unfade = function (element) {

        var op = 0.1;

        var timer = setInterval(function () {

            if (op < .5) {

                flag = true;
                element.css('opacity', op);
                element.css('filter', 'alpha(opacity=' + op * 100 + ')');
                op += op * 0.1;

                if (op >= .5)
                    clearInterval(timer);
            }
        }, 100);
    };
    var _add = function (model) {
        if (model != undefined) {
            $(model.parentDiv).append(get_html(model));
        }
        else {
            $('body').append(get_html(model));
        }
    };

    return {
        html: function (model) {
            _html(model);
        },
        show: function (model) {
            _show(model);
        },
        hide: function (model) {
            _hide(model);
        },
        unfade: function (model) {
            _unfade(model);
        },
        add: function (model) {
            _add(model);
        }
        //overRideDT: function(model)
        //{
        //    $('#' + model.id + '_processing').addClass('tempClass')
        //    $('#' + model.id + '_processing').html(returnHTMLwoModel(model));
        //}
    };
}();

var irisMultiselect = function IrisMultiselect() {
    var name = 'iris-ms';
    var selection = '';
    function load_html(model) {
        var html = '';
        html += '       <div class="dropdown-container">';
        html += '           <div class="dropdown-resize">';
        html += '               <div class="dropdown-button noselect">';
        html += '                   <div class="dropdown-label">' + model.name.toUpperCase() + '</div>';
        html += '                   <div class="dropdown-quantity">(<span class="quantity">0</span>)</div>';
        html += '                   <i class="fa fa-filter"></i>';
        html += '               </div>';
        html += '               <div class="dropdown-list">';
        html += '                   <input type="search" placeholder="Search..." class="dropdown-search search-box" name="focus" required>';
        html += '                   <ul class="ul_selection">';
        html += '                       <li>';
        html += '                           <span>';
        html += '                           <input id="select_all" type="checkbox" />';
        html += '                           </span>';
        html += '                           <label>(Select All)</label>';
        html += '                       </li>';
        html += '                   </ul>';
        html += '               </div>';
        html += '           </div>';
        html += '       </div>';
        $(model.id).html(html);
    }
    function load_css(model) {
        var float = model.float;
        var width = model.width;
        var min_width = model.min_width;
        var max_width = model.max_width;
        var height = model.height;
        var min_height = model.min_height;
        var max_height = model.max_height;
        var ul_height = model.height - 50;

        width += 'px';
        min_width += 'px';
        max_width += 'px';
        height += 'px';
        min_height += 'px';
        max_height += 'px';
        ul_height += 'px';

        $(model.id + ' .dropdown-container').css('float', float);
        $(model.id + ' .dropdown-container').css('margin-top', '0px');
        $(model.id + ' .dropdown-button').css('width', width);
        $(model.id + ' .dropdown-list').css('height', height);
        $(model.id + ' .dropdown-list').css('width', width);
        $(model.id + ' .dropdown-list').css('min-height', min_height);
        $(model.id + ' .dropdown-list').css('min-width', min_width);
        $(model.id + ' .dropdown-list').css('max-height', max_height);
        $(model.id + ' .dropdown-list').css('max-width', max_width);
        $(model.id + ' .ul_selection').css('max-height', ul_height);
        $(model.id + ' input[type="search"]').css('border-radius', '0px');

        if (!model.isExpanded) {
            if (model.isExpanded) {
                $(model.id + ' .dropdown-list').css('display', 'block');
            }
            else {
                $(model.id + ' .dropdown-list').css('display', 'none');
            }
        }

        if (model.showNumSelected) {
            $(model.id + ' .dropdown-quantity').css('display', 'block');
        }
        else {
            $(model.id + ' .dropdown-quantity').css('display', 'none');
        }

        if (model.isResizable) {
            $(model.id + ' .dropdown-list').resizable();
        }
    }
    function bind(model) {
        $(model.id + ' .dropdown-list').resizable();

        $(model.id + ' #select_all').live('click', function () {
            var count = 0;
            if ($(model.id + ' #select_all').parent().hasClass('checked')) {
                $(model.id + ' #select_all').parent().removeClass('checked');
                $(model.id + ' .li_select [type="checkbox"]').attr('checked', false);
                $(model.id + ' .quantity').text(count.toString());
                selected = '';
            }
            else {
                $(model.id + ' #select_all').parent().addClass('checked');
                if ($(model.id + ' .dropdown-search').val().length == 0) {
                    $(model.id + ' .li_select [type="checkbox"]').attr('checked', true);
                    $(model.id + ' .quantity').text($(model.id + ' .li_select').length);
                }
                else {
                    var li_filtered = $(model.id + ' .li_select').filter(function () { return $(this).css('display') != 'none'; });
                    var li_unfiltered = $(model.id + ' .li_select').filter(function () { return $(this).css('display') == 'none'; });

                    for (i = 0; i < li_filtered.length; i++) {
                        $($(li_filtered[i]).children()[0]).attr('checked', true);
                        count++;
                    }

                    for (i = 0; i < li_unfiltered.length; i++) {
                        $($(li_unfiltered[i]).children()[0]).attr('checked', false);
                    }

                    $(model.id + ' .quantity').text(count.toString());
                }
            }

            irisMultiselect.set_selection(model);
            if (model.ownerId != undefined) {
                table[model.ownerId].ajax.reload();
            }
        });

        $(model.id + ' .dropdown-button').live('click', function () {
            $(model.id + ' .dropdown-list').toggle();
            model.isExpanded = model.isExpanded ? false : true;
            load_css(model);
        });

        $(model.id + ' .dropdown-search').live('input', function () {
            var target = $(this);
            var search = target.val().toLowerCase();

            if (!search) {
                $(model.id + ' .li_select').show();
                $(model.id + ' .li_select input').attr('checked', false);
                $(model.id + ' .ul_selection li:eq(0) label').html('(Select All)');
                $(model.id + ' #select_all').attr('checked', false);
                $(model.id + ' #select_all').parent().removeClass('checked');
                $(model.id + ' .quantity').text('0');
                irisMultiselect.set_selection(model);
                if (model.ownerId != undefined) {
                    table[model.ownerId].ajax.reload();
                }
                return false;
            }

            $(model.id + ' .li_select').each(function () {
                var text = $(this).text().toLowerCase();
                var match = text.indexOf(search) > -1;
                $(this).toggle(match);
            });

            $(model.id + ' .ul_selection li:eq(0) label').html('(Select Filtered)');
        });

        $(model.id + ' .li_select [type="checkbox"]').live('change', function () {
            var numChecked = $(model.id + ' .li_select [type="checkbox"]:checked').length;
            var numTotal = $(model.id + ' .li_select').length;

            if (numChecked != numTotal) {
                $(model.id + ' #select_all').parent().removeClass('checked');
                $(model.id + ' #select_all').attr('checked', false);
            }
            else {
                $(model.id + ' #select_all').parent().addClass('checked');
                $(model.id + ' #select_all').attr('checked', true);
            }

            $(model.id + ' .quantity').text(numChecked || '0');

            irisMultiselect.set_selection(model);

            if (model.ownerId != undefined) {
                table[model.ownerId].ajax.reload();
            }
        });
    }
    var _set_selection = function (model) {
        var id = '#' + name + '-' + model.name;
        selection = '';

        $(id + ' .ul_selection .li_select input[type="checkbox"]:checked').each(function () {
            if (typeof $(this).attr('name') !== 'undefined') {
                selection += $(this).attr('name') + ',';
            }
        });

        if (selection.length > 0) {
            selection = selection.substring(0, selection.length - 1);
        }
    };
    function parse_json(model) {
        var list = [];

        for (var i in model.jsonResult) {

            var obj = model.jsonResult[i];
            var key = obj[Object.keys(obj)[model.key]];
            var value = '';

            for (var j = 0; j < model.value.length; j++) {

                if (model.delimiter.length > 0) {
                    value += model.delimiter + obj[Object.keys(obj)[model.value[j]]];
                }
                else {
                    value += obj[Object.keys(obj)[model.value[j]]];
                }
            }

            if (model.hideKey != undefined && model.hideKey) {

                value = model.delimiter.length > 0 ? value.substr(1, value.length - 1) : value;
            }
            else {

                value = model.delimiter.length > 0 ? key + value : value;
            }

            list.push({ key: key, value: value });
        }

        var listTemplate = _.template(
            '<li class="li_select">' +
            '<input name="<%= key %>" type="checkbox" />' +
            '<label for="<%= key %>"><%= value %></label>' +
            '</li>'
        );

        _.each(list, function (s) {
            s.value = s.value.toUpperCase();
            $(model.id + ' .ul_selection').append(listTemplate(s));
        });
    }
    var _init = function (model) {
        model.id = '#' + name + '-' + model.name;
        model.min_height = model.height;
        model.max_height = model.height;
        model.min_width = model.min_width - 50;
        model.max_width = model.min_width + 50;
        model.selected = '';

        load_html(model);
        load_css(model);
        bind(model);
        parse_json(model);
    };
    return {
        init: function (model) {
            _init(model);
        },
        get_selection: function () {
            return selection;
        },
        set_selection: function (model) {
            _set_selection(model);
        }
    };
}();

irisTableHeightSelect = function IrisTableHeightSelect() {

    var name = 'iris-hs';
    var __html = '';
    var height = 0;
    var max_height = 600;
    var min_height = 200;

    function exists(element) {

        if (element.id != null) {

            if (element.id != undefined) {

                if (element.id.length > 0) {

                    return true;
                }
            }
        }

        return false;
    }

    function _html(model) {

        var id = model.id.substring(1, model.id.length);
        var localStorage = commonDynamic.functions.tools.getSetLocalStorage({ GetorSet: 'get', property: id });
        var currentHeight = +$('#' + model.tableName).css('height').replace('px', '');

        __html = '';
        __html += '<div class="btn-group" style="display: inline-block !important">';
        __html += '<button type="button" class="btn btn-default btn-secondary dropdown-toggle"';
        __html += 'data-toggle="dropdown" data-hover="dropdown" data-delay="1000" data-close-others="true" data-owner="' + model.tableName + '" id="' + model.tableName + '_tableHeight" title="Table Height" name="' + model.tableName + '_TableHeight">';

        if (exists({ id: localStorage }) && currentHeight <= max_height) {

            if (+localStorage == 0) {
                __html += '<span>Hidden</span>' + ' ';
            }
            else {
                __html += '<span>' + parseFloat(Math.round(+localStorage * 100).toFixed(2)) + '%' + '</span>' + ' ';
            }
        }
        else {

            if (exists({ id: model.defaultValue.toString() })) {

                if (model.value[model.defaultValue] == 0) {
                    __html += '<span>Hidden</span>' + ' ';
                }
                else {
                    __html += '<span>' + parseFloat(Math.round(model.value[model.defaultValue] * 100).toFixed(2)) + '%' + '</span>' + ' ';
                }
            }
            else {

                if (model.value[model.value.length - 1] == 0) {
                    __html += '<span>Hidden</span>' + ' ';
                }
                else {
                    __html += '<span>' + parseFloat(Math.round(model.value[model.value.length - 1] * 100).toFixed(2)) + '%' + '</span>' + ' ';
                }
            }
        }

        __html += '<i class="fa fa-angle-down" style="float: right; padding-top: 2px"></i>';
        __html += '</button>';
        __html += '<ul class="dropdown-menu pull-right" role="menu" style="width:' + model.width + '">';

        if (model.sort == 'a') {

            for (var i = 0; i < model.value.length; i++) {

                if (model.value[i] == 0) {
                    __html += '<li><a href="javascript:;" class="dropdown-item ' + id + '" data-size="' + model.value[i] + '">Hide</a></li>';
                }
                else {
                    __html += '<li><a href="javascript:;" class="dropdown-item ' + id + '" data-size="' + model.value[i] + '">' + parseFloat(Math.round(model.value[i] * 100).toFixed(2)) + '%' + '</a></li>';
                }

            }
        }

        if (model.sort == 'd') {

            for (var i = model.value.length - 1; i >= 0; i--) {

                if (model.value[i] == 0) {
                    __html += '<li><a href="javascript:;" class="dropdown-item ' + id + '" data-size="' + model.value[i] + '">Hide</a></li>';
                }
                else {
                    __html += '<li><a href="javascript:;" class="dropdown-item ' + id + '" data-size="' + model.value[i] + '">' + parseFloat(Math.round(model.value[i] * 100).toFixed(2)) + '%' + '</a></li>';
                }
            }
        }

        __html += '</ul>';
        __html += '</div>';

        $('#' + model.tableName + '_pager .btn-group.iris-pager').append(irisTableHeightSelect.get_html());
    }

    function _css(model) {

        $('#' + model.tableName + '_tableHeight').css('padding-left', '5px');
        $('#' + model.tableName + '_tableHeight').css('padding-right', '5px');
        $('#' + model.tableName + '_tableHeight').css('width', '70px');
        $('#' + model.tableName + '_tableHeight').css('height', '34px');
        $('#' + model.tableName + '_tableHeight').css('border', '1px solid #e5e5e5');
        $('#' + model.tableName + '_tableHeight').css('text-align', 'left');
        $('#' + model.tableName + '_tableHeight').css('display', 'inline-block !important');
        $('#' + model.tableName + '_tableHeight').css('color', '#aaaaaa');
    }

    function _adjust(model) {

        height = (height + 14).toString() + 'px';

        $('#' + model.tableName + '_wrapper .dataTables_scrollBody.scrollbar').css('max-height', height);
        $('#' + model.tableName + '_wrapper .dataTables_scrollBody.scrollbar').css('height', height);
        $('#' + model.tableName + '_wrapper .DTFC_LeftBodyWrapper').css('max-height', height);
        $('#' + model.tableName + '_wrapper .DTFC_LeftBodyWrapper').css('height', height);
        $('#' + model.tableName + '_wrapper .DTFC_LeftBodyLiner').css('max-height', height);
        $('#' + model.tableName + '_wrapper .DTFC_LeftBodyLiner').css('height', height);
    }

    function _bind(model) {

        var id = model.id.substring(1, model.id.length);

        $('.' + id).live('click', function (e) {

            height = 0;
            $('#' + model.tableName).parents('.dataTables_scroll').show();
            var table_height = +$('#' + model.tableName).css('height').replace('px', '');
            var selected = $(this).data('size');
            var page_height = +$('.page-content').css('height').replace('px', '');

            if (selected == 0) {
                $('#' + model.tableName).parents('.dataTables_scroll').hide();
            }
            else if (table_height > max_height || table_height > page_height) {

                //set default
                height = selected * max_height;
                height = height >= min_height ? height : min_height;
            }
            else if (table_height <= max_height && table_height <= page_height && table_height >= min_height) {

                //set selected
                height = selected * max_height;
                height = height >= min_height ? height : min_height;
            }
            else if (table_height < min_height) {

                //set table height
                height = table_height + 100;
            }
            else {
                height = 36;
            }

            commonDynamic.functions.tools.getSetLocalStorage({ GetorSet: 'set', property: id, value: selected });
            if (selected == 0) {
                $('#' + model.tableName + '_tableHeight span').text('Hidden');
            }
            else {
                $('#' + model.tableName + '_tableHeight span').text(parseFloat(Math.round(selected * 100).toFixed(2)) + '%');
            }
            irisTableHeightSelect.adjust(model);
            if (table[model.tableName]) { table[model.tableName].columns.adjust(); };
        });
    }

    var _init = function (model) {

        model.id = '#' + name + '-' + model.tableName.toLowerCase();
        max_height = model.max_height != undefined ? model.max_height : max_height;

        irisTableHeightSelect.html(model);
        irisTableHeightSelect.css(model);
        irisTableHeightSelect.bind(model);

        var choiceFirst = model.value[model.value.length - 1];
        var choiceSecond = model.value[model.value.length - 2];
        var choiceThird = model.value[model.value.length - 3];
        var defaultHeight = model.defaultValue != undefined ? model.value[model.defaultValue] : choiceFirst != undefined ? choiceFirst : choiceSecond != undefined ? choiceSecond : choiceThird;
        var id = irisTableHeightSelect.get_name() + '-' + model.tableName.toLowerCase();
        var value = commonDynamic.functions.tools.getSetLocalStorage({ GetorSet: 'get', property: id });

        if (value == null) {

            commonDynamic.functions.tools.getSetLocalStorage({ GetorSet: 'set', property: id, value: defaultHeight });
            id = '.' + id + '[data-size="' + defaultHeight + '"]';
        }
        else {

            id = '.' + id + '[data-size="' + value + '"]';
        }

        if ($(id).length > 0) {

            $(id).trigger('click');
        }
    };

    return {
        init: function (model) {
            _init(model);
        },
        html: function (model) {
            _html(model);
        },
        css: function (model) {
            _css(model);
        },
        bind: function (model) {
            _bind(model);
        },
        get_html: function () {
            return __html;
        },
        get_name: function () {
            return name;
        },
        get_height: function () {
            return height;
        },
        adjust: function (model) {
            _adjust(model);
        }
    };
}();

irisExport2Excel = function IrisExport2Excel() {

    var name = 'iris-e2e';
    var __html = '';

    function _html(model) {

        __html = '';
        __html += '<a type="button" href="#" name="' + model.tableName + '_exportBtn" data-source="' + model.name + '" class="btn btn-default" title="Export ' + model.name + '">';
        __html += '<i class="fa fa-file-excel-o fa-lg"></i>';
        __html += '</a>';

        $('#' + model.tableName + '_pager .btn-group.iris-pager').append(irisExport2Excel.get_html());
    }

    function _css(model) {
        $('[name="' + model.tableName + '_exportBtn"]').css('width', '41px');
        $('[name="' + model.tableName + '_exportBtn"]').css('height', '34px');
        $('[name="' + model.tableName + '_exportBtn"]').css('border', '1px solid #e5e5e5');
        $('[name="' + model.tableName + '_exportBtn"]').css('display', 'inline-block !important');
    }

    function _bind(model) {
        $('[name="' + model.tableName + '_exportBtn"]').live('click', (function (e) {
            $('#' + model.inputId[0]).val(model.name);
            $('#' + model.inputId[1]).submit();
        }));
    }

    var _init = function (model) {
        model.id = '#' + name + '-' + model.tableName.toLowerCase();
        irisExport2Excel.html(model);
        irisExport2Excel.css(model);
        irisExport2Excel.bind(model);
    };

    return {
        init: function (model) {
            _init(model);
        },
        html: function (model) {
            _html(model);
        },
        css: function (model) {
            _css(model);
        },
        bind: function (model) {
            _bind(model);
        },
        get_html: function () {
            return __html;
        }
    };
}();

irisTableZoom = function IrisTableZoom() {

    var name = 'iris-tz';
    var __html = '';

    function _html(model) {

        __html = '';
        __html += '<div class="btn-group">';
        __html += '<a type="button" class="btn btn-default" href="javascript:;" name="' + model.tableName + '_zoomBtn" data-toggle="dropdown" data-hover="dropdown" title="Zoom In/Out on this Datatable.">';
        //            __html += '<i class="icon-eyeglasses"></i>';
        __html += '<i class="mdi mdi-magnify-plus-outline"></i>';
        __html += '</a>';
        __html += '<ul class="dropdown-menu pull-right" id="ActionsUL' + model.tableName + '">';
        __html += '<li class="ResizeBuilder">';
        __html += '<div style="float: left; width: 84%;"><div class="slider slider-basic bg-contextualClassification" data-owner="' + model.tableName + '"></div></div>';
        __html += '</li>';
        //__html += '<div class="btn-group" style="display: inline-block !important">';
        __html += '<div class="btn-group" style="">';
        __html += '<a type="button" class="btn btn-default btn-xs" style="margin-top: 7px; float: right; border: 0"';
        __html += 'data-owner="' + model.tableName + '" id="' + model.tableName + '_resetZoom" title="Reset">';
        __html += '<i class="fa fa-refresh"></i>';
        __html += '</a>';
        __html += '</div>';
        __html += '</ul>';
        __html += '</div>';

        $('#' + model.tableName + '_pager .btn-group.iris-pager').append(irisTableZoom.get_html());
    }

    function _css(model) {
        $('.btn-default > i[class^="icon-"], .btn-default > i[class*="icon-"]').css('color', '#aaaaaa !important');
        $('[name="' + model.tableName + '_zoomBtn"]').css('width', '41px !important');
        $('[name="' + model.tableName + '_zoomBtn"]').css('height', '34px !important');
        $('.btn.btn-default[name="' + model.tableName + '_zoomBtn"]').css('border', '1px solid #e5e5e5 !important');
        $('[name="' + model.tableName + '_zoomBtn"]').css('display', 'inline-block !important');
    }

    function _bind(model) {
        $('#' + model.tableName + '_resetZoom').live('click', function () {
            var thisTable = $(this).data('owner');
            $('#' + thisTable + '_wrapper .dataTables_scroll').css('zoom', 1);

            $('#' + thisTable + '_wrapper .dataTables_scroll .dataTables_scrollHead .dataTables_scrollHeadInner table').css('zoom', 1);
            $('#' + thisTable + '_wrapper .dataTables_scroll .dataTables_scrollBody table').css('zoom', 1);

            $('#ActionsUL' + thisTable + ' .ui-slider-handle').css('left', '49.9844px');

            if (table[thisTable] != undefined) {
                if (table[thisTable]) { table[thisTable].columns.adjust(); };
            }
        });
        $('.slider-basic[data-owner="' + model.tableName + '"]').slider({
            value: 100,
            min: 25,
            max: 225,
            step: 5,
            slide: function (event, ui) {
                var thisTable = $(this).data('owner');
                var thisValue = ui.value / 100;

                //these two lines apply to IE, where if you zoom the container it just shrinks the entire thing so you have no more visible data than you did before zooming
                $('#' + thisTable + '_wrapper .dataTables_scroll .dataTables_scrollHead .dataTables_scrollHeadInner table').css('zoom', thisValue);
                $('#' + thisTable + '_wrapper .dataTables_scroll .dataTables_scrollBody table').css('zoom', thisValue);

                $('#' + thisTable + '_wrapper .dataTables_scroll').css('-moz-transform', 'scale(' + thisValue + ')');
            },
            change: _.debounce(function (event, ui) {
                if (table[model.tableName]) { table[model.tableName].columns.adjust(); };
            }, 250)
        });
    }

    var _init = function (model) {
        irisTableZoom.html(model);
        irisTableZoom.css(model);
        irisTableZoom.bind(model);
    };

    return {
        init: function (model) {
            _init(model);
        },
        html: function (model) {
            _html(model);
        },
        css: function (model) {
            _css(model);
        },
        bind: function (model) {
            _bind(model);
        },
        get_html: function () {
            return __html;
        }
    };
}();

var irisSelect2 = function IrisSelect2() {

    var _init = function (model) {

        var select2Struct = {
            dropdownCssClass: model.dropdownCssClass == undefined ? "csa-select2-dropdown" : model.dropdownCssClass, // apply css that makes the dropdown taller
            containerCssClass: model.containerCssClass == undefined ? "csa-select2-container" : model.containerCssClass
        };

        if (model.minimumInputLength != undefined) {
            select2Struct.minimumInputLength = model.minimumInputLength;
        }

        if (model.placeholder != undefined) {
            select2Struct.placeholder = model.placeholder;
        }

        if (model.initSelection != undefined) {
            select2Struct.initSelection = model.initSelection;
        }

        if (model.formatSelection != undefined) {
            select2Struct.formatSelection = model.formatSelection;
        }

        if (model.formatResult != undefined) {
            select2Struct.formatResult = model.formatResult;
        }

        if (model.results != undefined) {
            select2Struct.results = model.results;
        }


        if (model.escapeMarkup != undefined) {
            select2Struct.escapeMarkup = model.escapeMarkup;
        }

        if (model.createSearchChoice != undefined && model.createSearchChoice) {
            select2Struct.createSearchChoice = function (term, results) {
                if (term != '') {
                    if ($(results).filter(function () {
                        return term.localeCompare(this.text) === 0;
                    }).length === 0) {
                        return { id: term, text: $.trim(term + '*') };
                    }
                }
            };
        }

        if (model.ajax != undefined) {
            select2Struct.ajax = model.ajax;
        }

        if (model.data != undefined) {
            select2Struct.data = model.data;
        }

        if (model.placeholder != undefined) {
            select2Struct.placeholder = model.placeholder;
        }

        if (model.allowClear != undefined) {
            select2Struct.allowClear = model.allowClear;
        }

        if (model.multiple != undefined) {
            select2Struct.multiple = model.multiple;
            select2Struct.closeOnSelect = true;
        }

        if (model.closeOnSelect != undefined) {
            select2Struct.closeOnSelect = model.closeOnSelect;
        }
        else {
            select2Struct.closeOnSelect = true;
        }

        minimumInputLength: 3,

            select2Struct.selectOnBlur = true;

        //$(model.selector).select2('destroy');
        $(model.selector).select2(select2Struct);
    };

    return {
        init: function (model) {
            _init(model);
        }
    };
}();

function pad(pad, str, padLeft) {
    if (typeof str === 'undefined')
        return pad;
    if (padLeft) {
        return (pad + str).slice(-pad.length);
    }
    else {
        return (str + pad).substring(0, pad.length);
    }
}

function renderEllipsis(model) {
    var esc = function (t) {
        return t
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    };

    var addNewlines = function (str) {
        var result = '';
        while (str.length > 0) {
            result += str.substring(0, 28) + '\r\n';
            str = str.substring(28);
        }
        return result;
    };

    return function (data, type, row) {
        // Order, search and type get the original data
        if (type !== 'display' && type != 'data only') {
            return data;
        }

        if (typeof data !== 'number' && typeof data !== 'string') {
            return data;
        }

        data = data.toString(); // cast numbers

        if (data.length <= model.cutoff) {
            return data;
        }

        var shortened = data.substr(0, model.cutoff - 3);

        // Find the last white space character in the string
        if (model.wordbreak) {
            //shortened = shortened.replace(/\s([^\s]*)$/, '');
            shortened = shortened.trim();
        }

        // Protect against uncontrolled HTML input
        if (model.escapeHtml) {
            shortened = esc(shortened);
        }

        if (type == 'data only') {
            return shortened;
        }
        else {

            if (model.popover != undefined && model.popover) {

                var popoverType = 'data-trigger="hover"';

                if (model.popoverType != undefined) {

                    switch (model.popoverType) {
                        case 'toggle':
                            popoverType = 'data-toggle="popover"';
                            break;
                        case 'hover':
                            popoverType = 'data-trigger="hover"';
                            break;
                    }
                }

                if (model.popoverTitle != undefined) {

                    var title = 'title="' + model.popoverTitle + '"';

                    if (data.length > 0) {

                        return '<span class="popovers" ' + title.toUpperCase() + ' data-placement="top" data-container="body" ' + popoverType + ' data-content="' + addNewlines(esc(data)) + '">' + shortened + '&#8230;</span>';
                    }
                    else {

                        return '';
                    }
                }
                else {

                    if (data.length > 0) {

                        return '<span class="popovers" data-placement="top" data-container="body" ' + popoverType + ' data-content="' + addNewlines(esc(data)) + '">' + shortened + '&#8230;</span>';
                    }
                    else {

                        return '';
                    }
                }
            }
            else {

                if (data.length > 0) {

                    return '<span class="ellipsis" title="' + esc(data) + '">' + shortened + '&#8230;</span>';
                }
                else {

                    return '';
                }
            }
        }
    };
}

var CalendarComponentsPickers = function () {

    var handleDatePickers = function () {

        if (jQuery().datepicker) {
            $('.date-picker').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true
            }).on('changeDate', function (event) {

                var thisCalendar = $(this);
                var formattedDate = $.datepicker.formatDate("yy-mm-dd", thisCalendar.datepicker("getDate"));
                var d = new Date(formattedDate);

                if (d.isValid()) {
                    thisCalendar.data('olddate', formattedDate);

                    var thisDTParent = thisCalendar.parents('[data-tableowner]').data('tableowner');

                    if (thisDTParent != undefined) {
                        table[thisDTParent].ajax.reload();
                    }
                }
                else {
                    thisCalendar.datepicker("update", new Date(DateConverter({ value: thisCalendar.data('olddate'), method: 'shortdate' })));
                }

                //   $(this).data('tableowner');
                //alert($.datepicker.formatDate("yy-mm-dd", thisCalendar.datepicker("getDate")))



            });
            //$('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
        }

        //Sets width of object to max container
        $('.SideBarFiltersContainer .datepicker-inline').css('width', '100%');
        $('.datepicker .table-condensed').css('width', '100%');

        /* Workaround to restrict daterange past date select: http://stackoverflow.com/questions/11933173/how-to-restrict-the-selectable-date-ranges-in-bootstrap-datepicker */
    };
    return {
        //main function to initiate the module
        init: function () {
            handleDatePickers();
        }
    };

}();

