using System;

namespace Hotel.ApiGateways.Main.Areas.Reservation.ViewModels.RoomReserve.Get
{
    public class RoomReserveDto
    {
        public int RoomNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReservedFor { get; set; }
    }
}