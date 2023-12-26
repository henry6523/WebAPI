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
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using System.Data;

namespace MyWebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class UsersController : ControllerBase
	{
		private readonly IUsersRepository _usersRepository;
		private readonly IResponseServiceRepository _responseServiceRepository;

		public UsersController(IUsersRepository usersRepository, IResponseServiceRepository responseServiceRepository)
		{
			_usersRepository = usersRepository;
			_responseServiceRepository = responseServiceRepository;
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
		public IActionResult Gets([FromQuery] PaginationDTO paginationDTO)
		{
			var users = _usersRepository.Gets(paginationDTO);
			return _responseServiceRepository.CustomOkResponse("Data loaded successfully", users);
		}

		/// <summary>
		/// Show Roles has been assigned to User
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Returns the list roles of **User** by **UserId** that have been assigned access control on the referenced resource.
		/// </remarks>
		/// <response code="200">Successfully returns a list of User Roles.</response>
		/// <response code="500">An error has occurred within the server.</response>
		[HttpGet("GetRolesByUserId/{userId}")]
		[Authorize(Roles = "Reader")]
		public IActionResult GetRolesByUserId([Required] string userId)
		{
			var user = _usersRepository.GetRolesByUserId(userId);

			if (user == null)
			{
				return _responseServiceRepository.CustomNotFoundResponse("User not found", userId);
			}

			return _responseServiceRepository.CustomOkResponse("Data loaded successfully", user);
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
			var createRoleDTO = _usersRepository.AddRole(roleDto);

			return _responseServiceRepository.CustomCreatedResponse("Role created successfully", createRoleDTO);
		}

		/// <summary>
		/// Assign roles to User
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Assign role into an User
		/// </remarks>
		/// <response code="200">Successfully assigned role to User.</response>
		[HttpPost("AssignRole")]
		[Authorize(Roles = "Writer")]
		public IActionResult AssignRole([FromBody] EditRolesDTO editRoleDTO)
		{
			var user = _usersRepository.GetUser(editRoleDTO.UserId);
			var role = _usersRepository.GetRole(editRoleDTO.RoleName);

			if (user == null || role == null)
			{
				return _responseServiceRepository.CustomNotFoundResponse("User or role not found", editRoleDTO);
			}

			var userRole = _usersRepository.GetRolesByUserId(editRoleDTO.UserId);
			if (userRole != null)
			{
				return _responseServiceRepository.CustomBadRequestResponse("User already has the specified role", editRoleDTO.UserId); 
			}
			_usersRepository.AssignRole(editRoleDTO);
			return Ok("Role assigned successfully");
		}

		/// <summary>
		/// Update Roles by RolesId
		/// </summary>
		/// <returns>Not Implemented</returns>
		/// <response code="501">Not Implemented</response>
		[HttpPut("{id}")]
		[Authorize(Roles = "Editor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Update()
		{
			return StatusCode(StatusCodes.Status501NotImplemented, "Method not implemented");
		}

		/// <summary>
		/// Delete User's Role
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Delete role from User.
		/// </remarks>
		/// <response code="200">Successfully deleted User's Role</response>
		[HttpDelete("RemoveRole")]
		[Authorize(Roles = "Editor")]
		public IActionResult DeleteRole([FromQuery] EditRolesDTO editRoleDTO)
		{
			var user = _usersRepository.GetUser(editRoleDTO.UserId);
			if (user == null)
			{
				return _responseServiceRepository.CustomNotFoundResponse("User not found", editRoleDTO);
			}

			var roleAssigned = _usersRepository.GetRolesAssign(editRoleDTO);
			if (roleAssigned == null)
			{
				return _responseServiceRepository.CustomNotFoundResponse("User does not have the specified role", editRoleDTO);
			}

			_usersRepository.Delete(editRoleDTO);
			return NoContent();

		}
	}
}
