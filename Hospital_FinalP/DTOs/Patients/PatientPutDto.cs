using Hospital_FinalP.DTOs.Apointment;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_FinalP.DTOs.Patients
{
    public class PatientPutDto
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        //public List<AppointmentPutDto> Appointments { get; set; }

    }
}
