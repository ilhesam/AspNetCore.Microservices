using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Hotel.Common.Extensions;
using Hotel.Services.Identity.Core.ConstantValues;
using Hotel.Services.Identity.Core.Models;
using Hotel.Services.Identity.Core.Options;
using Hotel.Services.Identity.Core.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.Services.Identity.Core.Services
{
    public class JwtService : IJwtService
    {
        protected readonly JwtOptions Options;

        public JwtService(IOptions<JwtOptions> options)
        {
            Options = options.Value;
        }

        public ClaimsPrincipal GetPrincipal(string accessToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Key));

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(accessToken);

            if (jwtToken == null) return null;

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Options.Issuer,
                ValidAudience = Options.Issuer,
                IssuerSigningKey = securityKey
            };

            var principal = tokenHandler.ValidateToken(accessToken, parameters, out _);
            return principal;
        }

        public string ValidateToken(string accessToken)
        {
            var principal = GetPrincipal(accessToken);

            if (principal == null) return null;

            ClaimsIdentity identity;

            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            var usernameClaim = identity?.FindFirst(ClaimTypes.Name);
            return usernameClaim?.Value;
        }

        public JsonWebToken GenerateJsonWebToken(IList<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var expClaimValue = claims.Single(c => c.Type.Equals(JwtRegisteredClaimNames.Exp)).Value;
            var refreshTokenExpClaimValue = claims.Single(c => c.Type.Equals(JwtClaimNames.RefreshTokenExp)).Value;

            var accessTokenExpiresAt = DateTime.Parse(expClaimValue);
            var refreshTokenExpiresAt = DateTime.Parse(refreshTokenExpClaimValue);

            var token = new JwtSecurityToken(
                issuer: Options.Issuer,
                audience: Options.Issuer,
                claims: claims,
                expires: accessTokenExpiresAt,
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            var generatedJwt = new JsonWebToken
            {
                AccessToken = encodeToken,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExpiresAt = refreshTokenExpiresAt
            };

            return generatedJwt;
        }

        public IList<Claim> GetJwtClaims(string uniqueName, DateTime authTime)
            => new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.UniqueName, uniqueName),
                new(JwtRegisteredClaimNames.AuthTime, authTime.ToString(CultureInfo.InvariantCulture)),
                new(JwtRegisteredClaimNames.Exp, authTime.AddMinutes(Options.AccessTokenExpirationInMinutes).ToString(CultureInfo.InvariantCulture)),
                new(JwtClaimNames.RefreshTokenExp, authTime.AddMinutes(Options.RefreshTokenExpirationInMinutes).ToString(CultureInfo.InvariantCulture))
            };

        public IList<Claim> GetJwtClaims(string uniqueName, DateTime authTime, List<Claim> otherClaims)
        {
            var claims = GetJwtClaims(uniqueName, authTime).ToList();
            claims.AddRange(otherClaims);
            return claims;
        }

        public DateTime GetAccessTokenExpirationTime() => DateTime.Now.AddMinutes(Options.AccessTokenExpirationInMinutes);

        public DateTime GetRefreshTokenExpirationTime() => DateTime.Now.AddMinutes(Options.RefreshTokenExpirationInMinutes);
    }
}