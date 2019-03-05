using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinCity.RepositoryDomain;

namespace ThinCity.AbstractRepository
{
    public class EFRepository<T, ID> : IRepository<T> where T : TEntity<ID>, IAggregateRoot
    {
        protected readonly DatabaseContext _ctx;
        protected readonly DbSet<T> _dbSet;
        private readonly ILogger _logger;

        public EFRepository(DatabaseContext dbContext, ILogger logger)
        {
            _ctx = dbContext;
            _logger = logger;
            _dbSet = _ctx.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Critical, $"{_logger.GetType().Name}.Add: {ex.Message}");
                _ctx.Dispose();
            }
        }

        public async Task<int> CommitAsync()
        {
            int result = -1;
            try
            {
                result = await _ctx.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Critical, $"{_logger.GetType().Name}.Add: {ex.Message}");
                _ctx.Dispose();
            }
            return result;
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                entity = await _dbSet.FindAsync(entity.GetId());
                if (entity != null)
                {
                    _ctx.Entry(entity).State = EntityState.Deleted;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, $"{_logger.GetType().Name}.Delete: {ex.Message}");
                _ctx.Dispose();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<T, bool> predicate = null)
        {
            var result = default(IEnumerable<T>);
            if (predicate != null)
            {
                try
                {
                    result = _dbSet.Where(predicate);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Critical, $"{_logger.GetType().Name}.GetAll: {ex.Message}");
                    _ctx.Dispose();
                }
            }
            else
                result = _dbSet;

            return await Task.FromResult(result);
        }

        public async Task<T> GetOneAsync(Func<T, bool> predicate)
        {
            var result = default(T);
            var collection = await GetAllAsync(predicate);
            try
            {
                result = collection.SingleOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, $"{_logger.GetType().Name}.GetOne: {ex.Message}");
            }
            return result;
        }

        public void Update(T entity)
        {
            try
            {
                _dbSet.Attach(entity);
                _ctx.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, $"{_logger.GetType().Name}.GetAll: {ex.Message}");
                _ctx.Dispose();
            }
        }
    }
}
