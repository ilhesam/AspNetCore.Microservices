using System;
using Hotel.Common.Entities;

namespace Hotel.Services.Reservation.Domain.Entities
{
    public class RoomReserve : Entity
    {
        public RoomReserve(int roomNumber, string customerName, DateTime reservedFor)
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