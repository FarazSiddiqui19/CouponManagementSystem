using CouponManagementSystem.Data;
using CouponManagementSystem.Data.Models;
using CouponManagementSystem.Entities;
using CouponManagementSystem.Models;
using CouponManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CouponManagementSystem.Repositories;

public class ClaimRepository : Repository<Couponclaim>, IClaimRepository
{
    public ClaimRepository(Couponcontext context) : base(context) { }

    public async Task<PaginatedList<Couponclaim>> GetUserHistoryAsync(Guid userId, QueryParameters parameters)
    {
        var query = _context.Couponclaim
            .Include(cc => cc.coupon)
            .Include(cc => cc.user)
            .Where(cc => cc.userId == userId);

        query = ApplySorting(query, parameters);

        return await ToPaginatedListAsync(query, parameters);
    }

    public async Task<bool> HasUserClaimedAsync(Guid userId, Guid couponId)
    {
        return await _context.Couponclaim
            .AnyAsync(cc => cc.userId == userId && cc.couponId == couponId);
    }

    private static IQueryable<Couponclaim> ApplySorting(IQueryable<Couponclaim> query, QueryParameters parameters)
    {
        return parameters.SortBy?.ToLower() switch
        {
            "claimedat" or "date" => parameters.SortDirection == "desc"
                ? query.OrderByDescending(cc => cc.claimedAt)
                : query.OrderBy(cc => cc.claimedAt),
            "couponcode" or "code" => parameters.SortDirection == "desc"
                ? query.OrderByDescending(cc => cc.coupon.code)
                : query.OrderBy(cc => cc.coupon.code),
            _ => query.OrderByDescending(cc => cc.claimedAt)
        };
    }
}
