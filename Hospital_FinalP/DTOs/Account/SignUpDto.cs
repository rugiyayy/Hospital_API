using FluentValidation;
using System.Text.RegularExpressions;

namespace Hospital_FinalP.DTOs.Account
{
    public class SignUpDto
    {
        public required string FullName { get; set; }   
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required bool IsAdmin { get; set; }

        public class SignUpDtoValidator : AbstractValidator<SignUpDto>
        {
            public SignUpDtoValidator()
            {
                RuleFor(x => x.FullName)
                    .NotEmpty().WithMessage("FullName field is required!")
                    .Must(x => IsAlphaOnly(x)).WithMessage("FullName should contain only letters.");

                RuleFor(x => x.Email)
                   .NotEmpty().WithMessage("Email field is required!")
                   .EmailAddress().WithMessage("Invalid email format.")
                   .Must(x => IsEmailValid(x)).WithMessage("Invalid email format. Email should end with '.com' or '.ru'.");

                RuleFor(x => x.UserName)
                   .NotEmpty().WithMessage("UserName field is required!")
                   .Must(x => !IsEmailValid(x)).WithMessage("UserName cannot be an email address.");

                RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Password field is required!")
                   .MinimumLength(4).WithMessage("Password must be at least 4 characters long.")
                   .Must(x => HasLetterAndDigit(x)).WithMessage("Password must contain at least one letter and one digit.");


                RuleFor(x => x.IsAdmin)
                     .Must(x => x == false || x == true)
                     .NotNull().WithMessage("IsAdmin field is  required!");

            }


            private bool IsAlphaOnly(string value)
            {
                return Regex.IsMatch(value, @"^[a-zA-Z]+$");
            }
            private bool IsEmailValid(string value)
            {
                return Regex.IsMatch(value, @"@[a-zA-Z0-9\-\.]+\.(com|ru)$");
            }

            private bool HasLetterAndDigit(string value)
            {
                return Regex.IsMatch(value, @"^(?=.*[a-zA-Z])(?=.*\d).+$");
            }
        }
    }
}
