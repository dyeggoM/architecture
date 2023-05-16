using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.Architectures.Repositories.Entities;

namespace TEST.Architectures.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Find(long id);
        Task<T> GetById(long id);
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task Update(long id, T entity);
        Task UpdateRange(IEnumerable<long> ids, IEnumerable<T> entities);
        Task Delete(long id);
        Task DeleteRange(IEnumerable<long> id);
    }
}
