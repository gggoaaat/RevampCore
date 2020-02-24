function ifEven(conditional, options) {
    if ((conditional % 2) == 0) {
        return options.fn(this);
    } else {
        return options.inverse(this);
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


function isEmpty(index, options) {

    if (index == "" || index == undefined || index.length == 0) {
        return options.fn(this);
    } else {
        return options.inverse(this);
    }
}

function isNotEmpty(index, options) {

    if (index != "" && index != undefined && index.length > 0) {
        return options.fn(this);
    } else {
        return options.inverse(this);
    }
}

Handlebars.registerHelper('whitespace', function (object) {
    var text = Handlebars.escapeExpression(object)

    return new Handlebars.SafeString(text.replace(/\s/g, '') );
});

Handlebars.registerHelper('formatIntDate', function (passedString) {
    var theString = DateConverter({ value: passedString, method: 'time' })
    return new Handlebars.SafeString(theString)
});

Handlebars.registerHelper('resultStatus', function (passedString) {
    var theString = passedString ? '<i class="fa fa-check-circle" style="font-size: 24px; color: green;"></i>' : '<i class="fa fa-times-circle"  style="font-size: 24px; color: red;"></i>'
    return new Handlebars.SafeString(theString)
});

Handlebars.registerHelper('if_even', ifEven);

Handlebars.registerHelper('ifCondition', ifCondition);

Handlebars.registerHelper('isEmpty', isEmpty);

Handlebars.registerHelper('isNotEmpty', isNotEmpty);

var TemplateFactory = {};

//TemplateFactory.Wizard = Handlebars.compile($('#Template_Wizard').html());
//TemplateFactory.Tabs = Handlebars.compile($('#Template_Tabs').html());
