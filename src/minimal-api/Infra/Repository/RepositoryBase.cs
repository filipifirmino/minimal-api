using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Infra.Context;
using minimal_api.Infra.Repository.Interface;

namespace minimal_api.Infra.Repository;

public class RepositoryBase<T>(DataContext context) : IRepositoryBase<T>
    where T : class
{
    protected readonly DataContext _context = context;

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _context.Set<T>().ToListAsync();

    public virtual async Task<PagedResult<T>> GetPagedAsync(PaginationParameters paginationParameters)
    {
        var totalCount = await _context.Set<T>().CountAsync();
        
        var items = await _context.Set<T>()
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync();

        return new PagedResult<T>(items, totalCount, paginationParameters.PageNumber, paginationParameters.PageSize);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
        => await _context.Set<T>().FindAsync(id);

    public virtual async Task<T?> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}