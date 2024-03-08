using FluentValidation;
using System.Text.RegularExpressions;

namespace Hospital_FinalP.DTOs.Patients
{
    public class PatientPutDto
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }



        public class PatientPutDtoValidator : AbstractValidator<PatientPutDto>
        {
            public PatientPutDtoValidator()
            {
                RuleFor(x => x.Email)
                    .NotNull().WithMessage("Email field is required !")
                   .NotEmpty().WithMessage("Email field is empty !")
                   .EmailAddress().WithMessage("Invalid email format.")
                   .Must(x => IsEmailValid(x)).WithMessage("Invalid email format. Email should end with '.com' or '.ru'.");

                RuleFor(x => x.PhoneNumber)
                  .NotEmpty().WithMessage("Phone Number is required.")
                  .Matches(@"^[0-9]+$").WithMessage("Phone Number can only contain digits.");

            }


            private bool IsEmailValid(string value)
            {
                return Regex.IsMatch(value, @"@[a-zA-Z0-9\-\.]+\.(com|ru)$");
            }
        }

    }
}
