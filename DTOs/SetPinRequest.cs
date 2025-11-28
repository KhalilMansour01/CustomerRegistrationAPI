namespace CustomerRegistration.DTOs
{
    public class SetPinRequest
    {
        public int CustomerId { get; set; }
        public string Pin { get; set; } = null!;
        public string ConfirmPin { get; set; } = null!;
    }
}
