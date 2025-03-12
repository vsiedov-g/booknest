using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using booknest.Models;
using booknest.Models.DTO;
using booknest.Repository;
using booknest.Repository.IRepository;
using booknest.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace booknest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
         private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, IProductService productService) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productService = productService;
        }

        [Authorize]
        [HttpPut("add")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDtoWithImage productDtoWithImage)
        {
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            };
            var productDto = JsonSerializer.Deserialize<ProductDto>(productDtoWithImage.productDto, options);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(productDtoWithImage == null)
                return BadRequest("PRODUCT_NULL");

            var product = await _productService.AddProductAsync(productDto, productDtoWithImage.imageFile);

            return Ok(product);
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Categories,Author");
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mappedProducts = _mapper.Map<List<ProductDto>>(products);
            return Ok(mappedProducts);
        }

        [HttpGet("getById")]
        public IActionResult Get(int productId)
        {
            var product = _unitOfWork.Product.Get(u => productId == u.Id, includeProperties: "Categories,Author");
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(product == null)
            {
                return NotFound();
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDtoWithImage productDtoWithImage)
        {
           var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            }; 
            var productDto = JsonSerializer.Deserialize<ProductDto>(productDtoWithImage.productDto, options);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(productDtoWithImage == null)
                return BadRequest("product is null");

            var product = await _productService.UpdateProductAsync(productDto, productDtoWithImage.imageFile);
            
            return Ok(_mapper.Map<ProductDto>(product)); 
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult RemoveProduct(int productId)
        {
            var product = _unitOfWork.Product.Get(u => productId == u.Id);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(product == null)
            {
                return NotFound();
            }
            _productService.deleteProductImageDirectory(product.Id, product.ImageUrl);
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return Ok(product);
        }

        
        [HttpPut("uploadFile")]
        public IActionResult UploadProductFile(int productId, IFormFile file)
        {
            var product = _unitOfWork.Product.Get(p => p.Id == productId);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(product == null)
            {
                return NotFound();
            }
            string filePath = _productService.saveProductFile(product.Id, file);
            if(filePath == null)
            {
                return BadRequest("something went wrong");
            }

            product.FilePath = filePath;
            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
            return Ok(new {message = product.FilePath});
        }

        [HttpGet("downloadFile")]
        public IActionResult DownloadProductFile(int productId)
        {
            var product = _unitOfWork.Product.Get(p => p.Id == productId);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(product == null)
            {
                return NotFound();
            }
            if(product.FilePath == null)
            {
                return NotFound("This product does not have any files");
            }
            string filePath = _productService.getProductFileDirectory(product.FilePath);
            var provider = new FileExtensionContentTypeProvider();
            if(!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }

    }
    
}
