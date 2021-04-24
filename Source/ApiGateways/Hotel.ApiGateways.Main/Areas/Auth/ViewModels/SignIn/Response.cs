using Hotel.ApiGateways.Main.Common.ViewModels;

namespace Hotel.ApiGateways.Main.Areas.Auth.ViewModels.SignIn
{
    public class SignInResponse : ApiResponse<SignInResponseData>
    {
        public SignInResponse(string code, string message, SignInResponseData data) : base(code, message, data)
        {
        }
    }
}