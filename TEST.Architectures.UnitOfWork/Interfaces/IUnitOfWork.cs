using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.Architectures.UnitOfWork.Entities;

namespace TEST.Architectures.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<DbModel> dbModelRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
