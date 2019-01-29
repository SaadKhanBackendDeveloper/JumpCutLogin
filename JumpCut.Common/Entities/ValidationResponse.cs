

namespace JumpCut.Common.Configurations
{
    public class ValidationResponseObject
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ValidationResponse
    {
        public System.Collections.Generic.List<ValidationResponseObject> validationResponseObjects;
        public bool IsSuccess;
        public string GenericErrorMessage { get; set; }

        public ValidationResponse()
        {
            validationResponseObjects = new System.Collections.Generic.List<ValidationResponseObject>();
        }
        
    }
}
