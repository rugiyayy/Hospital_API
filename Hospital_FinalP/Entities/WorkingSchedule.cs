using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hospital_FinalP.Entities
{
    public class WorkingSchedule
    {
    
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        //public DayOfWeek DayOfWeek { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public List<WorkingDay> WorkingDays { get; set; }
    }

    public class WorkingDay
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int WorkingScheduleId { get; set; }
        public WorkingSchedule WorkingSchedule { get; set; }
    }

}
