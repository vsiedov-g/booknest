using System;
using booknest.Models;
using booknest.Models.DTO;

namespace booknest.Service.IService
{
    public interface IProductService
    {
       public Task<Product> AddProductAsync(ProductDto productDto, IFormFile? imageFile); 
       public Task<Product> UpdateProductAsync(ProductDto productDto, IFormFile? imageFile);
       public Task<string> saveProductImageAsync(int productId, IFormFile imageFile);
       public void deleteProductImage(string imageUrl);
       public void deleteProductImageDirectory(int productId, string productImageUrl);
       public string getProductFileDirectory(string filePath);
       public Task<string> saveProductFileAsync(int productId, IFormFile file);
       public void deleteProductFIle(string filePath);
       public void deleteProductFileDirectory(int productId, string filePath);
    }
}
