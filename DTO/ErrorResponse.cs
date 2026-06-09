namespace CouponManagementSystem.DTO;

public class ErrorResponse
{
    public string message { get; set; } = string.Empty;
    public List<string>? errors { get; set; }
}
