using System;
using Hotel.Common.Models;
using MediatR;

namespace Hotel.Common.Features.Reservation.RoomReserveFeatures.Commands.Create
{
    public class CreateRoomReserveCommand : IRequest<Result<CreateRoomReserveResult>>
    {
        public CreateRoomReserveCommand(int roomNumber, string customerName, DateTime reservedFor)
        {
            RoomNumber = roomNumber;
            CustomerName = customerName;
            ReservedFor = reservedFor;
        }

        public int RoomNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReservedFor { get; set; }
    }
}