using System;
using System.Threading.Tasks;
using MedTrack.Api.Data;
using MedTrack.Api.Dtos;
using MedTrack.Api.Models;
using MedTrack.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly MedTrackContext _context;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokens;

        public AuthController(MedTrackContext context, IPasswordHasher hasher, ITokenService tokens)
        {
            _context = context;
            _hasher = hasher;
            _tokens = tokens;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                return Conflict(new { message = "An account with that email already exists." });
            }

            var user = new User
            {
                Email = email,
                PasswordHash = _hasher.Hash(request.Password),
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _tokens.CreateToken(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || !_hasher.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            return _tokens.CreateToken(user);
        }
    }
}
