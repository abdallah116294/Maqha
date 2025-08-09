using Maqha.Core.IRepository;
using Maqha.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        //Using Dictionary to hold repositories for different entities
        private readonly Dictionary<Type, object> _repositories;
        //DbContext would be injected here in a real implementation
        private readonly MaqhaDbContext _context;

        public UnitOfWork(MaqhaDbContext context)
        {
            _repositories = new Dictionary<Type, object>();
            _context = context;
        }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
           await _context.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            //Using Dictionary to cache repositories
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new GenericRepository<TEntity>(_context);
                _repositories[type] = repositoryInstance;
            }
            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}
