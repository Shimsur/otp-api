
using System;
using System.Collections.Generic;
using System.Linq;

using OtpService.Api.Models;

namespace OtpService.Api.Services
{
    public class OtpServiceManager
    {
        // In-memory storage for OTPs
        private static List<OtpRecord> _otps = new List<OtpRecord>();

        // Generate OTP
        public string GenerateOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();

            // Remove old OTP for same email
            _otps.RemoveAll(x => x.Email == email);

            // Store new OTP with expiry
            _otps.Add(new OtpRecord
            {
                Email = email,
                Otp = otp,
                ExpiryTime = DateTime.Now.AddMinutes(5)
            });

            return otp;
        }

        // Verify OTP
        public bool VerifyOtp(string email, string otp)
        {
            var record = _otps.FirstOrDefault(x => x.Email == email && x.Otp == otp);

            if (record == null)
                return false;

            if (record.ExpiryTime < DateTime.Now)
            {
                _otps.Remove(record);
                return false;
            }

            // OTP is valid → remove it after use
            _otps.Remove(record);
            return true;
        }
    }
}