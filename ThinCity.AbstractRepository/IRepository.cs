using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinCity.RepositoryDomain;

namespace ThinCity.AbstractRepository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(Func<T, bool> predicate = null);

        Task<T> GetOneAsync(Func<T, bool> predicate);

        Task AddAsync(T entity);

        void Update(T entity);

        Task DeleteAsync(T entity);

        Task<int> CommitAsync();
    }
}
