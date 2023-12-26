using AutoMapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using X.PagedList;
using DataAccessLayer.Helpers;
using ModelsLayer.DTO;
using ModelsLayer.Entity;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesRepository _classesRepository;
        private readonly IResponseServiceRepository _responseServiceRepository;

        public ClassesController(
            IClassesRepository classesRepository,
            IResponseServiceRepository responseServiceRepository
            )
        {
            _classesRepository = classesRepository;
            _responseServiceRepository = responseServiceRepository;
        }
        public enum FilterType
        {
            ClassName
        }

        /// <summary>
        /// Show list of Class
        /// </summary>
        /// <returns></returns>
        /// <param name="paginationDTO"></param>
        /// <remarks>
        /// Returns the list of **Class** that have been assigned access control on the referenced resource.
        /// </remarks>
        /// <response code="200">Successfully returns a list of Class.</response>
        [HttpGet]
        [Authorize(Roles = "Reader")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClassesDTO>))]
        public IActionResult Gets([FromQuery] PaginationDTO paginationDTO)
        {
            var classes = _classesRepository.Gets(paginationDTO);
            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", classes);
        }

        /// <summary>
        /// Get a Class by ClassId
        /// </summary>
        /// <param name="id">Input ClassId to see **Class's info**</param>
        /// <returns></returns>
        /// <remarks>
        /// Retrieve *class* by **ClassId** and can custom select *any class*.
        /// 
        /// **Note**: If you need to find a Class but do not know the Class's Name,
        /// you can search on the System through the **ClassId**.
        /// </remarks>
        /// <response code="200">Information of Class</response>
        /// <response code="404">Class Id Not Found!!</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Reader")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClassesDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {

            var classes = _classesRepository.Get(id);

            if (classes == null)
                return _responseServiceRepository.CustomNotFoundResponse("Class not found", id);

            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", classes);
        }
        /// <summary>
        /// Create a Class
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Create a Class by input **Class Name**.
        /// </remarks>
        /// <response code="201">Successfully created a Class.</response>
        /// <response code="400">Class domain is not among the registered SSO 
        /// domains for this System!!</response>
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ClassesDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] CreateClassDTO classDTO)
        {
            var classes = _classesRepository.Add(classDTO);
            return _responseServiceRepository.CustomCreatedResponse("Class created", classes);
        }

        /// <summary>
        /// Update a Class by ClassId
        /// </summary>
        /// <param name="id">Input ClassId to **update** Class's info.</param>
        /// <param name="createClassDTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Update the specified _class_ to the System by **Class Id**.
        /// </remarks>
        /// <response code="204">Class's Info updated successfully</response>
        /// <response code="400">ClassId domain is not among the registered SSO 
        /// domains for this System!!</response>
        /// <response code="404">ClassId Not Found!!</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] CreateClassDTO createClassDTO)
        {

            var existingClass = _classesRepository.Get(id);
            if (existingClass == null) 
                return _responseServiceRepository.CustomNotFoundResponse("Class not found", id);
            _classesRepository.Update(id, createClassDTO);
            return NoContent();
        }

        /// <summary>
        /// Delete a Class's Info by ClassId
        /// </summary>
        /// <param name="id">Input ClassId to **delete** Class's info.</param>
        /// <returns></returns>
        /// <remarks>
        /// **Note:** Removes the specified Class from the list by **ClassId**.
        /// 
        /// </remarks>
        /// <response code="204">Class's Info deleted successfully</response>
        /// <response code="404">ClassId Not Found!!</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var existingClasses = _classesRepository.Get(id);

            if (existingClasses == null)
                return _responseServiceRepository.CustomNotFoundResponse("Category not found", id);

            _classesRepository.Delete(id);

            return NoContent();
        }
    }
}
