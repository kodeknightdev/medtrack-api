using System;
using System.Security.Claims;

namespace MedTrack.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            // "sub" is mapped to NameIdentifier by the jwt handler
            var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(value, out var id))
            {
                throw new InvalidOperationException("Authenticated user has no numeric id claim.");
            }

            return id;
        }
    }
}
