using FluentValidation;

namespace Hospital_FinalP.DTOs.WSchedule
{
    public class WorkingSchedulePostDto
    {

        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public string StartTime
        {
            get => _startTime.ToString(@"hh\:mm");
            set
            {
                
                _startTime = TimeSpan.Parse(value);
            }
        }

        public string EndTime
        {
            get => _endTime.ToString(@"hh\:mm");
            set
            {
                _endTime = TimeSpan.Parse(value);
            }
        }
        public WorkingSchedulePostDto()
        {
            if (WorkingDays == null || WorkingDays.Count == 0)
            {
                WorkingDays = new List<WorkingDayPostDto>();
                for (DayOfWeek day = DayOfWeek.Monday; day <= DayOfWeek.Saturday; day++)
                {
                    WorkingDays.Add(new WorkingDayPostDto { DayOfWeek = day });
                }
            }
        }
        public List<WorkingDayPostDto> WorkingDays { get; set; }



        public class WorkingSchedulePostDtoValidator : AbstractValidator<WorkingSchedulePostDto>
        {
            public WorkingSchedulePostDtoValidator()
            {
                RuleFor(x => x.StartTime)
                    .NotEmpty().WithMessage("Start Time is required.");

                RuleFor(x => x.EndTime)
                    .NotEmpty().WithMessage("End Time is required.")
                    .GreaterThan(x => x.StartTime).WithMessage("End Time should be greater than Start Time.");

            }
        }



    }
    public class WorkingDayPostDto
    {
        public DayOfWeek DayOfWeek { get; set; }
    }
}
