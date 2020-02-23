var sGlobal = {
    application: {
        id: sAPPID,
        name: sAPPNAME
    },
    system: {
        name: "CSA",
        id: sSYSTEMID
    },
    identity: {
        id: sIDENTITYID,
        userName: sUserName,
        uuid: sIdentityGuid
    },
    appPermissions: {
        inheritedFromIdentity: {
            MSC: [],
            UIC: [],
            PRIVILEGES: []
        },
        inheritedFromGroup: {
            MSC: [],
            UIC: [],
            PRIVILEGES: []
        },
        inheritedFromRole: {
            MSC: [],
            UIC: [],
            PRIVILEGES: []
        },
        inherited: {
            PRIVILEGES: [],
            MSC: [],
            UIC: []
        }
    },
    state: {
        intervalID: 0,
        permissionsLoaded: false
    },
    page: {}
};

function handleGetIdentityPermissions(model) {

    switch (model.direction) {
        case 'POST':

            model.data = { application_name: sGlobal.application.name, application_id: sGlobal.application.id };
            model.options = model.options == undefined ? {} : model.options;
            model.options.async = false;
            model.options.url = "/security/chamber/UserAppPermissions";
            model.options.callBack = function (model) {
                handleGetIdentityPermissions(model);
            };

            model.notification = model.notification == undefined ? {} : model.notification;
            model.notification.pulse = false;

            ajaxDynamic(model);


            break;
        case 'CALLBACK':
            switch (model.event) {
                case 'application':
                default:
                    sGlobal.appPermissions = model.response;
                    sGlobal.appPermissions.inherited = {
                        PRIVILEGES: [],
                        MSC: [],
                        UIC: []
                    };
                    sGlobal.state.permissionsLoaded = true;
                    mergePerms({});
                    break;
            }

            break;
    }
}

function loadAllPerms(model) {
    var appIDs = sGlobal.application.id + ',' + sGlobal.system.id;

    handleGetIdentityPermissions({ direction: 'POST', identity: sGlobal.identity.id, appids: appIDs, event: 'application' });
}

function mergePerms(model) {
    var RolePrivs = _.uniqWith(_.map(sGlobal.appPermissions.inheritedFromRole.PRIVILEGESs, function (item) { return { privilege_name: item.privilege_name, application_id: item.application_id }; }), _.isEqual);
    var GroupPrivs = _.uniqWith(_.map(sGlobal.appPermissions.inheritedFromGroup.PRIVILEGESs, function (item) { return { privilege_name: item.privilege_name, application_id: item.application_id }; }), _.isEqual);
    var IdentityPrivs = _.uniqWith(_.map(sGlobal.appPermissions.inheritedFromIdentity.PRIVILEGESs, function (item) { return { privilege_name: item.privilege_name, application_id: item.application_id }; }), _.isEqual);
    var MergedPrivs = RolePrivs.concat(GroupPrivs);
    MergedPrivs = MergedPrivs.concat(IdentityPrivs);

    sGlobal.appPermissions.inherited.PRIVILEGES = _.uniqWith(MergedPrivs, _.isEqual);

    var GroupMSC = _.map(_.uniqBy(sGlobal.appPermissions.inheritedFromGroup.MSCs, function (v) { return JSON.stringify([v.msc_code, v.application_id]) }), function (item) { return { msc_code: item.msc_code, application_id: item.application_id }; });
    var RoleMSC = _.map(_.uniqBy(sGlobal.appPermissions.inheritedFromRole.MSCs, function (v) { return JSON.stringify([v.msc_code, v.application_id]) }), function (item) { return { msc_code: item.msc_code, application_id: item.application_id }; });
    var IdentityMSC = _.map(_.uniqBy(sGlobal.appPermissions.inheritedFromIdentity.MSCs, function (v) { return JSON.stringify([v.msc_code, v.application_id]) }), function (item) { return { msc_code: item.msc_code, application_id: item.application_id }; });

    var MergedMSC = GroupMSC.concat(RoleMSC);
    MergedMSC = MergedMSC.concat(IdentityMSC);

    sGlobal.appPermissions.inherited.MSC = _.uniqBy(MergedMSC, function (v) { return JSON.stringify([v.msc_code, v.application_id]) });

    var GroupUIC = _.map(_.uniqBy(sGlobal.appPermissions.inheritedFromGroup.UICs, function (v) { return JSON.stringify([v.uic, v.application_id]) }), function (item) { return { uic: item.uic, application_id: item.application_id }; });
    var RoleUIC = _.map(_.uniqBy(sGlobal.appPermissions.inheritedFromRole.UICs, function (v) { return JSON.stringify([v.uic, v.application_id]) }), function (item) { return { uic: item.uic, application_id: item.application_id }; });
    var IdentityUIC = _.map(_.uniqBy(sGlobal.appPermissions.inheritedFromIdentity.UICs, function (v) { return JSON.stringify([v.uic, v.application_id]) }), function (item) { return { uic: item.uic, application_id: item.application_id }; });
    var MergedUIC = GroupUIC.concat(RoleUIC);
    MergedUIC = MergedUIC.concat(IdentityUIC);

    sGlobal.appPermissions.inherited.UIC = _.uniqBy(MergedUIC, function (v) { return JSON.stringify([v.uic, v.application_id]) });
}

function DoesUserHaveAppPrivilege(model) {
    var Available = _.some(sGlobal.appPermissions.inherited.PRIVILEGES, function (obj) {
        return obj.privilege_name != null
        && obj.privilege_name.toLowerCase() == (model.privilege == undefined ? '' : model.privilege.toLowerCase())
        && obj.application_id == (model.application_id == undefined ? sGlobal.application.id : model.application_id);
    });

    return Available;
}

function DoesUserHaveAppMSC(model) {
    var Available = _.some(sGlobal.appPermissions.inherited.MSC, function (obj) {
        return obj.msc_code != null
        && obj.msc_code.toLowerCase() == (model.msc == undefined ? '' : model.msc.toLowerCase())
        && obj.application_id == (model.application_id == undefined ? sGlobal.application.id : model.application_id);
    });

    return Available;
}

function DoesUserHaveAppUIC(model) {
    var Available = _.some(sGlobal.appPermissions.inherited.UIC, function (obj) {
        return obj.uic != null
        && obj.uic.toLowerCase() == (model.uic == undefined ? '' : model.uic.toLowerCase())
        && obj.application_id == (model.application_id == undefined ? sGlobal.application.id : model.application_id);
    });

    return Available;
}

///this will force a reload from the database when cache is stale (changed pemrisisons/apps)
function RefreshAppsAndPermissions(showToast) {

    var result;

    showToast = typeof showToast !== 'undefined' ? showToast : false;

    $.ajax({
        url: "/application/RefreshPerms",
        async: false,
        type: "POST",
        data: {
            "force": true
        },
        dataType: 'json',
        success: function (response) {
            result = response;
        }
    });

    getSidebarApps();

    $.ajax({
        url: "/application/RefreshPerms",
        async: false,
        type: "POST",
        data: {
            "force": false
        },
        dataType: 'json',
        success: function (response) {
            result = response;
        }
    });

    if (result.success && showToast) {
        toastr[ToastConstants.genericSuccess.type](ToastConstants.genericSuccess.msg, ToastConstants.genericSuccess.title);
    }
}



function canUser(model) {
    model = typeof model === 'object' ? model : { page: 'application', privilege: model };
    model.page = model.page == undefined ? 'application' : model.page;

    var privPropName = 'can' + model.privilege.replace(/ /g, '');

    var structValid;

    try {
        //structValid = eval('sGlobal.page.' + model.page) != undefined && eval('sGlobal.page.' + model.page + '.privilege') != undefined && eval('sGlobal.page.' + model.page + '.privilege[privPropName]') != undefined ? true : false;
        structValid = sGlobal.page[model.page] != undefined && sGlobal.page[model.page].privilege != undefined && sGlobal.page[model.page].privilege[privPropName] != undefined ? true : false;
    } catch (e) {
        structValid = false;
    }

    //var hasPriv = structValid ? eval('sGlobal.page.' + model.page + '.privilege[privPropName]')() : false;
    var hasPriv = structValid ? sGlobal.page[model.page].privilege[privPropName]() : false;
    var koEnabled = structValid ? ko.isObservable(sGlobal.page[model.page].privilege[privPropName]) : false;

    return hasPriv && koEnabled;
}

var KOPermissions = function KOPermissions(model) {
    function LockObject(lockThis) {
        var tempKO = ko.observable(DoesUserHaveAppPrivilege({ application_id: lockThis.id, privilege: lockThis.privilege_name }));

        sGlobal.page[lockThis.page].privilege[lockThis.privPropName] = tempKO;

        Object.defineProperty(sGlobal.page[lockThis.page].privilege, lockThis.privPropName, {
            value: tempKO,
            writable: false,
            enumerable: false,
            configurable: false
        });
    }

    function LockInPerms(model) {
        var appPerms = sGlobal.appPermissions.inherited.PRIVILEGES.filter(function (thesePrivs) { return thesePrivs.application_id == sGlobal[model.page].id; });

        sGlobal.page[model.page] = sGlobal.page[model.page] == undefined ? {} : sGlobal.page[model.page];
        sGlobal.page[model.page].privilege = sGlobal.page[model.page].privilege == undefined ? {} : sGlobal.page[model.page].privilege;

        for (var i = 0; i < appPerms.length; i++) {
            var privPropName = 'can' + appPerms[i].privilege_name.replace(/ /g, '');

            LockObject({ id: sGlobal[model.page].id, privilege_name: appPerms[i].privilege_name, privPropName: privPropName, page: model.page });

            sGlobal.page[model.page].privilege[privPropName] = ko.observable(DoesUserHaveAppPrivilege({ application_id: sGlobal[model.page].id, privilege: appPerms[i].privilege_name }));

            sGlobal.page[model.page].privilege[privPropName].subscribe(function (newValue) { location.reload(true); });
        }
    }

    function lockInAppPerms(model) {

        model.page = 'application';

        LockInPerms(model);
    }

    function lockInSysPerms(model) {

        model.page = 'system';

        LockInPerms(model);

    }

    return {
        lockInAppPerms: function (model) { lockInAppPerms(model); },
        lockInSysPerms: function (model) { lockInSysPerms(model); }
    };
}();

if (window.inTestMode === undefined) {
 //   loadAllPerms({});
}

window.sAPPID = window.sAPPID != undefined ? window.sAPPID : 0;
window.sAPPNAME = window.sAPPNAME != undefined ? window.sAPPNAME : 0;
window.sSYSTEMID = window.sSYSTEMID != undefined ? window.sSYSTEMID : 0;
window.sidentity = window.sidentity != undefined ? window.sidentity : 0;
window.sUserName = window.sUserName != undefined ? window.sUserName : 0;

//KOPermissions = Object.freeze(KOPermissions);
//KOPermissions.lockInAppPerms({ application_id: sGlobal.application.id });
//KOPermissions.lockInSysPerms({});