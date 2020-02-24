String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};

if (!String.prototype.endsWith) {
    String.prototype.endsWith = function (search, this_len) {
        if (this_len === undefined || this_len > this.length) {
            this_len = this.length;
        }
        return this.substring(this_len - search.length, this_len) === search;
    };
}

if (!String.prototype.startsWith) {
    Object.defineProperty(String.prototype, 'startsWith', {
        value: function (search, rawPos) {
            var pos = rawPos > 0 ? rawPos | 0 : 0;
            return this.substring(pos, pos + search.length) === search;
        }
    });
}

$.fn.revamp = {

    addHandlebar: function (o) {
        return o ? Handlebars.compile(o) : function () { console.log('Handlebar Not Present') };
    },
    addHandlebarPartial: function (m) {
        if (typeof m == 'object' && m.name) {
            return m.content ? Handlebars.registerPartial(m.name, m.content) : Handlebars.registerPartial(m.name, '');
        }
    },
    templates: {}
};

function ajaxDynamic(model) {
    var ajaxData;

    var _async = (model.options.async == undefined || model.options.async == null) ? true : model.options.async;
    var _type = (model.options.type == undefined || model.options.type == null) ? 'POST' : model.options.type;
    var _cache = (model.options.cache == undefined || model.options.cache == null) ? false : model.options.cache;

    model.notification = model.notification == undefined ? {} : model.notification;

    var requestObject = {
        url: model.options.url,
        async: _async,
        type: _type,
        data: model.data,   
        cache: _cache,
        headers: {
            "__RequestVerificationToken" : currentRequestVerificationToken
        },
        dataType: 'json',
        success: function (response, textStatus, jqXHR) {

            var callCase = (model.options.callCase == undefined || model.options.callCase == null) ? "" : model.options.callCase;

            model.options.model = model.options.model == undefined ? {} : model.options.model;

            model.direction = "CALLBACK";
            model.response = response;
            model.textStatus = textStatus;
            model.jqXHR = jqXHR;

            if (response.redirect) {
               
               // window.location.href = data.redirect;
                promptDialog.prompt({
                    promptID: 'Login-Load-Error',
                    body: 'There was an issue loading the page. Please Reload this page!',
                    close: function () { window.location.reload(); },
                    header: 'Error Loading Page',
                    buttons: [
                        {
                            text: "Reload",
                            close: true,
                            click: function () {                              
                                window.location.reload();

                            }
                        }]
                });
            } 

            if (model.options.callBack != undefined && model.options.callBack != "") {

                if (typeof model.options.callBack === "function") {
                    return model.options.callBack(model);
                }
            }
            else {
                ajaxData = response;
            }
        },
        error: function (result) {

            if (model.options.callBackError != undefined && model.options.callBackError != "") {

                if (typeof model.options.callBackError === "function") {
                    return model.options.callBackError(model);
                }
            }
           

            if (result.status == 403) {
                toastr[ToastConstants.noPrivilege.type](ToastConstants.noPrivilege.msg, ToastConstants.noPrivilege.title);
            }
            else {
                if (model.notification.pulse) {
                    toastr[ToastConstants.genericError.type](ToastConstants.genericError.msg, ToastConstants.genericError.title);
                }
            }
        },
        complete: function (response) {
            if (model.options.always != undefined && model.options.always != "") {

                if (typeof model.options.always === "function") {
                    return model.options.always(model);
                }
            }
        }
    };

    if (model.contentType != undefined) {
        requestObject.contentType = model.contentType;
    }

    $.ajax(requestObject);

    return ajaxData;
}

var promptDialog = function promptDialog() {


    var tempPrompt = '<div class="modal fade" id="{{promptID}}" data-choicetype="{{EditFormat}}" data-backdrop="static" tabindex="-1" role="dialog" aria-labelledby="{{id}}Title" aria-hidden="true"> <!-- Modal --> <div class="modal-dialog modal-dialog-centered" role="{{promptID}}-modal" style="min-width: {{width}}; top: 25%;"> <div class="modal-content" style="height: {{height}}"> <div class="modal-header"> <h5 class="modal-title" id="{{promptID}}-modal-title}">{{header}}</h5> {{#unless removeClose}} <button type="button" class="close" data-dismiss="modal" aria-label="Close"> <span aria-hidden="true">&times;</span> </button> {{/unless}} </div> <div class="modal-body modal-overflow"> <div>{{{body}}}</div> </div> <div class="modal-footer"> {{#buttons}} <button id="{{../promptID}}-btn-{{@index}}" type="button" {{#if close}} data-dismiss="modal" {{/if}} class="btn {{#if active}}btn-primary{{else}}btn-secondary{{/if}}{{btnClasses}}" {{!data-dismiss="modal"}}>{{text}}</button> {{/buttons}} </div> </div> </div></div>';
    
    function _prompt(model) {

        model.height = model.height == undefined ? 'auto' : model.height;
        model.width = model.width == undefined ? 'auto' : model.width;
        model.closeOnEscape = model.closeOnEscape == undefined ? true : model.closeOnEscape;
        model.removeClose = model.removeClose == undefined ? false : model.removeClose;

        if ($('#' + model.promptID + '').length > 0) {
            $('#' + model.promptID + '').modal('hide');
            $('#' + model.promptID + '').remove();
        }

        if ($.fn.revamp.templates.prompt == undefined) {
            $.fn.revamp.templates.prompt = $.fn.revamp.addHandlebar(tempPrompt);
        }

        $('body').append($.fn.revamp.templates.prompt(model));

        if (model.open && typeof model.open == "function") {
            $('#' + model.promptID).unbind('show.bs.modal', model.open);
            $('#' + model.promptID).bind('show.bs.modal', model.open);
        }

        if (model.opened && typeof model.open == "function") {
            $('#' + model.promptID).unbind('shown.bs.modal', model.opened);
            $('#' + model.promptID).bind('shown.bs.modal', model.opened);
        }

        if (model.close && typeof model.close == "function") {
            $('#' + model.promptID).unbind('hide.bs.modal', model.close);
            $('#' + model.promptID).bind('hide.bs.modal', model.close);
        }

        if (model.closed && typeof model.closed == "function") {
            $('#' + model.promptID).unbind('hidden.bs.modal', model.closed);
            $('#' + model.promptID).bind('hidden.bs.modal', model.closed);
        }

        if (model.buttons) {
            for (var bi = 0; bi < model.buttons.length; bi++) {
                var thisButton = model.buttons[bi];
                if (typeof thisButton.click == "function") {
                    var buttonSelector = '#' + model.promptID + '-btn-' + bi;
                    $(buttonSelector).unbind('click', thisButton.click);
                    $(buttonSelector).bind('click', thisButton.click);

                    if (thisButton.active) {
                        $(buttonSelector).focus();
                    }
                }
            }
        }

        $('#' + model.promptID).modal({ show: true, keyboard: model.closeOnEscape, focus: true });
        /* 
        $('#' + model.promptID + '').dialog({
             resizable: model.resizable == undefined ? false : model.resizable,
             draggable: model.draggable == undefined ? false : model.draggable,
             closeOnEscape: model.closeOnEscape == undefined ? true : model.closeOnEscape,
             title: model.header == undefined ? 'Prompt' : model.header,
             modal: model.modal == undefined ? true : model.modal,
             width: model.width == undefined ? '400px' : model.width,
             height: model.height == undefined ? 'auto' : model.height,
             bgiframe: model.bgiframe == undefined ? false : model.bgiframe,
             hide: model.hide == undefined ? { effect: 'scale', duration: 400 } : model.hide,
             beforeClose: function () {
                 if (model.forceBackDrop) {
                     $('#forceBackDrop').remove();
 
                     //this will move the background/overlay back behiond the modal when closing a dialog that was opened over top of a modal
                     $('.modal-backdrop').css('z-index', '10049');
                 }
             },
             buttons: model.buttons == undefined ? [
                 {
                     text: 'OK',
                     click: function () {
                        $(this).dialog('close');
                     }
                 }] : model.buttons
         });
 
         $('#' + model.promptID + '').parents('.ui-dialog').css('z-index', '99999');
 
         if (model.forceBackDrop) {
             var _forceBackDrop = '<div id="forceBackDrop" class="ui-widget-overlay ui-front" style="z-index: 99998;"></div>';
 
             //this will move the background/overlay above the modal (but behind the dialog) when opening a dialog on top of a modal window
             $('.modal-backdrop').css('z-index', '10052');
 
             $('body').append(_forceBackDrop);
         }
         */
    }

    return {
        prompt: function (model) {
            _prompt(model);
        }
    };
}();

//Prevents XSS
$(document).on('propertychange change click keyup input paste', 'input, textarea', function (e) {
    //catchPaste(e, this, function (clipData) {
    //    console.log(clipData);
    //});

    if (/<[^\w<>]*(?:[^<>"'\s]*:)?[^\w<>]*(?:\W*s\W*c\W*r\W*i\W*p\W*t|\W*f\W*o\W*r\W*m|\W*s\W*t\W*y\W*l\W*e|\W*s\W*v\W*g|\W*m\W*a\W*r\W*q\W*u\W*e\W*e|(?:\W*l\W*i\W*n\W*k|\W*o\W*b\W*j\W*e\W*c\W*t|\W*e\W*m\W*b\W*e\W*d|\W*a\W*p\W*p\W*l\W*e\W*t|\W*p\W*a\W*r\W*a\W*m|\W*i?\W*f\W*r\W*a\W*m\W*e|\W*b\W*a\W*s\W*e|\W*b\W*o\W*d\W*y|\W*m\W*e\W*t\W*a|\W*i\W*m\W*a?\W*g\W*e?|\W*v\W*i\W*d\W*e\W*o|\W*a\W*u\W*d\W*i\W*o|\W*b\W*i\W*n\W*d\W*i\W*n\W*g\W*s|\W*s\W*e\W*t|\W*i\W*s\W*i\W*n\W*d\W*e\W*x|\W*a\W*n\W*i\W*m\W*a\W*t\W*e)[^>\w])|(?:<\w[\s\S]*[\s\0\/]|['"])(?:formaction|style|background|src|lowsrc|ping|on(?:d(?:e(?:vice(?:(?:orienta|mo)tion|proximity|found|light)|livery(?:success|error)|activate)|r(?:ag(?:e(?:n(?:ter|d)|xit)|(?:gestur|leav)e|start|drop|over)?|op)|i(?:s(?:c(?:hargingtimechange|onnect(?:ing|ed))|abled)|aling)|ata(?:setc(?:omplete|hanged)|(?:availabl|chang)e|error)|urationchange|ownloading|blclick)|Moz(?:M(?:agnifyGesture(?:Update|Start)?|ouse(?:PixelScroll|Hittest))|S(?:wipeGesture(?:Update|Start|End)?|crolledAreaChanged)|(?:(?:Press)?TapGestur|BeforeResiz)e|EdgeUI(?:C(?:omplet|ancel)|Start)ed|RotateGesture(?:Update|Start)?|A(?:udioAvailable|fterPaint))|c(?:o(?:m(?:p(?:osition(?:update|start|end)|lete)|mand(?:update)?)|n(?:t(?:rolselect|extmenu)|nect(?:ing|ed))|py)|a(?:(?:llschang|ch)ed|nplay(?:through)?|rdstatechange)|h(?:(?:arging(?:time)?ch)?ange|ecking)|(?:fstate|ell)change|u(?:echange|t)|l(?:ick|ose))|m(?:o(?:z(?:pointerlock(?:change|error)|(?:orientation|time)change|fullscreen(?:change|error)|network(?:down|up)load)|use(?:(?:lea|mo)ve|o(?:ver|ut)|enter|wheel|down|up)|ve(?:start|end)?)|essage|ark)|s(?:t(?:a(?:t(?:uschanged|echange)|lled|rt)|k(?:sessione|comma)nd|op)|e(?:ek(?:complete|ing|ed)|(?:lec(?:tstar)?)?t|n(?:ding|t))|u(?:ccess|spend|bmit)|peech(?:start|end)|ound(?:start|end)|croll|how)|b(?:e(?:for(?:e(?:(?:scriptexecu|activa)te|u(?:nload|pdate)|p(?:aste|rint)|c(?:opy|ut)|editfocus)|deactivate)|gin(?:Event)?)|oun(?:dary|ce)|l(?:ocked|ur)|roadcast|usy)|a(?:n(?:imation(?:iteration|start|end)|tennastatechange)|fter(?:(?:scriptexecu|upda)te|print)|udio(?:process|start|end)|d(?:apteradded|dtrack)|ctivate|lerting|bort)|DOM(?:Node(?:Inserted(?:IntoDocument)?|Removed(?:FromDocument)?)|(?:CharacterData|Subtree)Modified|A(?:ttrModified|ctivate)|Focus(?:Out|In)|MouseScroll)|r(?:e(?:s(?:u(?:m(?:ing|e)|lt)|ize|et)|adystatechange|pea(?:tEven)?t|movetrack|trieving|ceived)|ow(?:s(?:inserted|delete)|e(?:nter|xit))|atechange)|p(?:op(?:up(?:hid(?:den|ing)|show(?:ing|n))|state)|a(?:ge(?:hide|show)|(?:st|us)e|int)|ro(?:pertychange|gress)|lay(?:ing)?)|t(?:ouch(?:(?:lea|mo)ve|en(?:ter|d)|cancel|start)|ime(?:update|out)|ransitionend|ext)|u(?:s(?:erproximity|sdreceived)|p(?:gradeneeded|dateready)|n(?:derflow|load))|f(?:o(?:rm(?:change|input)|cus(?:out|in)?)|i(?:lterchange|nish)|ailed)|l(?:o(?:ad(?:e(?:d(?:meta)?data|nd)|start)?|secapture)|evelchange|y)|g(?:amepad(?:(?:dis)?connected|button(?:down|up)|axismove)|et)|e(?:n(?:d(?:Event|ed)?|abled|ter)|rror(?:update)?|mptied|xit)|i(?:cc(?:cardlockerror|infochange)|n(?:coming|valid|put))|o(?:(?:(?:ff|n)lin|bsolet)e|verflow(?:changed)?|pen)|SVG(?:(?:Unl|L)oad|Resize|Scroll|Abort|Error|Zoom)|h(?:e(?:adphoneschange|l[dp])|ashchange|olding)|v(?:o(?:lum|ic)e|ersion)change|w(?:a(?:it|rn)ing|heel)|key(?:press|down|up)|(?:AppComman|Loa)d|no(?:update|match)|Request|zoom))[\s\0]*=/.test($(this).val())) {
        //console.log("error");
        $(this).val('');
        event.preventDefault();
        return false;
    }
});

//Validate input value based on specified pattern
//$(document).on('keydown', 'input[pattern]', function (e) {
//    var input = $(this);
//    var oldVal = input.val();
//    var regex = new RegExp(input.attr('pattern'), 'g');

//    setTimeout(function () {
//        var newVal = input.val();

//        if (newVal != "" && !regex.test(newVal)) {
//            input.val(oldVal);
//        }
//    }, 0);
//});

function catchPaste(evt, elem, callback) {
    if (navigator.clipboard && navigator.clipboard.readText) {
        // modern approach with Clipboard API
        navigator.clipboard.readText().then(callback);
    } else if (evt.originalEvent && evt.originalEvent.clipboardData) {
        // OriginalEvent is a property from jQuery, normalizing the event object
        callback(evt.originalEvent.clipboardData.getData('text'));
    } else if (evt.clipboardData) {
        // used in some browsers for clipboardData
        callback(evt.clipboardData.getData('text/plain'));
    } else if (window.clipboardData) {
        // Older clipboardData version for Internet Explorer only
        callback(window.clipboardData.getData('Text'));
    } else {
        // Last resort fallback, using a timer
        setTimeout(function () {
            callback(elem.value)
        }, 100);
    }
}


toastr.options = {
    "debug": false,
    //"positionClass": "toast-bottom-full-width",
    "positionClass": "toast-bottom-right",
    "onclick": null,
    "fadeIn": 300,
    "fadeOut": 1000,
    "timeOut": 5000,
    "extendedTimeOut": 1000,
    "closeButton": true
}

var theLoader = function IrisLoader() {

    var tempEllipse = '<div class="modal-backdrop in the-Loader-wrap app-loader-text-centered {{id}}" style=" z-index: 999999;"> <div class="app-loader"> <div class="app-loader-text"> <div id="" class="modal-backdrop in the-Loader {{id}}" style="display: none; opacity: 0; z-index: 999999;"> <div class=circleGWrap> <div id="circleG"> <div id="circleG_1" class="circleG"></div> <div id="circleG_2" class="circleG"></div> <div id="circleG_3" class="circleG"></div> </div> </div> </div> </div> </div></div>';

    if ($.fn.revamp.templates.loaderEllipses == undefined) {
        $.fn.revamp.templates.loaderEllipses = $.fn.revamp.addHandlebar(tempEllipse);
    }

    var addThis = function (model) {

        if ($('.' + model.id).length == 0) {
            var html = '';
            html += $.fn.revamp.templates.loaderEllipses(model);
            $('body').append(html);
        }
    };

    var showThis = function (model) {

        addThis(model);
        $('.' + model.id + '').show();
        $('.' + model.id + '').fadeTo(100, .5, function () {

        })
    };

    var hideThis = function (model) {

        $('.' + model.id + '').fadeOut(100, function () {
            $(this).remove();
            $('.dataTables_scrollBody table:visible').each(function (index, element) {
                table[$(element).attr('id')].columns.adjust();
            });
        });

    };

    var Unfade = function (element) {
        var op = 0.1;
        var timer = setInterval(function () {
            if (op < .5) {
                flag = true;
                element.css('opacity', op);
                element.css('filter', 'alpha(opacity=' + op * 100 + ')');
                op += op * 0.1;

                if (op >= .5) {
                    clearInterval(timer);
                }
            }
        }, 100);
    };

    return {
        //main function to initiate the module
        add: function (model) {
            addThis(model);
        },
        show: function (model) {
            showThis(model);
        },
        hide: function (model) {
            hideThis(model);
        }
    };
}();