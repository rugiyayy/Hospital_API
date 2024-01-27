namespace Hospital_FinalP.Entities
{
    public class DoctorDetail
    {
        public int Id { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public DateTime BirthDate { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        //public required List<Language> Languages { get; set; }

    }
}
