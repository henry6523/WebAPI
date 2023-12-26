// CourseController.cs
using AutoMapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using X.PagedList;
using DataAccessLayer.Helpers;
using System.ComponentModel.DataAnnotations;
using ModelsLayer.DTO;
using ModelsLayer.Entity;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
	{
		private readonly ICoursesRepository _coursesRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IResponseServiceRepository _responseServiceRepository;

		public CoursesController(
            ICoursesRepository courseRepository, 
            ICategoriesRepository categoryRepository, 
            IResponseServiceRepository responseServiceRepository
            )
		{
			_coursesRepository = courseRepository;
            _categoriesRepository = categoryRepository;
            _responseServiceRepository = responseServiceRepository;
		}
        public enum FilterType
        {
            CourseName
        }

        /// <summary>
        /// Show list of Course
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Returns the list of **Course** that have been assigned access control on the referenced resource.
        /// </remarks>
        /// <response code="200">Successfully returns a list of Source.</response>
        [HttpGet]
        [Authorize(Roles = "Reader")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CoursesDTO>))]
        public IActionResult Gets([FromQuery] PaginationDTO paginationDTO)
        {
            var courses = _coursesRepository.Gets(paginationDTO);
            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", courses);
        }

        /// <summary>
        /// Get a Course by CourseId
        /// </summary>
        /// <param name="id">Input CourseId to see **Course's info**</param>
        /// <returns></returns>
        /// <remarks>
        /// Retrieve *course* by **CourseId** and can custom select *any course*.
        /// 
        /// **Note**: If you need to find a Course but do not know the Course's Name,
        /// you can search on the System through the **CourseId**
        /// </remarks>
        /// <response code="200">Information of Course</response>
        /// <response code="404">CourseId Not Found!!</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Reader")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoursesDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var courses = _coursesRepository.Get(id);

            if (courses == null)
                return _responseServiceRepository.CustomNotFoundResponse("Class not found", id);

            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", courses);
        }

        /// <summary>
        /// Create a Course
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Create a Course by input **Course Name**.
        /// </remarks>
        /// <response code="201">Successfully created a Course.</response>
        /// <response code="400">Course domain is not among the registered SSO 
        /// domains for this System!!</response>
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CoursesDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromQuery][Required] int categoriesId, [FromBody] CreateCourseDTO createCourseDTO)
        {
			var existingCategory = _categoriesRepository.Get(categoriesId);

			if (existingCategory == null)
			{
				return _responseServiceRepository.CustomBadRequestResponse("Categories Id does not exits", categoriesId);
			}

            var createdCourseDTO = _coursesRepository.Add(categoriesId, createCourseDTO);

            return _responseServiceRepository.CustomCreatedResponse("Course created", createdCourseDTO);
        }

        /// <summary>
        /// Update a Course by CourseId
        /// </summary>
        /// <param name="id">Input CourseId to **update** Course's info.</param>
        /// <param name="createCourseDTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Update the specified _course_ to the System by **CourseId**.
        /// </remarks>
        /// <response code="204">Course's Info updated successfully</response>
        /// <response code="400">CourseId domain is not among the registered SSO 
        /// domains for this System!!</response>
        /// <response code="404">CourseId Not Found!!</response>
        [HttpPut("{id}")]
		[Authorize(Roles = "Editor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] CreateCourseDTO createCourseDTO)
        {
            var existingCategory = _categoriesRepository.Get(id);
            if (existingCategory == null)
                return _responseServiceRepository.CustomNotFoundResponse("Class not found", id);
            _coursesRepository.Update(id, createCourseDTO);
            return NoContent();
        }

        /// <summary>
        /// Delete a Course's Info by CourseId
        /// </summary>
        /// <param name="id">Input CourseId to **delete** Course's info.</param>
        /// <returns></returns>
        /// <remarks>
        /// **Note:** Removes the specified Course from the list by **CourseId**.
        /// 
        /// </remarks>
        /// <response code="204">Course's Info deleted successfully</response>
        /// <response code="404">CourseId Not Found!!</response>
        [HttpDelete("{id}")]
		[Authorize(Roles = "Editor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var existingCourses = _coursesRepository.Get(id);

            if (existingCourses == null)
                return _responseServiceRepository.CustomNotFoundResponse("Category not found", id);

            _coursesRepository.Delete(id);

            return NoContent();
        }
    }
}
