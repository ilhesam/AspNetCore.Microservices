using System.Threading.Tasks;
using Hotel.Services.Identity.Domain.Entities;

namespace Hotel.Services.Identity.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindUserAsync(User user);
        Task<User> FindByUserNameAsync(string userName);
        Task<User> AddAsync(User user);
        Task<bool> ExistsUserNameAsync(string userName);
        Task<bool> ExistsUserAsync(User user);
    }
}