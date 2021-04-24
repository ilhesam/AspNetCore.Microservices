using System;
using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Financial.FinancialTransactionFeatures.Commands.Create;
using Hotel.Common.Models;
using Hotel.Services.Financial.Database.Repositories.Interfaces;
using Hotel.Services.Financial.Domain.Entities;
using Hotel.Services.Financial.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hotel.Services.Financial.Core.Handlers.FinancialTransactionHandlers.Commands.Create
{
    public class CreateFinancialTransactionHandler : IRequestHandler<CreateFinancialTransactionCommand, Result<CreateFinancialTransactionResult>>
    {
        private readonly IFinancialTransactionRepository _repository;
        private readonly ILogger<CreateFinancialTransactionHandler> _logger;

        public CreateFinancialTransactionHandler(IFinancialTransactionRepository repository, ILogger<CreateFinancialTransactionHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<CreateFinancialTransactionResult>> Handle(CreateFinancialTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.AddAsync(
                    new FinancialTransaction(request.About, request.Amount, (FinancialTransactionTypes) request.Type));

                return Result<CreateFinancialTransactionResult>.Success(new CreateFinancialTransactionResult(entity.Id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result<CreateFinancialTransactionResult>.Fail(Error.Default());
            }
        }
    }
}