using FluentValidation;
using Hospital_FinalP.DTOs.DoctorDetail;
using static Hospital_FinalP.DTOs.DoctorDetail.DoctorDetailPutDto;
namespace Hospital_FinalP.DTOs.Doctors
{
    public class DoctorPutDto
    {
        //public IFormFile Photo { get; set; }
        public DoctorDetailPutDto DoctorDetail { get; set; }


        public class DoctorPutDtoValidator : AbstractValidator<DoctorPutDto>
        {
            public DoctorPutDtoValidator()
            {
                RuleFor(x => x.DoctorDetail)
               .NotNull().WithMessage("Doctor Detail is required.")
               .SetValidator(new DoctorDetailPuDtoValidator());


            }
        }
    }
}
