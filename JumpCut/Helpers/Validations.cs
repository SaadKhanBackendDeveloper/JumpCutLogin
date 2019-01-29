

using System.Text.RegularExpressions;
using JumpCut.Services;

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

            ValidationResponseObject emailValidationObject = this.EmailValidations(email.Trim(), true);
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

        public ValidationResponse LoginValidations(string email, string password)
        {
            ValidationResponse loginValidationResponse = new ValidationResponse();
            bool validationSuccess = true;

            ValidationResponseObject emailValidationObject = this.EmailValidations(email.Trim(), false);
            if (emailValidationObject.IsValid == false)
            {
                loginValidationResponse.validationResponseObjects.Add(emailValidationObject);
            }

            ValidationResponseObject passwordValidationObject = this.PasswordValidations(password.Trim(), Constants.Users.UserPassword);
            if (passwordValidationObject.IsValid == false)
            {
                loginValidationResponse.validationResponseObjects.Add(passwordValidationObject);
            }


            foreach (ValidationResponseObject validationResponseObject in loginValidationResponse.validationResponseObjects)
            {
                if (validationResponseObject.IsValid == false)
                {
                    validationSuccess = false;
                }
            }

            loginValidationResponse.IsSuccess = validationSuccess;

            return loginValidationResponse;
        }

        public ValidationResponse PasswordResetRequestValidations(string email)
        {
            ValidationResponse passwordResetValidationResponse = new ValidationResponse();
            bool validationSuccess = true;

            ValidationResponseObject emailValidationObject = this.EmailValidations(email.Trim(), false);
            if (emailValidationObject.IsValid == false)
            {
                passwordResetValidationResponse.validationResponseObjects.Add(emailValidationObject);
            }

            foreach (ValidationResponseObject validationResponseObject in passwordResetValidationResponse.validationResponseObjects)
            {
                if (validationResponseObject.IsValid == false)
                {
                    validationSuccess = false;
                }
            }

            passwordResetValidationResponse.IsSuccess = validationSuccess;

            return passwordResetValidationResponse;
        }

        public ValidationResponse PasswordResetValidations(string email, string password, string confirmPassword)
        {
            ValidationResponse passwordResetValidationResponse = new ValidationResponse();
            bool validationSuccess = true;

            ValidationResponseObject emailValidationObject = this.EmailValidations(email.Trim(), false);
            if (emailValidationObject.IsValid == false)
            {
                passwordResetValidationResponse.validationResponseObjects.Add(emailValidationObject);
            }

            ValidationResponseObject passwordValidationObject = this.PasswordValidations(password.Trim(), Constants.Users.UserPassword);
            if (passwordValidationObject.IsValid == false)
            {
                passwordResetValidationResponse.validationResponseObjects.Add(passwordValidationObject);
            }

            ValidationResponseObject confirmPasswordValidationObject = this.PasswordValidations(confirmPassword.Trim(), Constants.Users.ConfirmPassword);
            if (confirmPasswordValidationObject.IsValid == false)
            {
                passwordResetValidationResponse.validationResponseObjects.Add(confirmPasswordValidationObject);
            }

            if (password.Trim() != confirmPassword.Trim())
            {
                passwordResetValidationResponse.validationResponseObjects.Add(new ValidationResponseObject()
                {
                    ParameterName = Constants.Users.UserPassword,
                    IsValid = false,
                    ErrorMessage = Constants.ErrorMessages.Users.Password_ConfirmPassword_MisMatch
                });

                passwordResetValidationResponse.validationResponseObjects.Add(new ValidationResponseObject()
                {
                    ParameterName = Constants.Users.ConfirmPassword,
                    IsValid = false,
                    ErrorMessage = Constants.ErrorMessages.Users.Password_ConfirmPassword_MisMatch
                });

            }

            foreach (ValidationResponseObject validationResponseObject in passwordResetValidationResponse.validationResponseObjects)
            {
                if (validationResponseObject.IsValid == false)
                {
                    validationSuccess = false;
                }
            }

            passwordResetValidationResponse.IsSuccess = validationSuccess;

            return passwordResetValidationResponse;
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
            nameValidationResponse.ParameterName = Constants.Users.FullName;
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
        public ValidationResponseObject EmailValidations(string emailId, bool checkEmailDuplicate)
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
            else if (checkEmailDuplicate)
            {
                emailValidationResponse = this.IsDuplicateEmail(emailId);
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

        public ValidationResponseObject IsDuplicateEmail(string email)
        {
            bool result = false;
            string baseURL = string.Empty;
            ServiceLayer serviceLayer = new ServiceLayer();
            ValidationResponseObject emailValidationResponse = new ValidationResponseObject();
            ServiceResponse serviceResponse = new ServiceResponse();

            serviceResponse = serviceLayer.IsDuplicateEmail(email);
            if (serviceResponse != null)
            {
                emailValidationResponse.IsValid = !serviceResponse.Success;
                emailValidationResponse.ErrorMessage = serviceResponse.Message;
                emailValidationResponse.ParameterName = Constants.Users.EmailId;
            }

            return emailValidationResponse;
        }
    }
}
