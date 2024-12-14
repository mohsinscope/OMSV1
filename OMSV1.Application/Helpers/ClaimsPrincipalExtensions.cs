using System;
using System.Security.Claims;

namespace OMSV1.Application.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Retrieves the username (Name) from the ClaimsPrincipal.
        /// Throws an exception if the claim is not found.
        /// </summary>
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var username = user.FindFirstValue(ClaimTypes.Name)
                          ?? throw new Exception("Cannot get username from token");

            return username;
        }

        /// <summary>
        /// Retrieves the user ID (NameIdentifier) from the ClaimsPrincipal.
        /// Throws an exception if the claim is not found or invalid.
        /// </summary>
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("Cannot get user ID from token");

            // Parsing user ID to an integer
            if (!int.TryParse(userIdClaim, out var userId))
                throw new Exception("User ID from token is not a valid integer");

            return userId;
        }
    }
}
