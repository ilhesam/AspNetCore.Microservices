using System;
using System.Threading.Tasks;
using Hotel.Services.Identity.Database.Repositories.Interfaces;
using Hotel.Services.Identity.Domain.Entities;
using MongoDB.Driver;

namespace Hotel.Services.Identity.Database.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        protected readonly IdentityDbContext Db;

        public UserSessionRepository(IdentityDbContext db)
        {
            Db = db;
        }

        public virtual async Task<UserSession> FindAsync(string id)
            => await (await Db.UserSessions.FindAsync(session => session.Id.Equals(id)))
                .SingleOrDefaultAsync();

        public virtual async Task<UserSession> FindByAccessTokenAsync(string accessToken)
        {
            var accessTokenHash = UserSession.HashToken(accessToken);

            return await (await Db.UserSessions.FindAsync(session => session.AccessTokenHash.Equals(accessTokenHash)))
                .SingleOrDefaultAsync();
        }

        public virtual async Task<UserSession> AddAsync(UserSession userSession)
        {
            await Db.UserSessions.InsertOneAsync(userSession);
            return userSession;
        }

        public virtual async Task<UserSession> UpdateAsync(UserSession userSession)
        {
            await Db.UserSessions.ReplaceOneAsync(session => session.Id == userSession.Id, userSession);
            return userSession;
        }
    }
}