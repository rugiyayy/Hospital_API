//namespace Hospital_FinalP.DTOs.WSchedule
//{
//    public class WorkingSchedulePutDto
//    {

//        private TimeSpan _startTime;
//        private TimeSpan _endTime;

//        public string StartTime
//        {
//            get => _startTime.ToString(@"hh\:mm");
//            set
//            {

//                _startTime = TimeSpan.Parse(value);
//            }
//        }

//        public string EndTime
//        {
//            get => _endTime.ToString(@"hh\:mm");
//            set
//            {
//                _endTime = TimeSpan.Parse(value);
//            }
//        }
//        public WorkingSchedulePutDto()
//        {
//            if (WorkingDays == null || WorkingDays.Count == 0)
//            {
//                WorkingDays = new List<WorkingDayPutDto>();
//                for (DayOfWeek day = DayOfWeek.Monday; day <= DayOfWeek.Saturday; day++)
//                {
//                    WorkingDays.Add(new WorkingDayPutDto { DayOfWeek = day });
//                }
//            }
//        }
//        public List<WorkingDayPutDto> WorkingDays { get; set; }
//    }
//    public class WorkingDayPutDto
//    {
//        public DayOfWeek DayOfWeek { get; set; }
//    }
//}
