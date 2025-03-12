using System;
using Microsoft.EntityFrameworkCore;
using booknest.Models;

namespace booknest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);

            builder.Entity<Product>().HasData(
                new  Product {Id = 1, Title = "Lie", Description = "lie", AuthorId = 1,  Price = 15 },
                new  Product {Id = 2, Title = "Cold", Description = "cold", AuthorId = 2,  Price = 16 },
                new  Product {Id = 3, Title = "Hot", Description = "hot", AuthorId = 3, Price = 17 }
            );
            builder.Entity<Author>().HasData(
                new  Author {Id = 1, Name = "John Wick", },
                new  Author {Id = 2, Name = "Vlad Siedov"},
                new  Author {Id = 3, Name = "Antoni Proshuto"}
            );
            builder.Entity<Category>().HasData(
                new  Category {Id = 1, Name = "Action" },
                new  Category {Id = 2, Name = "Horror"},
                new  Category {Id = 3, Name = "Fantasy"}
            );
            builder.Entity<Product>()
            .HasMany(c => c.Categories)
            .WithMany(p => p.Products)
            .UsingEntity(j => j.HasData(
                new {CategoriesId = 1, ProductsId = 1},
                new {CategoriesId = 2, ProductsId = 2},
                new {CategoriesId = 3, ProductsId = 3},
                new {CategoriesId = 1, ProductsId = 3}
            ));
            builder.Entity<Product>()
            .HasMany(c => c.Users)
            .WithMany(p => p.Products)
            .UsingEntity(j => j.HasData(
                new {UsersId = 1, ProductsId = 1},
                new {UsersId = 1, ProductsId = 2},
                new {UsersId = 1, ProductsId = 3}
            ));
        }
    }
}
