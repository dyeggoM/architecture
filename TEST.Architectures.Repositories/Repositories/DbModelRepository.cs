using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.Architectures.Repositories.Data;
using TEST.Architectures.Repositories.Entities;
using TEST.Architectures.Repositories.Interfaces;

namespace TEST.Architectures.Repositories.Repositories
{
    public class DbModelRepository : BaseRepository<DbModel>, IDbModelRepository
    {
        public DbModelRepository(ApplicationContext context) : base(context)
        {
        }

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
