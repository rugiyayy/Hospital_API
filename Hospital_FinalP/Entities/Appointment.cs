using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_FinalP.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DoctorId { get; set; }
        public  Doctor Doctor { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        //public VisitType VisitType { get; set; }
       
        public decimal Cost { get; set; }


        //public decimal AppointmentCost()
        //{
        //    if (Doctor != null)
        //    {
        //        Cost = Doctor.CalculateServiceCost();
        //    }
        //    return Cost;

        //}


        //public string VisitType { get; set; }//"In-Office" or "Virtual"
        //public decimal Cost { get; set; }
    }
}
