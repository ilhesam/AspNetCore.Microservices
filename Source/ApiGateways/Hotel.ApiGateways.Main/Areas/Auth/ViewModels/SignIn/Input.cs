using FluentValidation;

namespace Hotel.ApiGateways.Main.Areas.Auth.ViewModels.SignIn
{
    public class SignInInput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SignInInputValidator : AbstractValidator<SignInInput>
    {
        public SignInInputValidator()
        {
            RuleFor(e => e.UserName)
                .NotNull()
                .NotEmpty();

            RuleFor(e => e.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}