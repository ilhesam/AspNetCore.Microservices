using Hotel.Services.Identity.Core.Models;

namespace Hotel.Services.Identity.Core.Helpers
{
    public interface IIdentityErrorDescriber
    {
        IdentityError UserNameIsExist();
        IdentityError UserNotFound();
    }

    public class IdentityErrorDescriber : IIdentityErrorDescriber
    {
        public virtual IdentityError UserNameIsExist() => new(nameof(UserNameIsExist), "UserName is exist");
        public virtual IdentityError UserNotFound() => new(nameof(UserNotFound), "User not found");
    }
}