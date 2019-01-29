using JumpCut.Common.Entities;
using System;


namespace JumpCut.Common.Configurations
{
    [Serializable]
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public Users User = new Users();

        public ServiceResponse()
        {
        }

    }

}
