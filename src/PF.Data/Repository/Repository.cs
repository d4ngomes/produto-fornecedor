using Microsoft.EntityFrameworkCore;
using PF.Business.Interfaces;
using PF.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PF.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext db;
        protected readonly DbSet<T> dbSet;

        public Repository(AppDbContext context)
        {
            db = context;
            dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> ObterPorId(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<List<T>> ObterTodos()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task Adicionar(T obj)
        {
            dbSet.Add(obj);
            await SaveChanges();
        }

        public virtual async Task Atualizar(T obj)
        {
            dbSet.Update(obj);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            dbSet.Remove(await dbSet.FindAsync(id));
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
