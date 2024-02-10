using Hospital_FinalP.DTOs.DocPhoto;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.Doctors;
using Hospital_FinalP.DTOs.ExaminationRooms;
using Hospital_FinalP.DTOs.Patients;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.DTOs.Apointment
{
    public class AppointmentGetDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string PatientFullName { get; set; }
        public string DoctorFullName { get; set; }

        public List<DoctorGetDto> Doctor { get; set; }
        public List<PatientGetDto> Patient { get; set; }

        public string Description { get; set; } //reason sympoms
    }
}