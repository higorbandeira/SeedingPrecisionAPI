using System;
namespace SeedingPrecision.Models.Responses
{
    public class SignUpResponse
    {
        public SignUpResponse(string token, string userName)
        {
            Token = token;
            UserName = userName;
        }

        public string Token { get; private set; }

        public string UserName { get; private set; }
    }
}
