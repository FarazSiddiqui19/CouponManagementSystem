namespace CouponManagementSystem.DTO.Auth;

public record RegisterRequest(
    string Name,
    string Email,
    string Password
);
