using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Hospital_FinalP.DTOs.Account
{
    public class ResetPasswordRequestDto
    {
        [Required, MinLength(6, ErrorMessage = "Please enter at least 4 characters!")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Email { get; set; }

        public string Token { get; set; }


        public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestDto>
        {
            public ResetPasswordRequestDtoValidator()
            {
                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password field is required!")
                    .MinimumLength(4).WithMessage("Password must be at least 4 characters long.")
                    .Must(x => HasLetterAndDigit(x)).WithMessage("Password must contain at least one letter and one digit.");


                RuleFor(x => x.ConfirmPassword)
                    .NotEmpty().WithMessage("Confirm Password is required.")
                    .Equal(x => x.Password).WithMessage("Passwords do not match.");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email address.");

                RuleFor(x => x.Token)
                    .NotEmpty().WithMessage("Token is required.");
            }

            private bool HasLetterAndDigit(string value)
            {
                return Regex.IsMatch(value, @"^(?=.*[a-zA-Z])(?=.*\d).+$");
            }
        }
    }
}
