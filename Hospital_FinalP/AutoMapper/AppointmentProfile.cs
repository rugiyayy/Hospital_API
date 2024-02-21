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
            CreateMap<Appointment, AppointmentGetDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.StartTime.Date >= DateTime.Today))
                .ForMember(dest => dest.FormattedStartTime, opt => opt.MapFrom(src => src.StartTime.ToString("dd-MM-yyyy HH:mm")))
                .ForMember(dest => dest.FormattedEndTime, opt => opt.MapFrom(src => src.EndTime.ToString("dd-MM-yyyy HH:mm")));

            CreateMap<AppointmentPostDto, Appointment>();
            CreateMap<AppointmentPutDto, Appointment>();
        }
    }
}
