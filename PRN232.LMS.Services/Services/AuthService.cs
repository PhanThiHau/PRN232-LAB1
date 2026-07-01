using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Repositories;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;

namespace PRN232.LMS.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user == null)
                {
                    return ApiResponse<TokenResponse>.ErrorResponse("Invalid username or password.");
                }

                // Verify password using BCrypt
                bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
                if (!passwordValid)
                {
                    return ApiResponse<TokenResponse>.ErrorResponse("Invalid username or password.");
                }

                // Generate tokens
                var accessToken = GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken();
                var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

                // Save refresh token to database
                await _userRepository.SaveRefreshTokenAsync(new RefreshToken
                {
                    Token = refreshToken,
                    UserId = user.UserId,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false,
                    CreatedAt = DateTime.UtcNow
                });

                var tokenResponse = new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = expiresInMinutes * 60,
                    TokenType = "Bearer"
                };

                return ApiResponse<TokenResponse>.SuccessResponse(tokenResponse, "Login successful.");
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.ErrorResponse($"Login failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var storedToken = await _userRepository.GetRefreshTokenAsync(request.RefreshToken);

                if (storedToken == null)
                {
                    return ApiResponse<TokenResponse>.ErrorResponse("Invalid refresh token.");
                }

                if (storedToken.IsRevoked)
                {
                    return ApiResponse<TokenResponse>.ErrorResponse("Refresh token has been revoked.");
                }

                if (storedToken.ExpiresAt < DateTime.UtcNow)
                {
                    return ApiResponse<TokenResponse>.ErrorResponse("Refresh token has expired.");
                }

                // Revoke the old refresh token
                await _userRepository.RevokeRefreshTokenAsync(request.RefreshToken);

                var user = storedToken.User;
                var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

                // Generate new tokens
                var newAccessToken = GenerateAccessToken(user);
                var newRefreshToken = GenerateRefreshToken();

                // Save new refresh token
                await _userRepository.SaveRefreshTokenAsync(new RefreshToken
                {
                    Token = newRefreshToken,
                    UserId = user.UserId,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false,
                    CreatedAt = DateTime.UtcNow
                });

                var tokenResponse = new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresIn = expiresInMinutes * 60,
                    TokenType = "Bearer"
                };

                return ApiResponse<TokenResponse>.SuccessResponse(tokenResponse, "Token refreshed successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.ErrorResponse($"Token refresh failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<object>> RevokeTokenAsync(RevokeTokenRequest request)
        {
            try
            {
                var storedToken = await _userRepository.GetRefreshTokenAsync(request.Token);
                if (storedToken == null)
                {
                    return ApiResponse<object>.ErrorResponse("Token not found.");
                }

                if (storedToken.IsRevoked)
                {
                    return ApiResponse<object>.ErrorResponse("Token is already revoked.");
                }

                await _userRepository.RevokeRefreshTokenAsync(request.Token);
                return ApiResponse<object>.SuccessResponse(null!, "Token revoked successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResponse($"Revoke failed: {ex.Message}");
            }
        }

        private string GenerateAccessToken(User user)
        {
            var secretKey = _configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey is not configured.");
            var issuer = _configuration["Jwt:Issuer"] ?? "PRN232.LMS.API";
            var audience = _configuration["Jwt:Audience"] ?? "PRN232.LMS.Client";
            var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
