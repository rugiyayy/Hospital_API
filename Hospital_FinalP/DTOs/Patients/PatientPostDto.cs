using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.ExaminationRooms;

namespace Hospital_FinalP.DTOs.Patients
{
    public class PatientPostDto
    {

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PatientIdentityNumber { get; set; }//ID(FIN)
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        //public List<AppointmentPutDto> Appointments { get; set; }

    }
}
