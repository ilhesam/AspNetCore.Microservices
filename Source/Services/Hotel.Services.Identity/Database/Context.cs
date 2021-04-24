using Hotel.Common.MongoDb;
using Hotel.Services.Identity.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Hotel.Services.Identity.Database
{
    public class IdentityDbContext
    {
        private readonly IMongoDatabase _database;

        public IdentityDbContext(IOptions<MongoDbOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>(nameof(User));
        public IMongoCollection<UserSession> UserSessions => _database.GetCollection<UserSession>(nameof(UserSession));
    }
}