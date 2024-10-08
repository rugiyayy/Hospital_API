﻿namespace Hospital_FinalP.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PatientIdentityNumber { get; set; }//ID(FIN)
        public DateTime BirthDate { get; set; }

        public List<Appointment> Appointments { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
