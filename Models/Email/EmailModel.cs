namespace Models.Email
{
    public class EmailModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public bool IsSuccess { get; set; } = false;

    }
}
