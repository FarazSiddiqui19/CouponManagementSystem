namespace CouponManagementSystem.DTO;

public class CouponResponse
{
    public Guid id { get; set; }
    public string code { get; set; } = string.Empty;
    public Guid ownerId { get; set; }
    public DateTime expirationDate { get; set; }
    public int count { get; set; }
    public uint RowVersion { get; set; }
}
