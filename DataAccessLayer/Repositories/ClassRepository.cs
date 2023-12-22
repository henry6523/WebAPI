using BusinessLogicLayer.DTO;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly DataContext _context;

        public ClassRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Classes> GetClasses()
        {
            return _context.Classes.ToList();
        }

        public Classes GetClass(int id)
        {
            return _context.Classes.Where(s => s.Id == id).FirstOrDefault();
        }

        public void AddClass(Classes classCreate)
        {
            _context.Classes.Add(classCreate);
            _context.SaveChanges();
        }

        public void UpdateClass(int id, ClassDTO classDTO)
        {
            var existingClass = _context.Classes.FirstOrDefault(c => c.Id == id);

            if (existingClass == null)
            {
                return;
            }

            existingClass.ClassName = classDTO.ClassName;

            _context.SaveChanges();
        }

        public void DeleteClass(Classes classDelete)
        {
            _context.Classes.Remove(classDelete);
            _context.SaveChanges();
        }
    }
}
