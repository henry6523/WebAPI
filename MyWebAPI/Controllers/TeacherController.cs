// TeacherController.cs
using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using X.PagedList;
using MyWebAPI.Services;
using DataAccessLayer.Repositories;
using DataAccessLayer.Helpers;

namespace MyWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
	{
		private readonly ITeacherRepository _teacherRepository;
		private readonly IMapper _mapper;
		private readonly ILogger<TeacherController> _logger;
        private readonly IResponseServiceRepository _responseServiceRepository;

		public TeacherController(
            ITeacherRepository teacherRepository, 
            IMapper mapper, 
            ILogger<TeacherController> logger, 
            IResponseServiceRepository responseServiceRepository
            )
		{
			_teacherRepository = teacherRepository;
			_mapper = mapper;
			_logger = logger;
            _responseServiceRepository = responseServiceRepository;
		}
        public enum FilterType
        {
            TeacherName
        }

        /// <summary>
        /// Show list of Teacher
        /// </summary>
        /// <returns></returns>
        /// <param name="filterValue">Input the Value you want to filt. Filt by TeacherName.</param>
        /// <param name="page">Index starting from 0 to designate the page for retrieval.</param>
		/// <param name="pageSize">Number of results per page to return</param>
        /// <remarks>
        /// Returns the list of **Teacher** that have been assigned access control on the referenced resource.
        /// </remarks>
        /// <response code="200">Successfully returns a list of Teacher.</response>
        [HttpGet]
		[Authorize(Roles = "Reader")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TeacherDTO>))]
        public IActionResult GetAllTeachers(string? filterValue = "", int? page = 0, int pageSize = 10)
        {
            IEnumerable<DataAccessLayer.Models.Teachers> teachers = _teacherRepository.GetTeachers();

            if (page < 1)
            {
                return _responseServiceRepository.CustomBadRequestResponse("The page value cannot below 1", page);
            }

            if (!string.IsNullOrEmpty(filterValue))
            {
                teachers = teachers.Where(t => t.TeacherName.Contains(filterValue)).ToList();
            }

            var pagedTeachers = teachers.ToPagedList(page ?? 0, pageSize);
            var teacherMap = _mapper.Map<IEnumerable<TeacherDTO>>(pagedTeachers);

            return Ok(teacherMap);
        }

        /// <summary>
        /// Get a Teacher by TeacherId
        /// </summary>
        /// <param name="id">Input Teacher Id to see **Teacher's info**</param>
        /// <returns></returns>
        /// <remarks>
        /// Retrieve *teacher* by **Teacher Id** and can custom select *any teacher*.
        /// 
        /// **Note**: If you need to find a Teacher but do not know the Teacher's Name,
        /// you can search on the System through the **Teacher Id**
        /// </remarks>
        /// <response code="200">Information of Teacher</response>
        /// <response code="404">TeacherId Not Found!!</response>
        [HttpGet("{id}")]
		[Authorize(Roles = "Reader")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeacherDTO))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetTeacher(int id)
		{
			var teacher = _teacherRepository.GetTeacher(id);

			if (teacher == null)
			{
				return _responseServiceRepository.CustomNotFoundResponse("Teacher not found", teacher);
			}

			var teacherMap = _mapper.Map<TeacherDTO>(teacher);

			return _responseServiceRepository.CustomOkResponse("Data loaded successfully", teacherMap);
		}

        /// <summary>
        /// Create a new Teacher
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Create a new Teacher by input the information like **Teacher Name**, **Email** and **Phone No**.
        /// </remarks>
        /// <response code="201">Successfully created a Teacher.</response>
        /// <response code="400">Teacher domain is not among the registered SSO 
        /// domains for this System!!</response>
        [HttpPost]
		[Authorize(Roles = "Writer")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TeacherDTO))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult AddTeacher([FromBody] CreateTeacherDTO createTeacherDTO)
		{
            var teacherEntity = _mapper.Map<Teachers>(createTeacherDTO);
            var createdTeacherDTO = _mapper.Map<TeacherDTO>(teacherEntity);

			_teacherRepository.AddTeacher(teacherEntity);

            createdTeacherDTO.Id = teacherEntity.Id;

            return CreatedAtAction(nameof(GetTeacher), new { id = createdTeacherDTO.Id }, createdTeacherDTO);
		}

        /// <summary>
        /// Update a Teacher by TeacherId
        /// </summary>
        /// <param name="id">Input TeacherId to **update** Teacher's info.</param>
        /// <param name="createTeacherDTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Update the specified _teacher_ to the System by **TeacherId**.
        /// </remarks>
        /// <response code="204">Teacher's Info updated successfully</response>
        /// <response code="400">TeacherId domain is not among the registered SSO 
        /// domains for this System!!</response>
        /// <response code="404">TeacherId Not Found!!</response>
        [HttpPut("{id}")]
		[Authorize(Roles = "Editor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult UpdateTeacher(int id, [FromBody] CreateTeacherDTO createTeacherDTO)
		{

			var existingTeacher = _teacherRepository.GetTeacher(id);

			if (existingTeacher == null)
			{
				return _responseServiceRepository.CustomNotFoundResponse("Teacher not found", existingTeacher);
			}

			_teacherRepository.UpdateTeacher(id, createTeacherDTO);

            var updatedTeacher = _teacherRepository.GetTeacher(id);

            // Map the updated category to CategoryDTO
            var updataTeacherDTO = new TeacherDTO
            {
                Id = updatedTeacher.Id,
                TeacherName = updatedTeacher.TeacherName,
                Email = updatedTeacher.Email,
                PhoneNo = updatedTeacher.PhoneNo,

            };

            return NoContent();
		}

        /// <summary>
        /// Delete a Teacher's Info by TeacherId
        /// </summary>
        /// <param name="id">Input TeacherId to **delete** Teacher's info.</param>
        /// <returns></returns>
        /// <remarks>
        /// **Note:** Removes the specified Teacher from the list by **TeacherId**.
        /// 
        /// </remarks>
        /// <response code="204">Teacher's Info deleted successfully</response>
        /// <response code="404">TeacherId Not Found!!</response>
        [HttpDelete("{id}")]
		[Authorize(Roles = "Editor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeleteTeacher(int id)
		{
			var teacherToDelete = _teacherRepository.GetTeacher(id);

			if (teacherToDelete == null)
			{
				return _responseServiceRepository.CustomNotFoundResponse("Teacher not found", teacherToDelete);
			}

			_teacherRepository.DeleteTeacher(teacherToDelete);

			return NoContent();
		}

	}
}
