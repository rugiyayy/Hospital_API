
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.ExaminationRooms;

namespace Hospital_FinalP.DTOs.Doctors
{
    public class DoctorGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int AvailableAppointments { get; set; }
        public int MaxAppointments { get; set; }
        public decimal ServiceCost { get; set; }
        public string DoctorTypeName { get; set; }
        public string DepartmentName { get; set; }
        public string PhotoPath { get; set; }
        public DoctorDetailGetDto DoctorDetail { get; set; }
        public ExaminationRoomGetDto ExaminationRoom { get; set; }

    }
}
