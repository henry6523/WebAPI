// IUsersRepository
using ModelsLayer.DTO;
using System.Collections.Generic;
using System.Security.Claims;

namespace DataAccessLayer.Interfaces
{
	public interface IUsersRepository
	{
		IEnumerable<UsersDTO> Gets(PaginationDTO paginationDTO);
		UsersDTO GetUser(string Id);
		RolesDTO GetRole(string Id);
		IEnumerable<string> GetRolesByUserId(string userId);
		EditRolesDTO GetRolesAssign(EditRolesDTO editRolesDTO);
		RolesDTO AddRole(RolesDTO roleDto);
		void AssignRole(EditRolesDTO editRoleDTO);
		void Delete(EditRolesDTO editRoleDTO);
	}
}
