using System.Security.Claims;
using CouponManagementSystem.DTO;
using CouponManagementSystem.DTO.Coupon;
using CouponManagementSystem.Models;
using CouponManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouponsController : ControllerBase
{
    private readonly ICouponService _couponService;

    public CouponsController(ICouponService couponService)
    {
        _couponService = couponService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGlobalPool([FromQuery] QueryParameters parameters)
    {
        var result = await _couponService.GetGlobalPoolAsync(parameters);
        return Ok(result);
    }

    [HttpGet("mine")]
    [Authorize]
    public async Task<IActionResult> GetMyCoupons([FromQuery] QueryParameters parameters)
    {
        var userId = Guid.Parse(User.FindFirstValue("userId")!);
        var result = await _couponService.GetUserCouponsAsync(userId, parameters);
        return Ok(result);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var result = await _couponService.GetByCodeAsync(code);
        return ToActionResult(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCouponRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue("userId")!);
        var result = await _couponService.CreateAsync(userId, request);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetByCode), new { code = request.code }, result.Data);

        return ToActionResult(result);
    }

    [HttpPut("{code}")]
    [Authorize]
    public async Task<IActionResult> Update(string code, [FromBody] UpdateCouponRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue("userId")!);
        var result = await _couponService.UpdateAsync(userId, code, request);
        return ToActionResult(result);
    }

    [HttpDelete("{code}")]
    [Authorize]
    public async Task<IActionResult> Delete(string code)
    {
        var userId = Guid.Parse(User.FindFirstValue("userId")!);
        var result = await _couponService.DeleteAsync(userId, code);
        return result.IsSuccess ? NoContent() : ToActionResult(result);
    }

    private IActionResult ToActionResult<T>(ServiceResult<T> result) =>
        result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new ErrorResponse { message = result.Error });
}
