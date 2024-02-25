using AutoMapper;
using Hospital_FinalP.DTOs.Department;
using Hospital_FinalP.DTOs.Email;
using Hospital_FinalP.DTOs.EmailSender;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.AutoMapper
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<Email, EmailGetDto>().ReverseMap();
            CreateMap<EmailPostDto, Email>();

        }
    }

}
