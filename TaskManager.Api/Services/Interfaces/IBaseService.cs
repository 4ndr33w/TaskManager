namespace TaskManager.Api.Services.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);

        Task<T> GetAsync(Guid id);
    }
}
