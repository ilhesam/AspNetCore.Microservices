using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.Services.Reservation.Domain.Entities;

namespace Hotel.Services.Reservation.Database.Repositories.Interfaces
{
    public interface IRoomReserveRepository
    {
        Task<List<RoomReserve>> ListAsync();
        Task<RoomReserve> AddAsync(RoomReserve user);
    }
}