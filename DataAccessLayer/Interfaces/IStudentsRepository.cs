// IStudentRepository.cs
using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IStudentsRepository
	{

        IEnumerable<StudentsDTO> Gets(PaginationDTO paginationDTO);
        StudentsDTO Get(string studentCard);
        StudentsDTO Add(int courseId, StudentsDTO studentsDTO);
        void Update(string studentCard, StudentsDTO studentsDTO);
        void Delete(string studentCard);
	}
}
