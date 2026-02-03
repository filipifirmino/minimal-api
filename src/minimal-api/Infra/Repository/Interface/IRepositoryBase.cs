using minimal_api.Dominio.DTOs;

namespace minimal_api.Infra.Repository.Interface;

public interface IRepositoryBase<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<PagedResult<T>> GetPagedAsync(PaginationParameters paginationParameters);
    Task<T?> GetByIdAsync(Guid id);
    Task<T?>  AddAsync (T entity);
    Task UpdateAsync (T entity);
    Task DeleteAsync (T entity);
}