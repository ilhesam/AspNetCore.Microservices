using System;
using System.Threading;
using System.Threading.Tasks;
using Hotel.Common.Features.Identity.Commands.ValidateToken;
using Hotel.Common.Models;
using Hotel.Services.Identity.Core.Services;
using Hotel.Services.Identity.Core.Services.Interfaces;
using Hotel.Services.Identity.Database.Repositories.Interfaces;
using Hotel.Services.Identity.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hotel.Services.Identity.Core.Handlers.Commands.ValidateToken
{
    public class ValidateTokenHandler : IRequestHandler<ValidateTokenCommand, Result<ValidateTokenResult>>
    {
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<ValidateTokenHandler> _logger;

        public ValidateTokenHandler(IUserSessionRepository userSessionRepository, IJwtService jwtService, ILogger<ValidateTokenHandler> logger)
        {
            _userSessionRepository = userSessionRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<Result<ValidateTokenResult>> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validate = _jwtService.ValidateToken(request.AccessToken);

                if (validate == null)
                {
                    return Result<ValidateTokenResult>.Fail(new Error(nameof(JwtService.ValidateToken), "Token is not valid"));
                }

                var userSession = await _userSessionRepository.FindByAccessTokenAsync(request.AccessToken);

                if (userSession == null || !userSession.IsActive(request.AccessToken))
                {
                    return Result<ValidateTokenResult>.Fail(new Error(nameof(UserSession.IsActive), "Token is not active"));
                }

                return Result<ValidateTokenResult>.Success(new ValidateTokenResult());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return Result<ValidateTokenResult>.Fail(Error.Default());
            }
        }
    }
}