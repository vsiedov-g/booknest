using System;
using booknest.Data;
using booknest.Models;

namespace booknest.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepository<User> User {get;}
        IRepository<Product> Product {get;}
        IRepository<Author> Author {get;}
        IRepository<Category> Category {get;}
        IRepository<Publisher> Publisher {get;}
        IRepository<RefreshToken> RefreshToken {get;}
        void Save();
    }
}
