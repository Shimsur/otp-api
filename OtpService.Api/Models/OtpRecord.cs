namespace OtpService.Api.Models
{
    public class OtpRecord
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}