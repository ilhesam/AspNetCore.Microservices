using Hotel.Common.MongoDb;
using Hotel.Services.Financial.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Hotel.Services.Financial.Database
{
    public class FinancialDbContext
    {
        private readonly IMongoDatabase _database;

        public FinancialDbContext(IOptions<MongoDbOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<FinancialTransaction> FinancialTransactions => _database.GetCollection<FinancialTransaction>(nameof(FinancialTransaction));
    }
}