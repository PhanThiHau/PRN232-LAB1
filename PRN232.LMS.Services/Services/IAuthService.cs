using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.RequestModels;
using PRN232.LMS.Services.ResponseModels;

namespace PRN232.LMS.Services.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<ApiResponse<object>> RevokeTokenAsync(RevokeTokenRequest request);
    }
}
