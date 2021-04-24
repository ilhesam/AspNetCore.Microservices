using System.Threading.Tasks;
using Hotel.Services.Identity.Domain.Entities;

namespace Hotel.Services.Identity.Database.Repositories.Interfaces
{
    public interface IUserSessionRepository
    {
        Task<UserSession> FindAsync(string id);
        Task<UserSession> FindByAccessTokenAsync(string accessToken);
        Task<UserSession> AddAsync(UserSession userSession);
        Task<UserSession> UpdateAsync(UserSession userSession);
    }
}