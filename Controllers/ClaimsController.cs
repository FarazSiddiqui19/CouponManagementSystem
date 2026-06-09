using System.Security.Claims;
using CouponManagementSystem.DTO;
using CouponManagementSystem.DTO.Claim;
using CouponManagementSystem.Models;
using CouponManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClaimsController : ControllerBase
{
    private readonly IClaimService _claimService;

    public ClaimsController(IClaimService claimService)
    {
        _claimService = claimService;
    }

    [HttpPost("{code}")]
    public async Task<IActionResult> Claim(string code)
    {
        var userId = Guid.Parse(User.FindFirstValue("userId")!);
        var result = await _claimService.ClaimAsync(userId, code);
        return ToActionResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetHistory([FromQuery] QueryParameters parameters)
    {
        var userId = Guid.Parse(User.FindFirstValue("userId")!);
        var result = await _claimService.GetUserHistoryAsync(userId, parameters);
        return Ok(result);
    }

    private IActionResult ToActionResult<T>(ServiceResult<T> result) =>
        result.IsSuccess ? Ok(result.Data) : StatusCode(result.StatusCode, new ErrorResponse { message = result.Error });
}
