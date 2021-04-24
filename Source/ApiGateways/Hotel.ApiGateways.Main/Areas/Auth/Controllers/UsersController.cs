using System.Threading.Tasks;
using Hotel.ApiGateways.Main.Areas.Auth.Common;
using Hotel.ApiGateways.Main.Areas.Auth.ViewModels.SignIn;
using Hotel.ApiGateways.Main.Areas.Auth.ViewModels.SignUp;
using Hotel.ApiGateways.Main.Common.Controllers;
using Hotel.ApiGateways.Main.Common.ViewModels;
using Hotel.Common.Features.Identity.Commands.SignIn;
using Hotel.Common.Features.Identity.Commands.SignUp;
using Hotel.Common.Models;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Hotel.Common.Features.Identity.Commands.SignIn.SignInResult;

namespace Hotel.ApiGateways.Main.Areas.Auth.Controllers
{
    public class UsersController : AuthAreaApiController
    {
        [HttpPost("[action]")]
        public IActionResult SignUp([FromBody] SignUpInput input)
        {
            ApiResponse response;

            var command = new SignUpCommand(input.UserName, input.Password);
            var result = RabbitMq.Publish<SignUpCommand, Result<SignUpResult>>(command);

            if (!result.IsSucceeded)
            {
                response = new ApiResponse(result.Error.Code, result.Error.Description);
                return BadRequest(response);
            }

            response = new ApiResponse("OK", "Success");
            return Ok(response);
        }

        [HttpPost("[action]")]
        public IActionResult SignIn([FromBody] SignInInput input)
        {
            SignInResponse response;
            SignInResponseData responseData;

            var command = new SignInCommand(input.UserName, input.Password);
            var result = RabbitMq.Publish<SignInCommand, Result<SignInResult>>(command);

            if (!result.IsSucceeded)
            {
                return BadRequest(new ApiResponse(result.Error.Code, result.Error.Description));
            }

            responseData = new SignInResponseData(result.Data.AccessToken, result.Data.RefreshToken);
            response = new SignInResponse("OK", "Success", responseData);
            return Ok(response);
        }
    }
}