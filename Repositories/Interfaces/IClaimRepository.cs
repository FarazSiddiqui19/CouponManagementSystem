using CouponManagementSystem.Data.Models;
using CouponManagementSystem.Entities;
using CouponManagementSystem.Models;

namespace CouponManagementSystem.Repositories.Interfaces;

public interface IClaimRepository : IRepository<Couponclaim>
{
    Task<PaginatedList<Couponclaim>> GetUserHistoryAsync(Guid userId, QueryParameters parameters);
    Task<bool> HasUserClaimedAsync(Guid userId, Guid couponId);
}
