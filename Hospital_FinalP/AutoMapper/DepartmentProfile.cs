using AutoMapper;
using Hospital_FinalP.DTOs.Department;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.AutoMapper
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentGetDto>().ReverseMap();
            CreateMap<DepartmentPostDto, Department>();
            CreateMap<DepartmentPutDto, Department>();


        }
    }
}
