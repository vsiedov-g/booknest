using System;
using booknest.Models;
using booknest.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace booknest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
         private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] string? categoryName)
        {
            if(categoryName != null)
            {
                var categories = await _unitOfWork.Category.GetAllAsync(c => c.Name.ToLower().Contains(categoryName.ToLower()));
                if( categories == null)
                {
                    return NotFound();
                }
                return Ok(categories);
            }
            var allCategories = await _unitOfWork.Category.GetAllAsync();
            return Ok(allCategories);
        }

        [Authorize]
        [HttpGet("getById")]
        public async Task<IActionResult> Get(int categoryId)
        {
            var category = await _unitOfWork.Category.GetAsync(u => categoryId == u.Id);
            if(category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [Authorize]
        [HttpPut("add")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Category.Add(category);
            await _unitOfWork.SaveAsync();

            return Ok(category);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Category.Update(category);
            await _unitOfWork.SaveAsync();

            return Ok(category);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _unitOfWork.Category.GetAsync(u => categoryId == u.Id);
            if(category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            await _unitOfWork.SaveAsync();
            return Ok(category);
        }

    }
}
