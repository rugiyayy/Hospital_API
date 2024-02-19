using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.Doctors;

namespace Hospital_FinalP.DTOs.Department
{
    public class DepartmentGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentDescription { get; set; }
        public decimal ServiceCost { get; set; }


    }
}
