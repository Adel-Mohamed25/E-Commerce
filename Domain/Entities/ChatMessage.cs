namespace Domain.Entities
{
    public class ChatMessage : BaseEntity
    {

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;


    }
}
