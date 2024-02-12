using Hospital_FinalP.Entities;

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
            // Проверяем, были ли предоставлены дни работы, и если нет, добавляем их по умолчанию
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
    }
    public class WorkingDayPostDto
    {
        public DayOfWeek DayOfWeek { get; set; }
    }
}
