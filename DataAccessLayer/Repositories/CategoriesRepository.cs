using AutoMapper;
using DataAccessLayer.Data;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoriesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<CategoriesDTO> Gets(PaginationDTO paginationDTO)
        {
            var query = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(paginationDTO.FilterValue))
            {
                query = query.Where(c => c.CategoriesName.Contains(paginationDTO.FilterValue));
            }
            var pagedCategories = query.Paginate(paginationDTO);
            return _mapper.Map<IEnumerable<CategoriesDTO>>(pagedCategories);

        }

        public CategoriesDTO Get(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return _mapper.Map<CategoriesDTO>(category);
        }

        public CategoriesDTO Add(CreateCategoryDTO createCategoryDTO)
        {
            var categoryEntity = _mapper.Map<Categories>(createCategoryDTO);
            _context.Categories.Add(categoryEntity);
            _context.SaveChanges();
            return _mapper.Map<CategoriesDTO>(categoryEntity);
        }

        public void Update(int id, CreateCategoryDTO createCategoryDTO)
        {
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (existingCategory == null)
            {
                return;
            }

            _mapper.Map(createCategoryDTO, existingCategory);

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var categoryToDelete = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (categoryToDelete == null)
            {
                return;
            }

            _context.Categories.Remove(categoryToDelete);
            _context.SaveChanges();
        }
    }
}
