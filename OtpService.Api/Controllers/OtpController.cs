using Microsoft.AspNetCore.Mvc;
using OtpService.Api.Services;

namespace OtpService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly OtpServiceManager _otpService;
        private readonly EmailService _emailService;

        public OtpController(OtpServiceManager otpService, EmailService emailService)
        {
            _otpService = otpService;
            _emailService = emailService;
        }

        // ✅ Generate OTP + Send Email
        [HttpPost("generate")]
        public IActionResult GenerateOtp([FromQuery] string email)
        {
            try
            {
                var otp = _otpService.GenerateOtp(email);

                var subject = "Your OTP Code";
                var body = $"<h3>Your OTP is: {otp}</h3><p>Valid for 5 minutes</p>";

                _emailService.SendEmail(email, subject, body);

                return Ok(new { message = "OTP sent to email successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ Verify OTP
        [HttpPost("verify")]
        public IActionResult VerifyOtp([FromQuery] string email, [FromQuery] string otp)
        {
            var result = _otpService.VerifyOtp(email, otp);

            if (!result)
                return BadRequest("Invalid or expired OTP");

            return Ok("OTP verified successfully");
        }
    }
}