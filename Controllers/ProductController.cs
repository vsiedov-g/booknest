using System;
using AutoMapper;
using booknest.Models;
using booknest.Models.DTO;
using booknest.Repository;
using booknest.Repository.IRepository;
using booknest.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(productDto == null)
                return BadRequest("PRODUCT_NULL");

            var product = await _productService.AddProductAsync(productDto);

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
            var newProducts = _mapper.Map<List<ProductDto>>(products);
            return Ok(newProducts);
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
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(productDto == null)
                return BadRequest("product is null");

            var product = await _productService.UpdateProductAsync(productDto);

            return Ok(product);
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
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return Ok(product);
        }

    }
    
}
