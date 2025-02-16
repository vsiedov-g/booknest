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
        public IActionResult GetAllUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_unitOfWork.User.GetAll());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [Authorize]
        [HttpGet("getById")]
        public IActionResult GetUser(int userId)
        {
            var user = _unitOfWork.User.Get(u => userId == u.Id);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] UserDto userDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest("shit");

            if(userDTO == null)
                return BadRequest("user is null");

            User userFromDb = _unitOfWork.User.Get(i => i.Id == userDTO.Id);
            if(userFromDb == null)
                return BadRequest("User does not exist!");
            userFromDb.Name = userDTO.Name;
            userFromDb.MobilePhone = userDTO.MobilePhone;
            userFromDb.Email = userDTO.Email;
            userFromDb.Role = userDTO.Role;
            _unitOfWork.User.Update(userFromDb);
            _unitOfWork.Save();

            return Ok(userDTO);
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult RemoveUser(int userId)
        {
            var user = _unitOfWork.User.Get(u => userId == u.Id);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(user == null)
            {
                return NotFound();
            }
            _unitOfWork.User.Remove(user);
            _unitOfWork.Save();
            return Ok(user);
        }

    }
}
