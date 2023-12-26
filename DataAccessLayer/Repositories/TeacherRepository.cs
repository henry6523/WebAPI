// TeacherRepository.cs

using AutoMapper;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class TeacherRepository : ITeacherRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public TeacherRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public IEnumerable<TeacherDTO> GetTeachers(string filterValue = "", int page = 1, int pageSize = 10)
		{
			var query = _context.Teachers.AsQueryable();

			if (!string.IsNullOrEmpty(filterValue))
			{
				query = query.Where(t => t.TeacherName.Contains(filterValue));
			}

			var pagedTeachers = query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			var teacherMap = _mapper.Map<IEnumerable<TeacherDTO>>(pagedTeachers);

			return teacherMap;
		}




		public TeacherDTO GetTeacher(int id)
		{
			var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);
			return _mapper.Map<TeacherDTO>(teacher);
		}

		public TeacherDTO AddTeacher(CreateTeacherDTO createTeacherDTO)
		{
			var teacherEntity = _mapper.Map<Teachers>(createTeacherDTO);
			_context.Teachers.Add(teacherEntity);
			_context.SaveChanges();
			return _mapper.Map<TeacherDTO>(teacherEntity);
		}

		public void UpdateTeacher(int id, CreateTeacherDTO createTeacherDTO)
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

		public void DeleteTeacher(int id)
		{
			var teacherToDelete = _context.Teachers.FirstOrDefault(t => t.Id == id);

			if (teacherToDelete == null)
			{
				return;
			}

			_context.Teachers.Remove(teacherToDelete);
			_context.SaveChanges();
		}
	}
}