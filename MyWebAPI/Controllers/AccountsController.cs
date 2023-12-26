using DataAccessLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ModelsLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Interfaces;
using System.Security.Cryptography;
using System.Data;
using ModelsLayer.DTO;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsRepository _accountRepository;
        private readonly IResponseServiceRepository _responseServiceRepository;

        public AccountsController(IAccountsRepository accountRepository, IResponseServiceRepository responseServiceRepository)
        {
            _accountRepository = accountRepository;
            _responseServiceRepository = responseServiceRepository;
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
        public IActionResult Register(UsersDTO model)
        {
            _accountRepository.Register(model);
            return Ok(new { message = "Registration successful" });
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
		public UserTokens Login(UserInfos model)
		{
			var userRoles = _accountRepository.GetUserRoles(model.UserName);

            _accountRepository.Login(model);
			return _accountRepository.BuildToken(model, userRoles);
		}
	}
}
