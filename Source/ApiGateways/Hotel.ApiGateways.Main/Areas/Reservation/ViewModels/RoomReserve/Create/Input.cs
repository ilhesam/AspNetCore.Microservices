using System;
using FluentValidation;

namespace Hotel.ApiGateways.Main.Areas.Reservation.ViewModels.RoomReserve.Create
{
    public class CreateRoomReserveInput
    {
        public int RoomNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReservedFor { get; set; }
    }

    public class CreateRoomReserveValidator : AbstractValidator<CreateRoomReserveInput>
    {
        public CreateRoomReserveValidator()
        {
            RuleFor(e => e.RoomNumber)
                .GreaterThan(0);

            RuleFor(e => e.CustomerName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(e => e.ReservedFor)
                .NotNull();
        }
    }
}