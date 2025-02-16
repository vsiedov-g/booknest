using System;
using System.Security.Claims;
using booknest.Models;
using booknest.Models.DTO;
using booknest.Repository.IRepository;
using booknest.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace booknest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }
        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult Signup([FromBody] AuthRequestData requestData)
        {
           if(!ModelState.IsValid)
           {
                return BadRequest("something went wrong");
           } 
           var user = _authService.Authenticate(requestData.Email, requestData.Password);
           if (user != null)
           {
                return BadRequest("User already exists");
           }
            user = _authService.CreateUser(requestData.Email, requestData.Password);
            var token = _tokenService.GenerateToken(user);
            AuthResponseData response = new AuthResponseData(){
                AccessToken = token,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
            
            SetRefreshToken(user.Id);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequestData requestData)
        {
            if(!ModelState.IsValid)
           {
                return BadRequest("something went wrong");
           }
            var user = _authService.Authenticate(requestData.Email, requestData.Password);
            if(user == null)
            {
                return BadRequest("email or password is invalid");
            }
            _tokenService.RevokeRefreshToken(user.Id);
            var token = _tokenService.GenerateToken(user);
            AuthResponseData response = new AuthResponseData(){
                AccessToken = token,
                Id = user.Id, 
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            SetRefreshToken(user.Id);

            return Ok(response);
        }
        
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] int userId)
        {
            if(!ModelState.IsValid)
           {
                return BadRequest("something went wrong");
           }
            _tokenService.RevokeRefreshToken(userId);
            return Ok(new {message = userId});
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromForm] string accessToken)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("something went wrong");
            }
            string refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
            {
                return BadRequest("REFRESH_TOKEN_NULL");
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;

            if(userEmail == null)
            {
                return BadRequest("user email is null");
            }

            User user = _unitOfWork.User.Get(u => u.Email == userEmail);

            if (user == null)
            {
                return BadRequest("user is null");
            }

            if(!_tokenService.ValidateRefreshToken(refreshToken, user.Id))
            {
                return BadRequest("REFRESH_TOKEN_EXPIRED");
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { message = token });
        }


        private void SetRefreshToken(int userId)
        {
            RefreshToken refreshToken = _tokenService.GenerateRefreshToken(userId);
            _tokenService.SaveRefreshTokenToDB(refreshToken);
            var cookieOptions = new CookieOptions {
                Expires = refreshToken.Expires,
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
