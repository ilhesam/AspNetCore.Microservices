using Microsoft.AspNetCore.Http;

namespace Hotel.ApiGateways.Main.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetBearerToken(this HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            string token = null;

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                token = authHeader.Substring("Bearer ".Length);
            }

            return token;
        }
    }
}