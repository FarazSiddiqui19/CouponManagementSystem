using System.ComponentModel.DataAnnotations;

namespace CouponManagementSystem.DTO.Coupon;

public class CreateCouponRequest
{
    [Required, MaxLength(100)]
    public string code { get; set; } = string.Empty;

    [Required]
    public DateTime expirationDate { get; set; }

    [Range(1, int.MaxValue)]
    public int count { get; set; }
}
