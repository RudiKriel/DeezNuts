namespace Common.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SenderUsername { get; set; }

        public string RecipientUsername { get; set; }

        public DateTime? DateRead { get; set; }

        public DateTime MessageSent { get; set; } = DateTime.UtcNow;

        public bool SenderDeleted { get; set; }

        public bool RecipientDeleted { get; set; }

        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        public int RecipientId { get; set; }
        public virtual User Recipient { get; set; }
    }
}
