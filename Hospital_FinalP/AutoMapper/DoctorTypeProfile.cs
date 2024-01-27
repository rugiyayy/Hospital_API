using AutoMapper;
using Hospital_FinalP.DTOs.DoctorType;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.AutoMapper
{
    public class DoctorTypeProfile :Profile
    {
        public DoctorTypeProfile()
        {
            CreateMap<DoctorType,DoctorTypeGetDto>().ReverseMap();
            CreateMap<DoctorTypePostDto, DoctorType>();
            CreateMap<DoctorTypePutDto, DoctorType>();

        }
    }
}
