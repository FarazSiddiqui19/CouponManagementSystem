using CouponManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouponManagementSystem.Data.Configurations
{
    public class CouponConfig : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(c => c.id);

            builder.Property(c => c.code)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.expirationDate)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Property(c => c.RowVersion)
                .IsRowVersion();

            builder.Property(c => c.deletedAt);

            builder.HasIndex(c => c.code)
                .IsUnique()
                .HasFilter("\"deletedAt\" IS NULL");

            builder.HasOne(c => c.owner)
                .WithMany(u => u.coupons)
                .HasForeignKey(c => c.ownerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.claims)
                .WithOne(cc => cc.coupon)
                .HasForeignKey(cc => cc.couponId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(c => c.deletedAt == null);
        }
    }
}
