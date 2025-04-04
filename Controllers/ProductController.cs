using System;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using booknest.Models;
using booknest.Models.DTO;
using booknest.Repository;
using booknest.Repository.IRepository;
using booknest.Service.IService;
using booknest.Utility;
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
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _config;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, IProductService productService, IHttpClientFactory httpFactory, IConfiguration config) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productService = productService;
            _httpFactory = httpFactory;
            _config = config;
        }

        [Authorize]
        [HttpPut("add")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDtoWithImage productDtoWithImage)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            };
            var productDto = JsonSerializer.Deserialize<ProductDto>(productDtoWithImage.productDto, options);

            var product = await _productService.AddProductAsync(productDto!, productDtoWithImage.imageFile);

            return Ok(product);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(int? userId)
        {
            if(userId != null)
            {
                var user = await _unitOfWork.User.GetAsync(u => u.Id == userId, includeProperties: "Products");
                if(user == null)
                {
                    return NotFound("User is not found");
                }
                var mappedUserProducts = _mapper.Map<List<ProductDto>>(user.Products);
                return Ok(mappedUserProducts);
            }
            var products = await _unitOfWork.Product.GetAllAsync(includeProperties: "Categories,Author");
            var mappedProducts = _mapper.Map<List<ProductDto>>(products);
            return Ok(mappedProducts);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> Get(int productId)
        {
            var product = await _unitOfWork.Product.GetAsync(u => productId == u.Id, includeProperties: "Categories,Author");
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
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
           var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            }; 
            var productDto = JsonSerializer.Deserialize<ProductDto>(productDtoWithImage.productDto, options);

            var product = await _productService.UpdateProductAsync(productDto!, productDtoWithImage.imageFile);
            
            return Ok(_mapper.Map<ProductDto>(product)); 
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            var product = await _unitOfWork.Product.GetAsync(u => productId == u.Id);
            if(product == null)
            {
                return NotFound();
            }
            _productService.deleteProductImageDirectory(product.Id, product.ImageUrl);
            _unitOfWork.Product.Remove(product);
            await _unitOfWork.SaveAsync();
            return Ok(product);
        }

        [Authorize]
        [HttpPut("uploadFile")]
        public async Task<IActionResult> UploadProductFile(int productId, IFormFile file)
        {
            var product = await _unitOfWork.Product.GetAsync(p => p.Id == productId);
            if(product == null)
            {
                return NotFound();
            }
            string filePath = await _productService.saveProductFileAsync(product.Id, file);
            if(filePath == null)
            {
                return BadRequest("something went wrong");
            }

            product.FilePath = filePath;
            _unitOfWork.Product.Update(product);
            await _unitOfWork.SaveAsync();
            return Ok(new {message = product.FilePath});
        }

        [Authorize]
        [HttpGet("downloadFile")]
        public async Task<IActionResult> DownloadProductFile(int productId)
        {
            var product = await _unitOfWork.Product.GetAsync(p => p.Id == productId);
            if(product == null)
            {
                return NotFound("Product not found");
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

        [Authorize]
        [HttpPost("purchase")]
        public async Task<IActionResult> CreatePayment(int productId, [FromForm] string redirectUrl)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = await _unitOfWork.User.GetAsync(u => u.Id.ToString() == userId, includeProperties: "Products");
            if(user == null)
            {
                return NotFound("User is not found");
            }
            Product product = await _unitOfWork.Product.GetAsync(p => p.Id == productId);
            if(product == null)
            {
                return NotFound("Product is not found");
            }
            if(user.Products.Any(p => p.Id == product.Id))
            {
                return BadRequest("User already purchased the product");
            }

            var data = new {
                amount = product.Price * 100,
                ccy = 980, 
                redirectUrl = redirectUrl,
                webHookUrl = $"{_config["NgrokUrl"]}/api/Product/callback"
            };

            var request = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var client = _httpFactory.CreateClient("monobank"); 

            var response = await client.PostAsync("/api/merchant/invoice/create", request); 
            
            if(response.IsSuccessStatusCode)
            {
                var content = JsonSerializer.Deserialize<PaymentResponse>(await response.Content.ReadAsStringAsync())!;
                Order order = new Order 
                {
                    UserId = user.Id,
                    ProductId = product.Id,
                    Price = product.Price,
                    CreatedDate = DateTime.Now,
                    Status = SD.PaymentStatusPending,
                    InvoiceId = content.invoiceId
                };

                _unitOfWork.Order.Add(order);
                await _unitOfWork.SaveAsync();
                return Ok(new {pageUrl = content.pageUrl});
            }else 
            {
                return BadRequest("Something went wrong");
            }

        }

        [HttpPost("callback")]
        public async Task<IActionResult> Callback([FromBody] CallbackRequest req)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = await _unitOfWork.Order.GetAsync(o => o.InvoiceId == req.InvoiceId, includeProperties: "Product");
            if(order == null)
            {
                return NotFound();
            }
            if(order.ModifiedDate < req.ModifiedDate)
            {
                order.ModifiedDate = req.ModifiedDate;
                order.Status = req.Status;
                _unitOfWork.Order.Update(order);
                await _unitOfWork.SaveAsync();
            }
            if(req.Status == "success")
            {
                var user = await _unitOfWork.User.GetAsync(u => u.Id == order.UserId, includeProperties: "Products");
                user.Products.Add(order.Product);
                await _unitOfWork.SaveAsync();
            }
            return Ok();
        }

    }
    
}
