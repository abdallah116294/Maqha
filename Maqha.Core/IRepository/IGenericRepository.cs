using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Core.IRepository
{
    public interface IGenericRepository<T> where T:class
    {
        //CURD operations
        //Get All Records
        Task<List<T>> GetAllAsync();
        //Get All Records with Include Properties=> With both include and Predicate
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes);
        //Get Record By Id
        Task<T>GetByIdAsync(int id);
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        //Add Record
        Task AddAsync(T entity);
        //Update Record
        Task UpdateAsync(T entity);
        //Updat Specific Record
        Task UpdateAsync(int id, Action<T> updateAction);
        //Delete Record
        Task DeleteAsync(T t);
    }
}
