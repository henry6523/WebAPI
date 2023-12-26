using ModelsLayer.DTO;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using ModelsLayer.Entity;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace DataAccessLayer.Repositories
{
    public class AddressesRepository : IAddressesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AddressesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Addresses Get(string studentCard)
        {
            return _context.Addresses.FirstOrDefault(a => a.Students.StudentCard == studentCard);
        }

        public AddressesDTO Add(string studentCard, AddressesDTO addressCreate)
        {
            var addressEntity = _mapper.Map<Addresses>(addressCreate);
            addressEntity.Students = _context.Students.FirstOrDefault(s => s.StudentCard == studentCard);

            _context.Addresses.Add(addressEntity);
            _context.SaveChanges();
            return _mapper.Map<AddressesDTO>(addressEntity);
        }

        public void Update(string studentCard, AddressesDTO addressDTO)
        {
            _mapper.Map(addressDTO, Get(studentCard));
            _context.SaveChanges();
        }

        public void Delete(Addresses addresses)
        {
            _context.Addresses.Remove(addresses);
            _context.SaveChanges();
        }
    }

}
