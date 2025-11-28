namespace CustomerRegistration.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string ICNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string Email { get; set; } = null!;

        public bool MobileVerified { get; set; }
        public bool EmailVerified { get; set; }
        public bool TermsAccepted { get; set; }
        public bool BiometricEnabled { get; set; }
        
        public string? PinHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}