using FluentValidation;
using Hospital_FinalP.DTOs.Account;
using Hospital_FinalP.DTOs.Apointment;
using System.ComponentModel.DataAnnotations.Schema;
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


            }


            private bool IsEmailValid(string value)
            {
                return Regex.IsMatch(value, @"@[a-zA-Z0-9\-\.]+\.(com|ru)$");
            }
        }

    }
}
