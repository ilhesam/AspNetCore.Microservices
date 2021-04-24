using System.Collections.Generic;
using Hotel.ApiGateways.Main.Areas.Reservation.ViewModels.RoomReserve.Get;
using Hotel.ApiGateways.Main.Common.ViewModels;

namespace Hotel.ApiGateways.Main.Areas.Reservation.ViewModels.RoomReserve.List
{
    public class RoomReservesResponseData
    {
        public RoomReservesResponseData(List<RoomReserveDto> roomReserves)
        {
            RoomReserves = roomReserves;
        }

        public List<RoomReserveDto> RoomReserves { get; set; }
    }
}