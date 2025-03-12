using System;
using booknest.Data;
using booknest.Models;
using booknest.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace booknest.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User user)
        {
            _db.Update(user);
        }
    }
}
