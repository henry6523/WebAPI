// IStudentRepository.cs
using BusinessLogicLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
	public interface IStudentRepository
	{
		ICollection<Students> GetStudents();
		Students GetStudentByCard(string studentCard);
		void AddStudent(int CourseId, Students studentCreate);
		Students GetStudentTrimToUpper(StudentDTO studentCreate);
		void UpdateStudent(Students studentUpdate);
		void DeleteStudent(Students studentDelete);
		bool IsStudentCardExists(string studentCard);
	}
}
