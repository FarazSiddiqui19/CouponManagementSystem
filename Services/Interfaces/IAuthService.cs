using CouponManagementSystem.DTO;
using CouponManagementSystem.DTO.Auth;

namespace CouponManagementSystem.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ServiceResult<AuthResponse>> LoginAsync(LoginRequest request);
}
