using FluentValidation;

namespace Hospital_FinalP.DTOs.ExaminationRooms
{
    public class ExaminationRoomPutDto
    {
        public int RoomNumber { get; set; }


        public class ExaminationRoomPutDtoValidator : AbstractValidator<ExaminationRoomPutDto>
        {
            public ExaminationRoomPutDtoValidator()
            {
                RuleFor(x => x.RoomNumber)
               .NotEmpty().WithMessage("Room Number is required.")
               .GreaterThan(0).WithMessage("Room Number must be greater than 0.");
            }
        }
    }
}
