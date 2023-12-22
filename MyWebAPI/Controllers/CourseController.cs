// CourseController.cs
using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using X.PagedList;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize]
    public class CourseController : ControllerBase
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;
        private readonly IResponseServiceRepository _responseServiceRepository;

		public CourseController(ICourseRepository courseRepository, IMapper mapper, IResponseServiceRepository responseServiceRepository)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
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
        /// <param name="filterValue">Input the Value you want to filt. Filt by CourseName.</param>
        /// <param name="page">Index starting from 0 to designate the page for retrieval.</param>
		/// <param name="pageSize">Number of results per page to return</param>
        /// <remarks>
        /// Returns the lists of **Course** that have been assigned access control on the referenced resource.
        /// </remarks>
        /// <response code="200">Successfully returns a list of Source.</response>
        [HttpGet]
        [Authorize(Roles = "Reader")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseDTO>))]
        public IActionResult GetAllCourses(string? filterValue = "", int? page = 0, int pageSize = 10)
        {
            IEnumerable<DataAccessLayer.Models.Courses> courses = _courseRepository.GetCourses();

            if (page < 1)
            {
                return _responseServiceRepository.CustomBadRequestResponse("The page value cannot below 1", page);
            }

            if (!string.IsNullOrEmpty(filterValue))
            {
                courses = courses.Where(t => t.CourseName.Contains(filterValue)).ToList();
            }

            var pagedCourses = courses.ToPagedList(page ?? 0, pageSize);
            var courseDTOs = _mapper.Map<IEnumerable<CourseDTO>>(courses);

            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", courseDTOs);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCourseById(int id)
        {
            var courseEntity = _courseRepository.GetCourse(id);

            if (courseEntity == null)
                return _responseServiceRepository.CustomNotFoundResponse("Course not found", courseEntity);

            var courseDTO = _mapper.Map<CourseDTO>(courseEntity);

            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", courseDTO);
        }

        private IActionResult CheckFieldLengthAndEmpty(string field, int maxLength, string fieldName)
        {
            if (string.IsNullOrEmpty(field))
            {
                return _responseServiceRepository.CustomBadRequestResponse("The characters you type in cannot be empty");
            }

            if (field.Length > maxLength)
            {
                return _responseServiceRepository.CustomBadRequestResponse($"The characters you type in for {fieldName} are over {maxLength}");
            }

            return null;
        }
        private IActionResult CheckIntField(int fieldValue, int maxLength, string fieldName)
        {
            if (fieldValue.ToString().Length > maxLength)
            {
                return _responseServiceRepository.CustomBadRequestResponse($"The characters for {fieldName} are over {maxLength}");
            }

            return null;
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
        /// domains for this organization!!</response>
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateCourse([FromQuery] int CategoryId, [FromBody] CourseDTO courseDTO)
        {


            var courseNameCheck = CheckFieldLengthAndEmpty(courseDTO.CourseName, 30, "course name");
            if (courseNameCheck != null) return courseNameCheck;

            var idCheck = CheckIntField(courseDTO.Id, 30, "Id");
            if (idCheck != null) return idCheck;

            var courseEntity = _mapper.Map<Courses>(courseDTO);

            _courseRepository.AddCourse(CategoryId, courseEntity);

            courseDTO.Id = courseEntity.Id;

            return _responseServiceRepository.CustomCreatedResponse("Course created", courseDTO);
        }

        /// <summary>
        /// Update a Course by CourseId
        /// </summary>
        /// <param name="id">Input CourseId to **update** Course's info.</param>
        /// <param name="courseDTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Update the specified _course_ to the organization by **CourseId**.
        /// </remarks>
        /// <response code="204">Course's Info updated successfully</response>
        /// <response code="400">CourseId domain is not among the registered SSO 
        /// domains for this organization!!</response>
        /// <response code="404">CourseId Not Found!!</response>
        [HttpPut("{id}")]
		[Authorize(Roles = "Editor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult UpdateCourse(int id, [FromBody] CourseDTO courseDTO)
		{

			var existingCourse = _courseRepository.GetCourse(id);

			if (existingCourse == null)
				return _responseServiceRepository.CustomNotFoundResponse("Course not found", existingCourse);

			_courseRepository.UpdateCourse(id, courseDTO);

			return _responseServiceRepository.CustomNoContentResponse("Course updated", courseDTO);
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
		public IActionResult DeleteCourse(int id)
		{
			var existingCourse = _courseRepository.GetCourse(id);

			if (existingCourse == null)
				return _responseServiceRepository.CustomNotFoundResponse("Course not found", existingCourse);

			_courseRepository.DeleteCourse(existingCourse);

			return _responseServiceRepository.CustomNoContentResponse("Course deleted", existingCourse);
		}
	}
}
