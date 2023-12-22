using BusinessLogicLayer.DTO;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
	public interface IAddressRepository
	{
		Addresses GetAddressByStudentCard(string studentCard);
		void AddAddress(string studentCard, Addresses addressCreate);
		void UpdateAddress(Addresses addressUpdate);
		void DeleteAddress(Addresses addressDelete);
	}
}
