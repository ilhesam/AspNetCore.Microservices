using Hotel.Common.Models;
using MediatR;

namespace Hotel.Common.Features.Identity.Commands.ValidateToken
{
    public class ValidateTokenCommand : IRequest<Result<ValidateTokenResult>>
    {
        public ValidateTokenCommand(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; }
    }
}