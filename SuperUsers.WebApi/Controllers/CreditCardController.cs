using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SuperUsers.Domain.Dtos;
using SuperUsers.Domain.Entities;
using SuperUsers.Domain.Services;
using SuperUsers.Encryption.Cryptography;
using SuperUsers.WebApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SuperUsers.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreditCardController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public CreditCardController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }


        [HttpPost("UpdateCreditCard")]
        public async Task<ActionResult<string>> UpdateCreditCard(CreditCardDto request)
        {

            var userName = _userService.GetName();
            User userEntity = _userService.GetUser(userName);
            if (userEntity == null)
                return BadRequest("Service Invalid");

            userEntity.CreditCardNumber = SymmetricManager.Encrypt(request.Name, _configuration.GetSection("AppSettings:SymKeyBase64").Value!);

            _userService.UpdateUser(userEntity);

            return Ok("Encrypted data");
        }

        [HttpGet("GetCreditCard")]
        public async Task<ActionResult<string>> GetCreditCard()
        {
            var userName = _userService.GetName();
            User userEntity = _userService.GetUser(userName);
            if (userEntity == null)
                return BadRequest("Service Invalid");

            var data = SymmetricManager.Decrypt(userEntity.CreditCardNumber, _configuration.GetSection("AppSettings:SymKeyBase64").Value!);

            return Ok(data);
        }

    }
}
