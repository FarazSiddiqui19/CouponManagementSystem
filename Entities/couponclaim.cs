namespace CouponManagementSystem.Entities
{
    public class Couponclaim
    {
        public Guid id { get; set; }
        public Guid couponId { get; set; }
        public Guid userId { get; set; }
        public DateTime claimedAt { get; set; }
        public DateTime? deletedAt { get; set; }

        public User user { get; set; } = null!;
        public Coupon coupon { get; set; } = null!;
    }
}
