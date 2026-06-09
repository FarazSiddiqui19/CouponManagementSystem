using CouponManagementSystem.Data;
using CouponManagementSystem.Data.Models;
using CouponManagementSystem.Entities;
using CouponManagementSystem.Models;
using CouponManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CouponManagementSystem.Repositories;

public class CouponRepository : Repository<Coupon>, ICouponRepository
{
    public CouponRepository(Couponcontext context) : base(context) { }

    public async Task<PaginatedList<Coupon>> GetGlobalPoolAsync(QueryParameters parameters)
    {
        var query = _context.Coupon
            .Include(c => c.claims)
            .Where(c => c.count > 0 && c.expirationDate > DateTime.UtcNow);

        query = ApplySorting(query, parameters);

        return await ToPaginatedListAsync(query, parameters);
    }

    public async Task<PaginatedList<Coupon>> GetUserCouponsAsync(Guid userId, QueryParameters parameters)
    {
        var query = _context.Coupon
            .Include(c => c.claims)
            .Where(c => c.ownerId == userId);

        query = ApplySorting(query, parameters);

        return await ToPaginatedListAsync(query, parameters);
    }

    public async Task<Coupon?> GetByCodeAsync(string code)
    {
        return await _context.Coupon
            .FirstOrDefaultAsync(c => c.code == code);
    }

    private static IQueryable<Coupon> ApplySorting(IQueryable<Coupon> query, QueryParameters parameters)
    {
        return parameters.SortBy?.ToLower() switch
        {
            "code" => parameters.SortDirection == "desc"
                ? query.OrderByDescending(c => c.code)
                : query.OrderBy(c => c.code),
            "expirationdate" or "expiration" => parameters.SortDirection == "desc"
                ? query.OrderByDescending(c => c.expirationDate)
                : query.OrderBy(c => c.expirationDate),
            "count" => parameters.SortDirection == "desc"
                ? query.OrderByDescending(c => c.count)
                : query.OrderBy(c => c.count),
            _ => query.OrderBy(c => c.id)
        };
    }
}
