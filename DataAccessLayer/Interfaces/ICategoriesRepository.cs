using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface ICategoriesRepository
	{
        IEnumerable<CategoriesDTO> Gets(PaginationDTO paginationDTO);
        CategoriesDTO Get(int id);
        CategoriesDTO Add(CreateCategoryDTO createCategoryDTO);
        void Update(int id, CreateCategoryDTO createCategoryDTO);
        void Delete(int id);
    }
}
