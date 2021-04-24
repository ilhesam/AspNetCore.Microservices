using Hotel.Common.Models;
using MediatR;

namespace Hotel.Common.Features.Identity.Commands.SignIn
{
    public class SignInCommand : IRequest<Result<SignInResult>>
    {
        public SignInCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get;  }
        public string Password { get; }
    }
}