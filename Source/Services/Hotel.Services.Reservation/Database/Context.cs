using Hotel.Common.MongoDb;
using Hotel.Services.Reservation.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Hotel.Services.Reservation.Database
{
    public class ReservationDbContext
    {
        private readonly IMongoDatabase _database;

        public ReservationDbContext(IOptions<MongoDbOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<RoomReserve> RoomReserves => _database.GetCollection<RoomReserve>(nameof(RoomReserve));
    }
}