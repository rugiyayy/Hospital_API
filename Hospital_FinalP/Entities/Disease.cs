namespace Hospital_FinalP.Entities
{
    public class Disease
    {
        public int Id { get; set; }
        public string DiseaseName { get; set; }
        public string Symptom { get; set; }
        public int DepartmentId { get; set; }
        public  Department Department { get; set; }
     
        public  List<Symptom> Symptoms { get; set; }

    }
}
