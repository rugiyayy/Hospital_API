using FluentValidation;
using Hospital_FinalP.DTOs.Account;
using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.ExaminationRooms;
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

                RuleFor(x => x.Email)
                   .NotEmpty().WithMessage("Email field is required!")
                   .EmailAddress().WithMessage("Invalid email format.")
                   .Must(x => IsEmailValid(x)).WithMessage("Invalid email format. Email should end with '.com' or '.ru'.");


                RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Password field is required!")
                   .MinimumLength(4).WithMessage("Password must be at least 4 characters long.")
                   .Must(x => HasLetterAndDigit(x)).WithMessage("Password must contain at least one letter and one digit.");


               

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
