namespace Hospital_FinalP.Entities
{
    public class ExaminationRoom
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int DoctorId { get; set; }
        public  Doctor Doctor { get; set; }

    }
}
