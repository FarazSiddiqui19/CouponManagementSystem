using CouponManagementSystem.Entities;

namespace CouponManagementSystem.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
