using System;
using AutoMapper;
using booknest.Models;
using booknest.Models.DTO;
using booknest.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace booknest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _mapper.Map<List<UserDto>>(await _unitOfWork.User.GetAllAsync());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [Authorize]
        [HttpGet("getById")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _unitOfWork.User.GetAsync(u => userId == u.Id);
            if(user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            if(userDTO == null)
                return BadRequest("user is null");

            User userFromDb = await _unitOfWork.User.GetAsync(i => i.Id == userDTO.Id);
            if(userFromDb == null)
                return BadRequest("User does not exist!");
            userFromDb.Name = userDTO.Name;
            userFromDb.MobilePhone = userDTO.MobilePhone;
            userFromDb.Email = userDTO.Email;
            userFromDb.Role = userDTO.Role;
            _unitOfWork.User.Update(userFromDb);
            await _unitOfWork.SaveAsync();

            return Ok(userDTO);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            var user = await _unitOfWork.User.GetAsync(u => userId == u.Id);
            if(user == null)
            {
                return NotFound();
            }
            _unitOfWork.User.Remove(user);
            await _unitOfWork.SaveAsync();
            return Ok(user);
        }

    }
}
