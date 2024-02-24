using AutoMapper;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.Doctors;
using Hospital_FinalP.DTOs.ExaminationRooms;
using Hospital_FinalP.Entities;
using System.Text;

namespace Hospital_FinalP.AutoMapper
{
    public class DoctorProfile: Profile
    {
        public DoctorProfile()
        {
            CreateMap<Doctor, DoctorGetDto>()
            .ForMember(dest => dest.DoctorDetail, opt => opt.MapFrom(src => src.DoctorDetail))
            .ForMember(dest => dest.ExaminationRoom, opt => opt.MapFrom(src => src.ExaminationRoom));


            CreateMap<DoctorPostDto, Doctor>();
            CreateMap<DoctorPutDto, Doctor>();






        }
    }
}
