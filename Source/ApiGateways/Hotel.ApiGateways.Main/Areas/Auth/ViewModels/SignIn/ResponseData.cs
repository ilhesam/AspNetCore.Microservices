namespace Hotel.ApiGateways.Main.Areas.Auth.ViewModels.SignIn
{
    public class SignInResponseData
    {
        public SignInResponseData(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}