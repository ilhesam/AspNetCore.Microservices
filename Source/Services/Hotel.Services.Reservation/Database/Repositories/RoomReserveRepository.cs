using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.Services.Reservation.Database.Repositories.Interfaces;
using Hotel.Services.Reservation.Domain.Entities;
using MongoDB.Driver;

namespace Hotel.Services.Reservation.Database.Repositories
{
    public class RoomReserveRepository : IRoomReserveRepository
    {
        protected readonly ReservationDbContext Db;

        public RoomReserveRepository(ReservationDbContext db)
        {
            Db = db;
        }

        public async Task<List<RoomReserve>> ListAsync()
            => await Db.RoomReserves.Find(book => true)
                .ToListAsync();

        public async Task<RoomReserve> AddAsync(RoomReserve user)
        {
            await Db.RoomReserves.InsertOneAsync(user);
            return user;
        }
    }
}