using System;
using System.Collections.Generic;
using System.Security.Claims;
using Hotel.Services.Identity.Core.Models;

namespace Hotel.Services.Identity.Core.Services.Interfaces
{
    public interface IJwtService
    {
        ClaimsPrincipal GetPrincipal(string accessToken);
        string ValidateToken(string accessToken);
        JsonWebToken GenerateJsonWebToken(IList<Claim> claims);
        IList<Claim> GetJwtClaims(string uniqueName, DateTime authTime);
        IList<Claim> GetJwtClaims(string uniqueName, DateTime authTime, List<Claim> otherClaims);
        DateTime GetAccessTokenExpirationTime();
        DateTime GetRefreshTokenExpirationTime();
    }
}