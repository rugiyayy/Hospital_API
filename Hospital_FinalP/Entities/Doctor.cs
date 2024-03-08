namespace Hospital_FinalP.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? PhotoPath { get; set; }

        public int MaxAppointments { get; set; }

        public WorkingSchedule WorkingSchedule { get; set; }

        public int CalculateMaxAppointments(TimeSpan appointmentDuration)
        {

            if (WorkingSchedule != null)
            {
                TimeSpan totalWorkingHours = WorkingSchedule.EndTime - WorkingSchedule.StartTime;
                int maxAppointments = (int)(totalWorkingHours.TotalMinutes / appointmentDuration.TotalMinutes);
                MaxAppointments = maxAppointments;

                return MaxAppointments;
            }

            throw new InvalidOperationException("WorkingSchedule is not set for the doctor.");
        }


        public int AvailableAppointments
        {
            get
            {
                if (Appointments == null || Appointments.Count == 0)
                    return MaxAppointments;

                int bookedAppointmentsCount = Appointments.Count;
                return MaxAppointments - bookedAppointmentsCount;
            }
        }
        public int DepartmentId { get; set; }
        public int DoctorTypeId { get; set; }

        public Department Department { get; set; }
        public DoctorType DoctorType { get; set; }

        public DoctorDetail DoctorDetail { get; set; }
        public ExaminationRoom ExaminationRoom { get; set; }
        public List<Appointment> Appointments { get; set; }

    }
}