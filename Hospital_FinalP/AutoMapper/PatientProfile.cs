using AutoMapper;
using Hospital_FinalP.DTOs.Patients;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.AutoMapper
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<Patient, PatientGetDto>().ReverseMap();
            CreateMap<PatientPostDto, Patient>();
            CreateMap<PatientPutDto, Patient>();


        }


    }
}
