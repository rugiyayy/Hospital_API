namespace Hospital_FinalP.DTOs.Email
{
    public class EmailGetDto
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime SentTime { get; set; } = DateTime.UtcNow;
    }
}
