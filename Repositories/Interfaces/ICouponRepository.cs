using CouponManagementSystem.Data.Models;
using CouponManagementSystem.Entities;
using CouponManagementSystem.Models;

namespace CouponManagementSystem.Repositories.Interfaces;

public interface ICouponRepository : IRepository<Coupon>
{
    Task<PaginatedList<Coupon>> GetGlobalPoolAsync(QueryParameters parameters);
    Task<PaginatedList<Coupon>> GetUserCouponsAsync(Guid userId, QueryParameters parameters);
    Task<Coupon?> GetByCodeAsync(string code);
}
