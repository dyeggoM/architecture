using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.Architectures.UnitOfWork.Data;
using TEST.Architectures.UnitOfWork.Entities;
using TEST.Architectures.UnitOfWork.Interfaces;

namespace TEST.Architectures.UnitOfWork.Repositories
{
    public class UnitOfWorkPattern : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        public UnitOfWorkPattern(ApplicationContext context)
        {
            _context = context;
        }

        private readonly IRepository<DbModel> _dbModelRepository;
        public IRepository<DbModel> dbModelRepository => _dbModelRepository ?? new BaseRepository<DbModel>(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
