

using FluentValidation;

namespace Hospital_FinalP.DTOs.Apointment
{
    public class AppointmentPostDto
    {
        public DateTime StartTime { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Description { get; set; }



        public class AppointmentPostDtoValidator : AbstractValidator<AppointmentPostDto>
        {
            public AppointmentPostDtoValidator()
            {
                RuleFor(x => x.StartTime)
                    .NotEmpty().WithMessage("Start time is required.");

                RuleFor(x => x.DoctorId)
                    .NotEmpty().WithMessage("Doctor ID is required.")
                    .GreaterThan(0).WithMessage("Doctor ID must be greater than 0.");

                RuleFor(x => x.PatientId)
                    .NotEmpty().WithMessage("Patient ID is required.")
                    .GreaterThan(0).WithMessage("Patient ID must be greater than 0.");

                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required.")
                    .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");
            }
        }
    }



   
}
