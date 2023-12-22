using BusinessLogicLayer.DTO;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly DataContext _context;

        public TeacherRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Teachers> GetTeachers()
        {
            return _context.Teachers.ToList();
        }

        public Teachers GetTeacher(int id)
        {
            return _context.Teachers.FirstOrDefault(t => t.Id == id);
        }

        public void AddTeacher(Teachers teacherCreate)
        {
            _context.Teachers.Add(teacherCreate);
            _context.SaveChanges();
        }

        public void UpdateTeacher(int id, TeacherDTO teacherDTO)
        {
            var existingTeacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

            if (existingTeacher == null)
            {
                return;
            }

            existingTeacher.TeacherName = teacherDTO.TeacherName;
            existingTeacher.Email = teacherDTO.Email;
            existingTeacher.PhoneNo = teacherDTO.PhoneNo;

            _context.SaveChanges();
        }

        public void DeleteTeacher(Teachers teacherDelete)
        {
            _context.Teachers.Remove(teacherDelete);
            _context.SaveChanges();
        }
    }
}
