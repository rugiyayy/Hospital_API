﻿namespace Hospital_FinalP.Entities
{
    public class DoctorDetail
    {
        public int Id { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string FormattedBirthDate => BirthDate.ToString("dd.MM.yyyy"); 

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }


    }
}
