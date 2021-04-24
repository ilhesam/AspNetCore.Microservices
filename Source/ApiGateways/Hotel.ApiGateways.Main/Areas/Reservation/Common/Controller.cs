using Hotel.ApiGateways.Main.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.ApiGateways.Main.Areas.Reservation.Common
{
    [Area("Reservation")]
    [Route("API/Areas/Reservation/[controller]")]
    [Authorize]
    public class ReservationAreaApiController : BaseApiController
    {

    }
}