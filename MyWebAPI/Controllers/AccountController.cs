using DataAccessLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Interfaces;
using System.Security.Cryptography;
using System.Data;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;

        public AccountController(
            IConfiguration configuration,
            IAccountRepository accountRepository,
            DataContext dataContext
            )
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Register a new User to Login
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Input username, email and password to register.
        /// </remarks>
        /// <response code="200">Registered successfully.</response>
        [HttpPost("Register")]
        public IActionResult Register(UserDTO model)
        {
            _accountRepository.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        private UserToken BuildToken(UserInfo userInfo, IEnumerable<string> userRoles)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, userInfo.UserName));

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddMinutes(15);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                signingCredentials: credentials);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

		/// <summary>
		/// Login to System
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Input username and password to login.
		/// </remarks>
		/// <response code="200">Login successfully.</response>
		[HttpPost("Login")]
		public UserToken Login(UserInfo model)
		{
			var userRoles = _accountRepository.GetUserRoles(model.UserName);

			_accountRepository.Login(model);
			return BuildToken(model, userRoles);
		}
	}
}
