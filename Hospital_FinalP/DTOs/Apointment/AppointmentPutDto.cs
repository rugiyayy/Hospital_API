using Hospital_FinalP.DTOs.Doctors;
using Hospital_FinalP.DTOs.Patients;

namespace Hospital_FinalP.DTOs.Apointment
{
    public class AppointmentPutDto
    {
        public DateTime StartTime { get; set; } 
        public int DoctorId { get; set; }
        public string Description { get; set; }//reason sympoms
    }
}
