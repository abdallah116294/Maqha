using Maqha.Core.IRepository;
using Maqha.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Repository.Repository
{
    public class GenericRepository <T>: IGenericRepository<T> where T : class
    {
        //DbContext would be injected here in a real implementation
        private readonly MaqhaDbContext _context;
        
        public GenericRepository(MaqhaDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T t)
        {
            _context.Set<T>().Remove(t);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            var entities = await _context.Set<T>().ToListAsync();
            return entities;
        }

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            if(predicate != null)
            {
                query = query.Where(predicate);
            }
            return query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
           var result = await _context.Set<T>().FindAsync(id);
            return result!;
        }

        public async Task<T> GetByIdAsync(int id,  params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task UpdateAsync(T entity)
        {
          _context.Set<T>().Update(entity);
          await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Action<T> updateAction)
        {
            var entiy= await GetByIdAsync(id);
            if (entiy != null)
            {
                updateAction(entiy);
                _context.Set<T>().Update(entiy);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }
        }
    }
}
