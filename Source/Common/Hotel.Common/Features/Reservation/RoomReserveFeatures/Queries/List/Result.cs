using System.Collections.Generic;

namespace Hotel.Common.Features.Reservation.RoomReserveFeatures.Queries.List
{
    public class ListRoomReservesResult
    {
        public ListRoomReservesResult(List<object> roomReserves)
        {
            RoomReserves = roomReserves;
        }

        public List<object> RoomReserves { get; set; }
    }
}