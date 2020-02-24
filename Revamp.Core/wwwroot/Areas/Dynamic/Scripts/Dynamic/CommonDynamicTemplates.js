function ifEven(conditional, options) {
    if ((conditional % 2) == 0) {
        return options.fn(this);
    } else {
        return options.inverse(this);
    }
}

function ifEmpty(conditional, options) {
    if (conditional == undefined || $.trim(conditional) == "" || conditional == "null" || conditional == "false" || conditional == false) {
        return options.fn(this);
    } else {
        return options.inverse(this);
    }
}

function unlessEmpty(conditional, options) {
    if (conditional == undefined || $.trim(conditional) == "" || conditional == "null" || conditional == "false" || conditional == false) {
        return options.inverse(this);
    } else {
        return options.fn(this);
    }
}

function ifCondition(index, evalcond, options) {

    var thisCond = isNaN(index) ? ("'" + index + "' " + evalcond) : (index + evalcond);

    if (eval(thisCond)) {
        return options.fn(this);
    } else {
        return options.inverse(this);
    }
}   

Handlebars.registerHelper('ifFirst', function (index, options) {
    if (index == 0) {
        return options.fn(this);
    } else {
        return options.inverse(this);
    }

});

Handlebars.registerHelper('json', function (context) {
    return JSON.stringify(context);
});

Handlebars.registerHelper('replace', function (find, regexMatch, replaceWith) {
    var temp = find.replace(new RegExp(regexMatch, "g"), replaceWith)
    return temp;
});


Handlebars.registerHelper('if_even', ifEven);

Handlebars.registerHelper('ifCondition', ifCondition);

Handlebars.registerHelper('ifEmpty', ifEmpty);

Handlebars.registerHelper('unlessEmpty', unlessEmpty);

Handlebars.registerHelper('noSpace', function (context) { return context.replace(/ /g, "_"); });

$.holdReady(true);
var GetHtmlTemplates = typeof GetHtmlTemplates === 'object' && GetHtmlTemplates != undefined ? GetHtmlTemplates : {};
GetHtmlTemplates.data = {  };
GetHtmlTemplates.options = GetHtmlTemplates.options == undefined ? {} : GetHtmlTemplates.options;
GetHtmlTemplates.options.async = false;
GetHtmlTemplates.options.url = '/dynamic/getTemplates?htmltemplate';
GetHtmlTemplates.options.callBack = function (GetHtmlTemplates) {
    if (GetHtmlTemplates.response != undefined && GetHtmlTemplates.response.d) {

        for (template in GetHtmlTemplates.response.d) {
            $.fn.revamp.templates[template] = $.fn.revamp.addHandlebar(GetHtmlTemplates.response.d[template]);
        }        
    }

    $.holdReady(false);
}

ajaxDynamic(GetHtmlTemplates);

