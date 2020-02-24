var mGlobal = {};

mGlobal.templates = {
    login: {
        html: null,
        handlebar: null
    }
}

mGlobal.page =
    {
        login: {
            object_type: "stage",
            data: []

        }
    }

mGlobal[uniquerIdentifier] = {};

mGlobal[uniquerIdentifier].login = {
    validation: {
        formSelector: '#' + uniquerIdentifier,
        rules: {
            'UserName': {
                required: true,
                minlength: 4,
                maxlength: 50,
                //lockDown: function () {
                //    commonDynamic.functions.validation.dynamicFormValidation.lockDownObjectInput({
                //        selector: '#' + uniquerIdentifier + ' [name="UserName"]',
                //        /*regex: /^(?=.{0,50}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_])$/*/
                //        //regex: /^[A-Za-z0-9_.]*$/
                //        regex: /^[a-zA-Z0-9][a-zA-Z0-9_.]{4,29}$/
                //    });
                //},
                //regex: /^(?:[a-zA-Z0-9]{4,50}|[a-zA-Z!$#\+\-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4})$/
            },
            'EmailAddress': {
                required: true,
                depends: function () {
                    $(this).val($.trim($(this).val()));
                    return true;
                },
                regex: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
            },
            'Password': {
                required: true,
                minlength: 14,
                regex: /^(?=.*[A-Z].*[A-Z])(?=.*[_!#$%^&*@@])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z]).{14,}$/
            },
            'ConfirmPassword': {
                required: true,
                equalTo: '#' + uniquerIdentifier + ' [name="Password"]'
            },
            'CaptchaInputText':
            {
                required: true,
            }
        },
        messages: {
            'UserName': {
                required: "Please enter a username.",
                regex: "Special characters are not allowed",
                remote: "Username not available."
            },
            'EmailAddress': {
                required: "Please enter an email address.",
                email: "Please enter a valid email address.",
                remote: "This email is already in use.",
                regex: "Please enter a valid email address."
            },
            'Password': {
                required: "Please enter a password."
            },
            'ConfirmPassword': {
                required: 'Confirm Password is required',
                equalTo: 'Password mismatch'
            },
            'CaptchaInputText':
            {
                required: 'Captcha Required',
            }
        }
    }
}