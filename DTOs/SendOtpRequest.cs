namespace CustomerRegistration.DTOs
{
    public class SendOtpRequest
    {
        public int CustomerId { get; set; }
        public string Type { get; set; } = null!; // "mobile" or "email"
    }
}
