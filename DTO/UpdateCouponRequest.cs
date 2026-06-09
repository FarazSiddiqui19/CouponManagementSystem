using System.ComponentModel.DataAnnotations;

namespace CouponManagementSystem.DTO;

public class UpdateCouponRequest
{
    [Range(0, int.MaxValue)]
    public int? count { get; set; }

    public DateTime? expirationDate { get; set; }
}
