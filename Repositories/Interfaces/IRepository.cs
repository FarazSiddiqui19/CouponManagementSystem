using CouponManagementSystem.Data.Models;

namespace CouponManagementSystem.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    void Update(T entity);
    Task SoftDeleteAsync(Guid id);
    IQueryable<T> Query();
}
