using FluentValidation;

namespace Hospital_FinalP.DTOs.Account
{
    public class SignInDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }




        public class SignInDtoValidator : AbstractValidator<SignInDto>
        {
            public SignInDtoValidator()
            {
                RuleFor(x => x.Password)
                  .NotEmpty().WithMessage("Password field is required!")
                  .MinimumLength(4).WithMessage("Password must be at least 4 characters long.");

                RuleFor(x => x.UserName)
                  .NotEmpty().WithMessage("UserName field is required!")
                  .NotNull().WithMessage("UserName field is  required!");

            }
        }
    }
}
