using AutoMapper;
using Hospital_FinalP.DTOs.Account;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.AutoMapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<SignUpDto, AppUser>();
            CreateMap<SignInDto, AppUser>();

        }
    }
}
