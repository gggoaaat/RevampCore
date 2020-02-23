$(document).ready(function () {
    //code here
    $('.passwordstrength').keyup(function () {
        // keyup code here
        // set password variable
        var pswd = $(this).val();
        //validate the length
        if (pswd.length < 14) {
            $('#length').removeClass('valid').addClass('invalid');
        } else {
            $('#length').removeClass('invalid').addClass('valid');
        }
        //validate lower letter
        if (pswd.match(/^(.*?[a-z]){2,}/)) {
            $('#letter').removeClass('invalid').addClass('valid');
        } else {
            $('#letter').removeClass('valid').addClass('invalid');
        }
        // $(this).next('#pswd_info').attr("style","color:green");
        //validate capital letter
        if (pswd.match(/^(.*?[A-Z]){2,}/)) {
            $('#capital').removeClass('invalid').addClass('valid');
        } else {
            $('#capital').removeClass('valid').addClass('invalid');
        }

        //validate special letter
        if (pswd.match(/[!@#$%&*_]/)) {
            $('#special').removeClass('invalid').addClass('valid');
        } else {
            $('#special').removeClass('valid').addClass('invalid');
        }

        //validate number
        if (pswd.match(/^(.*?\d){2,}/)) {
            $('#number').removeClass('invalid').addClass('valid');
        } else {
            $('#number').removeClass('valid').addClass('invalid');
        }

        //Prevent space key presses in both password fields
        $(document).on('keydown', this, function (e) {
            if (e.keyCode == 32) return false;
        });

        //validate space
        if (pswd.match(/^\S*$/)) {
            $('#pspaces').removeClass('invalid').addClass('valid');
        } else {
            //Strips the string in case of copy/past
            $(this).val($(this).val().replace(/[\s]/gi, ''));
        }

        checkValidation('.passwordstrength');

    });
    $('.passwordstrength').focus(function () {
        checkValidation('.passwordstrength');
    });

    $('.passwordstrength').blur(function () {
        checkValidation('.passwordstrength');
    });

    function checkValidation(field) {
        if ($('#length').hasClass('valid') && $('#letter').hasClass('valid') && $('#capital').hasClass('valid') && $('#special').hasClass('valid') && $('#pspaces').hasClass('valid') && $('#number').hasClass('valid')) {
            $(field).parent().find("#pswd_info").hide();
        }
        else {
            $(field).parent().find("#pswd_info").show();
        }
    }


    $('.passwordstrength1').keyup(function () {
        // keyup code here
        // set password variable
        var pswd = $(this).val();
        //validate the length
        if (pswd.length < 14) {
            $('#length1').removeClass('valid').addClass('invalid');
        } else {
            $('#length1').removeClass('invalid').addClass('valid');
        }
        //validate lower letter
        if (pswd.match(/^(.*?[a-z]){2,}/)) {
            $('#letter1').removeClass('invalid').addClass('valid');
        } else {
            $('#letter1').removeClass('valid').addClass('invalid');
        }
        // $(this).next('#pswd_info').attr("style","color:green");
        //validate capital letter
        if (pswd.match(/^(.*?[A-Z]){2,}/)) {
            $('#capital1').removeClass('invalid').addClass('valid');
        } else {
            $('#capital1').removeClass('valid').addClass('invalid');
        }

        //validate special letter
        if (pswd.match(/[!@#$%&*_]/)) {
            $('#special1').removeClass('invalid').addClass('valid');
        } else {
            $('#special1').removeClass('valid').addClass('invalid');
        }

        //validate number
        if (pswd.match(/^(.*?\d){2,}/)) {
            $('#number1').removeClass('invalid').addClass('valid');
        } else {
            $('#number1').removeClass('valid').addClass('invalid');
        }

        //Prevent space key presses in both password fields
        $(document).on('keydown', this, function (e) {
            if (e.keyCode == 32) return false;
        });

        //validate space
        if (pswd.match(/^\S*$/)) {
            $('#pspaces1').removeClass('invalid').addClass('valid');
        } else {
            //Strips the string in case of copy/past
            $(this).val($(this).val().replace(/[\s]/gi, ''));
        }

        checkValidation1('.passwordstrength1');

    });
    $('.passwordstrength1').focus(function () {
        checkValidation1('.passwordstrength1');
    })
    $('.passwordstrength1').blur(function () {
        checkValidation1('.passwordstrength1');

    });

    function checkValidation1(field) {
        if ($('#length1').hasClass('valid') && $('#letter1').hasClass('valid') && $('#capital1').hasClass('valid') && $('#special1').hasClass('valid') && $('#pspaces1').hasClass('valid') && $('#number1').hasClass('valid')) {
            $(field).parent().find("#pswd_info").hide();
        }
        else {
            $(field).parent().find("#pswd_info").show();
        }
    }
});