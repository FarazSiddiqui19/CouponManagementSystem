using System.ComponentModel.DataAnnotations;

namespace CouponManagementSystem.DTO;

public class RegisterRequest
{
    [Required, MaxLength(100)]
    public string name { get; set; } = string.Empty;

    [Required, MaxLength(255), EmailAddress]
    public string email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(255)]
    public string password { get; set; } = string.Empty;
}
