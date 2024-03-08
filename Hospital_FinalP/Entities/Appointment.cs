namespace Hospital_FinalP.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime
        {
            get => StartTime.Add(Duration);
            set => Duration = value - StartTime;
        }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string FormattedStartTime => StartTime.ToString("dd-MM-yyyy HH:mm");
        public string FormattedEndTime => EndTime.ToString("dd-MM-yyyy HH:mm");

    }

}
