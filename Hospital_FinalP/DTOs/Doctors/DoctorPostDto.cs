using FluentValidation;
using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.ExaminationRooms;
using Hospital_FinalP.DTOs.WSchedule;
using Hospital_FinalP.Entities;
using System.Text.RegularExpressions;

namespace Hospital_FinalP.DTOs.Doctors
{
    public class DoctorPostDto
    {
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorTypeId { get; set; }
        public IFormFile Photo { get; set; }
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




            }
            private bool IsAlphaOnly(string value)
            {
                if (value == null) return false;
                    return Regex.IsMatch(value, @"^[a-zA-Z]+$");
            }
            private bool IsEmailValid(string value)
            {
                if (value == null) return false;

                return Regex.IsMatch(value, @"@[a-zA-Z0-9\-\.]+\.(com|ru)$");
            }

            private bool HasLetterAndDigit(string value)
            {
                if (value == null) return false;

                return Regex.IsMatch(value, @"^(?=.*[a-zA-Z])(?=.*\d).+$");
            }
        }



    }
}
