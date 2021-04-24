using FluentValidation;

namespace Hotel.ApiGateways.Main.Areas.Auth.ViewModels.SignUp
{
    public class SignUpInput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SignUpInputValidator : AbstractValidator<SignUpInput>
    {
        public SignUpInputValidator()
        {
            RuleFor(e => e.UserName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(20);

            RuleFor(e => e.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8);
        }
    }
}