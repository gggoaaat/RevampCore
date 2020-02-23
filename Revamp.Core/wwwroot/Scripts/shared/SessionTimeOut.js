var currentTime;
var nextTime = new Date();
var millisecondMultiplier = 1000;
var seconds = 30;
var intervalInMilliseconds = seconds * millisecondMultiplier;

nextTime = nextTime.setSeconds(nextTime.getSeconds() + seconds);

function gets() {
    currentTime = new Date();
    currentTime = currentTime.setSeconds(currentTime.getSeconds() + 0);
    if (currentTime > nextTime || nextTime == null) {
        var model = typeof model === 'object' && model != undefined ? model : {};

        model.data = { token: model.token };
        model.options = model.options == undefined ? {} : model.options;
        model.options.async = true;
        model.options.url = "/session/isActive";
        model.options.callBack = function (model) {

            var isItActive = model.response;

            if (isItActive) {
                //  extend(0)
            }
            else {
                window.onblur = null;

                $('html').hide();

                try {
                    if ($.browser.msie) {
                        //Do Nothing
                    }
                    else {
                        window.stop();
                    }
                }
                catch (err) {

                }
                var current = window.location.href;

                window.location.href = window.location.href;
                //window.location.href 
                //window.location.reload();
            }
        };
        model.options.callBackError = function (model) {
            var current = window.location.href;

            window.location.href = window.location.href;
            //window.location.reload();
        };
        ajaxDynamic(model);

        nextTime = new Date();
        nextTime = nextTime.setSeconds(nextTime.getSeconds() + seconds);
    }    
}

function extend() {
    var model = typeof model === 'object' && model != undefined ? model : {};

    model.data = { token: model.token };
    model.options = model.options == undefined ? {} : model.options;
    model.options.async = true;
    model.options.url = "/session/extend";
    model.options.callBack = function (model) {

        
    }
    model.options.callBackError = function (model) {
        window.location.reload();
    };

    ajaxDynamic(model);
}

//$(document).bind("click keydown keyup mousemove", _.debounce(gets, 10000, {
//    'leading': true,
//    'trailing': false,
//    'maxWait': 1000
//}));

$(document).bind("click keydown keyup mousemove", gets);