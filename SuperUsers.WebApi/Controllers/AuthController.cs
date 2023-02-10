using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SuperUsers.Domain.Dtos;
using SuperUsers.Domain.Entities;
using SuperUsers.Domain.Services;
using SuperUsers.WebApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SuperUsers.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet("getMeAuthorize"), Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetName();
            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterDto request)
        {
            User user = new User();

            user.Email = request.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var userEntity = _userService.RegisterUser(user);

            return Ok(userEntity);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            User userEntity = _userService.GetUser(request.Email);

            if (userEntity == null)
                return BadRequest("Service Invalid");

            if (userEntity.Email != request.Email
                || !BCrypt.Net.BCrypt.Verify(request.Password, userEntity.PasswordHash))
            {
                return BadRequest("User or Password Invalid");
            }


            string token = CreateToken(userEntity);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(userEntity.Email, refreshToken);

            return Ok(token);
        }

        [HttpPost("logout"), Authorize]
        public async Task<ActionResult<string>> Logout()
        {
            var userName = _userService.GetName();
            User userEntity = _userService.GetUser(userName);
            if (userEntity == null)
                return BadRequest("Service Invalid");

            var refreshToken = Request.Cookies["refreshToken"];
            if (!userEntity.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Logout Token Invalid");
            }
            else if (userEntity.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Logout Token expire");
            }

            var logoutToken = GenerateLogoutToken();
            SetRefreshToken(userName, logoutToken);

            return NoContent();
        }

        [HttpPost("refreshToken"), Authorize]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var userName = _userService.GetName();
            User userEntity = _userService.GetUser(userName);
            if (userEntity == null)
                return BadRequest("Service Invalid");

            var refreshToken = Request.Cookies["refreshToken"];
            if (!userEntity.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Refresh Token Invalid");
            }
            else if (userEntity.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expire");
            }

            string token = CreateToken(userEntity);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(userName, newRefreshToken);

            return Ok(token);
        }

        private RefreshToken GenerateLogoutToken()
        {
            return GenerateToken(-1.0);
        }
        private RefreshToken GenerateRefreshToken()
        {
            return GenerateToken(Double.Parse(_configuration.GetSection("AppSettings:TokenExpire").Value!));
        }

        private RefreshToken GenerateToken(double value)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(value),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(string email, RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            var user = _userService.GetUser(email);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            _userService.UpdateRefreshToken(user);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };
            
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(Double.Parse(_configuration.GetSection("AppSettings:TokenExpire").Value!)),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
