using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.Services.Financial.Domain.Entities;

namespace Hotel.Services.Financial.Database.Repositories.Interfaces
{
    public interface IFinancialTransactionRepository
    {
        Task<List<FinancialTransaction>> ListAsync();
        Task<FinancialTransaction> AddAsync(FinancialTransaction user);
    }
}