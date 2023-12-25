﻿// CourseController.cs
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
using DataAccessLayer.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize]
    public class CourseController : ControllerBase
	{
		private readonly ICourseRepository _courseRepository;
        private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;
        private readonly IResponseServiceRepository _responseServiceRepository;

		public CourseController(
            ICourseRepository courseRepository, 
            ICategoryRepository categoryRepository, 
            IMapper mapper, 
            IResponseServiceRepository responseServiceRepository
            )
		{
			_courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
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
        /// Returns the list of **Course** that have been assigned access control on the referenced resource.
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateCourse([FromQuery][Required] int CategoryId, [FromBody] CreateCourseDTO createCourseDTO)
        {
			var existingCategory = _categoryRepository.GetCategory(CategoryId);

			if (existingCategory == null)
			{
				return _responseServiceRepository.CustomBadRequestResponse("Please enter correct data to the box", existingCategory);
			}

			var courseEntity = _mapper.Map<Courses>(createCourseDTO);
            var createdCourseDTO = _mapper.Map<CourseDTO>(courseEntity);

            _courseRepository.AddCourse(CategoryId, courseEntity);

            createdCourseDTO.Id = courseEntity.Id;

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
		public IActionResult UpdateCourse(int id, [FromBody] CreateCourseDTO createCourseDTO)
		{

			var existingCourse = _courseRepository.GetCourse(id);

			if (existingCourse == null)
				return _responseServiceRepository.CustomNotFoundResponse("Course not found", existingCourse);

			_courseRepository.UpdateCourse(id, createCourseDTO);

            var updatedCourse = _courseRepository.GetCourse(id);

            // Map the updated category to CategoryDTO
            var updataCourseDTO = new ClassDTO
            {
                Id = updatedCourse.Id,
                ClassName = updatedCourse.CourseName
            };

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
		public IActionResult DeleteCourse(int id)
		{
			var existingCourse = _courseRepository.GetCourse(id);

			if (existingCourse == null)
				return _responseServiceRepository.CustomNotFoundResponse("Course not found", existingCourse);

			_courseRepository.DeleteCourse(existingCourse);

			return NoContent();
		}
	}
}
