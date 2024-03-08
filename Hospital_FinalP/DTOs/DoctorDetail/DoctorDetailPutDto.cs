using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Hospital_FinalP.DTOs.DoctorDetail
{
    public class DoctorDetailPutDto
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public class DoctorDetailPuDtoValidator : AbstractValidator<DoctorDetailPutDto>
        {
            public DoctorDetailPuDtoValidator()
            {
                RuleFor(x => x.PhoneNumber)
                     .NotEmpty().WithMessage("Phone Number is required.")
                     .MinimumLength(10).WithMessage("Phone Number must be at least 10 digits.")
                     .Matches("^[0-9]*$").WithMessage("Phone Number should only contain numeric characters.");

                RuleFor(x => x.Email)
                   .NotEmpty().WithMessage("Email field is required!")
                   .EmailAddress().WithMessage("Invalid email format.")
                   .Must(x => IsEmailValid(x)).WithMessage("Invalid email format. Email should end with '.com' or '.ru'.");

            }


            private bool IsEmailValid(string value)
            {
                return Regex.IsMatch(value, @"@[a-zA-Z0-9\-\.]+\.(com|ru|az)$");
            }

        }


    }
}
