using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Core.IRepository
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        //Function Signature for Unit of Work Pattern
        IGenericRepository<TEntity> Repository<TEntity>()where TEntity : class;
        Task<int> CompleteAsync();
    }
}
