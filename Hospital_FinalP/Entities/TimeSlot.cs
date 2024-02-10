namespace Hospital_FinalP.Entities
{
    public class TimeSlot
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }

        public TimeSlot(TimeSpan startTime, TimeSpan endTime, bool isAvailable)
        {
            StartTime = startTime;
            EndTime = endTime;
            IsAvailable = isAvailable;
        }
    }
}
