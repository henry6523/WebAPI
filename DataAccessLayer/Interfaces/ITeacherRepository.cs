// ITeacherRepository.cs

using BusinessLogicLayer.DTO;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
	public interface ITeacherRepository
	{
		IEnumerable<TeacherDTO> GetTeachers(string filterValue, int page, int pageSize);
		TeacherDTO GetTeacher(int id);
		TeacherDTO AddTeacher(CreateTeacherDTO createTeacherDTO);
		void UpdateTeacher(int id, CreateTeacherDTO createTeacherDTO);
		void DeleteTeacher(int id);
	}
}
