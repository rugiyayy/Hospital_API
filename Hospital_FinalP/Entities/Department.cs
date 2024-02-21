namespace Hospital_FinalP.Entities
{
    public class Department
    //add kakie Diseases & Conditions dla kajdoqo departmenta, (kakie bolezni has kakie sympthoms dobav obazatelno eto i dalshe department id click olunduqda pust ob etom i budet 
    //nujno sozdat one to one s department 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentDescription { get; set; }
        public decimal ServiceCost { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Disease> Diseases { get; set; }



        


    }
}
