﻿namespace Hospital_FinalP.Entities
{
    public class Email
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentTime { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}
