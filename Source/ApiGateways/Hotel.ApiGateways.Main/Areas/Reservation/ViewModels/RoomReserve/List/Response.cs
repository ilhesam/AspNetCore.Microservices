using Hotel.ApiGateways.Main.Common.ViewModels;

namespace Hotel.ApiGateways.Main.Areas.Reservation.ViewModels.RoomReserve.List
{
    public class RoomReservesResponse : ApiResponse<RoomReservesResponseData>
    {
        public RoomReservesResponse(string code, string message, RoomReservesResponseData data) : base(code, message, data)
        {
        }
    }
}