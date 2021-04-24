namespace Hotel.Common.Features.Identity.Commands.SignIn
{
    public class SignInResult
    {
        public SignInResult(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}