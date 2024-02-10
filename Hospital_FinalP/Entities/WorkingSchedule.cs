namespace Hospital_FinalP.Entities
{
    public class WorkingSchedule
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
         public DayOfWeek DayOfWeek { get; set; }

        //public List<DayOfWeek> WorkingDays { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
