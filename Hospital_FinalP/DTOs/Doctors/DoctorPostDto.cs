using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.ExaminationRooms;
using Hospital_FinalP.DTOs.WSchedule;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.DTOs.Doctors
{
    public class DoctorPostDto
    {
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorTypeId { get; set; }
        public IFormFile Photo { get; set; }
        public  DoctorDetailPostDto DoctorDetail { get; set; }
        public WorkingSchedulePostDto WorkingSchedule { get; set; }

        public ExaminationRoomPostDto ExaminationRoom { get; set; }
        public string Password { get; set; }


    }
}
