using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.ExaminationRooms;

namespace Hospital_FinalP.DTOs.Doctors
{
    public class DoctorPutDto
    {
        public string FullName { get; set; }
        public decimal ServiceCost { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorTypeId { get; set; }
        public IFormFile Photo { get; set; }
        public int MaxAppointments { get; set; } // select default value 
        public DoctorDetailPutDto DoctorDetail { get; set; }
        public ExaminationRoomPutDto ExaminationRoom { get; set; }
    }
}
