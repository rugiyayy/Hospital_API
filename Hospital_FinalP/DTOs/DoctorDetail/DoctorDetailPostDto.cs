using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Hospital_FinalP.DTOs.DoctorDetail
{
    public class DoctorDetailPostDto
    {
        public  string PhoneNumber { get; set; }
        public required string Email { get; set; }
       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime BirthDate { get; set; }



        public class DoctorDetailPostDtoValidator : AbstractValidator<DoctorDetailPostDto>
        {
            public DoctorDetailPostDtoValidator()
            {
                RuleFor(x => x.PhoneNumber)
                    .NotEmpty().WithMessage("Phone Number is required.")
                    .MinimumLength(10).WithMessage("Phone Number must be at least 10 digits.")
                    .Matches("^[0-9]*$").WithMessage("Phone Number should only contain numeric characters.");

                RuleFor(x => x.Email)
                   .NotEmpty().WithMessage("Email field is required!")
                   .EmailAddress().WithMessage("Invalid email format.")
                   .Must(x => IsEmailValid(x)).WithMessage("Invalid email format. Email should end with '.com' or '.ru'.");


                RuleFor(x => x.BirthDate)
                    .NotEmpty().WithMessage("Birth Date is required.")
                    .Must(BeAValidDate).WithMessage("Invalid Birth Date (must be greater or equal to 25.)");


            }

            private bool BeAValidDate(DateTime birthDate)
            {
                var validAge = DateTime.Today.AddYears(-25);
                return birthDate <= validAge;
            }

            private bool IsEmailValid(string value)
            {
                return Regex.IsMatch(value, @"@[a-zA-Z0-9\-\.]+\.(com|ru|az)$");
            }

        }
    }
}
