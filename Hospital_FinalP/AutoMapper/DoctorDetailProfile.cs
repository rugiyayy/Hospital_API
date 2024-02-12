using AutoMapper;
using Hospital_FinalP.DTOs.Department;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.AutoMapper
{
    public class DoctorDetailProfile : Profile
    {
        public DoctorDetailProfile()
        {
            CreateMap<DoctorDetail, DoctorDetailGetDto>().ReverseMap();
            CreateMap<DoctorDetailPostDto, DoctorDetail>();
            CreateMap<DoctorDetailPutDto, DoctorDetail>();

        }
    }

}
