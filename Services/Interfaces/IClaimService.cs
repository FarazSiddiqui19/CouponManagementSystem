using CouponManagementSystem.Data.Models;
using CouponManagementSystem.DTO;
using CouponManagementSystem.DTO.Claim;
using CouponManagementSystem.Models;

namespace CouponManagementSystem.Services.Interfaces;

public interface IClaimService
{
    Task<ServiceResult<ClaimResponse>> ClaimAsync(Guid userId, string couponCode);
    Task<PaginatedList<ClaimHistoryResponse>> GetUserHistoryAsync(Guid userId, QueryParameters parameters);
}
