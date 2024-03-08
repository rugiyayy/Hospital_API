namespace Hospital_FinalP.Entities
{

    public class DoctorType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Doctor> Doctors { get; set; }
    }

}
