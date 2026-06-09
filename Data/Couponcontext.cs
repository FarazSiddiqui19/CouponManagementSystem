using Microsoft.EntityFrameworkCore;
using CouponManagementSystem.Entities;

namespace CouponManagementSystem.Data
{
    public class Couponcontext : DbContext
    {
        public Couponcontext(DbContextOptions<Couponcontext> options) : base(options) { }

        public DbSet<User> User => Set<User>();
        public DbSet<Couponclaim> Couponclaim => Set<Couponclaim>();
        public DbSet<Coupon> Coupon => Set<Coupon>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Couponcontext).Assembly);
        }
    }
}
