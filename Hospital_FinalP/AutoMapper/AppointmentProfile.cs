using AutoMapper;
using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.AutoMapper
{
    public class AppointmentProfile :Profile
    {
        public AppointmentProfile()
        {
            CreateMap<Appointment, AppointmentGetDto>();
            CreateMap<AppointmentPostDto, Appointment>().ReverseMap();
            CreateMap<AppointmentPutDto, Appointment>();
        }
    }
}
