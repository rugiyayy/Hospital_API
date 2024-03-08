using FluentValidation;

namespace Hospital_FinalP.DTOs.Account
{
    public class ForgotPasswordDto 
    {
        public string Email { get; set; }
        public string FrontendPort { get; set; }



        public class ForgotPasswordDtoValidator :AbstractValidator<ForgotPasswordDto>
        {
            public ForgotPasswordDtoValidator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email address.");

                RuleFor(x => x.FrontendPort)
                    .NotNull().WithMessage(" FrontendPort can't be null")
                    .NotEmpty().WithMessage("FrontendPort is required.");

            }
          
        }
    }
}
