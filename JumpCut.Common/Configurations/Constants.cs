using System;
using System.Configuration;

namespace JumpCut.Common.Configurations
{
    public class Constants
    {
        public class Session
        {
            public static string UserId = "UserId";
            public static string FullName = "FullName";
            public static string EmailId = "EmailId";
        }
        public class Users
        {
            public static string UserId = "UserId";
            public static string FullName = "FullName";
            public static string UserPassword = "UserPassword";
            public static string ConfirmPassword = "ConfirmPassword";
            public static string EmailId = "EmailId";
            public static string RoleId = "RoleId";

        }

        public class ErrorMessages
        {
            public class Users
            {
                public static string FullName_Empty = "Full name is required";
                public static string FullName_MaximumLength = "Maximum allowed length for full name can be " + WebConfigKeys.UserNameMaxLength.ToString();

                public static string Email_Empty = "Valid email is required";
                public static string Email_MaximumLength = "Maximum allowed length for email can be " + WebConfigKeys.EmailMaxLength.ToString();
                public static string Email_Invalid = "Valid email is required";
            
                public static string Password_Empty = "Minimum length for password should be " + WebConfigKeys.PasswordMinimumLength.ToString();
                public static string Password_MinimumLength = "Minimum length for password should be " + WebConfigKeys.PasswordMinimumLength.ToString();
                public static string Password_MaximumLength = "Maximum length for password should be " + WebConfigKeys.PasswordMaximumLength.ToString();

                public static string Password_ConfirmPassword_MisMatch = "Password and confirm password should match each other";

                public static string SignUpError = "Some error occurred while creating your account. Please contact customer support for assistance. We appologize for this inconvenience.";

            }

        }

        public class WebConfigKeys
        {
            public static int UserNameMaxLength = Convert.ToInt32(ConfigurationManager.AppSettings["UserNameMaxLength"]);
            public static int EmailMaxLength = Convert.ToInt32(ConfigurationManager.AppSettings["EmailMaxLength"]);
            public static int PasswordMinimumLength = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordMinimumLength"]);
            public static int PasswordMaximumLength = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordMaximumLength"]);
            public static int ClientRoleId = Convert.ToInt32(ConfigurationManager.AppSettings["ClientRoleId"]);



        }

        public class WebAPI
        {
            public static string AccessToken = ConfigurationManager.AppSettings["JumpCutServiceAccessToken"];
            public static string JumpCutServiceURL = ConfigurationManager.AppSettings["JumpCutServiceURL"];
            /// <summary>
            /// This is the name of header parameter not the actual value
            /// </summary>
            public static string ContentTypeParameter = "content-type";
            /// <summary>
            /// This is the name of header parameter not the actual value
            /// </summary>
            public static string AuthorizationParameter = "Authorization";


            public class URLs
            {
                public static string CreateUser = ConfigurationManager.AppSettings["CreateUserURL"];
                public static string CheckDuplicateEmail = ConfigurationManager.AppSettings["CheckDuplicateEmailURL"];
                public static string Login = ConfigurationManager.AppSettings["LoginURL"];
                public static string ConfirmEmail = ConfigurationManager.AppSettings["ConfirmEmailURL"];
                public static string PasswordResetRequest = ConfigurationManager.AppSettings["PasswordResetRequestURL"];
                public static string PasswordReset = ConfigurationManager.AppSettings["PasswordResetURL"];


            }

            public class Email
            {
                public static string Authorization = ConfigurationManager.AppSettings["JumpCutEmailServiceAuthorization"];
                public static string URL = ConfigurationManager.AppSettings["JumpCutEmailServiceURL"];
                public static string ContentType = ConfigurationManager.AppSettings["JumpCutEmailServiceContentType"];
                public static string Body = "{\"to\":\"<<EmailTo>>\",\"content\":\"<<EmailBody>>\"}";
                public static string EmailJson = "EmailJson";

                public static string EmailToParameter = "to";
                public static string EmailBodyParameter = "content";

                //This is test email content. <a href='http://www.saadzafar.com'>Click Here</a>\"
            }
        }
    }
}
