using Hotel.Common.Entities;
using Hotel.Services.Financial.Domain.Enums;

namespace Hotel.Services.Financial.Domain.Entities
{
    public class FinancialTransaction : Entity
    {
        private FinancialTransaction() { }

        public FinancialTransaction(string about, double amount, FinancialTransactionTypes type)
        {
            About = about;
            Amount = amount;
            Type = type;
        }

        public string About { get; set; }
        public double Amount { get; set; }
        public FinancialTransactionTypes Type { get; set; }
    }
}