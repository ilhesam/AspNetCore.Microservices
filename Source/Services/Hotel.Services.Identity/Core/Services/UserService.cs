using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.Services.Identity.Core.Helpers;
using Hotel.Services.Identity.Core.Models;
using Hotel.Services.Identity.Core.Services.Interfaces;
using Hotel.Services.Identity.Database;
using Hotel.Services.Identity.Database.Repositories.Interfaces;
using Hotel.Services.Identity.Domain.Entities;

namespace Hotel.Services.Identity.Core.Services
{
    public class UserService : IUserService
    {
        protected readonly IdentityDbContext Db;
        protected readonly IUserRepository Repository;
        protected readonly IIdentityErrorDescriber ErrorDescriber;

        public UserService(IdentityDbContext db, IUserRepository repository, IIdentityErrorDescriber errorDescriber)
        {
            Db = db;
            Repository = repository;
            ErrorDescriber = errorDescriber;
        }

        public virtual async Task<IdentityResult> SignUpAsync(string userName, string password)
        {
            var errors = new List<IdentityError>();

            var existsUserName = await Repository.ExistsUserNameAsync(userName);

            if (existsUserName)
            {
                errors.Add(ErrorDescriber.UserNameIsExist());
                return IdentityResult.Fail(errors);
            }

            var user = new User(password)
            {
                UserName = userName
            };

            await Repository.AddAsync(user);
            return IdentityResult.Success();
        }

        public virtual async Task<IdentityResult> CheckPasswordAsync(string userName, string password)
        {
            var errors = new List<IdentityError>();

            var user = new User(password)
            {
                UserName = userName
            };

            var existsUser = await Repository.ExistsUserAsync(user);

            if (!existsUser)
            {
                errors.Add(ErrorDescriber.UserNotFound());
                return IdentityResult.Fail(errors);
            }
            
            return IdentityResult.Success();
        }
    }
}