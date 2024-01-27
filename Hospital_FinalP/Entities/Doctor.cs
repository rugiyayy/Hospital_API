namespace Hospital_FinalP.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public decimal ServiceCost { get; set; }
        public int MaxAppointments { get; set; }

        public int DepartmentId { get; set; }
        public int DoctorTypeId { get; set; }

        public Department Department { get; set; }
        public DoctorType DoctorType { get; set; }

        public DoctorDetail DoctorDetail { get; set; }
        public DocPhoto? DocPhoto { get; set; }
        public  ExaminationRoom ExaminationRoom { get; set; }
        public List<Appointment> Appointments { get; set; }

    }
}
