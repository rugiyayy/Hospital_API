namespace Hospital_FinalP.Entities
{
    public class Symptom
    {
        public int Id { get; set; }

        public int DiseaseId { get; set; }

        public string SymptomName { get; set; }

        public virtual Disease Disease { get; set; }
    }
}
