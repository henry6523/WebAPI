using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IAddressesRepository
	{
		Addresses Get(string studentCard);
		AddressesDTO Add(string studentCard, AddressesDTO addressCreate);
		void Update(string studentCard, AddressesDTO addressUpdate);
		void Delete(Addresses addresses);
	}
}
