using BusinessLogicLayer.DTO;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Categories> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Categories GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void AddCategory(Categories categoryUpdate)
        {
            _context.Categories.Add(categoryUpdate);
            _context.SaveChanges();
        }

        public void UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (existingCategory == null)
            {
                return;
            }

            existingCategory.CategoriesName = categoryDTO.CategoriesName;

            _context.SaveChanges();
        }

        public void DeleteCategory(Categories categoryDelete)
        {
            _context.Categories.Remove(categoryDelete);
            _context.SaveChanges();
        }
    }
}
