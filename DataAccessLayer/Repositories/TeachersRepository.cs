using AutoMapper;
using DataAccessLayer.Data;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using IdentityServer4.Extensions;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class TeachersRepository : ITeachersRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public TeachersRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public IEnumerable<TeachersDTO> Gets(PaginationDTO paginationDTO)
		{
            var query = _context.Teachers.AsQueryable();
            if (!string.IsNullOrEmpty(paginationDTO.FilterValue))
            {
                query = query.Where(c => c.TeacherName.Contains(paginationDTO.FilterValue));
            }
            var paged = query.Paginate(paginationDTO);
            return _mapper.Map<IEnumerable<TeachersDTO>>(paged);
        }

		public TeachersDTO Get(int id)
		{
			var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);
			return _mapper.Map<TeachersDTO>(teacher);
		}

		public TeachersDTO Add(CreateTeacherDTO createTeacherDTO)
		{
			var teacherEntity = _mapper.Map<Teachers>(createTeacherDTO);
			_context.Teachers.Add(teacherEntity);
			_context.SaveChanges();
			return _mapper.Map<TeachersDTO>(teacherEntity);
		}

		public void Update(int id, CreateTeacherDTO createTeacherDTO)
		{
			var existingTeacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

			if (existingTeacher == null)
			{
				return;
			}

			existingTeacher.TeacherName = createTeacherDTO.TeacherName;
			existingTeacher.Email = createTeacherDTO.Email;
			existingTeacher.PhoneNo = createTeacherDTO.PhoneNo;

			_context.SaveChanges();
		}

        public void Delete(int id)
        {
            var categoryToDelete = _context.Teachers.FirstOrDefault(c => c.Id == id);

            if (categoryToDelete == null)
            {
                return;
            }

            _context.Teachers.Remove(categoryToDelete);
            _context.SaveChanges();
        }
    }
}