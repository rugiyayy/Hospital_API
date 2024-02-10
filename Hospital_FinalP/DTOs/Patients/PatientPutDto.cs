using Hospital_FinalP.DTOs.Apointment;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_FinalP.DTOs.Patients
{
    public class PatientPutDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public string IdentityNumber { get; set; }//ID(FIN)
        public DateTime BirthDate { get; set; }

        //public List<AppointmentPutDto> Appointments { get; set; }

    }
}
