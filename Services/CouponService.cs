using CouponManagementSystem.Data;
using CouponManagementSystem.Data.Models;
using CouponManagementSystem.DTO;
using CouponManagementSystem.DTO.Coupon;
using CouponManagementSystem.Entities;
using CouponManagementSystem.Models;
using CouponManagementSystem.Repositories.Interfaces;
using CouponManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CouponManagementSystem.Services;

public class CouponService : ICouponService
{
    private readonly Couponcontext _context;
    private readonly ICouponRepository _couponRepo;

    public CouponService(Couponcontext context, ICouponRepository couponRepo)
    {
        _context = context;
        _couponRepo = couponRepo;
    }

    public async Task<PaginatedList<CouponResponse>> GetGlobalPoolAsync(QueryParameters parameters)
    {
        var result = await _couponRepo.GetGlobalPoolAsync(parameters);
        return Map(result);
    }

    public async Task<PaginatedList<CouponResponse>> GetUserCouponsAsync(Guid userId, QueryParameters parameters)
    {
        var result = await _couponRepo.GetUserCouponsAsync(userId, parameters);
        return Map(result);
    }

    public async Task<ServiceResult<CouponResponse>> GetByCodeAsync(string code)
    {
        var coupon = await _couponRepo.GetByCodeAsync(code);
        if (coupon is null)
            return ServiceResult<CouponResponse>.Failure(404, "Coupon not found");

        return ServiceResult<CouponResponse>.Success(Map(coupon));
    }

    public async Task<ServiceResult<CouponResponse>> CreateAsync(Guid userId, CreateCouponRequest request)
    {
        var existing = await _couponRepo.GetByCodeAsync(request.code);
        if (existing is not null)
            return ServiceResult<CouponResponse>.Failure(409, "Coupon code already exists");

        if (request.expirationDate <= DateTime.UtcNow)
            return ServiceResult<CouponResponse>.Failure(400, "Expiration date must be in the future");

        var coupon = new Coupon
        {
            id = Guid.NewGuid(),
            code = request.code,
            ownerId = userId,
            expirationDate = request.expirationDate,
            count = request.count,
        };

        await _couponRepo.AddAsync(coupon);

        return ServiceResult<CouponResponse>.Success(Map(coupon));
    }

    public async Task<ServiceResult<CouponResponse>> UpdateAsync(Guid userId, string code, UpdateCouponRequest request)
    {
        var coupon = await _couponRepo.GetByCodeAsync(code);
        if (coupon is null)
            return ServiceResult<CouponResponse>.Failure(404, "Coupon not found");

        if (coupon.ownerId != userId)
            return ServiceResult<CouponResponse>.Failure(403, "Forbidden");

        if (request.count.HasValue)
        {
            var claimedCount = await _context.Couponclaim
                .CountAsync(cc => cc.couponId == coupon.id);
            if (request.count.Value < claimedCount)
                return ServiceResult<CouponResponse>.Failure(400,
                    $"Count cannot be less than the number of already-placed claims ({claimedCount})");
            coupon.count = request.count.Value;
        }

        if (request.expirationDate.HasValue)
            coupon.expirationDate = request.expirationDate.Value;

        _couponRepo.Update(coupon);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return ServiceResult<CouponResponse>.Failure(409,
                "Conflict: coupon was modified by another request. Reload and try again.");
        }

        return ServiceResult<CouponResponse>.Success(Map(coupon));
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid userId, string code)
    {
        var coupon = await _couponRepo.GetByCodeAsync(code);
        if (coupon is null)
            return ServiceResult<bool>.Failure(404, "Coupon not found");

        if (coupon.ownerId != userId)
            return ServiceResult<bool>.Failure(403, "Forbidden");

        coupon.deletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ServiceResult<bool>.Success(true);
    }

    private static CouponResponse Map(Coupon c) => new()
    {
        id = c.id,
        code = c.code,
        ownerId = c.ownerId,
        expirationDate = c.expirationDate,
        count = c.count,
        RowVersion = c.RowVersion,
    };

    private static PaginatedList<CouponResponse> Map(PaginatedList<Coupon> source) =>
        new(source.Items.Select(Map).ToList(), source.TotalItems);
}
