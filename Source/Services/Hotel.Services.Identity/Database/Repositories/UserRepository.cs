using System.Threading.Tasks;
using Hotel.Services.Identity.Database.Repositories.Interfaces;
using Hotel.Services.Identity.Domain.Entities;
using MongoDB.Driver;

namespace Hotel.Services.Identity.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly IdentityDbContext Db;

        public UserRepository(IdentityDbContext db)
        {
            Db = db;
        }

        public async Task<User> FindUserAsync(User user)
            => await (await Db.Users.FindAsync(u => u.UserName.Equals(user.UserName) && u.PasswordHash.Equals(user.PasswordHash)))
                .SingleOrDefaultAsync();

        public virtual async Task<User> FindByUserNameAsync(string userName)
            => await (await Db.Users.FindAsync(user => user.UserName.Equals(userName)))
                .SingleOrDefaultAsync();

        public virtual async Task<User> AddAsync(User user)
        {
            await Db.Users.InsertOneAsync(user);
            return user;
        }

        public virtual async Task<bool> ExistsUserNameAsync(string userName)
            => await FindByUserNameAsync(userName) != null;

        public async Task<bool> ExistsUserAsync(User user)
            => await FindUserAsync(user) != null;
    }
}