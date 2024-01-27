using FluentValidation;

namespace Hospital_FinalP.DTOs.DoctorType
{
    public class DoctorTypePutDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }


    public class DoctorTypePutDtoValidator : AbstractValidator<DoctorTypePutDto>
    {
        public DoctorTypePutDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Doctor Type name is required!")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Department name is empty")
                .Length(3, 50).WithMessage("Doctor Type name can't be less than 3 characters and more than 50 characters!");

            RuleFor(x => x.Description)
                .NotNull().WithMessage("Doctor Type Description is required!")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Doctor Type Description is empty")
                .Length(3, 200).WithMessage("Doctor Type Description can't be less than 3 characters and more than 200 characters!");
        }
    }
}
