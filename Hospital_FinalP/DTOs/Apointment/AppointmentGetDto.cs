namespace Hospital_FinalP.DTOs.Apointment
{
    public class AppointmentGetDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PatientFullName { get; set; }
        public string PatientEmail { get; set; }
        public string DoctorEmail { get; set; }
        public string Department { get; set; }
        public string Type { get; set; }


        public string DoctorFullName { get; set; }
        public decimal ServiceCost { get; set; }
        public bool IsActive { get; set; }
        public string FormattedStartTime => StartTime.ToString("dd-MM-yyyy HH:mm");
        public string FormattedEndTime => EndTime.ToString("dd-MM-yyyy HH:mm");
        public string Description { get; set; } 
    }
}