namespace Hospital_FinalP.DTOs.Patients
{
    public class PatientGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PatientIdentityNumber { get; set; }//ID(FIN)
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
