//Method to Perform Login Operation
function processFurther() {

    var emailId = $(selectorInputNameEmail).val();
    var password = $(selectorInputNamePassword).val();

    var data = { email: emailId, password: password };

    $.ajax({
        url: urlToLogin,
        type: ajaxMethodType,
        contentType: ajaxContentType,
        data: JSON.stringify(data),
        success: function (obj) {
            hideLoader();
            if (obj.success) {

              //  alert(obj.message);
                window.location = urlToHome;
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

