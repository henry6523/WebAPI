using AutoMapper;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using Swashbuckle.AspNetCore.SwaggerGen;
using DataAccessLayer.Helpers;
using System.ComponentModel.DataAnnotations;
using ModelsLayer.Entity;
using ModelsLayer.DTO;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly IAddressesRepository _addressesRepository;
        private readonly IResponseServiceRepository _responseServiceRepository;

        public AddressesController(
            IStudentsRepository studentRepository,
            IAddressesRepository addressRepository,
            IResponseServiceRepository responseServiceRepository
            )
        {
            _studentsRepository = studentRepository;
            _addressesRepository = addressRepository;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressesDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Gets(string studentCard)
        {
            var addresses = _addressesRepository.Get(studentCard);

            if (addresses == null)
                return _responseServiceRepository.CustomNotFoundResponse("Student address not found", studentCard);

            return _responseServiceRepository.CustomOkResponse("Data Loaded Successful!", addresses);
        }

        /// <summary>
        /// Get an Address by AddressId
        /// </summary>
        /// <returns>Not Implemented</returns>
        /// <response code="501">Not Implemented</response>
        [HttpGet("address")]
        [Authorize(Roles = "Reader")]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public IActionResult Get()
        {
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddressesDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([Required] string studentCard, [FromBody] AddressesDTO addressDTO)
        {

            var existingStudent = _studentsRepository.Get(studentCard);

            if (existingStudent == null)
            {
                return _responseServiceRepository.CustomNotFoundResponse("Student not found", studentCard);
            }

            var addressCreated = _addressesRepository.Add(studentCard, addressDTO);

            return _responseServiceRepository.CustomCreatedResponse("Student address created", addressCreated);
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
        public IActionResult Update(string studentCard, [FromBody] AddressesDTO addressDTO)
        {
            var existingAddress = _addressesRepository.Get(studentCard);
            if (existingAddress == null)
                return _responseServiceRepository.CustomNotFoundResponse("Student address not found", addressDTO);

            _addressesRepository.Update(studentCard, addressDTO);

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
        public IActionResult Delete(string studentCard)
        {

            var existingAddress = _addressesRepository.Get(studentCard);

            if (existingAddress == null)
                return _responseServiceRepository.CustomNotFoundResponse("Student address not found", studentCard);

            _addressesRepository.Delete(existingAddress);

            return NoContent();
        }
    }
}
