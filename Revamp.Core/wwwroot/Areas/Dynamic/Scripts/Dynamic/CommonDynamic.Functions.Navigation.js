commonDynamic.functions.navigation = (function () {
    return {
        sidebar: (function () {
            return {
                tabs: function (model) {

                    var overrideSideBar = {
                        navTabs: []
                    };

                    if (mGlobal.page[model.page].sidebar) {
                        for (i in mGlobal.page[model.page].sidebar.tabs) {
                            var tab = mGlobal.page[model.page].sidebar.tabs[i];

                            overrideSideBar.navTabs.push({
                                id: mGlobal.page[model.page].name + '_sidebar_tab_' + tab.name,
                                title: tab.name.toUpperCase(),
                                innerHtml: '',
                                active: false
                            });
                        };
                    }

                    return overrideSideBar;
                },
                css: function (model) {
                    var tableType = mGlobal.page[model.page].tableType;
                    var tableName = mGlobal.page[model.page].tableName;

                    if (mGlobal.page[model.page].sidebar) {
                        for (i in mGlobal.page[model.page].sidebar.tabs) {
                            var tab = mGlobal.page[model.page].sidebar.tabs[i];
                            var selector = $('#sidebar_' + tab.name + '_' + tableName);

                            $('#' + tableName + '_sidebar_tab_' + tab.name).append(selector.show());

                            selector.css('margin-top', '-5.5px');
                            selector.css('width', '260px');
                        }
                    }

                    $('#tab_' + tableType).css('margin-top', '-5px');
                    $('#Filters_Accordion_' + tableName).css('padding-top', '0px');
                    $('#Filters_Accordion_' + tableName).css('padding-left', '10px');
                    $('#Filters_Accordion_' + tableName).css('padding-right', '10px');
                    $('#Filters_Accordion_' + tableName).css('margin-top', '0px');
                    $('.page-quick--filter-sidebar .tab-content').css('border-left', '1px solid #ddd');
                },
                populate: {
                    actions: function (model) {

                        for (tabMenu in mGlobal.page[model.page].sidebar.tabs) {
                            var currentTabMenu = mGlobal.page[model.page].sidebar.tabs[tabMenu];
                            var html = '';

                            if (currentTabMenu.name == model.type) {
                                for (i in currentTabMenu.sections) {
                                    var currentSection = currentTabMenu.sections[i];
                                    currentSection.templateObject[0].visible = mGlobal.page[model.page].selected.row != -1 ? true : false;

                                    html += mGlobal.handlebars.template.SideBar_Buttons(currentSection.templateObject[0]);
                                }

                                $('#' + model.tabID).append(html);

                                $('#nestable_' + model.table).nestable({
                                    collapsedClass: 'dd-collapsed'
                                }).nestable('expandAll');
                            }
                        }
                    }
                },
                loadActions: function (model) {
                    var actionItems = mGlobal.page[model.page].sidebar.tabs[model.whichTab].sections[model.index].actions;

                    for (var i = 0; i < actionItems.length; i++) {
                        var thisActionItem = actionItems[i];

                        for (thisPrivilege in thisActionItem.privilege) {
                            if (canUser(thisActionItem.privilege[thisPrivilege])) {
                                switch (thisActionItem.buttonType) {
                                    case 'default':
                                        mGlobal.page[model.page].sidebar.tabs[model.whichTab].sections[model.index].defaultBtns.push(thisActionItem);
                                        break;
                                    case 'selected':
                                        mGlobal.page[model.page].sidebar.tabs[model.whichTab].sections[model.index].selectedBtns.push(thisActionItem);
                                        break;
                                }
                            }
                        }
                    }

                    return {
                        defaultBtns: mGlobal.page[model.page].sidebar.tabs[model.whichTab].sections[model.index].defaultBtns,
                        selectedBtns: mGlobal.page[model.page].sidebar.tabs[model.whichTab].sections[model.index].selectedBtns
                    }
                },
                visibility: function () {
                    var sidebar = function (model) {
                        var open = $('[name="FilterDiv_' + model.table + '"]').hasClass('page-quick-filter-sidebar-wrapper-open') ? true : false;
                        var row = mGlobal.page[model.table].selected.row;
                        var state = mGlobal.page[model.table].sidebar.initialState;

                        if (!open && row != -1) {
                            if (state == '') { mGlobal.page[model.table].sidebar.initialState = 'closed'; }
                            $('#' + model.table + '_filter div.actions div[name="filterToggleLi"] a').click();
                        }
                        else if (open && row == -1 && state == 'closed') {
                            $('#' + model.table + '_filter div.actions div[name="filterToggleLi"] a').click();
                        }
                    };

                    var actions = function (model) {
                        var row = mGlobal.page[model.table].selected.row;

                        if (row != -1) {
                            $('#dd-item-' + model.table + '-actions-selected').show();
                        }
                        else {
                            $('#dd-item-' + model.table + '-actions-selected').hide();
                        }
                    };

                    return {
                        rowSelect: function (model) {
                            sidebar(model);
                            actions(model);
                        }
                    }
                }
            }
        })()
    }
})();
