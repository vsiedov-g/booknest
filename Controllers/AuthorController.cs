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
        public IActionResult GetAll([FromQuery]string? authorName)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(authorName != null)
            {
                var authors = _unitOfWork.Author.GetAll(a => a.Name.Contains(authorName));
                return Ok(authors);
            }
            var allAuthors = _unitOfWork.Author.GetAll();
            return Ok(allAuthors);
        }

        [Authorize]
        [HttpGet("get")]
        public IActionResult GetAuthor([FromQuery]int authorId)
        {
            var author = _unitOfWork.Author.Get(u => authorId == u.Id);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [Authorize]
        [HttpPut("add")]
        public IActionResult AddAuthor([FromBody] Author author)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(author == null)
                return BadRequest("author is null");
            
            _unitOfWork.Author.Add(author);
            _unitOfWork.Save();

            return Ok(author);
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateAuthor([FromBody] Author author)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(author == null)
                return BadRequest("author is null");
            
            _unitOfWork.Author.Update(author);
            _unitOfWork.Save();

            return Ok(author);
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteAuthor(int authorId)
        {
            Author author = _unitOfWork.Author.Get(u => authorId == u.Id);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(author == null)
            {
                return NotFound();
            }
            _unitOfWork.Author.Remove(author);
            _unitOfWork.Save();
            return Ok(author);
        }

    }
}
