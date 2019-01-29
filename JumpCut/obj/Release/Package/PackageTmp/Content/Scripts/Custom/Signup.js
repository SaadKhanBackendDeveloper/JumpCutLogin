//Method to Implement New User Signup Process
function processFurther() {
    var fullName = $(selectorInputNameFullName).val();
    var emailId = $(selectorInputNameEmail).val();
    var password = $(selectorInputNamePassword).val();
    var confirmPassword = $(selectorInputNameConfirmPassword).val();

    var data = { name: fullName, email: emailId, password: password, confirmPassword: confirmPassword };

    $.ajax({
        url: urlToSignup,
        type: ajaxMethodType,
        contentType: ajaxContentType,
        data: JSON.stringify(data),
        success: function (obj) {
            hideLoader();
            if (obj.success) {
                //TODO: Send alert with some nice alert box
                alert(obj.message);
                window.location = urlToLogin;
            }
            else {
                if (obj.validationError) {
                    var validationErrors = JSON.parse(obj.data);

                    showBackendValidation(validationErrors);
                }
                else {
                    alert(obj.message);
                }
            }
        },
        error: function (obj) {

        },
        complete: function (obj) {
            hideLoader();
        }
    });
}


function confirmEmail(confirmToken) {
    var data = { confirmToken: confirmToken };

    $.ajax({
        url: urlToConfirmEmail,
        type: ajaxMethodType,
        contentType: ajaxContentType ,
        data: JSON.stringify(data),
        success: function (obj) {
            hideLoader();
            if (obj.success) {

                alert(obj.message);
                window.location.href = urlToLogin;
            }
            else {
                alert(obj.message);
            }
        },

        error: function (obj) {
            alert(obj);
        },
        complete: function (obj) {

        }
    });
}
