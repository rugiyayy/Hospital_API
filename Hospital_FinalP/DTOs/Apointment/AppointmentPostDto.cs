using Hospital_FinalP.DTOs.Doctors;
using Hospital_FinalP.DTOs.Patients;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_FinalP.DTOs.Apointment
{
    public class AppointmentPostDto
    {
        public DateTime StartTime { get; set; }
        
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Description { get; set; } //reason sympoms
    }
}
