var emailReg = /^[A-Za-z0-9][\W]*([\w+\-.%]+\@[\w\-.]+\.[A-Za-z]+[\W]*,{1}[\W]*)*([\w+\-.%]+\@[\w\-.]+\.[A-Za-z]+)[\W]*$/;

$.validator.addMethod("specialChar", function (value, element) {
    return this.optional(element) || /^\s*[a-zA-Z0-9_\s{2,}]+\s*$/.test(value);
}, "Object must contain only letters & numbers.");

$.validator.addMethod("specialEmailChar", function (value, element) {
    return this.optional(element) || emailReg.test(value);

}, "Please enter valid email address(es)");

$.validator.addMethod("preventBlankEmail", function (value, element) {
    return this.optional(element) || /^[^,].*[^-,]$/.test(value);
}, "Please enter valid email address(es)");


commonDynamic.functions.validation = (function () {
    return {
        dynamicFormValidation: function () {

            var initFormValidation = function (model) {

                function copyRulesWithoutLockDown(rules) {
                    var theseRules = _.cloneDeep(rules, true);
                    for (thisRule in theseRules) {
                        if (theseRules[thisRule].lockDown != undefined) {
                            delete theseRules[thisRule].lockDown;
                        }
                    }
                    return theseRules;
                }

                $(model.formSelector).validate({
                    errorElement: model.errorElement != undefined ? model.errorElement : 'span', //default input error message container
                    errorClass: model.errorClass != undefined ? model.errorClass : 'help-block', // default input error message class
                    focusInvalid: model.focusInvalid != undefined ? model.focusInvalid : false, // do not focus the last invalid input
                    rules: copyRulesWithoutLockDown(model.rules),
                    messages: model.messages,

                    invalidHandler: model.invalidHandler != undefined ? model.invalidHandler : function (event, validator) { //display error alert on form submit
                        $('.alert-danger', $(model.formSelector)).show();
                    },

                    highlight: model.highlight != undefined ? model.highlight : function (element) { // hightlight error inputs
                        $(element).closest('.form-group').addClass('has-error'); // set error class to the control group
                    },

                    success: model.success != undefined ? model.success : function (label) {
                        label.closest('.form-group').removeClass('has-error');
                        label.closest('.form-group').find('.form-control').attr('aria-describedby', '');
                        label.remove();
                    },

                    errorPlacement: model.errorPlacement != undefined ? model.errorPlacement : function (error, element) {
                        //error.insertAfter(element.closest('.input-icon'));
                        error.insertAfter(element);
                    },

                    submitHandler: model.submitHandler != undefined ? model.submitHandler : function (form) {
                        form.submit(); // form validation success, call ajax form submit
                    }
                });

                if (model.rules != undefined) {
                    for (rule in model.rules) {
                        if (model.rules[rule].lockDown != undefined && typeof model.rules[rule].lockDown === "function") {
                            model.rules[rule].lockDown()
                        }
                    }
                }

                $(model.formSelector).validate();
                if ($(model.formSelector).length > 0) {
                    $(model.formSelector).valid();
                }

                $(model.formSelector + ' input').keypress(function (e) {

                    $(model.formSelector).valid();

                });

            };

            function regexTest(model) {
                if (model.regex != undefined && model.regex instanceof RegExp) {
                    return model.regex.test(model.value);
                }
                else {
                    return false;
                }
            }

            var lockDownObjectInput = function (model) {
                if (model != undefined && model.selector != undefined && model.regex != undefined) {

                    if ($(model.selector).length > 0) {
                        $(model.selector).on('keyup keydown keypress input change paste', function (e) {
                            var MoveForward = false;

                            var acceptedKeys = ['delete', 'tab', 'backspace', 'home', 'end', 'shift']

                            if (e.ctrlKey && (e.key != undefined && e.key.toLowerCase() == 'v')) {
                                MoveForward = false;
                            }
                            else if (e.ctrlKey || e.keyCode == 17 || (e.key != undefined && e.key.toLowerCase() == 'control')) {
                                MoveForward = false;
                            }
                            else if (e.type != undefined && e.type.toLowerCase() == 'paste') {
                                MoveForward = false;
                            }
                            else if (e.key != undefined && (acceptedKeys.indexOf(e.key.toLowerCase()) > -1)) {
                                MoveForward = true;
                            }
                            else if (e.type != undefined && (acceptedKeys.indexOf(e.type.toLowerCase()) > -1)) {
                                MoveForward = true;
                            }
                            else {
                               // if (model.method == "*") {
                                    MoveForward = regexTest({ regex: model.regex, value: $(this).val() + e.key, event: e }) || regexTest({ regex: model.regex, value: e.key, event: e });
                                //}
                                //else {
                                //    MoveForward = regexTest({ regex: model.regex, value: $(this).val() + e.key, event: e });
                                //}
                            }

                            return MoveForward;
                        });
                    }
                }
            };

            return {
                //main function to initiate the module
                init: function (model) {
                    initFormValidation(model);
                },
                lockDownObjectInput: function (model) {
                    lockDownObjectInput(model);
                }
            };
        }()
    }
})();
