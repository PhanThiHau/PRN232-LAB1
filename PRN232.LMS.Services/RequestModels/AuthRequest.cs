using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.RequestModels
{
    /// <summary>Request model for user login.</summary>
    public class LoginRequest
    {
        /// <summary>The username.</summary>
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; } = string.Empty;

        /// <summary>The password.</summary>
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>Request model for refreshing an access token.</summary>
    public class RefreshTokenRequest
    {
        /// <summary>The refresh token string.</summary>
        [Required(ErrorMessage = "RefreshToken is required.")]
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>Request model for revoking a refresh token.</summary>
    public class RevokeTokenRequest
    {
        /// <summary>The refresh token to revoke.</summary>
        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; } = string.Empty;
    }
}
