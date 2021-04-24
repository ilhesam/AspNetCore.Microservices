using System;
using Hotel.Common.Entities;
using Hotel.Common.Extensions;

namespace Hotel.Services.Identity.Domain.Entities
{
    public class UserSession : Entity
    {
        private UserSession() { }

        public string AccessTokenHash { get; private set; }

        public UserSession(string accessToken, string refreshToken, string userName)
        {
            AccessTokenHash = HashToken(accessToken);
            RefreshTokenHash = HashToken(refreshToken);
            CreatedByUserName = userName;
        }

        public DateTime AccessTokenExpiresAt { get; init; }
        public bool Expired { get; private set; }

        public string RefreshTokenHash { get; private set; }
        public DateTime RefreshTokenExpiresAt { get; init; }

        public bool Refreshed { get; private set; }
        public DateTime? RefreshedAt { get; private set; }

        public string CreatedByUserName { get; private set; }

        public static string HashToken(string token) => token.ComputeSha256Hash();

        public bool IsRefreshable(string accessToken, string refreshToken)
        {
            var accessTokenHash = HashToken(accessToken);
            var refreshTokenHash = HashToken(refreshToken);

            if (AccessTokenHash != accessTokenHash)
            {
                return false;
            }

            if (RefreshTokenHash != refreshTokenHash)
            {
                return false;
            }

            if (Refreshed)
            {
                return false;
            }

            if (CreatedByUserName == null)
            {
                return false;
            }

            return true;
        }

        public bool IsActive(string accessToken)
        {
            var accessTokenHash = HashToken(accessToken);

            if (AccessTokenHash != accessTokenHash)
            {
                return false;
            }

            if (AccessTokenExpiresAt.CompareTo(DateTime.Now) == -1)
            {
                return false;
            }

            if (Expired)
            {
                return false;
            }

            if (Refreshed)
            {
                return false;
            }

            if (CreatedByUserName == null)
            {
                return false;
            }

            return true;
        }

        public void Refresh()
        {
            Refreshed = true;
            RefreshedAt = DateTime.Now;
        }

        public void Expire()
        {
            Expired = true;
        }
    }
}