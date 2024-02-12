using AutoMapper;
using Hospital_FinalP.DTOs.WSchedule;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.AutoMapper
{
    public class WorkingScheduleProfile : Profile
    {
        public WorkingScheduleProfile()
        {
            CreateMap<WorkingSchedule, WorkingScheduleGetDto>();
            CreateMap<WorkingSchedulePostDto, WorkingSchedule>();


        }
    }


    public class WorkingDayProfile : Profile
    {
        public WorkingDayProfile()
        {
            CreateMap<WorkingDay, WorkingDayGetDto>();
            CreateMap<WorkingDayPostDto, WorkingDay>();
        }
    }
}
