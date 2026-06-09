namespace CouponManagementSystem.Entities
{
    public class Coupon
    {
        public Guid id { get; set; }
        public string code { get; set; } = string.Empty;
        public Guid ownerId { get; set; }
        public DateTime expirationDate { get; set; }
        public int count { get; set; }
        public uint RowVersion { get; set; }
        public DateTime? deletedAt { get; set; }

        public User owner { get; set; } = null!;
        public List<Couponclaim> claims { get; set; } = new();
    }
}
