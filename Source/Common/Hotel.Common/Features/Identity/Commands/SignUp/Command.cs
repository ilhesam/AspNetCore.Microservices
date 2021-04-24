using Hotel.Common.Models;
using MediatR;

namespace Hotel.Common.Features.Identity.Commands.SignUp
{
    public class SignUpCommand : IRequest<Result<SignUpResult>>
    {
        public SignUpCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; }
        public string Password { get; }
    }
}