using FluentValidation;

namespace Hospital_FinalP.DTOs.ExaminationRooms
{
    public class ExaminationRoomPostDto
    {
        public int RoomNumber { get; set; }


        public class ExaminationRoomPostDtoValidator : AbstractValidator<ExaminationRoomPostDto>
        {
            public ExaminationRoomPostDtoValidator()
            {
                RuleFor(x => x.RoomNumber)
               .NotEmpty().WithMessage("Room Number is required.")
               .GreaterThan(0).WithMessage("Room Number must be greater than 0.");
            }
        }

    }
}
