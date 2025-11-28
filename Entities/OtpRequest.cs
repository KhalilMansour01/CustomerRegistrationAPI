namespace CustomerRegistration.Entities
{
    public class OtpRequest
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public string Code { get; set; } = null!;
        public string Type { get; set; } = null!; // "mobile" or "email"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Used { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
