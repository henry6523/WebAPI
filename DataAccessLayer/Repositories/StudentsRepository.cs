using ModelsLayer.Entity;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using ModelsLayer.DTO;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Helpers;
using AutoMapper;

namespace DataAccessLayer.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public StudentsRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<StudentsDTO> Gets(PaginationDTO paginationDTO)
        {
            var query = _context.Students.AsQueryable();
            if (!string.IsNullOrEmpty(paginationDTO.FilterValue))
            {
                query = query.Where(c => c.StudentName.Contains(paginationDTO.FilterValue));
            }
            var paged = query.Paginate(paginationDTO);
            return _mapper.Map<IEnumerable<StudentsDTO>>(paged);

        }

        public StudentsDTO Get(string studentCard)
        {
            var students = _context.Students.FirstOrDefault(c => c.StudentCard == studentCard);
            return _mapper.Map<StudentsDTO>(students);
        }

        public StudentsDTO Add(int courseId, StudentsDTO studentsDTO)
        {
            var studentsEntity = _mapper.Map<Students>(studentsDTO);

            var studentCourseEntity = _context.Courses.Where(a => a.Id == courseId).FirstOrDefault();
            var studentCourse = new StudentCourses()
            {
                Students = studentsEntity,
                Courses = studentCourseEntity,
            };
            _context.StudentCourses.Add(studentCourse);
            _context.SaveChanges();

            return _mapper.Map<StudentsDTO>(studentsEntity);
        }

        public void Update(string studentCard, StudentsDTO studentsDTO)
        {
            var existingStudentCard = _context.Students.FirstOrDefault(c => c.StudentCard == studentCard);

            if (existingStudentCard == null)
            {
                return;
            }

            _mapper.Map(studentsDTO, existingStudentCard);
            _context.SaveChanges();
        }

        public void Delete(string studentsCard)
        {
            var categoryToDelete = _context.Students.FirstOrDefault(c => c.StudentCard == studentsCard);

            if (categoryToDelete == null)
            {
                return;
            }

            _context.Students.Remove(categoryToDelete);
            _context.SaveChanges();
        }
    }
}
