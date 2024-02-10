namespace Hospital_FinalP.DTOs.WSchedule
{
    public class WorkingScheduleGetDto
    {
        public int Id { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
