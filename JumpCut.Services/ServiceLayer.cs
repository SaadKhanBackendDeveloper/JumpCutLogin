using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using JumpCut.Common.Configurations;

using RestSharp;
using Newtonsoft.Json;

namespace JumpCut.Services
{
    public class ServiceLayer
    { 
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

            headerKeys.Add("email");
            headerElements.Add(new KeyValuePair<string, string>("email", email));

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

        public ServiceResponse CreateNewUser(string name, string email, string password, string currentURL)
        {
            bool newUserCreated = false;
            string passwordHash = string.Empty;
            string baseURL = string.Empty;
            string emailVerificationKey = Guid.NewGuid().ToString();
            ServiceResponse responseToController = new ServiceResponse();
            responseToController.Success = true;
            password = this.ComputeSha256Hash(password);

            List<KeyValuePair<string, string>> headerElements = new List<KeyValuePair<string, string>>();
            List<string> headerKeys = new List<string>();

            List<KeyValuePair<string, string>> bodyElements = new List<KeyValuePair<string, string>>();
            List<string> bodyKeys = new List<string>();

            headerKeys.Add("Authorization");
            headerElements.Add(new KeyValuePair<string, string>("Authorization", Constants.WebAPI.AccessToken));

            headerKeys.Add("Name");
            headerElements.Add(new KeyValuePair<string, string>("Name", name));

            headerKeys.Add("Email");
            headerElements.Add(new KeyValuePair<string, string>("Email", email));

            headerKeys.Add("Password");
            headerElements.Add(new KeyValuePair<string, string>("Password", password));

            headerKeys.Add("RoleId");
            headerElements.Add(new KeyValuePair<string, string>("RoleId", Convert.ToString(Constants.WebConfigKeys.ClientRoleId)));

            headerKeys.Add("EmailVerificationKey");
            headerElements.Add(new KeyValuePair<string, string>("EmailVerificationKey", emailVerificationKey));

            headerKeys.Add("content-type");
            headerElements.Add(new KeyValuePair<string, string>("content-type", "application/json"));

            string createNewUserURL = Constants.WebAPI.URLs.CreateUser;
            baseURL = Common.Configurations.Constants.WebAPI.JumpCutServiceURL;

            //Insert credentials into database for new user
            string createUserAPIResponse = this.SendPostRequest(baseURL + createNewUserURL, Method.POST, headerElements, headerKeys, bodyElements, bodyKeys, "application/json");

            ServiceResponse createUserResponseObject = new ServiceResponse();
            createUserResponseObject = JsonConvert.DeserializeObject<ServiceResponse>(createUserAPIResponse);

            if (createUserResponseObject != null)
            {
                if (createUserResponseObject.Success)
                {
                    //TODO: Send email for verification
                    string jumpCutEmailServiceURL = Constants.WebAPI.Email.URL;
                    string emailTo = email;
                    string emailBody = "<h1>Thanks for joining us. We are happy that you are here </h1><a href='" + currentURL + "/User/Signup?confirmToken=" + emailVerificationKey + "' target='_blank'>Click Here</a> so that your email address can be verified:<br/><br/> After verifying your email you are all set to go on. You can <a href='" + currentURL + "/User/Login' target='_blank'> Click Here To Login</a><br/><br/> Once again we welcome you onboard and wishing you success ahead.<br/><b>Team JumpCut</b>";

                    headerElements = null;
                    headerKeys = null;
                    bodyElements = null;
                    bodyKeys = null;
                    headerElements = new List<KeyValuePair<string, string>>();
                    headerKeys = new List<string>();
                    bodyElements = new List<KeyValuePair<string, string>>();
                    bodyKeys = new List<string>();

                    headerKeys.Add(Constants.WebAPI.ContentTypeParameter);
                    headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.ContentTypeParameter, Constants.WebAPI.Email.ContentType));

                    headerKeys.Add(Constants.WebAPI.AuthorizationParameter);
                    headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.AuthorizationParameter, Constants.WebAPI.Email.Authorization));

                    bodyKeys.Add(Constants.WebAPI.Email.EmailToParameter);
                    bodyElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.Email.EmailToParameter, emailTo));

                    bodyKeys.Add(Constants.WebAPI.Email.EmailBodyParameter);
                    bodyElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.Email.EmailBodyParameter, emailBody));

                    string createUserEmailServiceResponse = this.SendPostRequest(Constants.WebAPI.Email.URL, Method.POST, headerElements, headerKeys, bodyElements, bodyKeys, Constants.WebAPI.Email.ContentType);

                    dynamic emailServiceResponse = JsonConvert.DeserializeObject(createUserEmailServiceResponse);

                    if (Convert.ToBoolean(emailServiceResponse.success))
                    {
                        //Email sent. Send success message to user.
                        responseToController.Success = emailServiceResponse.success;
                        responseToController.Message = "User sccessfully created. Please verify email so that you can login.";
                    }
                    else
                    {
                        //Email not sent. Send error message to user.
                        responseToController.Success = emailServiceResponse.success;
                        responseToController.Message = "Some error occurred while creating your account. Please try again or contact support with support code: 92";

                    }
                }
                else
                {
                    responseToController.Success = false;
                    responseToController.Message = createUserResponseObject.Message;
                }
            }
            else
            {
                responseToController.Success = false;
                responseToController.Message = "Error occurred while interacting with API.";

            }


            return responseToController;
        }

        public ServiceResponse Login(string email, string password)
        {
            bool newUserCreated = false;
            string passwordHash = string.Empty;
            string baseURL = string.Empty;
            string emailVerificationKey = Guid.NewGuid().ToString();
            ServiceResponse responseToController = new ServiceResponse();
            responseToController.Success = true;
            password = this.ComputeSha256Hash(password);

            List<KeyValuePair<string, string>> headerElements = new List<KeyValuePair<string, string>>();
            List<string> headerKeys = new List<string>();

            List<KeyValuePair<string, string>> bodyElements = new List<KeyValuePair<string, string>>();
            List<string> bodyKeys = new List<string>();

            headerKeys.Add(Constants.WebAPI.AuthorizationParameter);
            headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.AuthorizationParameter, Constants.WebAPI.AccessToken));

            headerKeys.Add("Email");
            headerElements.Add(new KeyValuePair<string, string>("Email", email));

            headerKeys.Add("Password");
            headerElements.Add(new KeyValuePair<string, string>("Password", password));

            headerKeys.Add(Constants.WebAPI.ContentTypeParameter);
            headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.ContentTypeParameter, Constants.WebAPI.Email.ContentType));

            string createNewUserURL = Constants.WebAPI.URLs.Login;
            baseURL = Common.Configurations.Constants.WebAPI.JumpCutServiceURL;

            //Insert credentials into database for new user
            string loginAPIResponse = this.SendPostRequest(baseURL + createNewUserURL, Method.POST, headerElements, headerKeys, bodyElements, bodyKeys, "application/json");

            ServiceResponse loginResponseObject = new ServiceResponse();
            loginResponseObject = JsonConvert.DeserializeObject<ServiceResponse>(loginAPIResponse);

            if (loginResponseObject != null)
            {
                if (loginResponseObject.Success)
                {
                    responseToController.User = loginResponseObject.User;
                    responseToController.Success = loginResponseObject.Success;
                    responseToController.Message = loginResponseObject.Message;
                }
            }
            else
            {
                responseToController.Success = false;
                responseToController.Message = "Error occurred while signing you in. Please try again in few moments";
            }

            return responseToController;
        }

        public ServiceResponse ConfirmEmail(string confirmToken)
        {
            bool newUserCreated = false;
            string passwordHash = string.Empty;
            string baseURL = string.Empty;
            string emailVerificationKey = Guid.NewGuid().ToString();
            ServiceResponse responseToController = new ServiceResponse();
            responseToController.Success = true;

            List<KeyValuePair<string, string>> headerElements = new List<KeyValuePair<string, string>>();
            List<string> headerKeys = new List<string>();

            List<KeyValuePair<string, string>> bodyElements = new List<KeyValuePair<string, string>>();
            List<string> bodyKeys = new List<string>();

            headerKeys.Add(Constants.WebAPI.AuthorizationParameter);
            headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.AuthorizationParameter, Constants.WebAPI.AccessToken));

            headerKeys.Add(Constants.WebAPI.ContentTypeParameter);
            headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.ContentTypeParameter, Constants.WebAPI.Email.ContentType));

            headerKeys.Add("ConfirmToken");
            headerElements.Add(new KeyValuePair<string, string>("ConfirmToken", confirmToken));

            string createNewUserURL = Constants.WebAPI.URLs.ConfirmEmail;
            baseURL = Common.Configurations.Constants.WebAPI.JumpCutServiceURL;

            //Insert credentials into database for new user
            string confirmEmailAPIResponse = this.SendPostRequest(baseURL + createNewUserURL, Method.POST, headerElements, headerKeys, bodyElements, bodyKeys, "application/json");

            ServiceResponse loginResponseObject = new ServiceResponse();
            loginResponseObject = JsonConvert.DeserializeObject<ServiceResponse>(confirmEmailAPIResponse);

            if (loginResponseObject != null)
            {
                if (loginResponseObject.Success)
                {
                    responseToController.User = loginResponseObject.User;
                }
            }

            responseToController.Success = loginResponseObject.Success;
            responseToController.Message = loginResponseObject.Message;

            return responseToController;
        }

        public ServiceResponse PasswordResetRequest(string email, string currentURL)
        {
            bool newPasswordSent = false;
            string passwordHash = string.Empty;
            string baseURL = string.Empty;
            string passwordResetKey = Guid.NewGuid().ToString();
            ServiceResponse responseToController = new ServiceResponse();
            responseToController.Success = true;
            
            List<KeyValuePair<string, string>> headerElements = new List<KeyValuePair<string, string>>();
            List<string> headerKeys = new List<string>();

            List<KeyValuePair<string, string>> bodyElements = new List<KeyValuePair<string, string>>();
            List<string> bodyKeys = new List<string>();

            headerKeys.Add(Constants.WebAPI.AuthorizationParameter);
            headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.AuthorizationParameter, Constants.WebAPI.AccessToken));

            headerKeys.Add(Constants.WebAPI.ContentTypeParameter);
            headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.ContentTypeParameter, Constants.WebAPI.Email.ContentType));

            headerKeys.Add("Email");
            headerElements.Add(new KeyValuePair<string, string>("Email", email));

            headerKeys.Add("PasswordResetKey");
            headerElements.Add(new KeyValuePair<string, string>("PasswordResetKey", passwordResetKey));

            string passwordResetURL = Constants.WebAPI.URLs.PasswordResetRequest;
            baseURL = Common.Configurations.Constants.WebAPI.JumpCutServiceURL;

            //Insert credentials into database for new user
            string passwordResetAPIResponse = this.SendPostRequest(baseURL + passwordResetURL, Method.POST, headerElements, headerKeys, bodyElements, bodyKeys, Constants.WebAPI.Email.ContentType);

            ServiceResponse passwordResetResponseObject = new ServiceResponse();
            passwordResetResponseObject = JsonConvert.DeserializeObject<ServiceResponse>(passwordResetAPIResponse);

            if (passwordResetResponseObject != null)
            {
                if (passwordResetResponseObject.Success)
                {
                    //TODO: Send email for verification
                    string jumpCutEmailServiceURL = Constants.WebAPI.Email.URL;
                    string emailTo = email;
                    string emailBody = "<h1>Forgot Password? No worries, we got you covered. </h1><a href='" + currentURL + "/User/PasswordReset?resetToken=" + passwordResetKey+ "' target='_blank'>Click Here To Reset</a> your Password.<br/><br/> This link will be valid for 24 hours. If you want to login <a href='" + currentURL + "/User/Login' target='_blank'> Click Here </a><br/><br/> if you need any assistance feel free to contact us by replying to this email.<br/><b>Team JumpCut</b>";
                        
                    headerElements = null;
                    headerKeys = null;
                    bodyElements = null;
                    bodyKeys = null;
                    headerElements = new List<KeyValuePair<string, string>>();
                    headerKeys = new List<string>();
                    bodyElements = new List<KeyValuePair<string, string>>();
                    bodyKeys = new List<string>();

                    headerKeys.Add(Constants.WebAPI.ContentTypeParameter);
                    headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.ContentTypeParameter, Constants.WebAPI.Email.ContentType));

                    headerKeys.Add(Constants.WebAPI.AuthorizationParameter);
                    headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.AuthorizationParameter, Constants.WebAPI.Email.Authorization));

                    bodyKeys.Add(Constants.WebAPI.Email.EmailToParameter);
                    bodyElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.Email.EmailToParameter, emailTo));

                    bodyKeys.Add(Constants.WebAPI.Email.EmailBodyParameter);
                    bodyElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.Email.EmailBodyParameter, emailBody));

                    string createUserEmailServiceResponse = this.SendPostRequest(Constants.WebAPI.Email.URL, Method.POST, headerElements, headerKeys, bodyElements, bodyKeys, Constants.WebAPI.Email.ContentType);

                    dynamic emailServiceResponse = JsonConvert.DeserializeObject(createUserEmailServiceResponse);

                    if (Convert.ToBoolean(emailServiceResponse.success))
                    {
                        //Email sent. Send success message to user.
                        responseToController.Success = emailServiceResponse.success;
                        responseToController.Message = passwordResetResponseObject.Message;
                    }
                    else
                    {
                        //Email not sent. Send error message to user.
                        responseToController.Success = emailServiceResponse.success;
                        responseToController.Message = "We have encountered a problem while resetting your account password. Please try again or contact support with code: 93";

                    }
                }
                else
                {
                    responseToController.Success = false;
                    responseToController.Message = passwordResetResponseObject.Message;
                }
            }
            else
            {
                responseToController.Success = false;
                responseToController.Message = "We have encountered a problem while resetting your account password. Please try again or contact support with code: 94";

            }


            return responseToController;
        }

        public ServiceResponse PasswordReset(string email, string password, string confirmPassword, string passwordResetKey, string currentURL)
        {
            bool newPasswordUpdated = false;
            string passwordHash = string.Empty;
            string baseURL = string.Empty;

            ServiceResponse responseToController = new ServiceResponse();
            responseToController.Success = true;

            List<KeyValuePair<string, string>> headerElements = new List<KeyValuePair<string, string>>();
            List<string> headerKeys = new List<string>();

            List<KeyValuePair<string, string>> bodyElements = new List<KeyValuePair<string, string>>();
            List<string> bodyKeys = new List<string>();

            password = this.ComputeSha256Hash(password);
            confirmPassword = this.ComputeSha256Hash(confirmPassword);

            headerKeys.Add(Constants.WebAPI.AuthorizationParameter);
            headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.AuthorizationParameter, Constants.WebAPI.AccessToken));

            headerKeys.Add(Constants.WebAPI.ContentTypeParameter);
            headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.ContentTypeParameter, Constants.WebAPI.Email.ContentType));

            headerKeys.Add("Email");
            headerElements.Add(new KeyValuePair<string, string>("Email", email));

            headerKeys.Add("Password");
            headerElements.Add(new KeyValuePair<string, string>("Password", password));

            headerKeys.Add("ConfirmPassword");
            headerElements.Add(new KeyValuePair<string, string>("ConfirmPassword", confirmPassword));

            headerKeys.Add("PasswordResetKey");
            headerElements.Add(new KeyValuePair<string, string>("PasswordResetKey", passwordResetKey));

            string passwordResetURL = Constants.WebAPI.URLs.PasswordReset;
            baseURL = Common.Configurations.Constants.WebAPI.JumpCutServiceURL;

            //Insert credentials into database for new user
            string passwordResetAPIResponse = this.SendPostRequest(baseURL + passwordResetURL, Method.POST, headerElements, headerKeys, bodyElements, bodyKeys, Constants.WebAPI.Email.ContentType);

            ServiceResponse passwordResetResponseObject = new ServiceResponse();
            passwordResetResponseObject = JsonConvert.DeserializeObject<ServiceResponse>(passwordResetAPIResponse);

            if (passwordResetResponseObject != null)
            {
                if (passwordResetResponseObject.Success)
                {
                    //TODO: Send email for verification
                    string jumpCutEmailServiceURL = Constants.WebAPI.Email.URL;
                    string emailTo = email;
                    string emailBody = "<h1>Password Successfully Updated</h1>Your password is successfully updated.<br/><br/> Please <a href='" + currentURL + "/User/Login' target='_blank'> Click Here </a> to login with your new password <br/><br/> if you need any assistance feel free to contact us by replying to this email.<br/><b>Team JumpCut</b>";

                    headerElements = null;
                    headerKeys = null;
                    bodyElements = null;
                    bodyKeys = null;
                    headerElements = new List<KeyValuePair<string, string>>();
                    headerKeys = new List<string>();
                    bodyElements = new List<KeyValuePair<string, string>>();
                    bodyKeys = new List<string>();

                    headerKeys.Add(Constants.WebAPI.ContentTypeParameter);
                    headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.ContentTypeParameter, Constants.WebAPI.Email.ContentType));

                    headerKeys.Add(Constants.WebAPI.AuthorizationParameter);
                    headerElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.AuthorizationParameter, Constants.WebAPI.Email.Authorization));

                    bodyKeys.Add(Constants.WebAPI.Email.EmailToParameter);
                    bodyElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.Email.EmailToParameter, emailTo));

                    bodyKeys.Add(Constants.WebAPI.Email.EmailBodyParameter);
                    bodyElements.Add(new KeyValuePair<string, string>(Constants.WebAPI.Email.EmailBodyParameter, emailBody));

                    string createUserEmailServiceResponse = this.SendPostRequest(Constants.WebAPI.Email.URL, Method.POST, headerElements, headerKeys, bodyElements, bodyKeys, Constants.WebAPI.Email.ContentType);

                    dynamic emailServiceResponse = JsonConvert.DeserializeObject(createUserEmailServiceResponse);

                    if (Convert.ToBoolean(emailServiceResponse.success))
                    {
                        //Email sent. Send success message to user.
                        responseToController.Success = emailServiceResponse.success;
                        responseToController.Message = passwordResetResponseObject.Message;
                    }
                    else
                    {
                        //Email not sent. Send error message to user.
                        responseToController.Success = emailServiceResponse.success;
                        responseToController.Message = "We have encountered a problem while resetting your account password. Please try again or contact support with code: 93";

                    }
                }
                else
                {
                    responseToController.Success = false;
                    responseToController.Message = passwordResetResponseObject.Message;
                }
            }
            else
            {
                responseToController.Success = false;
                responseToController.Message = "We have encountered a problem while resetting your account password. Please try again or contact support with code: 94";

            }


            return responseToController;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string SendPostRequest(string url, RestSharp.Method methodType, List<KeyValuePair<string, string>> headerElements, List<string> headerKeys, List<KeyValuePair<string, string>> bodyElements, List<string> bodyKeys, string contentType)
        {
            string responseString = string.Empty;
            RestClient restClient = new RestClient();
            string jsonBody = string.Empty;

            //Assign Target URL
            restClient = new RestClient(url);

            //Create RestRequest and Assign Method Type
            RestRequest restRequest = new RestRequest(methodType);

            //Assign Header Keys
            if (headerKeys.Count > 0)
            {
                foreach (string headerKey in headerKeys)
                {
                    restRequest.AddHeader(headerKey, headerElements.Where(x => x.Key.Equals(headerKey)).ElementAt(0).Value);
                }
            }

            if (bodyKeys.Count > 0)
            {
                jsonBody = "{";
                int loopCounter = 1;
                foreach (string bodyKey in bodyKeys)
                {
                    if (loopCounter == 1)
                    {
                        jsonBody = jsonBody + "\"" + bodyKey + "\":\"" + bodyElements.Where(x => x.Key.Equals(bodyKey)).ElementAt(0).Value + "\",";
                    }

                    if (loopCounter > 1 && (loopCounter < bodyKeys.Count))
                    {
                        jsonBody = jsonBody + "\"" + bodyKey + "\":\"" + bodyElements.Where(x => x.Key.Equals(bodyKey)).ElementAt(0).Value + "\",";

                    }

                    if (loopCounter == bodyKeys.Count)
                    {
                        jsonBody = jsonBody + "\"" + bodyKey + "\":\"" + bodyElements.Where(x => x.Key.Equals(bodyKey)).ElementAt(0).Value + "\"";
                    }

                    loopCounter = loopCounter + 1;
                }
                jsonBody = jsonBody + "}";

                //restRequest.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

                restRequest.AddJsonBody(jsonBody);
            }

            IRestResponse response = restClient.Execute(restRequest);

            return responseString = response.Content;
        }


    }
}
