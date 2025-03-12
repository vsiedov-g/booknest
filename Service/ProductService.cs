using System;
using booknest.Data;
using booknest.Models;
using booknest.Models.DTO;
using booknest.Repository;
using booknest.Repository.IRepository;
using booknest.Service.IService;
using Microsoft.AspNetCore;

namespace booknest.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IUnitOfWork unitOfWork, AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<Product> AddProductAsync(ProductDto productDto, IFormFile? imageFile)
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
                    if(imageFile != null)
                    {
                        string imageUrl = saveProductImage(product.Id, imageFile);
                        product.ImageUrl = imageUrl;
                        _unitOfWork.Product.Update(product);
                        _unitOfWork.Save();
                    }
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

        public async Task<Product> UpdateProductAsync(ProductDto productDto, IFormFile? imageFile)
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

                    if(imageFile != null)
                    {
                        if(product.ImageUrl != null)
                            deleteProductImage(product.ImageUrl);
                        string imageUrl = saveProductImage(product.Id, imageFile);
                        product.ImageUrl = imageUrl;
                        _unitOfWork.Product.Update(product);
                        _unitOfWork.Save();
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

        public string saveProductImage(int productId, IFormFile imageFile)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string productPath = @"images/products/product-" + productId;
            string finalPath = Path.Combine(wwwRootPath, productPath);

            if(!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }

            using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
                fileStream.Flush();
            }

            return @"/" + productPath + @"/" + fileName;
        }

        public void deleteProductImage(string imageUrl)
        {
            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(oldImagePath))
            {
               File.Delete(oldImagePath);
            }
        }

        public void deleteProductImageDirectory(int productId, string productImageUrl)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string productPath = @"images/products/product-" + productId;
            string finalPath = Path.Combine(wwwRootPath, productPath);
            if(Directory.Exists(finalPath))
            {
                deleteProductImage(productImageUrl);
                Directory.Delete(finalPath);
            }
        }

        public string getProductFileDirectory(string filePath)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
        }

        public string saveProductFile(int productId, IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = @"products/product-" + productId;
            string finalPath = Path.Combine(wwwRootPath, productPath);

            if(!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }

            using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            return @"/" + productPath + @"/" + fileName;
        }

        public void deleteProductFIle(string filePath)
        {
            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }

        public void deleteProductFileDirectory(int productId, string filePath)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string productPath = @"products/product-" + productId;
            string finalPath = Path.Combine(wwwRootPath, productPath);
            if(Directory.Exists(finalPath))
            {
                deleteProductImage(filePath);
                Directory.Delete(finalPath);
            }
        }
    }
}
