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
    public class ClassesRepository : IClassesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ClassesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<ClassesDTO> Gets(PaginationDTO paginationDTO)
        {
            var query = _context.Classes.AsQueryable();
            if (!string.IsNullOrEmpty(paginationDTO.FilterValue))
            {
                query = query.Where(c => c.ClassName.Contains(paginationDTO.FilterValue));
            }
            var paged = query.Paginate(paginationDTO);
            return _mapper.Map<IEnumerable<ClassesDTO>>(paged);

        }

        public ClassesDTO Get(int id)
        {
            var classes = _context.Classes.FirstOrDefault(c => c.Id == id);
            return _mapper.Map<ClassesDTO>(classes);
        }

        public ClassesDTO Add(CreateClassDTO createClassDTO)
        {
            var classesEntity = _mapper.Map<Classes>(createClassDTO);
            _context.Classes.Add(classesEntity);
            _context.SaveChanges();
            return _mapper.Map<ClassesDTO>(classesEntity);
        }

        public void Update(int id, CreateClassDTO createClassDTO)
        {
            var existingClass = _context.Classes.FirstOrDefault(c => c.Id == id);

            if (existingClass == null)
            {
                return;
            }

            _mapper.Map(createClassDTO, existingClass);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var categoryToDelete = _context.Classes.FirstOrDefault(c => c.Id == id);

            if (categoryToDelete == null)
            {
                return;
            }

            _context.Classes.Remove(categoryToDelete);
            _context.SaveChanges();
        }
    }
}
