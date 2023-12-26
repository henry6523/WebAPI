using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IClassesRepository
	{
        IEnumerable<ClassesDTO> Gets(PaginationDTO paginationDTO);
        ClassesDTO Get(int id);
        ClassesDTO Add(CreateClassDTO createClassDTO);
        void Update(int id, CreateClassDTO createClassDTO);
        void Delete(int id);
    }
}
