var urlToSignup = "/User/Signup";
var urlToLogin = "/User/Login";
var urlToHome = "/User/Home";
var urlToConfirmEmail = '/User/ConfirmEmail';
var urlToPasswordResetRequest = '/User/PasswordResetRequest';
var urlToPasswordReset = '/User/PasswordReset';

//Attributes name for showing validation errors 
var originalTitleAttribute = 'data-original-title';
var errorMessageTextAttribute = 'data-custom-text';

var inputTypeText = 'text';
var inputTypeEmail = 'email';
var inputTypePassword = 'password';

//Use in jquery Selector
var selectorInputTypeText = "input[type=text]";
var selectorInputTypePassword = "input[type=password]";


var selectorInputNameFullName = "input[name=fullname]";
var selectorInputNameEmail = "input[name=email]";
var selectorInputNamePassword = "input[name=password]"; 
var selectorInputNameConfirmPassword = "input[name=confirmpassword]";

var ajaxContentType = "application/json; charset=utf-8";
var ajaxMethodType = "POST";


function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};

function showLoader() {
    $(".loader").attr("style", "");
}

function hideLoader() {
    $(".loader").attr("style", "visibility: hidden");
}

(function ($) {
    //"use strict";

    if (window.location.pathname.toLowerCase() == urlToSignup.toLowerCase()) {

        var confirmToken = this.getUrlParameter('confirmToken');

        if (confirmToken != undefined && confirmToken != NaN && confirmToken != '') {
            
            confirmEmail(confirmToken);
        }
       
    }

    /*================================================================== [ Validate ]*/
var input = $('.validate-input .input100');

$('.validate-form').on('submit', function () {
    var isValid = true;

    $(selectorInputTypeText).parent().css("border-color", "white");
    $(selectorInputTypePassword).parent().css("border-color", "white");

    $(selectorInputTypeText).attr(originalTitleAttribute, '');
    $(selectorInputTypePassword).attr(originalTitleAttribute, '');
    $(selectorInputTypeText).attr('title', '');
    $(selectorInputTypePassword).attr('title', '');

    for (var i = 0; i < input.length; i++) {
        if (validate(input[i]) == false) {
            showValidate(input[i]);
            isValid = false;
        }
    }

    if (isValid == true) {
        showLoader();
    processFurther();

    }
    return false;
});


$('.validate-form .input100').each(function () {
    $(this).focus(function () {
        hideValidate(this);
    });
});

function validate(input) {
    if ($(input).attr('type') == inputTypeEmail || $(input).attr('name') == 'email') {
        if ($(input).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
            return false;
        }
    }
    else if ($(input).attr('type') == inputTypePassword ) {
        if ($(input).attr('name') == 'password' || $(input).attr('name') == 'confirmpassword') {

            //Validates minimum Length of Password
            if ($(input).val().trim().length < 8) {
                return false;
            } else {
                if (passwordField != undefined && confirmPasswordField != undefined) {

                    //Password and Confirm Password Fields
                    var passwordField = $(selectorInputNamePassword);
                    var confirmPasswordField = $(selectorInputNameConfirmPassword);

                    if (passwordField.val().trim() != confirmPasswordField.val().trim()) {

                        $(passwordField).attr(errorMessageTextAttribute, "Password and Confirm Password should match each other.");
                        $(confirmPasswordField).attr(errorMessageTextAttribute, "Password and Confirm Password should match each other.");

                        return false;
                    }
                }
            }
        }
    }
    else {
        if ($(input).val().trim() == '') {
            return false;
        }
    }
}

function showValidate(input) {
    var controlName = $(input).attr('name');

    var message = $("input[name=" + controlName + "]").attr(errorMessageTextAttribute);

    $("input[name=" + controlName + "]").tooltip({
        content: $("input[name=" + controlName + "]").attr(title, message),
        track: true,
        show: { effect: "blind", duration: 800 }
    });

    $("input[name=" + controlName + "]").parent().css("border-color", "red");

}

function hideValidate(input) {

    var controlName = $(input).attr('name');

    $("input[name=" + controlName + "]").tooltip({
        content: $("input[name=" + controlName + "]").attr(originalTitleAttribute, ""),
    });

    $("input[name=" + controlName + "]").parent().css("border-color", "white");
}



})(jQuery);

function showBackendValidation(validationErrors) {

    for (var i = 0; i < validationErrors.validationResponseObjects.length; i++) {

        if (validationErrors.validationResponseObjects[i].IsValid == false) {

            switch (validationErrors.validationResponseObjects[i].ParameterName) {
                case "FullName":

                    $(selectorInputNameFullName).tooltip({
                        content: $(selectorInputNameFullName).attr(originalTitleAttribute, validationErrors.validationResponseObjects[i].ErrorMessage),
                        track: true,
                        position: { my: "left+15 center", at: "right center" },
                        show: { effect: "blind", duration: 800 }
                    });

                    $(selectorInputNameFullName).parent().css("border-color", "red");
                    break;
                case "EmailId":


                    $(selectorInputNameEmail).tooltip({
                        content: $(selectorInputNameEmail).attr(originalTitleAttribute, validationErrors.validationResponseObjects[i].ErrorMessage),
                        track: true,
                        position: { my: "left+15 center", at: "right center" },
                        show: { effect: "blind", duration: 800 }
                    });

                    $(selectorInputNameEmail).parent().css("border-color", "red");

                    break;
                case "UserPassword":

                    $(selectorInputNamePassword).tooltip({
                        content: $(selectorInputNamePassword).attr(originalTitleAttribute, validationErrors.validationResponseObjects[i].ErrorMessage),
                        track: true,
                        position: { my: "left+15 center", at: "right center" },
                        show: { effect: "blind", duration: 800 }
                    });

                    $(selectorInputNamePassword).parent().css("border-color", "red");
                    break;

                case "ConfirmPassword":

                    $(selectorInputNameConfirmPassword).tooltip({
                        content: $(selectorInputNameConfirmPassword).attr(originalTitleAttribute, validationErrors.validationResponseObjects[i].ErrorMessage),
                        track: true,
                        position: { my: "left+15 center", at: "right center" },
                        show: { effect: "blind", duration: 800 }
                    });

                    $(selectorInputNameConfirmPassword).parent().css("border-color", "red");
                    break;
            }
        }
    }
}