using AutoMapper;
using Hospital_FinalP.DTOs.ExaminationRooms;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.AutoMapper
{
    public class ExaminationRoomProfile : Profile
    {
        public ExaminationRoomProfile()
        {
            CreateMap<ExaminationRoom, ExaminationRoomGetDto>();
            CreateMap<ExaminationRoomPostDto,ExaminationRoom>();
            CreateMap<ExaminationRoomPutDto, ExaminationRoom>();
            CreateMap<ExaminationRoom, ExaminationRoomPostDto>().ReverseMap();

        }
    }
}
