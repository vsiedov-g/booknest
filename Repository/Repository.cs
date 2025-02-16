using System;
using System.Linq.Expressions;
using booknest.Data;
using booknest.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace booknest.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext db) 
        {
            _db = db;
            dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var property in includeProperties
                    .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(property);
                    }
            }
            return query.FirstOrDefault(filter);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null) {
                query = query.Where(filter);
            }
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var property in includeProperties
                    .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(property);
                    }
            }
            
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
        public void Update (T entity)
        {
            _db.Update(entity);
        }
    }
}
