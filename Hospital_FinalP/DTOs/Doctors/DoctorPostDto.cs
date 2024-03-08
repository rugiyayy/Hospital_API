using FluentValidation;
using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.ExaminationRooms;
using Hospital_FinalP.DTOs.WSchedule;
using Hospital_FinalP.Entities;
using System.Text.RegularExpressions;
using static Hospital_FinalP.DTOs.DoctorDetail.DoctorDetailPostDto;
using static Hospital_FinalP.DTOs.ExaminationRooms.ExaminationRoomPostDto;
using static Hospital_FinalP.DTOs.WSchedule.WorkingSchedulePostDto;

namespace Hospital_FinalP.DTOs.Doctors
{
    public class DoctorPostDto
    {
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorTypeId { get; set; }
        public IFormFile? Photo { get; set; }
        public  DoctorDetailPostDto DoctorDetail { get; set; }
        public WorkingSchedulePostDto WorkingSchedule { get; set; }
        public ExaminationRoomPostDto ExaminationRoom { get; set; }

        public string Password { get; set; }


        public class DoctorPostDtoValidator : AbstractValidator<DoctorPostDto>
        {
            public DoctorPostDtoValidator()
            {
                RuleFor(x => x.FullName)
                    .NotNull().WithMessage("name cannot be null")
                    .NotEmpty().WithMessage("FName field is required!")
                    .Must(x => IsAlphaOnly(x)).WithMessage("FullName should contain only letters.");


                RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Password field is required!")
                   .MinimumLength(4).WithMessage("Password must be at least 4 characters long.")
                   .Must(x => HasLetterAndDigit(x)).WithMessage("Password must contain at least one letter and one digit.");

                RuleFor(x => x.Photo)
                .NotEmpty().WithMessage("Photo field is required!");


                RuleFor(x => x.DoctorDetail)
               .NotNull().WithMessage("Doctor Detail is required.")
               .SetValidator(new DoctorDetailPostDtoValidator());

                RuleFor(x => x.WorkingSchedule)
                    .NotNull().WithMessage("Working Schedule is required.")
                    .SetValidator(new WorkingSchedulePostDtoValidator());

                RuleFor(x => x.ExaminationRoom)
                    .NotNull().WithMessage("Examination Room is required.")
                    .SetValidator(new ExaminationRoomPostDtoValidator());

            }
            private bool IsAlphaOnly(string value)
            {
                if (value == null) return false;
                return Regex.IsMatch(value, @"^[a-zA-Z\s]+$");
            }
           

            private bool HasLetterAndDigit(string value)
            {
                if (value == null) return false;

                return Regex.IsMatch(value, @"^(?=.*[a-zA-Z])(?=.*\d).+$");
            }
        }



    }
}
