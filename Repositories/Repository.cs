using CouponManagementSystem.Data;
using CouponManagementSystem.Data.Models;
using CouponManagementSystem.Models;
using CouponManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CouponManagementSystem.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly Couponcontext _context;
    protected readonly DbSet<T> _set;

    public Repository(Couponcontext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _set.FindAsync(id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var entry = await _set.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public virtual void Update(T entity)
    {
        _set.Update(entity);
    }

    public virtual async Task SoftDeleteAsync(Guid id)
    {
        var entity = await _set.FindAsync(id);
        if (entity is null) return;

        var prop = entity.GetType().GetProperty("deletedAt");
        if (prop is not null)
        {
            prop.SetValue(entity, DateTime.UtcNow);
        }

        await _context.SaveChangesAsync();
    }

    public virtual IQueryable<T> Query()
    {
        return _set.AsQueryable();
    }

    protected async Task<PaginatedList<T>> ToPaginatedListAsync(
        IQueryable<T> query,
        QueryParameters parameters)
    {
        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, totalItems);
    }
}
