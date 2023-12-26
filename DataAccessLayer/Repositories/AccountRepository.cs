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

namespace DataAccessLayer.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public AccountRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<string> GetUserRoles(string username)
        {
            var userRoles = _context.UserRoles
                    .Where(ur => ur.Users.UserName == username)
                    .Select(ur => ur.Roles.Name);

            return userRoles;
        }

        public void Login(UserInfo model)
        {
            var user = _context.Users.SingleOrDefault(x => x.UserName == model.UserName);

            // validate
            BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            // authentication successful
            _mapper.Map<UserDTO>(user);
        }

        public void Register(UserDTO model)
        {
            // validate
            if (_context.Users.Any(x => x.UserName == model.Email))
                throw new Exception("Username '" + model.Email + "' is already taken");

            // map model to new user object

            var user = _mapper.Map<Users>(model);

            // hash password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }

    }
}
