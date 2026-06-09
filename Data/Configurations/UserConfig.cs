using CouponManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouponManagementSystem.Data.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.id);

            builder.Property(u => u.name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.passwordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.createdAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(u => u.deletedAt);

            builder.HasIndex(u => u.email)
                .IsUnique()
                .HasFilter("\"deletedAt\" IS NULL");

            builder.HasMany(u => u.coupons)
                .WithOne(c => c.owner)
                .HasForeignKey(c => c.ownerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(u => u.deletedAt == null);
        }
    }
}
