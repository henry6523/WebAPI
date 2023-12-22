using BusinessLogicLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
	public interface ICourseRepository
	{
		Courses GetCourse(int id);
		ICollection<Courses> GetCourses();
		void AddCourse(int categoryId, Courses courseCreate);
		void UpdateCourse(int id, CreateCourseDTO createCourseDTO);
		void DeleteCourse(Courses courseDelete);
    }
}
