using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.ExaminationRooms;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.DTOs.Doctors
{
    public class DoctorPostDto
    {
        public string FullName { get; set; }
        public decimal ServiceCost { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorTypeId { get; set; }
        public IFormFile Photo { get; set; }
        public int MaxAppointments { get; set; } // select default value 
        public  DoctorDetailPostDto DoctorDetail { get; set; }
        public ExaminationRoomPostDto ExaminationRoom { get; set; }
    }
}
