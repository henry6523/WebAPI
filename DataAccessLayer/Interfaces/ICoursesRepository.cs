using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface ICoursesRepository
	{
        IEnumerable<CoursesDTO> Gets(PaginationDTO paginationDTO);
        CoursesDTO Get(int id);
        CoursesDTO Add(int categoriesId, CreateCourseDTO createCourseDTO);
        void Update(int id, CreateCourseDTO createCourseDTO);
        void Delete(int id);
    }
}
