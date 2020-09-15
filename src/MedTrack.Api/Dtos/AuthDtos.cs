using System;

namespace MedTrack.Api.Dtos
{
    public class RegisterRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
