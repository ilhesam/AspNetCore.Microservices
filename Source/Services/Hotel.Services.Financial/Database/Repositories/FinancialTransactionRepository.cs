using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.Services.Financial.Database.Repositories.Interfaces;
using Hotel.Services.Financial.Domain.Entities;
using MongoDB.Driver;

namespace Hotel.Services.Financial.Database.Repositories
{
    public class FinancialTransactionRepository : IFinancialTransactionRepository
    {
        protected readonly FinancialDbContext Db;

        public FinancialTransactionRepository(FinancialDbContext db)
        {
            Db = db;
        }

        public async Task<List<FinancialTransaction>> ListAsync()
            => await Db.FinancialTransactions.Find(book => true)
                .ToListAsync();

        public async Task<FinancialTransaction> AddAsync(FinancialTransaction user)
        {
            await Db.FinancialTransactions.InsertOneAsync(user);
            return user;
        }
    }
}