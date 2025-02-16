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
        public IActionResult GetAll([FromQuery] string? categoryName)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(categoryName != null)
            {
                var categories = _unitOfWork.Category.GetAll(c => c.Name.Contains(categoryName));
                if( categories == null)
                {
                    return NotFound();
                }
                return Ok(categories);
            }
            var allCategories = _unitOfWork.Category.GetAll();
            return Ok(allCategories);
        }

        [Authorize]
        [HttpGet("getById")]
        public IActionResult Get(int categoryId)
        {
            var category = _unitOfWork.Category.Get(u => categoryId == u.Id);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [Authorize]
        [HttpPut("add")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(category == null)
                return BadRequest("category is null");
            
            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();

            return Ok(category);
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateCategory([FromBody] Category category)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(category == null)
                return BadRequest("user is null");
            
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();

            return Ok(category);
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var category = _unitOfWork.Category.Get(u => categoryId == u.Id);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            return Ok(category);
        }

    }
}
