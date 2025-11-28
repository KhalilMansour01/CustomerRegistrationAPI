using CustomerRegistration.Data;
using CustomerRegistration.DTOs;
using CustomerRegistration.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerRegistration.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _db;

        public CustomerService(AppDbContext db)
        {
            _db = db;
        }

        // Register New User
        public async Task<Customer> RegisterAsync(RegisterRequest req)
        {
            var existing = await _db.Customers
                .FirstOrDefaultAsync(c => c.ICNumber == req.ICNumber);

            if (existing != null)
                throw new InvalidOperationException(
                    "Customer already exists. Use migration API."
                );

            var customer = new Customer
            {
                ICNumber = req.ICNumber,
                Name = req.Name,
                Mobile = req.Mobile,
                Email = req.Email,
                EmailVerified = false,
                MobileVerified = false,
                TermsAccepted = false,
                BiometricEnabled = false
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            return customer;
        }

        // Migrate Existing User
        public async Task<Customer> MigrateAsync(MigrationRequest req)
        {
            var customer = await _db.Customers
                .FirstOrDefaultAsync(c => c.ICNumber == req.ICNumber);

            if (customer == null)
                throw new Exception("User not found.");

            // Reset verification flags so user can go through the registration flow again
            customer.MobileVerified = false;
            customer.EmailVerified = false;
            customer.TermsAccepted = false;
            customer.BiometricEnabled = false;

            await _db.SaveChangesAsync();
            return customer;
        }

        // Send OTP
        public async Task<OtpRequest> SendOtpAsync(SendOtpRequest req)
        {
            var customer = await _db.Customers.FindAsync(req.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found.");

            string code = new Random().Next(100000, 999999).ToString();

            var otp = new OtpRequest
            {
                CustomerId = req.CustomerId,
                Code = code,
                Type = req.Type,
                Used = false,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                CreatedAt = DateTime.UtcNow
            };

            _db.OtpRequests.Add(otp);
            await _db.SaveChangesAsync();

            return otp;
        }

        // Verify OTP
        public async Task<string> VerifyOtpAsync(VerifyOtpRequest req)
        {
            var otp = await _db.OtpRequests
                .Where(o =>
                    o.CustomerId == req.CustomerId &&
                    o.Type == req.Type &&
                    !o.Used &&
                    o.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (otp == null)
                throw new InvalidOperationException("OTP is incorrect or expired.");

            if (otp.Code != req.Code)
                throw new InvalidOperationException("OTP is incorrect.");

            otp.Used = true;

            var customer = await _db.Customers.FindAsync(req.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found.");

            if (req.Type == "mobile")
                customer.MobileVerified = true;

            if (req.Type == "email")
                customer.EmailVerified = true;

            await _db.SaveChangesAsync();
            return $"{req.Type} verified.";
        }

        // Accept Terms
        public async Task AcceptTermsAsync(int customerId)
        {
            var customer = await _db.Customers.FindAsync(customerId);
            if (customer == null)
                throw new Exception("Customer not found");

            customer.TermsAccepted = true;
            await _db.SaveChangesAsync();
        }

        // Set PIN
        public async Task SetPinAsync(int customerId, string pin)
        {
            var customer = await _db.Customers.FindAsync(customerId);
            if (customer == null)
                throw new Exception("Customer not found");

            customer.PinHash = BCrypt.Net.BCrypt.HashPassword(pin);

            await _db.SaveChangesAsync();
        }

        // Enable Biometrics
        public async Task EnableBiometricAsync(int customerId)
        {
            var customer = await _db.Customers.FindAsync(customerId);
            if (customer == null)
                throw new Exception("Customer not found");

            customer.BiometricEnabled = true;
            await _db.SaveChangesAsync();
        }

        // Status (just to get all customer info)
        public async Task<object> GetStatusAsync(int customerId)
        {
            var customer = await _db.Customers.FindAsync(customerId);
            if (customer == null)
                throw new Exception("Customer not found");

            return new
            {
                customer.Id,
                customer.Name,
                customer.Mobile,
                customer.Email,
                customer.MobileVerified,
                customer.EmailVerified,
                customer.TermsAccepted,
                customer.BiometricEnabled
            };
        }
    }
}
