using CouponManagementSystem.Data;
using CouponManagementSystem.Data.Models;
using CouponManagementSystem.DTO;
using CouponManagementSystem.DTO.Claim;
using CouponManagementSystem.Entities;
using CouponManagementSystem.Models;
using CouponManagementSystem.Repositories.Interfaces;
using CouponManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CouponManagementSystem.Services;

public class ClaimService : IClaimService
{
    private readonly Couponcontext _context;
    private readonly IClaimRepository _claimRepo;
    private const int MaxRetries = 3;

    public ClaimService(Couponcontext context, IClaimRepository claimRepo)
    {
        _context = context;
        _claimRepo = claimRepo;
    }

    public async Task<ServiceResult<ClaimResponse>> ClaimAsync(Guid userId, string couponCode)
    {
        for (var attempt = 1; attempt <= MaxRetries; attempt++)
        {
            var coupon = await _context.Coupon
                .FirstOrDefaultAsync(c => c.code == couponCode);

            if (coupon is null)
                return ServiceResult<ClaimResponse>.Failure(404, "Coupon not found");

            if (coupon.ownerId == userId)
                return ServiceResult<ClaimResponse>.Failure(400, "Cannot claim your own coupon");

            if (coupon.expirationDate <= DateTime.UtcNow)
                return ServiceResult<ClaimResponse>.Failure(410, "Coupon has expired");

            if (coupon.count <= 0)
                return ServiceResult<ClaimResponse>.Failure(410, "Coupon is sold out");

            var alreadyClaimed = await _context.Couponclaim
                .AnyAsync(cc => cc.couponId == coupon.id && cc.userId == userId);
            if (alreadyClaimed)
                return ServiceResult<ClaimResponse>.Failure(409, "Already claimed this coupon");

            var claim = new Couponclaim
            {
                id = Guid.NewGuid(),
                couponId = coupon.id,
                userId = userId,
            };

            coupon.count--;
            _context.Couponclaim.Add(claim);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (attempt == MaxRetries)
                    return ServiceResult<ClaimResponse>.Failure(409,
                        "Conflict: coupon was modified by another request. Try again.");

                _context.Entry(claim).State = EntityState.Detached;
                await _context.Entry(coupon).ReloadAsync();
                continue;
            }

            return ServiceResult<ClaimResponse>.Success(new ClaimResponse
            {
                id = claim.id,
                couponId = coupon.id,
                couponCode = coupon.code,
                userId = userId,
                claimedAt = claim.claimedAt,
            });
        }

        return ServiceResult<ClaimResponse>.Failure(409,
            "Conflict: coupon was modified by another request. Try again.");
    }

    public async Task<PaginatedList<ClaimHistoryResponse>> GetUserHistoryAsync(
        Guid userId, QueryParameters parameters)
    {
        var result = await _claimRepo.GetUserHistoryAsync(userId, parameters);

        return new PaginatedList<ClaimHistoryResponse>(
            result.Items.Select(cc => new ClaimHistoryResponse
            {
                id = cc.id,
                couponId = cc.couponId,
                couponCode = cc.coupon.code,
                claimedAt = cc.claimedAt,
            }).ToList(),
            result.TotalItems);
    }
}
