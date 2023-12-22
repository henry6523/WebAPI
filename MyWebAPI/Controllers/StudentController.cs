using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using X.PagedList;
using System;
using System.ComponentModel;
using System.Reflection;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
    public class StudentController : ControllerBase
	{
		private readonly IStudentRepository _studentRepository;
		private readonly IClassRepository _classRepository;
		private readonly IMapper _mapper;
        private readonly IResponseServiceRepository _responseServiceRepository;

		public StudentController(IStudentRepository studentRepository, IClassRepository classRepository, IMapper mapper, IResponseServiceRepository responseServiceRepository)
		{
			_studentRepository = studentRepository;
			_classRepository = classRepository;
			_mapper = mapper;
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
        /// <param name="filterValue">Input the Value you want to filt. This parameter is case-insensitive.</param>
        /// <param name="filterBy">**0-->StudentName**      **1-->Email** </param>
        /// <param name="page">Index starting from 0 to designate the page for retrieval.</param>
        /// <param name="pageSize">Number of results per page to return</param>
        /// <returns></returns>
        /// <remarks>
        /// Returns the list of **Student** that have been assigned access control on the referenced resource.
        /// </remarks>
        /// <response code="200">Successfully returns a list of Student.</response>
        [HttpGet]
		[Authorize(Roles = "Reader")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllStudents(string? filterValue = "", FilterType filterBy = FilterType.StudentName, int? page = 0, int pageSize = 10)
		{
            if (page < 1)
            {
                return _responseServiceRepository.CustomBadRequestResponse("The page value cannot below 1", page);
            }

            var students = _studentRepository.GetStudents();
            var filteredStudents = students.Where(s =>
            {
                if (!string.IsNullOrEmpty(filterValue) && filterBy == FilterType.StudentName)
                {
                    return s.StudentName.Contains(filterValue);
                }
                if (!string.IsNullOrEmpty(filterValue) && filterBy == FilterType.Email)
                {
                    return s.Email.Contains(filterValue);
                }
                return true;
            });

            var pagedStudents = filteredStudents.ToPagedList(page ?? 0, pageSize);
            var studentMap = _mapper.Map<IEnumerable<StudentDTO>>(pagedStudents.ToList());

			return _responseServiceRepository.CustomOkResponse("Data loaded successfully",studentMap);
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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetStudentByCard(string studentCard)
		{
			var studentEntity = _studentRepository.GetStudentByCard(studentCard);

			if (studentEntity == null)
				return _responseServiceRepository.CustomNotFoundResponse("Student not found", studentEntity);

			var studentMap = _mapper.Map<StudentDTO>(studentEntity);

			return _responseServiceRepository.CustomOkResponse("Data loaded successfully", studentMap);
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
        /// Create a new Student
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Create a new Student by input the information like **Course Id** and **Class Id**.
        /// </remarks>
        ///<param name="courseId">Input CourseId to **create** Student's info.</param>
        ///<param name="classesId">Input ClassesId to **create** Student's info.</param>
        /// <param name="studentCreate"></param>
        /// <response code="201">Successfully created a Student.</response>
        /// <response code="400">Student domain is not among the registered SSO 
        /// domains for this System!!</response>
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateStudent([FromQuery] int courseId, [FromQuery] int classesId, [FromBody] StudentDTO studentDTO)
        {
            var studentCardCheck = CheckFieldLengthAndEmpty(studentDTO.StudentCard, 30, "student card");
            if (studentCardCheck != null) return studentCardCheck;

            var studentNameCheck = CheckFieldLengthAndEmpty(studentDTO.StudentName, 30, "student name");
            if (studentNameCheck != null) return studentNameCheck;

            var emailCheck = CheckFieldLengthAndEmpty(studentDTO.Email, 50, "email");
            if (emailCheck != null) return emailCheck;

            var birthDateCheck = CheckIntField(studentDTO.BirthDate, 30, "birth date");
            if (birthDateCheck != null) return birthDateCheck;

            var phoneNoCheck = CheckIntField(studentDTO.PhoneNo, 30, "phone number");
            if (phoneNoCheck != null) return phoneNoCheck;

            _studentRepository.GetStudentTrimToUpper(studentDTO);

            var studentEntity = _mapper.Map<Students>(studentDTO);

            studentEntity.Classes = _classRepository.GetClass(classesId);

            _studentRepository.AddStudent(courseId, studentEntity);

            return _responseServiceRepository.CustomOkResponse("Student created", studentEntity);
        }

        /// <summary>
        /// Update a Student by Student Card
        /// </summary>
        /// <param name="studentCard">Input StudentCard to **update** Student's info.</param>
        /// <param name="studentDTO"></param>
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
		public IActionResult UpdateStudent(string studentCard, [FromBody] StudentDTO studentDTO)
		{

			var existingStudent = _studentRepository.GetStudentByCard(studentCard);

			if (existingStudent == null)
				return _responseServiceRepository.CustomNotFoundResponse("Student not found", existingStudent);

			_mapper.Map(studentDTO, existingStudent);

			_studentRepository.UpdateStudent(existingStudent);

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
		public IActionResult DeleteStudent(string studentCard)
		{
			var existingStudent = _studentRepository.GetStudentByCard(studentCard);

			if (existingStudent == null)
				return _responseServiceRepository.CustomNotFoundResponse("Student not found", existingStudent);

			_studentRepository.DeleteStudent(existingStudent);

			return NoContent();
		}
	}
}
