using FluentValidation;
using System.Text.RegularExpressions;

namespace Hospital_FinalP.DTOs.Patients
{
    public class PatientPostDto
    {

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PatientIdentityNumber { get; set; }//ID(FIN)
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }

        public class PatientPostDtoValidator : AbstractValidator<PatientPostDto>
        {
            public PatientPostDtoValidator()
            {
                RuleFor(x => x.FullName)
                    .NotEmpty().WithMessage("FullName field is required!")
                    .Must(x => IsAlphaOnly(x)).WithMessage("FullName should contain only letters.");

                RuleFor(x => x.PhoneNumber)
                    .NotEmpty().WithMessage("Phone Number is required.")
                    .Matches(@"^[0-9]+$").WithMessage("Phone Number can only contain digits.");

                RuleFor(x => x.Email)
                   .NotEmpty().WithMessage("Email field is required!")
                   .EmailAddress().WithMessage("Invalid email format.")
                   .Must(x => IsEmailValid(x)).WithMessage("Invalid email format. Email should end with '.com' or '.ru'.");


                RuleFor(x => x.PatientIdentityNumber)
                    .NotEmpty().WithMessage("Patient Identity Number is required.")
                    .Matches(@"^[0-9a-zA-Z]+$").WithMessage("Patient Identity Number can only contain digits and letters.");


                RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Password field is required!")
                   .MinimumLength(4).WithMessage("Password must be at least 4 characters long.")
                   .Must(x => HasLetterAndDigit(x)).WithMessage("Password must contain at least one letter and one digit.");


                RuleFor(x => x.BirthDate)
                    .NotEmpty().WithMessage("Birth Date is required.")
                    .Must(BeAValidDate).WithMessage("Invalid Birth Date.");


            }


            private bool BeAValidDate(DateTime birthDate)
            {
                var validAge = DateTime.Today.AddYears(-18);
                return birthDate <= validAge;
            }

            private bool IsAlphaOnly(string value)
            {
                return Regex.IsMatch(value, @"^[a-zA-Z\s]+$");
            }
            private bool IsEmailValid(string value)
            {
                return Regex.IsMatch(value, @"@[a-zA-Z0-9\-\.]+\.(com|ru|edu|az)$");
            }

            private bool HasLetterAndDigit(string value)
            {
                return Regex.IsMatch(value, @"^(?=.*[a-zA-Z])(?=.*\d).+$");
            }
        }

    }
}
