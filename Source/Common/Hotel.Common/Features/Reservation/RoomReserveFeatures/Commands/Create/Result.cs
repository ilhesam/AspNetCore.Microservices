namespace Hotel.Common.Features.Reservation.RoomReserveFeatures.Commands.Create
{
    public class CreateRoomReserveResult
    {
        public CreateRoomReserveResult(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}