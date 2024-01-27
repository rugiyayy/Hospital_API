using System.ComponentModel.DataAnnotations;

namespace Hospital_FinalP.DTOs.DoctorDetail
{
    public class DoctorDetailPostDto
    {
        public  string PhoneNumber { get; set; }
        public  string Email { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime BirthDate { get; set; }
    }
}
