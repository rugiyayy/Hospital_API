using FluentValidation;

namespace Hospital_FinalP.DTOs.EmailSender
{
    public class EmailPostDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }




        public class EmailPostDtoValidator : AbstractValidator<EmailPostDto>
        {
            public EmailPostDtoValidator()
            {
                RuleFor(x => x.From)
                    .NotEmpty().WithMessage("From address is required.")
                    .EmailAddress().WithMessage("Invalid email address format for 'From'.");

                RuleFor(x => x.To)
                    .NotEmpty().WithMessage("To address is required.")
                    .EmailAddress().WithMessage("Invalid email address format for 'To'.");

                RuleFor(x => x.Subject)
                    .NotEmpty().WithMessage("Subject is required.")
                    .MaximumLength(255).WithMessage("Subject cannot exceed 255 characters.");

                RuleFor(x => x.Body)
                    .NotEmpty().WithMessage("Body is required.");
            }
        }

    }
}
