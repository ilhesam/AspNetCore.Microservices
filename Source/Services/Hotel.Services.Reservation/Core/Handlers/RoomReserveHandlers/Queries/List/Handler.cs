using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Reservation.RoomReserveFeatures.Queries.List;
using Hotel.Common.Models;
using Hotel.Services.Reservation.Database.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hotel.Services.Reservation.Core.Handlers.RoomReserveHandlers.Queries.List
{
    public class ListRoomReservesHandler : IRequestHandler<ListRoomReservesQuery, Result<ListRoomReservesResult>>
    {
        private readonly IRoomReserveRepository _repository;
        private readonly ILogger<ListRoomReservesHandler> _logger;

        public ListRoomReservesHandler(IRoomReserveRepository repository, ILogger<ListRoomReservesHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<ListRoomReservesResult>> Handle(ListRoomReservesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entities = await _repository.ListAsync();

                return Result<ListRoomReservesResult>.Success(new ListRoomReservesResult(entities.Cast<object>().ToList()));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result<ListRoomReservesResult>.Fail(Error.Default());
            }
        }
    }
}