using BusinessLogicLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
	public interface ICategoryRepository
	{
		Categories GetCategory(int id);
		ICollection<Categories> GetCategories();
		void AddCategory(Categories categoryCreate);
		void UpdateCategory(int id, CreateCategoryDTO createCategoryDTO);
		void DeleteCategory(Categories categoryDelete);
	}
}
