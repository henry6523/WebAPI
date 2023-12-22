using BusinessLogicLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
	public interface IClassRepository
	{
		Classes GetClass(int id);
		ICollection<Classes> GetClasses();
		void AddClass(Classes classCreate);
		void UpdateClass(int id, CreateClassDTO createClassDTO);
		void DeleteClass(Classes classDelete);
	}
}
