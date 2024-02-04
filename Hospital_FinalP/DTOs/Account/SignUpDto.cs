using FluentValidation;

namespace Hospital_FinalP.DTOs.Account
{
    public class SignUpDto
    {
        public required string FullName { get; set; }   
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required bool IsAdmin { get; set; }
        // mojno vmesto etoqo prosto string ostavit , i po podxodashim bukvam i tp proverit eto admin ili user ili Appointment Scheduler ili eshe kto to
        //v dalneyshim poosmotru kak imenno handlit eto
        //vmesto etoqo napishesh select qde budut neskolko rols !!!!



        public class SignUpDtoValidator : AbstractValidator<SignUpDto>
        {
            public SignUpDtoValidator()
            {
                RuleFor(x => x.IsAdmin)
                     .Must(x => x == false || x == true)
                     .NotNull().WithMessage("IsAdmin field is  required!");

            }

        }
    }
}
