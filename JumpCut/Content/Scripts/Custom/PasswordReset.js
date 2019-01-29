//Method to Update Customer Entered Password in Database
function processFurther() {
    var email = $(selectorInputNameEmail).val();
    var password = $(selectorInputNamePassword).val();
    var confirmPassword = $(selectorInputNameConfirmPassword).val();
    var resetToken = getUrlParameter('resetToken');

    var data = { email: email,password: password, confirmPassword: confirmPassword, passwordResetToken: resetToken };

    $.ajax({
        url: urlToPasswordReset,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (obj) {
            hideLoader();

            if (obj.success) {

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
            alert(obj);
        },
        complete: function (obj) {

            hideLoader();
        }
    });
}