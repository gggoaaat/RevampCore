commonDynamic.functions.settings = (function () {
    return {
        getIdentitySetting: function (model) {
            var returnedSettings = mGlobal.settings.appSettings.data.filter(function (userSetting) { return userSetting.setting == model.setting; });

            return returnedSettings;
        },
        saveIdentityAppSetting: function (model) {
            switch (model.direction) {
                case 'POST':

                    ajaxUniversal('/ApplicationJSON/Create_User_App_Settings', model.saveData, { callCase: 'Save Identity App Setting', model: model });

                    model = model != undefined ? model : {};

                    model.data = model.saveData
                    model.options = {};
                    model.options.async = true;
                    model.options.url = '/ApplicationJSON/Create_User_App_Settings';
                    model.options.callBack = function (model) {

                        model.SavedAppSettings = model.response;
                        commonDynamic.functions.settings.saveIdentityAppSetting(model);
                    }
                    model.notification = model.notification == undefined ? {} : model.notification;
                    model.notification.pulse = false;
                    ajaxDynamic(model);

                    break;
                case 'CALLBACK':
                    promptDialog.prompt({
                        promptID: 'Setting-Saved-Prompt',
                        body: 'Setting Saved!',
                        header: 'IRIS Settings'
                    });
                    break;
            }
        },
        saveSettingStruct: function (model) {
            var thisSaveModel = {
                'theseModels[0].I_BASE_IDENTITY_APP_SETTING_ID': model.thisBaseID,
                'theseModels[0].I_PREV_IDENTITY_APP_SETTING_ID': model.thisPrevID,
                'theseModels[0].I_SETTING': model.thisSetting,
                'theseModels[0].I_VALUE': model.thisValue
            };

            return thisSaveModel;
        },
        getIdentityAppSettings: function (model) {

            mGlobal.settings = mGlobal.settings == undefined ? {} : mGlobal.settings;

            mGlobal.settings.appSettings = ajaxCorePersonnel('/dynamic/dynosearch?iReport=IdentityAppSettings', {
                SearchReport: 'Identity App Settings',
                start: 0,
                length: -1,
                'order[0][column]': '1',
                'order[0][dir]': 'asc',
                P_DYNO_COL: '',
                P_IDENTITY_ID: sGlobal.identityID.id,
                P_GET_LATEST: 'T',
                schema: 'csa',
                template: 'system',
                queryType: "query"
            },
            {
                async: false
            });
        }
    }
})();
