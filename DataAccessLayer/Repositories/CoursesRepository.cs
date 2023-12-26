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
    public class CoursesRepository : ICoursesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CoursesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<CoursesDTO> Gets(PaginationDTO paginationDTO)
        {
            var query = _context.Courses.AsQueryable();
            if (!string.IsNullOrEmpty(paginationDTO.FilterValue))
            {
                query = query.Where(c => c.CourseName.Contains(paginationDTO.FilterValue));
            }
            var paged = query.Paginate(paginationDTO);
            return _mapper.Map<IEnumerable<CoursesDTO>>(paged);

        }

        public CoursesDTO Get(int id)
        {
            var courses = _context.Courses.FirstOrDefault(c => c.Id == id);
            return _mapper.Map<CoursesDTO>(courses);
        }

        public CoursesDTO Add(int categoriesId ,CreateCourseDTO createCourseDTO)
        {
            var coursesEntity = _mapper.Map<Courses>(createCourseDTO);
            coursesEntity.Categories = _context.Categories.FirstOrDefault(c => c.Id == categoriesId);
            _context.Courses.Add(coursesEntity);
            _context.SaveChanges();
            return _mapper.Map<CoursesDTO>(coursesEntity);
        }

        public void Update(int id, CreateCourseDTO createCourseDTO)
        {
            var existingClass = _context.Courses.FirstOrDefault(c => c.Id == id);

            if (existingClass == null)
            {
                return;
            }

            _mapper.Map(createCourseDTO, existingClass);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var categoryToDelete = _context.Courses.FirstOrDefault(c => c.Id == id);

            if (categoryToDelete == null)
            {
                return;
            }

            _context.Courses.Remove(categoryToDelete);
            _context.SaveChanges();
        }
    }
}
