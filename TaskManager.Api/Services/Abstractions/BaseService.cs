

using TaskManager.Api.Services.Interfaces;

namespace TaskManager.Api.Services.Abstractions
{
    public abstract class BaseService<T> : IBaseService<T> where T : class
    {
        public Task<bool> CreateAsync(T entity)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
