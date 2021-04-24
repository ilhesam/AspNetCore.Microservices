using Hotel.Common.Models;
using MediatR;

namespace Hotel.Common.Features.Financial.FinancialTransactionFeatures.Queries.List
{
    public class ListFinancialTransactionsQuery : IRequest<Result<ListFinancialTransactionsResult>>
    {
        
    }
}