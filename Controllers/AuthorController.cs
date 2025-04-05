using System;
using booknest.Models;
using booknest.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace booknest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery]string? authorName)
        {
            if(authorName != null)
            {
                var authors = await _unitOfWork.Author.GetAllAsync(a => a.Name.ToLower().Contains(authorName.ToLower()));
                return Ok(authors);
            }
            var allAuthors = await _unitOfWork.Author.GetAllAsync();
            return Ok(allAuthors);
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetAuthor([FromQuery]int authorId)
        {
            Author author = await _unitOfWork.Author.GetAsync(u => authorId == u.Id);
            if(author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [Authorize]
        [HttpPut("add")]
        public async Task<IActionResult> AddAuthor([FromBody] Author author)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(author == null)
                return BadRequest("author is null");
            
            _unitOfWork.Author.Add(author);
            await _unitOfWork.SaveAsync();

            return Ok(author);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAuthor([FromBody] Author author)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(author == null)
                return BadRequest("author is null");
            
            _unitOfWork.Author.Update(author);
            await _unitOfWork.SaveAsync();

            return Ok(author);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            Author author = await _unitOfWork.Author.GetAsync(u => authorId == u.Id);
            if(author == null)
            {
                return NotFound();
            }
            _unitOfWork.Author.Remove(author);
            await _unitOfWork.SaveAsync();
            return Ok(author);
        }

    }
}
