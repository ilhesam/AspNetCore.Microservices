using System.Collections.Generic;
using Hotel.ApiGateways.Main.Areas.Reservation.Common;
using Hotel.ApiGateways.Main.Areas.Reservation.ViewModels.RoomReserve.Create;
using Hotel.ApiGateways.Main.Areas.Reservation.ViewModels.RoomReserve.Get;
using Hotel.ApiGateways.Main.Areas.Reservation.ViewModels.RoomReserve.List;
using Hotel.ApiGateways.Main.Common.ViewModels;
using Hotel.Common.Extensions;
using Hotel.Common.Features.Reservation.RoomReserveFeatures.Commands.Create;
using Hotel.Common.Features.Reservation.RoomReserveFeatures.Queries.List;
using Hotel.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.ApiGateways.Main.Areas.Reservation.Controllers
{
    public class RoomReservesController : ReservationAreaApiController
    {
        [HttpGet]
        public IActionResult List()
        {
            RoomReservesResponse response;
            RoomReservesResponseData responseData;
            List<RoomReserveDto> list;

            var query = new ListRoomReservesQuery();
            var result = RabbitMq.Publish<ListRoomReservesQuery, Result<ListRoomReservesResult>>(query);

            if (!result.IsSucceeded)
            {
                return BadRequest(new ApiResponse(result.Error.Code, result.Error.Description));
            }

            list = result.Data.RoomReserves.ConvertJsonObjectListTo<RoomReserveDto>();
            responseData = new RoomReservesResponseData(list);
            response = new RoomReservesResponse("OK", "Success", responseData);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateRoomReserveInput input)
        {
            ApiResponse response;

            var command = new CreateRoomReserveCommand(input.RoomNumber, input.CustomerName, input.ReservedFor);
            var result = RabbitMq.Publish<CreateRoomReserveCommand, Result<CreateRoomReserveResult>>(command);

            if (!result.IsSucceeded)
            {
                response = new ApiResponse(result.Error.Code, result.Error.Description);
                return BadRequest(response);
            }

            response = new ApiResponse("OK", "Success");
            return Ok(response);
        }
    }
}