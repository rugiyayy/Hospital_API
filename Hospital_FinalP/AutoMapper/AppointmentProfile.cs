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
                 .ForMember(dest => dest.FormattedStartTime, opt => opt.MapFrom(src => src.StartTime.ToUniversalTime().ToString("dd-MM-yyyy HH:mm")))
                 .ForMember(dest => dest.FormattedEndTime, opt => opt.MapFrom(src => src.EndTime.ToUniversalTime().ToString("dd-MM-yyyy HH:mm")))
                 .ForMember(dest => dest.IsActive, opt => opt.MapFrom((src, _, _, context) =>
     {
         DateTime currentTimeUtc = DateTime.UtcNow;

         DateTime startTimeUtc = src.StartTime.ToUniversalTime();

         return src.IsActive && startTimeUtc > currentTimeUtc; 
     }));


            CreateMap<AppointmentPostDto, Appointment>();
        }
    }
}
