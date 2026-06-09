using CouponManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouponManagementSystem.Data.Configurations
{
    public class CouponclaimConfig : IEntityTypeConfiguration<Couponclaim>
    {
        public void Configure(EntityTypeBuilder<Couponclaim> builder)
        {
            builder.HasKey(c => c.id);

            builder.Property(c => c.claimedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(c => c.deletedAt);

            builder.HasOne(cc => cc.coupon)
                .WithMany(c => c.claims)
                .HasForeignKey(cc => cc.couponId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.user)
                .WithMany(u => u.claims)
                .HasForeignKey(cc => cc.userId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => new { c.couponId, c.userId })
                .IsUnique()
                .HasFilter("\"deletedAt\" IS NULL");

            builder.HasQueryFilter(cc => cc.deletedAt == null);
        }
    }
}
