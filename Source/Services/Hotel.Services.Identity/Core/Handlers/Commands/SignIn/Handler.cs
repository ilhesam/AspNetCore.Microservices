using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Identity.Commands.SignIn;
using Hotel.Common.Models;
using Hotel.Services.Identity.Core.Handlers.Commands.SignUp;
using Hotel.Services.Identity.Core.Services.Interfaces;
using Hotel.Services.Identity.Database.Repositories.Interfaces;
using Hotel.Services.Identity.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hotel.Services.Identity.Core.Handlers.Commands.SignIn
{
    public class SignInHandler : IRequestHandler<SignInCommand, Result<SignInResult>>
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly ILogger<SignUpHandler> _logger;

        public SignInHandler(IJwtService jwtService, IUserService userService, IUserSessionRepository userSessionRepository, ILogger<SignUpHandler> logger)
        {
            _jwtService = jwtService;
            _userService = userService;
            _userSessionRepository = userSessionRepository;
            _logger = logger;
        }

        public async Task<Result<SignInResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkPassword = await _userService.CheckPasswordAsync(request.UserName, request.Password);

                if (!checkPassword.Succeeded)
                {
                    Error error;

                    if (checkPassword.Errors != null && checkPassword.Errors.Any())
                    {
                        var identityError = checkPassword.Errors.First();
                        error = new Error(identityError.Code, identityError.Description);
                    }
                    else
                    {
                        error = Error.Default();
                    }

                    return Result<SignInResult>.Fail(error);
                }

                var authTime = DateTime.Now;

                var claims = _jwtService.GetJwtClaims(request.UserName, authTime).ToList();
                var jwt = _jwtService.GenerateJsonWebToken(claims);

                var userSession = new UserSession(jwt.AccessToken, jwt.RefreshToken, request.UserName)
                {
                    AccessTokenExpiresAt = _jwtService.GetAccessTokenExpirationTime(),
                    RefreshTokenExpiresAt = _jwtService.GetRefreshTokenExpirationTime()
                };
                await _userSessionRepository.AddAsync(userSession);

                return Result<SignInResult>.Success(new SignInResult(jwt.AccessToken, jwt.RefreshToken));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result<SignInResult>.Fail(Error.Default());
            }
        }
    }
}