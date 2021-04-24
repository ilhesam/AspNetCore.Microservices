using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Identity.Commands.SignUp;
using Hotel.Common.Models;
using Hotel.Common.RabbitMq.Manager;
using Hotel.Services.Identity.Core.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace Hotel.Services.Identity.Core.Handlers.Commands.SignUp
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, Result<SignUpResult>>
    {
        private readonly IUserService _userService;
        private readonly ILogger<SignUpHandler> _logger;

        public SignUpHandler(IUserService userService, ILogger<SignUpHandler> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<Result<SignUpResult>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userService.SignUpAsync(request.UserName, request.Password);

                if (result.Succeeded)
                {
                    return Result<SignUpResult>.Success(new SignUpResult());
                }

                Error error;

                if (result.Errors != null && result.Errors.Any())
                {
                    var identityError = result.Errors.First();
                    error = new Error(identityError.Code, identityError.Description);
                }
                else
                {
                    error = Error.Default();
                }

                return Result<SignUpResult>.Fail(error);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result<SignUpResult>.Fail(Error.Default());
            }
        }
    }
}