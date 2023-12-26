using ModelsLayer.DTO;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface ITeachersRepository
	{
		IEnumerable<TeachersDTO> Gets(PaginationDTO paginationDTO);
		TeachersDTO Get(int id);
		TeachersDTO Add(CreateTeacherDTO createTeacherDTO);
		void Update(int id, CreateTeacherDTO createTeacherDTO);
		void Delete(int id);
	}
}
