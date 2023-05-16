using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.Architectures.Repositories.Entities;

namespace TEST.Architectures.Repositories.Interfaces
{
    public interface IDbModelRepository : IRepository<DbModel>
    {
        void Dispose();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
