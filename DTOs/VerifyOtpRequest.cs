namespace CustomerRegistration.DTOs
{
    public class VerifyOtpRequest
    {
        public int CustomerId { get; set; }
        public string Type { get; set; } = null!; // "mobile" or "email"
        public string Code { get; set; } = null!;
    }
}
