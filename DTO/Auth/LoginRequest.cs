namespace CouponManagementSystem.DTO.Auth;

public record LoginRequest(
    string Email,
    string Password
);
