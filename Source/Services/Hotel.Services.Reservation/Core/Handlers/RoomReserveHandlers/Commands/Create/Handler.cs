using System;
using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Reservation.RoomReserveFeatures.Commands.Create;
using Hotel.Common.Models;
using Hotel.Services.Reservation.Database.Repositories.Interfaces;
using Hotel.Services.Reservation.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hotel.Services.Reservation.Core.Handlers.RoomReserveHandlers.Commands.Create
{
    public class CreateRoomReserveHandler : IRequestHandler<CreateRoomReserveCommand, Result<CreateRoomReserveResult>>
    {
        private readonly IRoomReserveRepository _repository;
        private readonly ILogger<CreateRoomReserveHandler> _logger;

        public CreateRoomReserveHandler(IRoomReserveRepository repository, ILogger<CreateRoomReserveHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<CreateRoomReserveResult>> Handle(CreateRoomReserveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.AddAsync(
                    new RoomReserve(request.RoomNumber, request.CustomerName, request.ReservedFor));

                return Result<CreateRoomReserveResult>.Success(new CreateRoomReserveResult(entity.Id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result<CreateRoomReserveResult>.Fail(Error.Default());
            }
        }
    }
}