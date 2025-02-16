using System;
using booknest.Data;
using booknest.Models;
using booknest.Models.DTO;
using booknest.Repository;
using booknest.Repository.IRepository;
using booknest.Service.IService;

namespace booknest.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;

        public ProductService(IUnitOfWork unitOfWork, AppDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public async Task<Product> AddProductAsync(ProductDto productDto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            {
                try
                {
                    var author = _unitOfWork.Author.Get(p => p.Name == productDto.Author);
                    if (author == null)
                    {
                        throw new ArgumentException($"Author with name {productDto.Author} not found");
                    }
                    var categories = new List<Category>();
                    foreach(var categoryName in productDto.Categories)
                    {
                        var category = _unitOfWork.Category.Get(c => c.Name == categoryName);
                        if (category == null)
                        {
                            throw new ArgumentException($"Category with name {categoryName} not found");
                        }
                        categories.Add(category);  
                    }
                    var product = new Product
                    {
                        Title = productDto.Title,
                        Description = productDto.Description,
                        Author = author,
                        Categories = categories,
                        Price = productDto.Price
                    };

                    _unitOfWork.Product.Add(product);
                    _unitOfWork.Save();

                    await transaction.CommitAsync();
                    return product;
                } 
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<Product> UpdateProductAsync(ProductDto productDto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            {
                try
                {
                    var product = _unitOfWork.Product.Get(c => c.Id == productDto.Id, includeProperties: "Categories,Author");
        
                    if (product == null)
                    {
                        throw new ArgumentException($"Product with name {productDto.Title} not found");
                    }

                    product.Title = productDto.Title;
                    product.Description = productDto.Description;
                    product.Price = productDto.Price;
                    
                    var author = _unitOfWork.Author.Get(p => p.Name == productDto.Author);
                    if (author == null)
                    {
                        throw new ArgumentException($"Author with name {productDto.Author} not found");
                    }

                    product.Author = author;

                    foreach (var categoryName in product.Categories)
                    {
                        product.Categories.Remove(categoryName);
                    }
                    foreach(var categoryName in productDto.Categories)
                    {
                        var category = _unitOfWork.Category.Get(c => c.Name == categoryName);
                        if (category == null)
                        {
                            throw new ArgumentException($"Category with name {categoryName} not found");
                        }
                        product.Categories.Add(category);
                    }
                    
                    _unitOfWork.Product.Update(product);
                    _unitOfWork.Save();

                    await transaction.CommitAsync();
                    return product;
                } 
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
