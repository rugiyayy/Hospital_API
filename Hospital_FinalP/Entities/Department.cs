namespace Hospital_FinalP.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentDescription { get; set; }
        public List<Doctor> Doctors { get; set; }


    }
}
