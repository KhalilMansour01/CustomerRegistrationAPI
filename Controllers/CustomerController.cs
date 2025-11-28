using CustomerRegistration.DTOs;
using CustomerRegistration.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRegistration.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _service;

        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var customer = await _service.RegisterAsync(request);
                return Ok(customer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("migrate")]
        public async Task<IActionResult> Migrate(MigrationRequest request)
        {
            return Ok(await _service.MigrateAsync(request));
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(SendOtpRequest request)
        {
            return Ok(await _service.SendOtpAsync(request));
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
        {
            try
            {
                var result = await _service.VerifyOtpAsync(request);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("accept-terms")]
        public async Task<IActionResult> AcceptTerms(AcceptTermsRequest request)
        {
            await _service.AcceptTermsAsync(request.CustomerId);
            return Ok("Terms accepted.");
        }

        [HttpPost("set-pin")]
        public async Task<IActionResult> SetPin(SetPinRequest request)
        {
            if (request.Pin != request.ConfirmPin)
                return BadRequest("Pins do not match");

            await _service.SetPinAsync(request.CustomerId, request.Pin);
            return Ok("PIN set successfully");
        }

        [HttpPost("enable-biometrics")]
        public async Task<IActionResult> EnableBiometric(BiometricRequest request)
        {
            await _service.EnableBiometricAsync(request.CustomerId);
            return Ok("Biometrics enabled.");
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> Status(int id)
        {
            return Ok(await _service.GetStatusAsync(id));
        }
    }
}
