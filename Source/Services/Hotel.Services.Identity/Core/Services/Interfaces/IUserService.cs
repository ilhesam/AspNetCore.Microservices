using System.Threading.Tasks;
using Hotel.Common.Models;
using Hotel.Services.Identity.Core.Models;

namespace Hotel.Services.Identity.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> SignUpAsync(string userName, string password);
        Task<IdentityResult> CheckPasswordAsync(string userName, string password);
    }
}