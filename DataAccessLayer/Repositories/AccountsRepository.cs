using AutoMapper;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.DTO;
using ModelsLayer.Entity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DataAccessLayer.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountsRepository(DataContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
			_configuration = configuration;
		}

        public IEnumerable<string> GetUserRoles(string username)
        {
            var userRoles = _context.UserRoles
                    .Where(ur => ur.Users.UserName == username)
                    .Select(ur => ur.Roles.Name);

            return userRoles;
        }

        public void Login(UserInfos model)
        {
            var user = _context.Users.SingleOrDefault(x => x.UserName == model.UserName);

            BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            _mapper.Map<UsersDTO>(user);
        }

        public void Register(UsersDTO model)
        {
            if (_context.Users.Any(x => x.UserName == model.Email))
                throw new Exception("Username '" + model.Email + "' is already taken");

            var user = _mapper.Map<Users>(model);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public UserTokens BuildToken(UserInfos userInfo, IEnumerable<string> userRoles)
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

            return new UserTokens()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

    }
}
