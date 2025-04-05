using System;
using booknest.Data;
using booknest.Models;
using booknest.Repository.IRepository;

namespace booknest.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IRepository<User> User { get; private set; }
        public IRepository<Product> Product { get; private set; }
        public IRepository<Author> Author { get; private set; }
        public IRepository<Category> Category { get; private set; }
        public IRepository<Publisher> Publisher { get; private set; }
        public IRepository<RefreshToken> RefreshToken { get; private set; }
        public IRepository<Order> Order { get; private set; }
        public UnitOfWork(AppDbContext db) {
            _db = db;
            User = new Repository<User>(_db);
            Product = new Repository<Product>(_db);
            Author = new Repository<Author>(_db);
            Category = new Repository<Category>(_db);
            Publisher = new Repository<Publisher>(_db);
            RefreshToken = new Repository<RefreshToken>(_db);
            Order = new Repository<Order>(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
