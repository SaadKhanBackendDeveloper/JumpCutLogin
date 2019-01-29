

using RestSharp;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JumpCut.Common.Configurations
{
    public class Validations
    {
        public ValidationResponse SignUpValidations(string name, string email, string password, string confirmPassword)
        {
            ValidationResponse signUpValidationResponse = new ValidationResponse();
            bool validationSuccess = true;

            ValidationResponseObject userNameValidationObject = this.UserNameValidations(name.Trim());
            if (userNameValidationObject.IsValid == false)
            {
                signUpValidationResponse.validationResponseObjects.Add(userNameValidationObject);

            }

            ValidationResponseObject emailValidationObject = this.EmailValidations(email.Trim());
            if (emailValidationObject.IsValid == false)
            {
                signUpValidationResponse.validationResponseObjects.Add(emailValidationObject);
            }


            ValidationResponseObject passwordValidationObject = this.PasswordValidations(password.Trim(), Constants.Users.UserPassword);
            if (passwordValidationObject.IsValid == false)
            {
                signUpValidationResponse.validationResponseObjects.Add(passwordValidationObject);
            }

            ValidationResponseObject confirmPasswordValidationObject = this.PasswordValidations(confirmPassword.Trim(), Constants.Users.ConfirmPassword);
            if (confirmPasswordValidationObject.IsValid == false)
            {
                signUpValidationResponse.validationResponseObjects.Add(confirmPasswordValidationObject);
            }



            if (password.Trim() != confirmPassword.Trim())
            {
                signUpValidationResponse.validationResponseObjects.Add(new ValidationResponseObject()
                {
                    ParameterName = Constants.Users.UserPassword,
                    IsValid = false,
                    ErrorMessage = Constants.ErrorMessages.Users.Password_ConfirmPassword_MisMatch
                });

                signUpValidationResponse.validationResponseObjects.Add(new ValidationResponseObject()
                {
                    ParameterName = Constants.Users.ConfirmPassword,
                    IsValid = false,
                    ErrorMessage = Constants.ErrorMessages.Users.Password_ConfirmPassword_MisMatch
                });

            }


            foreach (ValidationResponseObject validationResponseObject in signUpValidationResponse.validationResponseObjects)
            {
                if (validationResponseObject.IsValid == false)
                {
                    validationSuccess = false;
                }
            }

            signUpValidationResponse.IsSuccess = validationSuccess;

            return signUpValidationResponse;
        }

        /// <summary>
        /// Perform all validations required for name of user
        /// Name: Name of user
        /// </summary>
        /// <returns></returns>
        public ValidationResponseObject UserNameValidations(string name)
        {
            ValidationResponseObject nameValidationResponse = new ValidationResponseObject();
            nameValidationResponse.IsValid = true;
            nameValidationResponse.ParameterName = Constants.Users.FullName; ;
            //nameValidationResponse.ParameterValue = name;


            if (string.IsNullOrEmpty(name))
            {
                nameValidationResponse.IsValid = false;
                nameValidationResponse.ErrorMessage = Constants.ErrorMessages.Users.FullName_Empty;
            }
            else if (name.Length > Constants.WebConfigKeys.UserNameMaxLength)
            {
                nameValidationResponse.IsValid = false;
                nameValidationResponse.ErrorMessage = Constants.ErrorMessages.Users.FullName_MaximumLength;
            }


            return nameValidationResponse;
        }

        /// <summary>
        /// Perform all validations required for email of user
        /// Email: Email address of user
        /// </summary>
        /// <returns></returns>
        public ValidationResponseObject EmailValidations(string emailId)
        {
            ValidationResponseObject emailValidationResponse = new ValidationResponseObject();
            emailValidationResponse.ParameterName = Constants.Users.EmailId;
            emailValidationResponse.IsValid = true;
            Regex emailRegex = new Regex("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{2,3}\\.[0-9]{2,3}\\.[0-9]{2,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{1,5}|[0-9]{2,3})(\\]?)$");



            if (string.IsNullOrEmpty(emailId))
            {
                emailValidationResponse.IsValid = false;
                emailValidationResponse.ErrorMessage = Constants.ErrorMessages.Users.Email_Empty;

            }
            else if (emailId.Length > Constants.WebConfigKeys.EmailMaxLength)
            {

                emailValidationResponse.IsValid = false;
                emailValidationResponse.ErrorMessage = Constants.ErrorMessages.Users.Email_MaximumLength;

            }
            else if (!emailRegex.Match(emailId).Success)
            {

                emailValidationResponse.IsValid = false;
                emailValidationResponse.ErrorMessage = Constants.ErrorMessages.Users.Email_Invalid;
            }

            return emailValidationResponse;
        }

        public ValidationResponseObject PasswordValidations(string password, string parameterName)
        {
            ValidationResponseObject passwordValidationResponse = new ValidationResponseObject();

            passwordValidationResponse.ParameterName = parameterName;
            //passwordValidationResponse.ParameterValue = password;
            passwordValidationResponse.IsValid = true;

            if (string.IsNullOrEmpty(password))
            {
                passwordValidationResponse.IsValid = false;
                passwordValidationResponse.ErrorMessage = Constants.ErrorMessages.Users.Password_Empty;

            }
            else if (password.Length < Constants.WebConfigKeys.PasswordMinimumLength)
            {

                passwordValidationResponse.IsValid = false;
                passwordValidationResponse.ErrorMessage = Constants.ErrorMessages.Users.Password_MinimumLength;

            }
            else if (password.Length > Constants.WebConfigKeys.PasswordMaximumLength)
            {

                passwordValidationResponse.IsValid = false;
                passwordValidationResponse.ErrorMessage = Constants.ErrorMessages.Users.Password_MaximumLength;

            }

            return passwordValidationResponse;
        }

        public ServiceResponse IsDuplicateEmail(string email)
        {
            bool result = false;
            string baseURL = string.Empty;
            ServiceResponse responseToController = new ServiceResponse();

            List<KeyValuePair<string, string>> headerElements = new List<KeyValuePair<string, string>>();
            List<string> headerKeys = new List<string>();

            List<KeyValuePair<string, string>> bodyElements = new List<KeyValuePair<string, string>>();
            List<string> bodyKeys = new List<string>();

            headerKeys.Add("content-type");
            headerElements.Add(new KeyValuePair<string, string>("content-type", "application/json"));

            headerKeys.Add("Authorization");
            headerElements.Add(new KeyValuePair<string, string>("Authorization", "bearer " + Constants.WebAPI.AccessToken));

            string duplicateEmailURL = Constants.WebAPI.URLs.CheckDuplicateEmail;
            baseURL = Common.Configurations.Constants.WebAPI.JumpCutServiceURL;

            string serviceResponse = this.SendPostRequest(baseURL + duplicateEmailURL, Method.POST, headerElements, headerKeys, bodyElements, bodyKeys, "application/json");

            ServiceResponse responseObject = new ServiceResponse();
            responseObject = JsonConvert.DeserializeObject<ServiceResponse>(serviceResponse);

            if (responseObject != null)
            {
                responseToController.Success = responseObject.Success;
                responseToController.Message = responseObject.Message;
            }

            return responseToController;
        }
    }
}
