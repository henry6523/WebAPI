using AutoMapper;
using DataAccessLayer.Data;
using ModelsLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.DTO;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using X.PagedList;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UsersController( DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public enum FilterType
        {
            UserName
        }

        /// <summary>
        /// Show list of User
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Returns the lists of **User** that have been assigned access control on the referenced resource.
        /// </remarks>
        /// <response code="200">Successfully returns a list of User.</response>
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<List<GetUsers>>> GetListUsers()
        {
            var queryable = _context.Users.AsQueryable();
            return await queryable
                .Select(x => new GetUsers { UserId = x.Id, UserName = x.UserName, Email = x.Email }).ToListAsync();
        }

        /// <summary>
        /// Show list Roles
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Returns the list of **Roles** that have been access control on the referenced resource.
        /// </remarks>
        /// <response code="200">Successfully returns a list of Roles.</response>
        [HttpGet("Roles")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<List<RolesDTO>>> Get()
        {
            return await _context.Roles
                .Select(x => new RolesDTO { Name = x.Name }).ToListAsync();
        }

		/// <summary>
		/// Show list Roles of User
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Returns the list of **Roles** that have been assigned access control on the referenced resource.
		/// </remarks>
		/// <response code="200">Successfully returns a list of User Roles.</response>
		/// <response code="500">An error has occurred within the server.</response>
		[HttpGet("GetRolesByUserId/{userId}")]
		[Authorize(Roles = "Reader")]
		public async Task<ActionResult<List<string>>> GetRolesByUserId([Required]string userId)
		{
			try
			{
				var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Roles)
												.FirstOrDefaultAsync(u => u.Id == userId);

				if (user == null)
				{
					return NotFound($"User with ID {userId} not found");
				}

				var roles = user.UserRoles.Select(ur => ur.Roles.Name).ToList();
				return Ok(roles);
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal server error");
			}
		}

		/// <summary>
		/// Add Roles into System
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Add Roles into System
		/// </remarks>
		/// <response code="200">Successfully add a role into System.</response>
		[HttpPost("AddRole")]
        [Authorize(Roles = "Writer")]
        public IActionResult AddRole([FromBody] RolesDTO roleDto)
        {
            var role = new Roles { Name = roleDto.Name };
            _context.Roles.Add(role);
            _context.SaveChanges();
            return Ok(new { Message = "Role added successfully", RoleId = role.Id });
        }

        /// <summary>
        /// Assign roles into User
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Assign role into an User
        /// </remarks>
        /// <response code="200">Successfully assign role into an User.</response>
        [HttpPost("AssignRole")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> AssignRole([FromBody] EditRolesDTO editRoleDTO)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == editRoleDTO.UserId);
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == editRoleDTO.RoleName);

                if (user == null || role == null)
                {
                    return BadRequest("User or role not found");
                }

                var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
                if (userRole != null)
                {
                    return BadRequest("User already has the specified role");
                }

                _context.UserRoles.Add(new UserRoles { UserId = user.Id, RoleId = role.Id });
                await _context.SaveChangesAsync();

                return Ok("Role assigned successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete User's Role
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Delete role from an User.
        /// </remarks>
        /// <response code="200">Successfully deleted User's Role</response>
        [HttpDelete("RemoveRole")]
        [Authorize(Roles = "Editor")]
        public async Task<ActionResult> RemoveRole([FromBody] EditRolesDTO editRoleDTO)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Roles)
                    .SingleOrDefaultAsync(u => u.Id == editRoleDTO.UserId);

                if (user == null)
                {
                    return NotFound($"User with ID {editRoleDTO.UserId} not found.");
                }

                var roleToRemove = user.UserRoles.FirstOrDefault(ur => ur.Roles.Name == editRoleDTO.RoleName);

                if (roleToRemove == null)
                {
                    return BadRequest($"User does not have the role '{editRoleDTO.RoleName}'.");
                }

                user.UserRoles.Remove(roleToRemove);

                await _context.SaveChangesAsync();

                return Ok($"Role '{editRoleDTO.RoleName}' removed successfully for user with ID {editRoleDTO.UserId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
