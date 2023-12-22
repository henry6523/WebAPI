using BusinessLogicLayer.DTO;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DataContext _context;

        public AddressRepository(DataContext context)
        {
            _context = context;
        }

        public Addresses GetAddressByStudentCard(string studentCard)
        {
            return _context.Addresses.FirstOrDefault(a => a.Students.StudentCard == studentCard);
        }

        public void AddAddress(string studentCard, Addresses addressCreate)
        {
            addressCreate.Students = _context.Students.FirstOrDefault(s => s.StudentCard == studentCard);

            _context.Addresses.Add(addressCreate);
            _context.SaveChanges();
        }

        public void UpdateAddress(Addresses addressUpdate)
        {
            _context.Addresses.Update(addressUpdate);
            _context.SaveChanges();
        }

        public void DeleteAddress(Addresses addressDelete)
        {
            _context.Addresses.Remove(addressDelete);
            _context.SaveChanges();
        }
    }

}
