using System;
using booknest.Models;
using booknest.Models.DTO;

namespace booknest.Service.IService
{
    public interface IProductService
    {
       public Task<Product> AddProductAsync(ProductDto productDto); 
       public Task<Product> UpdateProductAsync(ProductDto productDto);
    }
}
