
using JumpCut.Common.Configurations;
using System.Web.Mvc;
using JumpCut.Services;
using System;

namespace JumpCut.Controllers
{
    public class UserController : Controller
    {

        [HttpGet]
        public ActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml", "~/Views/Shared/_Layout.cshtml");
        }

        [HttpGet]
        public ActionResult Signup()
        {

            return View("~/Views/Signup/Signup.cshtml", "~/Views/Shared/_Layout.cshtml");

        }

        [HttpGet]
        public ActionResult Home()
        {
            //To Verify if user is logged in or not. In big application we will move this to a separate class with just one function call here.
            //Another implementation would be to implement in a Attribute and just simple tag the target Action with that attribute. For example we
            //tagged this method with [HttpGet] attribute.
            string userId = string.Empty;

            userId = Session[Constants.Session.UserId] != null ? Convert.ToString(Session[Constants.Session.UserId]) : string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                return   RedirectToAction("Login", "User");

            }

            return View("~/Views/User/Home.cshtml", "~/Views/Shared/_Layout.cshtml");

        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session[Constants.Session.UserId] = null;
            Session[Constants.Session.EmailId] = null;
            Session[Constants.Session.FullName] = null;

            return RedirectToAction("Login", "User");

        }

        //[HttpGet]
        //public ActionResult Signup(string confirmToken)
        //{
        //    //TODO: Confirm user email
        //    return View("~/Views/Signup/Signup.cshtml", "~/Views/Shared/_Layout.cshtml");
        //}

        [HttpGet]
        public ActionResult PasswordResetRequest()
        {

            return View("~/Views/Login/PasswordResetRequest.cshtml", "~/Views/Shared/_Layout.cshtml");
        }

        [HttpGet]
        public ActionResult PasswordReset()
        {

            return View("~/Views/Login/PasswordReset.cshtml", "~/Views/Shared/_Layout.cshtml");
        }

        [HttpPost]
        public JsonResult PasswordResetRequest(string email)
        {
            string responseContent = string.Empty;
            ServiceResponse passwordResetSuccessfulResponse = new ServiceResponse(); ;

            ValidationResponse passwordResetValidationResponse = new ValidationResponse();
            Validations validationService = new Validations();
            JsonResult jsonResonse = new JsonResult();
            ServiceLayer serviceLayer = new ServiceLayer();

            //Perform Validations at backend to avoid any hacking attempt of bypassing front end validation
            passwordResetValidationResponse = validationService.PasswordResetRequestValidations(email);

            if (passwordResetValidationResponse.IsSuccess)
            {
                //Send Request to Web Service to send password reset instructions
                passwordResetSuccessfulResponse = serviceLayer.PasswordResetRequest(email, Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, ""));

                if (passwordResetSuccessfulResponse.Success)
                {
                    jsonResonse = Json(new { success = passwordResetSuccessfulResponse.Success, validationError = false, message = passwordResetSuccessfulResponse.Message });
                    jsonResonse.MaxJsonLength = System.Int32.MaxValue;
                }
                else
                {
                    //In case there is any error at backend for creating new user. Show error message to user and log error at backend
                    jsonResonse = Json(new { success = passwordResetSuccessfulResponse.Success, validationError = false, message = passwordResetSuccessfulResponse.Message });
                    jsonResonse.MaxJsonLength = System.Int32.MaxValue;
                }
            }
            else
            {   //Show validations error to user on front end
                responseContent = Newtonsoft.Json.JsonConvert.SerializeObject(passwordResetValidationResponse);

                jsonResonse = Json(new { success = passwordResetValidationResponse.IsSuccess, validationError = true, data = responseContent });
                jsonResonse.MaxJsonLength = System.Int32.MaxValue;
            }

            return jsonResonse;
        }

        [HttpPost]
        public JsonResult PasswordReset(string email, string password, string confirmPassword,string passwordResetToken)
        {
            string responseContent = string.Empty;
            ServiceResponse passwordResetSuccessfulResponse = new ServiceResponse(); ;

            ValidationResponse passwordResetValidationResponse = new ValidationResponse();
            Validations validationService = new Validations();
            JsonResult jsonResonse = new JsonResult();
            ServiceLayer serviceLayer = new ServiceLayer();

            //Perform Validations at backend to avoid any hacking attempt of bypassing front end validation
            passwordResetValidationResponse = validationService.PasswordResetValidations(email,password, confirmPassword);

            if (passwordResetValidationResponse.IsSuccess)
            {
                //Send Request to Web Service to send password reset instructions
                passwordResetSuccessfulResponse = serviceLayer.PasswordReset(email,password,confirmPassword,passwordResetToken, Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, ""));

                if (passwordResetSuccessfulResponse.Success)
                {
                    jsonResonse = Json(new { success = passwordResetSuccessfulResponse.Success, validationError = false, message = passwordResetSuccessfulResponse.Message });
                    jsonResonse.MaxJsonLength = System.Int32.MaxValue;
                }
                else
                {
                    //In case there is any error at backend for creating new user. Show error message to user and log error at backend
                    jsonResonse = Json(new { success = passwordResetSuccessfulResponse.Success, validationError = false, message = passwordResetSuccessfulResponse.Message });
                    jsonResonse.MaxJsonLength = System.Int32.MaxValue;
                }
            }
            else
            {   //Show validations error to user on front end
                responseContent = Newtonsoft.Json.JsonConvert.SerializeObject(passwordResetValidationResponse);

                jsonResonse = Json(new { success = passwordResetValidationResponse.IsSuccess, validationError = true, data = responseContent });
                jsonResonse.MaxJsonLength = System.Int32.MaxValue;
            }

            return jsonResonse;
        }

        public ActionResult ConfirmEmail(string confirmToken)
        {

            JsonResult jsonResonse = new JsonResult();
            ServiceLayer serviceLayer = new ServiceLayer();

            ServiceResponse emailVerificationResponse = new ServiceResponse();
            string responseContent = string.Empty;

            //Authenticate user credentials
            emailVerificationResponse = serviceLayer.ConfirmEmail(confirmToken.Trim());

            if (emailVerificationResponse.Success)
            {
                jsonResonse = Json(new { success = emailVerificationResponse.Success, validationError = false, message = emailVerificationResponse.Message });
                jsonResonse.MaxJsonLength = System.Int32.MaxValue;
            }
            else
            {
                jsonResonse = Json(new { success = emailVerificationResponse.Success, validationError = false, message = emailVerificationResponse.Message });
                jsonResonse.MaxJsonLength = System.Int32.MaxValue;
            }

            return jsonResonse;
        }


        [HttpPost]
        public JsonResult Signup(string name, string email, string password, string confirmPassword)
        {
            string responseContent = string.Empty;
            ServiceResponse signUpSuccessfullResponse = new ServiceResponse(); ;

            ValidationResponse signupValidationResponse = new ValidationResponse();
            Validations validationService = new Validations();
            JsonResult jsonResonse = new JsonResult();
            ServiceLayer serviceLayer = new ServiceLayer();

            //Perform Validations at backend to avoid any hacking attempt of bypassing front end validation
            signupValidationResponse = validationService.SignUpValidations(name, email, password, confirmPassword);

            if (signupValidationResponse.IsSuccess)
            {

                //Send Request to Web Service to Create New User
                signUpSuccessfullResponse = serviceLayer.CreateNewUser(name, email, password, Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, ""));

                if (signUpSuccessfullResponse.Success)
                {
                    jsonResonse = Json(new { success = signUpSuccessfullResponse.Success, validationError = false, message = signUpSuccessfullResponse.Message });
                    jsonResonse.MaxJsonLength = System.Int32.MaxValue;
                }
                else
                {
                    //In case there is any error at backend for creating new user. Show error message to user and log error at backend
                    jsonResonse = Json(new { success = signUpSuccessfullResponse.Success, validationError = false, message = signUpSuccessfullResponse.Message });
                    jsonResonse.MaxJsonLength = System.Int32.MaxValue;
                }
            }
            else
            {   //Show validations error to user on front end
                responseContent = Newtonsoft.Json.JsonConvert.SerializeObject(signupValidationResponse);

                jsonResonse = Json(new { success = signupValidationResponse.IsSuccess, validationError = true, data = responseContent });
                jsonResonse.MaxJsonLength = System.Int32.MaxValue;
            }


            return jsonResonse;
        }

        [HttpPost]
        public JsonResult Login(string email, string password)
        {
            JsonResult jsonResonse = new JsonResult();
            ServiceLayer serviceLayer = new ServiceLayer();

            Validations validationService = new Validations();
            ValidationResponse loginValidationResponse = new ValidationResponse();
            ServiceResponse loginSuccessfullResponse = new ServiceResponse();
            string responseContent = string.Empty;

            //Perform backend validation to avoid any hacker attempt to bypass front end validation
            loginValidationResponse = validationService.LoginValidations(email.Trim(), password.Trim());

            if (loginValidationResponse.IsSuccess)
            {
                //Authenticate user credentials
                loginSuccessfullResponse = serviceLayer.Login(email.Trim(), password.Trim());

                if (loginSuccessfullResponse.Success)
                {
                    Session[Constants.Session.UserId] = loginSuccessfullResponse.User.UserId;
                    Session[Constants.Session.FullName] = loginSuccessfullResponse.User.FullName;

                    jsonResonse = Json(new { success = loginSuccessfullResponse.Success, validationError = false, message = loginSuccessfullResponse.Message });
                    jsonResonse.MaxJsonLength = System.Int32.MaxValue;
                }
                else
                {
                    jsonResonse = Json(new { success = loginSuccessfullResponse.Success, validationError = false, message = loginSuccessfullResponse.Message });
                    jsonResonse.MaxJsonLength = System.Int32.MaxValue;
                }

            }
            else
            {
                //Show validations error to user on front end 
                responseContent = Newtonsoft.Json.JsonConvert.SerializeObject(loginValidationResponse);

                jsonResonse = Json(new { success = loginValidationResponse.IsSuccess, validationError = true, data = responseContent });
                jsonResonse.MaxJsonLength = System.Int32.MaxValue;
            }

            return jsonResonse;
        }
    }
}