using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.Architectures.Repositories.Data;
using TEST.Architectures.Repositories.Entities;
using TEST.Architectures.Repositories.Interfaces;

namespace TEST.Architectures.Repositories.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> _entities;
        public BaseRepository(ApplicationContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            //return _entities.AsEnumerable();
            return await _entities.ToListAsync();
        }

        public async Task<T> Find(long id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<T> GetById(long id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public async Task Update(long id, T entity)
        {
            var result = await _entities.FindAsync(id);
            if (result == null)
                return;
            _entities.Update(entity);
        }
        public async Task UpdateRange(IEnumerable<long> ids, IEnumerable<T> entities)
        {
            var entityList = entities.ToList();
            foreach (var item in ids)
            {
                var result = await _entities.FindAsync(item);
                if (result == null)
                    entityList.Remove(result);
            }
            _entities.UpdateRange(entityList);
        }

        public async Task Delete(long id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
        }
        public async Task DeleteRange(IEnumerable<long> id)
        {
            IEnumerable<T> entities = await GetAll();
            var entity = entities.Where(x => id.Contains(x.Id));
            _entities.RemoveRange(entity);
        }
    }
}
