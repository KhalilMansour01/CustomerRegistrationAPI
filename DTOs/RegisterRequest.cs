namespace CustomerRegistration.DTOs
{
    public class RegisterRequest
    {
        public string ICNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
