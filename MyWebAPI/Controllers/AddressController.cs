using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using MyWebAPI.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using DataAccessLayer.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Repository
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
		private readonly IStudentRepository _studentRepository;
		private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IResponseServiceRepository _responseServiceRepository;

        public AddressController(
			IStudentRepository studentRepository,
			IAddressRepository addressRepository, 
            IMapper mapper, 
            IResponseServiceRepository responseServiceRepository
            )
        {
			_studentRepository = studentRepository;
			_addressRepository = addressRepository;
            _mapper = mapper;
            _responseServiceRepository = responseServiceRepository;
        }

        /// <summary>
        /// Get a Student's address by StudentCard
        /// </summary>
        /// <param name="studentCard">Input Student Card to see **the address's info** of Student</param>
        /// <returns></returns>
        /// <remarks>
        /// Retrieve *student's address* by **Student Card** and can custom select *any student's address*.
        /// 
        /// **Note**: If you need to find a Address of Student but do not know the Student's Name,
        /// you can search on the System through the **Student Card**
        /// </remarks>
        /// <response code="200">Data Loaded Successful!</response>
        /// <response code="404">Student's Address Not Found</response>
        [HttpGet("{studentCard}")]
        [Authorize(Roles = "Reader")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetAddressByStudentCard(string studentCard)
        {

            var addressEntity = _addressRepository.GetAddressByStudentCard(studentCard);

            if (addressEntity == null)
                return _responseServiceRepository.CustomNotFoundResponse("Student address not found", addressEntity);

            var addressMap = _mapper.Map<AddressDTO>(addressEntity);
            return _responseServiceRepository.CustomOkResponse("Data Loaded Successful!", addressMap);
        }

		/// <summary>
		/// Get an Address by AddressId (Not Implemented)
		/// </summary>
		/// <returns>Not Implemented</returns>
		/// <response code="501">Not Implemented</response>
		[HttpGet("address")]
		[Authorize(Roles = "Reader")]
		[ProducesResponseType(StatusCodes.Status501NotImplemented)]
		public IActionResult GetAddressByAddressId()
		{
			// This method is intentionally not implemented
			return StatusCode(StatusCodes.Status501NotImplemented, "Method not implemented");
		}


		/// <summary>
		/// Create a Student's Address by new StundentCard
		/// </summary>
		/// <param name="studentCard">Input Student Card to **create** new Student address's info.</param>
		/// <param name="addressDTO"></param>
		/// <returns></returns>
		/// <remarks>
		/// Create the specified _address_ to the System by **Student Card**.
		/// 
		/// 
		/// **Note**: If the System uses *Single Sign-On* (SSO) and is configured to block external (non-SSO) users from joining,
		/// you can only invite **Student Card** from the domains associated with this System.
		/// </remarks>
		/// <response code="201">Student's Address created successfully</response>
		/// <response code="400">Student Card domain is not among the registered SSO 
		/// domains for this System!!</response>
		[HttpPost("{studentCard}")]
		[Authorize(Roles = "Writer")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddressDTO))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult CreateAddress([Required]string studentCard, [FromBody] AddressDTO addressDTO)
		{

			var existingStudent = _studentRepository.GetStudentByCard(studentCard);

			if (existingStudent == null)
			{
				return _responseServiceRepository.CustomBadRequestResponse("Please enter correct data to the box", existingStudent);
			}

			var addressEntity = _mapper.Map<Addresses>(addressDTO);

			_addressRepository.AddAddress(studentCard, addressEntity);

			var createdAddressMap = _mapper.Map<AddressDTO>(addressEntity);
			return _responseServiceRepository.CustomCreatedResponse("Student address created", createdAddressMap);
		}

		/// <summary>
		/// Update a Student's Address by StundentCard
		/// </summary>
		/// <param name="studentCard">Input Student Card to **update** new Student address's info.</param>
		/// <param name="addressDTO"></param>
		/// <returns></returns>
		/// <remarks>
		/// Update the specified _address_ to the System by **Student Card**.
		/// </remarks>
		/// <response code="204">Student's Address updated successfully</response>
		/// <response code="400">Student Card domain is not among the registered SSO 
		/// domains for this System!!</response>
		/// <response code="404">StudentCard Not Found!!</response>
		[HttpPut("{studentCard}")]
        [Authorize(Roles = "Editor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateAddress(string studentCard, [FromBody] AddressDTO addressDTO)
        {
            var existingAddress = _addressRepository.GetAddressByStudentCard(studentCard);

            if (existingAddress == null)
                return _responseServiceRepository.CustomNotFoundResponse("Student address not found", existingAddress);

            _mapper.Map(addressDTO, existingAddress);
            _addressRepository.UpdateAddress(existingAddress);

            return _responseServiceRepository.CustomNoContentResponse();
        }

        /// <summary>
        /// Delete a Student's Address by StundentCard
        /// </summary>
        /// <param name="studentCard">Input Student Card to **delete** Student address's info.</param>
        /// <returns></returns>
        /// <remarks>
        /// **Note:** Removes the specified students's address from the class by **Student Card**.
		/// If the specified student was invited to the class but has not joined yet,
		/// this cancels their invitation.
        /// </remarks>
		/// <response code="204">Student's Address deleted successfully</response>
        /// <response code="404">StudentCard Not Found!!</response>
        [HttpDelete("{studentCard}")]
        [Authorize(Roles = "Editor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteAddress(string studentCard)
        {

            var existingAddress = _addressRepository.GetAddressByStudentCard(studentCard);

            if (existingAddress == null)
                return _responseServiceRepository.CustomNotFoundResponse("Student address not found");

            _addressRepository.DeleteAddress(existingAddress);

            return _responseServiceRepository.CustomNoContentResponse();
        }
    }
}
