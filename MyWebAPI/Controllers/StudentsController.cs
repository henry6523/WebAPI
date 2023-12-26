using AutoMapper;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using X.PagedList;
using System;
using System.ComponentModel;
using System.Reflection;
using DataAccessLayer.Helpers;
using System.ComponentModel.DataAnnotations;
using ModelsLayer.Entity;
using ModelsLayer.DTO;
using DataAccessLayer.Repositories;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
    public class StudentsController : ControllerBase
	{
		private readonly ICoursesRepository _coursesRepository;
		private readonly IStudentsRepository _studentsRepository;
		private readonly IClassesRepository _classesRepository;
        private readonly IResponseServiceRepository _responseServiceRepository;

		public StudentsController(
            ICoursesRepository courseRepository,
            IStudentsRepository studentRepository, 
            IClassesRepository classRepository, 
            IResponseServiceRepository responseServiceRepository
            )
		{
            _coursesRepository = courseRepository;
            _studentsRepository = studentRepository;
            _classesRepository = classRepository;
            _responseServiceRepository = responseServiceRepository;
		}
        public enum FilterType
        {
            StudentName,
            Email
        }
        /// <summary>
        /// Show list of Student
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Returns the list of **Student** that have been assigned access control on the referenced resource.
        /// </remarks>
        /// <response code="200">Successfully returns a list of Student.</response>
        [HttpGet]
		[Authorize(Roles = "Reader")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentsDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Gets([FromQuery] PaginationDTO paginationDTO)
        {
            var students = _studentsRepository.Gets(paginationDTO);
            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", students);
        }
        
        /// <summary>
        /// Get a Student by Student Card
        /// </summary>
        /// <param name="studentCard">Input Student Card to see **Student's info**</param>
        /// <returns></returns>
        /// <remarks>
        /// Retrieve *student* by **Student Card** and can custom select *any student*.
        /// 
        /// **Note**: If you need to find a Student but do not know the Student's Name,
        /// you can search on the System through the **Student Card**
        /// </remarks>
        /// <response code="200">Information of Student</response>
        /// <response code="404">StudentCard Not Found!!</response>
        [HttpGet("{studentCard}")]
		[Authorize(Roles = "Reader")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentsDTO))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult Get(string studentCard)
        {
            var students = _studentsRepository.Get(studentCard);

            if (students == null)
                return _responseServiceRepository.CustomNotFoundResponse("Class not found", studentCard);

            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", students);
        }

        /// <summary>
        /// Create a new Student
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Create a new Student by input the information like **Course Id** and **Class Id**.
        /// </remarks>
        ///<param name="coursesId">Input CourseId to **create** Student's info.</param>
        ///<param name="classesId">Input ClassesId to **create** Student's info.</param>
        /// <param name="studentDTO"></param>
        /// <response code="201">Successfully created a Student.</response>
        /// <response code="400">Student domain is not among the registered SSO 
        /// domains for this System!!</response>
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([Required]int coursesId, [Required] int classesId, [FromBody] StudentsDTO studentDTO)
        {
			var courseExists = _coursesRepository.Get(coursesId);
			var classExists = _classesRepository.Get(classesId);


            if (courseExists == null)
			{
				return _responseServiceRepository.CustomBadRequestResponse("Please enter corect data to Course ID", coursesId);
			}

			if (classExists == null)
			{
				return _responseServiceRepository.CustomBadRequestResponse("Please enter corect data to Class ID", classesId);
			}

            var students = _studentsRepository.Add(coursesId, studentDTO);

            return _responseServiceRepository.CustomOkResponse("Student created", students);
        }

        /// <summary>
        /// Update a Student by Student Card
        /// </summary>
        /// <param name="studentCard">Input StudentCard to **update** Student's info.</param>
        /// <param name="studentsDTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Update the specified _student_ to the System by **Student Card**.
        /// </remarks>
        /// <response code="204">Student's Info updated successfully</response>
        /// <response code="400">Student Card domain is not among the registered SSO 
        /// domains for this System!!</response>
        /// <response code="404">Student Card Not Found!!</response>
        [HttpPut("{studentCard}")]
		[Authorize(Roles = "Editor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Update(string studentCard, [FromBody] StudentsDTO studentsDTO)
		{

			var existingStudent = _studentsRepository.Get(studentCard);

			if (existingStudent == null)
				return _responseServiceRepository.CustomNotFoundResponse("Student not found", studentCard);

			_studentsRepository.Update(studentCard, studentsDTO);

			return NoContent();
		}

        /// <summary>
        /// Delete a Student's Info by Student Card
        /// </summary>
        /// <param name="studentCard">Input Student Card to **delete** Course's info.</param>
        /// <returns></returns>
        /// <remarks>
        /// **Note:** Removes the specified Student from the list by **Student Card**.
        /// 
        /// </remarks>
        /// <response code="204">Student's Info deleted successfully</response>
        /// <response code="404">Student Card Not Found!!</response>
        [HttpDelete("{studentCard}")]
		[Authorize(Roles = "Editor")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string studentsCard)
        {
            var existingStudents = _studentsRepository.Get(studentsCard);

            if (existingStudents == null)
                return _responseServiceRepository.CustomNotFoundResponse("Category not found", studentsCard);

            _studentsRepository.Delete(studentsCard);

            return NoContent();
        }
    }
}
