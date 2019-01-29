//Method to Send Password Reset Instruction To User
function processFurther() {
    var email = $(selectorInputNameEmail).val();
    var data = { email: email };

    $.ajax({
        url: urlToPasswordResetRequest,
        type: ajaxMethodType,
        contentType: ajaxContentType,
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