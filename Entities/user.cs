namespace CouponManagementSystem.Entities
{
    public class User
    {
        public Guid id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string passwordHash { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
        public DateTime? deletedAt { get; set; }

        public List<Coupon> coupons { get; set; } = new();
        public List<Couponclaim> claims { get; set; } = new();
    }
}
