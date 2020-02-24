var dynamic = {
    functions: {
        reports: {

        }
    }
};


var mGlobal = mGlobal == undefined ? {} : mGlobal;

mGlobal.states = mGlobal.states == undefined ? {} : mGlobal.states;

mGlobal.page = mGlobal.page == undefined ? {} : mGlobal.page;

mGlobal.variable = mGlobal.variable == undefined ? {} : mGlobal.variable;

mGlobal.page.reports = {
    currentJsonData: []
    //Hold Vars
};

mGlobal.page.builder = {
    datatableName: 'this_DataTable_Builder',
    currentJsonData: []
    //Hold Vars
};

mGlobal.validation = mGlobal.validation == undefined ? {} : mGlobal.validation;

mGlobal.validation.customReportSave = {
        
        formSelector: '#SaveReportForm',
        rules: {
            'I_REPORT_NAME': {
                required: true,
                regex: /^[\w\s]+$/,
                lockDown: function () { commonDynamic.functions.validation.dynamicFormValidation.lockDownObjectInput({ selector: '#SaveReportForm [name="I_REPORT_NAME"]', regex: /^[\w\s]+$/ }) }
            },
            'I_DESCRIPTION': {
                required: true
            }
        },
        messages: {
            'I_REPORT_NAME': {
                required: 'A Custom Report Name is required.',
                regex: 'Only allows Letters, Numbers, Space and Underscores.'
            },
            'I_DESCRIPTION': {
                required: 'A Description is required.'
            }
        }
    }


commonDynamic.functions.validation.dynamicFormValidation.init(mGlobal.validation.customReportSave)


mGlobal.variable.currentSelectedBaseCustomReportID = -1;
mGlobal.variable.currentSelectedRootReportName = '';
mGlobal.variable.currentSelectedBaseReportBaseID = '';
mGlobal.variable.currentSelectedBaseReportSchema = '';
mGlobal.variable.currentCustomReportID = '';
mGlobal.variable.currentSelectedTemplate = '';
