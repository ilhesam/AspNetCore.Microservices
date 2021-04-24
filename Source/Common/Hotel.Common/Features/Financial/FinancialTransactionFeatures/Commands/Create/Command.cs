using Hotel.Common.Models;
using MediatR;

namespace Hotel.Common.Features.Financial.FinancialTransactionFeatures.Commands.Create
{
    public class CreateFinancialTransactionCommand : IRequest<Result<CreateFinancialTransactionResult>>
    {
        public CreateFinancialTransactionCommand(string about, double amount, byte type)
        {
            About = about;
            Amount = amount;
            Type = type;
        }

        public string About { get; set; }
        public double Amount { get; set; }
        public byte Type { get; set; }
    }
}