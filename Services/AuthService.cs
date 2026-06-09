using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CouponManagementSystem.Data;
using CouponManagementSystem.DTO;
using CouponManagementSystem.Entities;
using CouponManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CouponManagementSystem.Services;

public class AuthService : IAuthService
{
    private readonly Couponcontext _context;
    private readonly IConfiguration _config;

    public AuthService(Couponcontext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<ServiceResult<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await _context.User.AnyAsync(u => u.email == request.email))
            return ServiceResult<AuthResponse>.Failure(409, "Email already exists");

        var user = new User
        {
            id = Guid.NewGuid(),
            name = request.name,
            email = request.email,
            passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password),
        };

        _context.User.Add(user);
        await _context.SaveChangesAsync();

        return ServiceResult<AuthResponse>.Success(
            new AuthResponse { token = GenerateToken(user) });
    }

    public async Task<ServiceResult<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.email == request.email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.password, user.passwordHash))
            return ServiceResult<AuthResponse>.Failure(401, "Invalid email or password");

        return ServiceResult<AuthResponse>.Success(
            new AuthResponse { token = GenerateToken(user) });
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("userId", user.id.ToString()),
            new Claim("email", user.email),
        };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                double.Parse(_config["JwtSettings:ExpiryInHours"] ?? "1")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
