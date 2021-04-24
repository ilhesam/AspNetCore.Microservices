using Hotel.ApiGateways.Main.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.ApiGateways.Main.Areas.Financial.Common
{
    [Area("Financial")]
    [Route("API/Areas/Financial/[controller]")]
    [Authorize]
    public class FinancialAreaApiController : BaseApiController
    {
        
    }
}