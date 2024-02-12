using Hospital_FinalP.Entities;

namespace Hospital_FinalP.DTOs.WSchedule
{
    public class WorkingScheduleGetDto
    {
        public int Id { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<WorkingDayGetDto> WorkingDays { get; set; }

    }

    public class WorkingDayGetDto
    {
        public DayOfWeek DayOfWeek { get; set; }
    }


}
