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
    }
}
