commonDynamic.functions.ajaxData = (function () {
    return {
        handles: (function () {
            return {
                ajaxCall: function (model) {
                    switch (model.direction) {
                        case 'POST':
                            if (!mGlobal.ajaxData.handles[model.formType][model.handle].isLoaded || model.reset) {
                                model.init = true;
                                mGlobal.ajaxData.handles[model.formType][model.handle].isLoaded = false;
                                ajaxDynamic(mGlobal.ajaxData.handles[model.formType][model.handle].fn(model));
                            }
                            else {
                                model.direction = 'CALLBACK';
                                model.response = mGlobal.ajaxData.handles[model.formType][model.handle].response;

                                model.init = false;
                                commonDynamic.functions.ajaxData.handles.ajaxCall(model);
                            }
                            break;
                        case 'CALLBACK':
                            if (model.init) {
                                mGlobal.ajaxData.handles[model.formType][model.handle].response = model.response;
                                mGlobal.ajaxData.handles[model.formType][model.handle].isLoaded = model.response != undefined ? true : false;
                            }
                            if (!model.init || model.reset) {
                                if (mGlobal.ajaxData.handles[model.formType][model.handle].callBack) {
                                    mGlobal.ajaxData.handles[model.formType][model.handle].callBack(model);
                                }
                            }
                    }
                },

                load: function (model) {
                    var iterateCalls = function (model) {
                        //Run individual handles
                        if (model && model.handles) {
                            _.forEach(model.handles, function (handle, i, arr) {
                                if (!_.includes(model.exclude, handle)) {
                                    commonDynamic.functions.ajaxData.handles.ajaxCall({ formType: model.formType, direction: 'POST', handle: handle, reset: model.reset });
                                }
                            });
                        }
                            //Run all handles per form
                        else {
                            var forms = (model && model.forms) ? model.forms : mGlobal.ajaxData.handles.list.forms[model.formType];

                            _.forEach(forms, function (form, i, arr1) {
                                if (!_.includes(model.exclude, form)) {
                                    _.forEach(mGlobal.ajaxData.handles.list[model.formType][form], function (handle, j, arr2) {
                                        if (!_.includes(model.exclude, handle)) {
                                            commonDynamic.functions.ajaxData.handles.ajaxCall({ formType: model.formType, direction: 'POST', handle: handle, reset: model.reset });
                                        }
                                    });
                                }
                            });
                        }
                    };

                    model.reset = (model && model.reset) ? model.reset : false;
                    model.exclude = (model && model.exclude) ? model.exclude : []; //will filter out specified handles

                    if (model.formTypes) {
                        _.forEach(model.formTypes, function (type, i, arr) {
                            model.formType = type;
                            iterateCalls(model);
                        })
                    }
                    else if (model.formType) {
                        iterateCalls(model);
                    }
                },

                check: function (model) {
                    var check = false;
                    var exclude = (model.exclude) ? model.exclude : [];

                    var checkHandles = function (model) {
                        var handleCheck = true;
                        //model.list = (model.list != undefined && model.formType == undefined) ? model.list :
                        //    ((model.formType != undefined && $.isArray(mGlobal.ajaxData.handles.list.forms[model.formType])) ? mGlobal.ajaxData.handles.list.forms[model.formType] :
                        //        mGlobal.ajaxData.handles[model.formType]);
                        model.list = model.list != undefined ? model.list : mGlobal.ajaxData.handles.list.forms[model.formType];

                        //if ($.isArray(model.list)) {
                        _.forEach(model.list, function (handle, i, arr) {
                            if (!_.includes(exclude, handle) && mGlobal.ajaxData.handles[model.formType][handle]) {
                                handleCheck = mGlobal.ajaxData.handles[model.formType][handle].isLoaded;

                                if (!handleCheck) { return handleCheck; }
                            }
                        });
                        return handleCheck;
                        //}
                        //else if (mGlobal.ajaxData.handles[model.formType][model.list] != undefined) {
                        //    check = mGlobal.ajaxData.handles[model.formType][model.list].isLoaded;
                        //    return check;
                        //}
                    };

                    if (model.handles) {
                        check = checkHandles(model);
                    }
                    else {
                        var forms = (model.forms) ? model.forms : mGlobal.ajaxData.handles.list.forms[model.formType];

                        _.forEach(forms, function (form, i, arr1) {
                            if (!_.includes(exclude, form)) {
                                model.list = _.includes(mGlobal.ajaxData.handles.list.forms[model.formType], form) ? mGlobal.ajaxData.handles.list[model.formType][form] : form;

                                if (model.list) {
                                    check = checkHandles(model);

                                    if (!check) { return check; }
                                }
                                else {
                                    check = true;
                                }
                            }
                        });
                    }

                    return check;
                },

                wait: function (model) {
                    var check = false;

                    model.fn = model.fn != undefined ? model.fn : function () { IrisEllipse_Hide(); };
                    model.autohide = model.autohide != undefined ? model.autohide : true;

                    //multiple type check&load
                    if (model.formTypes) {
                        var checkAll = function (model) {
                            var temp = false;

                            _.forEach(model.formTypes, function (type, i, arr) {
                                model.formType = type;
                                check = commonDynamic.functions.ajaxData.handles.check(model);

                                if (!check) {
                                    return false;
                                };
                            })

                            return check;
                        }

                        if (!checkAll(model)) {
                            IrisEllipse_Show();

                            _.forEach(model.formTypes, function (type, i, arr) {
                                model.formType = type;
                                commonDynamic.functions.ajaxData.handles.load(model);
                            })
                        }

                        var checkHandles = setInterval(function () {
                            if (checkAll(model)) {
                                model.fn(model);

                                if (model.autohide) { IrisEllipse_Hide(); }
                                clearInterval(checkHandles);
                            }
                        }, 100);
                    }
                    //singular type check&load
                    else if (model.formType) {
                        if (!commonDynamic.functions.ajaxData.handles.check(model) || model.reset) {
                            IrisEllipse_Show();
                            commonDynamic.functions.ajaxData.handles.load(model);
                        }

                        var checkHandles = setInterval(function () {
                            check = commonDynamic.functions.ajaxData.handles.check(model);

                            if (check) {
                                model.fn(model);

                                if (model.autohide) { IrisEllipse_Hide(); }
                                clearInterval(checkHandles);
                            }
                        }, 100);
                    }

                    return true;
                }
            }
        })()
    }
})();
