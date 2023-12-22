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

namespace DataAccessLayer.Repository
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IResponseService _responseService;
        private readonly IStudentRepository _studentRepository;

        public AddressController(IAddressRepository addressRepository, IMapper mapper, IResponseService responseService, IStudentRepository studentRepository)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _responseService = responseService;
            _studentRepository = studentRepository;
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
                return _responseService.CustomNotFoundResponse("Student's Address Not Found", ModelState);

            var addressMap = _mapper.Map<AddressDTO>(addressEntity);
            return _responseService.CustomOkResponse("Data Loaded Successful!", addressMap);
        }

        /// <summary>
        /// Create a Student's Address by new StundentCard
        /// </summary>
        /// <param name="studentCard">Input Student Card to **create** new Student address's info.</param>
        /// <param name="addressDTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Create the specified _address_ to the organization by **Student Card**.
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
        public IActionResult CreateAddress(string studentCard, [FromBody] AddressDTO addressDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addressEntity = _mapper.Map<Addresses>(addressDTO);

            _addressRepository.AddAddress(studentCard, addressEntity);

            var createdAddressMap = _mapper.Map<AddressDTO>(addressEntity);
            return CreatedAtAction(nameof(GetAddressByStudentCard), new { studentCard }, createdAddressMap);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateAddress(string studentCard, [FromBody] AddressDTO addressDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAddress = _addressRepository.GetAddressByStudentCard(studentCard);

            if (existingAddress == null)
                return NotFound();

            _mapper.Map(addressDTO, existingAddress);
            _addressRepository.UpdateAddress(existingAddress);

            return NoContent();
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
                return NotFound();

            _addressRepository.DeleteAddress(existingAddress);

            return NoContent();
        }
    }
}
