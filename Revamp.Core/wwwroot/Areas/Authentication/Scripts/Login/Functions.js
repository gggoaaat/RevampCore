var check;
var ti = 40;

var authentication = {
    login: {
        fn: (function () {
            var lengthOfTime = ti;
            $('.login-wrapper').html('')
            var alreadyLoaded = false;

            function reloadCaptcha(model) {
                var c = Base64.decode(model.value.c);
                var thisCaptcha = '{{{c}}}';
                var hh = Handlebars.compile(thisCaptcha)

                $('#captchaIOWrap').html(hh({ c: c }));
            }

            var start = Date.now(),
                diff,
                minutes,
                seconds;
            var duration;
            var display;
            var timerInterval = 0;

            var keepRunning = true;

            function stopTimer() {
                keepRunning = false;
                clearInterval(timerInterval);
            }

            theseTemplate = {};

            function timer() {

                // get the number of seconds that have elapsed since 
                // startTimer() was called
                diff = duration - (((Date.now() - start) / 1000) | 0);

                // does the same job as parseInt truncates the float
                minutes = (diff / 60) | 0;
                seconds = (diff % 60) | 0;

                minutes = minutes < 10 ? "0" + minutes : minutes;
                seconds = seconds < 10 ? "0" + seconds : seconds;

                display.textContent = minutes + ":" + seconds;

                if (minutes == 0 && seconds == 0) {
                    clearInterval(timerInterval);
                }

                if (diff <= 0) {
                    // add one second so that the count down starts at the full duration
                    // example 05:00 not 04:59
                    start = Date.now() + 1000;
                }
            };

            var loadForm = _.debounce(function () {
                authentication.login.fn.acceptedMethod({ loadObject: true });
            }, (ti * 1000), {
                    'leading': true,
                    'trailing': false,
                    'maxWait': 1000
                });
            return {

                getObjects: function (model) {

                    var model = typeof model === 'object' && model != undefined ? model : {};

                    model.data = { token: model.token };
                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/loginjson/get";
                    model.options.callBack = function (model) {
                        if (typeof model.loadObject == 'boolean' && model.loadObject) {
                            if (model.response) {

                                if (typeof model.loadObject == 'boolean' && model.loadObject) {
                                    if (!alreadyLoaded) {


                                        var thisFormHH = Handlebars.compile(Base64.decode(model.response.value.s));


                                        var content = {
                                            objects: JSON.parse(Base64.decode(model.response.value.d)),
                                            currentRequestVerificationToken: Base64.decode(model.response.value.a),
                                            currentRequestVerificationToken2: Base64.decode(model.response.value.a2),
                                            captcha: Base64.decode(model.response.value.c),
                                            randomObject: uniquerIdentifier,
                                            message: message,
                                            returnurl: returnurl != '' ? '?returnurl=' + returnurl : '',
                                            ca: model.response.value.ic,
                                            cacPresent: model.response.value.ca,
                                            logo: logo
                                        };

                                        currentRequestVerificationToken = Base64.decode(model.response.value.z)

                                        $('.login-wrapper').html(thisFormHH(content));
                                        authentication.login.bi.init({});
                                        $('.login-wrapper').fadeIn();


                                        alreadyLoaded = true;

                                        const form = document.getElementById(uniquerIdentifier);
                                        form.addEventListener('submit', stopTimer);

                                        if ($('#CACLogin').length > 0) {
                                            const form2 = document.getElementById('CACLogin');
                                            form2.addEventListener('submit', stopTimer);
                                        }

                                        function goToPass() {
                                            if ($('[name="UserName"]').val()) {
                                                $(".part-2").show();
                                                $('#screen-progress').hide()
                                                $('.login-slider').slick('slickGoTo', 1);
                                                $('#screen-progress').html('please enter your password.').fadeIn(1000);
                                                // $('[name="Password"]').focus();
                                                $(mGlobal[uniquerIdentifier].login.validation.formSelector).valid();
                                                setTimeout(function () { $('#pwd-next').remove(); $('[name="UserName"]').attr('type', 'hidden'); }, 1000);
                                                authentication.login.fn.loadbackground();

                                                setTimeout(function () {
                                                    $('[name="Password"]').focus()
                                                    $('#pwd-next').remove();
                                                    $('[name="UserName"]').attr('type', 'hidden');
                                                }, 1000);
                                            }
                                        }

                                        function goToCaptcha() {
                                            if ($('[name="Password"]').val()) {
                                                loadForm();
                                                $('#screen-progress').hide()
                                                $('.login-slider').slick('slickGoTo', 2);
                                                $('#screen-progress').html('please enter the correct captcha.').fadeIn(1000);
                                                $(mGlobal[uniquerIdentifier].login.validation.formSelector).valid();
                                                $('.part-3').show();
                                                authentication.login.fn.loadbackground();

                                                setTimeout(function () {
                                                    $('[name="CaptchaInputText"]').focus()
                                                    $('#captcha-next').remove();
                                                    $('[name="Password"]').attr('type', 'hidden');
                                                }, 1000);

                                            }
                                        }

                                        $('#pwd-next').unbind('click', goToPass);
                                        $('#pwd-next').bind('click', goToPass);

                                        $('#captcha-next').unbind('click', goToCaptcha);
                                        $('#captcha-next').bind('click', goToCaptcha);

                                        function pressEnterUser(event) {

                                            var keycode = (event.keyCode ? event.keyCode : event.which);
                                            if (keycode == '13') {
                                                event.preventDefault();
                                                $('#pwd-next').trigger('click');
                                            }
                                        }

                                        function pressEnterPassword(event) {

                                            var keycode = (event.keyCode ? event.keyCode : event.which);
                                            if (keycode == '13') {
                                                event.preventDefault();
                                                $('#captcha-next').trigger('click');
                                            }
                                        }
                                        $('[name="UserName"]').unbind('keyup keydown press', pressEnterUser);
                                        $('[name="UserName"]').bind('keyup keydown press', pressEnterUser);
                                        $('[name="Password"]').unbind('keyup keydown press', pressEnterPassword);
                                        $('[name="Password"]').bind('keyup keydown press', pressEnterPassword);

                                        $('.login-slider').slick({
                                            dots: false,
                                            infinite: false,
                                            arrows: false,
                                            draggable: false,
                                            speed: 500,
                                            cssEase: 'ease'
                                        });
                                        $('[name="UserName"]').focus();
                                        function validateForm(e) {


                                            if ($('[name="UserName"]').val() && $('[name="Password"]').val()) {
                                                theLoader.show({
                                                    id: 'attempt-login'
                                                })

                                                return true
                                            } else { return false }
                                        }

                                        $('[name="login-form-action"]').unbind('submit', validateForm)
                                        $('[name="login-form-action"]').bind('submit', validateForm)
                                    }
                                    else {
                                        reloadCaptcha(model.response)
                                    }

                                    var display = document.querySelector('#time');
                                    authentication.login.fn.startTimer(lengthOfTime, display);
                                }
                            }
                        }
                        else {

                        }
                    }

                    model.options.callBackError = function (model) {
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

                    ajaxDynamic(model);
                },
                startMethod: function (model) {

                    if ($.cookie('revamp-agreement') == 'accepted') {
                        // theLoader.show({ id: 'load-from-loader' });
                        $(document).unbind("click keydown keyup mousemove", loadForm);
                        $(document).bind("click keydown keyup mousemove", loadForm);
                    }
                    else {


                        var tempModelName = typeof tempModelName === 'object' && tempModelName != undefined ? tempModelName : {};
                        tempModelName.data = { q: '' };
                        tempModelName.options = tempModelName.options == undefined ? {} : tempModelName.options;
                        tempModelName.options.async = true;
                        tempModelName.options.url = "/Login/prompt";
                        tempModelName.options.callBack = function (tempModelName) {
                            if (tempModelName.response != undefined && tempModelName.response.value) {
                                callback(tempModelName.response.results);
                            }
                            $.fn.revamp.templates.prompt = $.fn.revamp.addHandlebar(Base64.decode(tempModelName.response.d));
                            theseTemplate.disclaimer = $.fn.revamp.addHandlebar(Base64.decode(tempModelName.response.d2));
                            $.fn.revamp.templates.loaderEllipses = $.fn.revamp.addHandlebar(Base64.decode(tempModelName.response.d3));


                            if (theseTemplate.disclaimer) {
                                promptDialog.prompt({
                                    width: '800px',
                                    promptID: 'Disclaimer',
                                    body: theseTemplate.disclaimer(),
                                    header: 'Disclaimer',
                                    buttons: [
                                        {
                                            text: "Agree",
                                            close: true,
                                            click: function () {
                                                theLoader.show({ id: 'load-from-loader' });
                                                $(document).unbind("click keydown keyup mousemove", loadForm);
                                                $(document).bind("click keydown keyup mousemove", loadForm);
                                                $.cookie('revamp-agreement', 'accepted');
                                            }
                                        }]
                                });

                            }
                        }


                        ajaxDynamic(tempModelName);
                    }
                },
                acceptedMethod: function (model) {

                    start = Date.now();
                    if (alreadyLoaded) {
                        var display = document.querySelector('#time');
                        authentication.login.fn.startTimer(lengthOfTime, display);
                    }

                    var model = typeof model === 'object' && model != undefined ? model : {};

                    model.options = model.options == undefined ? {} : model.options;
                    model.options.async = true;
                    model.options.url = "/loginjson/start";
                    model.options.callBack = function (model) {
                        if (model.response) {

                            if (typeof model.loadObject == 'boolean' && model.loadObject) {

                                authentication.login.fn.getObjects({ loadObject: model.loadObject, token: model.response });

                                theLoader.hide({ id: 'load-from-loader' });

                            }

                            if (typeof model.clearSession == 'boolean' && model.clearSession) {

                            }

                            authentication.login.fn.loadbackground();
                        }
                    }

                    model.options.callBackError = function (model) {
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

                    if (keepRunning) {
                        ajaxDynamic(model);
                    }
                },
                startTimer: function (duration2, display2) {
                    clearInterval(timerInterval);
                    duration = duration2;
                    display = display2;

                    // we don't want to wait a full second before the timer starts
                    timer();
                    timerInterval = setInterval(timer, 1000);
                },
                loadbackground: function () {

                    var images = [{ path: '13h.jpg', size: 'auto', linear: 'linear-gradient(#1b7cc1, rgba(29, 190, 248, 0.79))' },
                    { path: 'background.jpg', size: 'register-bg.jpg', linear: "linear-gradient(#1b7cc1, rgba(29, 190, 248, 0.79))" },
                    { path: 'some-features-bg.jpg', size: '', linear: "linear-gradient(#1b7cc1, rgba(29, 190, 248, 0.79))" }]

                    function getRandomInt(min, max) {
                        min = Math.ceil(min);
                        max = Math.floor(max);
                        return Math.floor(Math.random() * (max - min + 1)) + min;
                    }
                    var backgroundSlot = getRandomInt(0, images.length - 1);
                    var background = images[backgroundSlot];
                    $(".page-content").css({
                        "background": "" + background.linear + (background.linear ? "," : "") + " url(/assets/frontend/img/" + background.path + ")",
                        "background-size": background.size ? background.size : "cover",

                    });

                },

            }
        })()
    }
}

authentication.login.bi = function (model) {

    var freeze = function () {
        Object.freeze(collections.register.bi);
    }

    var loadBinds = function (model) {

        // $('#searchInput').on('change', collections.inventory.fn.search);

        commonDynamic.functions.validation.dynamicFormValidation.init(mGlobal[uniquerIdentifier].login.validation);
    }

    return {
        //main function to initiate the module
        init: function () {
            loadBinds(model);
        }
    };
}();





var movementStrength = 10;
var height = movementStrength / $(window).height();
var width = movementStrength / $(window).width();
$('body').mousemove(function (e) {
    var pageX = e.pageX - ($(window).width() / 2);
    var pageY = e.pageY - ($(window).height() / 2);
    var newvalueX = width * pageX * -1 - 1;
    var newvalueY = height * pageY * -1 - 1;
    $(".page-content").css("background-position", newvalueX + "px     " + newvalueY + "px");
});