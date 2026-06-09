using CouponManagementSystem.Data.Models;
using CouponManagementSystem.DTO;
using CouponManagementSystem.DTO.Coupon;
using CouponManagementSystem.Models;

namespace CouponManagementSystem.Services.Interfaces;

public interface ICouponService
{
    Task<PaginatedList<CouponResponse>> GetGlobalPoolAsync(QueryParameters parameters);
    Task<PaginatedList<CouponResponse>> GetUserCouponsAsync(Guid userId, QueryParameters parameters);
    Task<ServiceResult<CouponResponse>> GetByCodeAsync(string code);
    Task<ServiceResult<CouponResponse>> CreateAsync(Guid userId, CreateCouponRequest request);
    Task<ServiceResult<CouponResponse>> UpdateAsync(Guid userId, string code, UpdateCouponRequest request);
    Task<ServiceResult<bool>> DeleteAsync(Guid userId, string code);
}
