namespace PRN232.LMS.Services.ResponseModels
{
    /// <summary>Response model containing JWT tokens after successful authentication.</summary>
    public class TokenResponse
    {
        /// <summary>The JWT access token.</summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>The opaque refresh token for token renewal.</summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>Access token lifetime in seconds.</summary>
        public int ExpiresIn { get; set; }

        /// <summary>The token type (always Bearer).</summary>
        public string TokenType { get; set; } = "Bearer";
    }
}
