using CouponManagementSystem.Data;
using CouponManagementSystem.Entities;
using CouponManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CouponManagementSystem.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(Couponcontext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.User
            .FirstOrDefaultAsync(u => u.email == email);
    }
}
