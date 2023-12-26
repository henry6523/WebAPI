// UsersRepository
using AutoMapper;
using DataAccessLayer.Data;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccessLayer.Repositories
{
	public class UsersRepository : IUsersRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public UsersRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public IEnumerable<UsersDTO> Gets(PaginationDTO paginationDTO)
		{
			var query = _context.Users.AsQueryable();
			if (!string.IsNullOrEmpty(paginationDTO.FilterValue))
			{
				query = query.Where(c => c.UserName.Contains(paginationDTO.FilterValue));
			}
			var paged = query.Paginate(paginationDTO);
			return _mapper.Map<IEnumerable<UsersDTO>>(paged);
		}

		public UsersDTO GetUser(string Id)
		{
			var user = _context.Users.FirstOrDefault(u => u.Id == Id);
			return _mapper.Map<UsersDTO>(user);
		}

		public RolesDTO GetRole(string Id)
		{
			var role = _context.Roles.FirstOrDefault(u => u.Name == Id);
			return _mapper.Map<RolesDTO>(role);
		}

		public IEnumerable<string> GetRolesByUserId(string userId)
		{
			var user = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Roles)
										.FirstOrDefault(u => u.Id == userId);

			return user.UserRoles.Select(ur => ur.Roles.Name).ToList();
		}

		public EditRolesDTO GetRolesAssign(EditRolesDTO editRoleDTO)
		{
			var roleAssign = _context.UserRoles.FirstOrDefault(ur => ur.Roles.Name == editRoleDTO.RoleName);
			return _mapper.Map<EditRolesDTO>(roleAssign);
		}

		public RolesDTO AddRole(RolesDTO roleDto)
		{
			var role = _mapper.Map<Roles>(roleDto);
			_context.Roles.Add(role);
			_context.SaveChanges();
			return _mapper.Map<RolesDTO>(role);
		}

		public void AssignRole(EditRolesDTO editRoleDTO)
		{
			var user = _context.Users.FirstOrDefault(u => u.Id == editRoleDTO.UserId);
			var role = _context.Roles.FirstOrDefault(r => r.Name == editRoleDTO.RoleName);

			var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id && ur.RoleId == role.Id);

			_context.UserRoles.Add(new UserRoles { UserId = user.Id, RoleId = role.Id });
			_context.SaveChanges();
		}

		public void Delete(EditRolesDTO editRoleDTO)
		{
			var roleToRemove = _context.UserRoles.FirstOrDefault(ur => ur.Roles.Name == editRoleDTO.RoleName);
			_context.UserRoles.Remove(roleToRemove);
			_context.SaveChanges();
		}
	}
}
