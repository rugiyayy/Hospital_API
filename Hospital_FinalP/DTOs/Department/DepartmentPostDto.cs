using FluentValidation;

namespace Hospital_FinalP.DTOs.Department
{
    public class DepartmentPostDto
    {
        public string Name { get; set; }
        public string DepartmentDescription { get; set; }
    }


    public class DepartmentPostDtoValidator : AbstractValidator<DepartmentPostDto>
    {
        public DepartmentPostDtoValidator()
        {
            RuleFor(x=>x.Name)
                .NotNull().WithMessage("Department name is required!")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Department name is empty")
                .Length(3, 50).WithMessage("Department name can't be less than 3 characters and more than 50 characters!");

            RuleFor(x => x.DepartmentDescription)
                .NotNull().WithMessage("Department Description is required!")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Department Description is empty")
                .Length(3, 200).WithMessage("Department Description can't be less than 3 characters and more than 200 characters!");
        }
    }

}
