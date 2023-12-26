using DataAccessLayer.Interfaces;
using ModelsLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.DTO;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TeacherController : ControllerBase
{
	private readonly ITeacherRepository _teacherRepository;
	private readonly IResponseServiceRepository _responseServiceRepository;

	public TeacherController(
		ITeacherRepository teacherRepository,
		IResponseServiceRepository responseServiceRepository
	)
	{
		_teacherRepository = teacherRepository;
		_responseServiceRepository = responseServiceRepository;
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
	public IActionResult Gets(string? filterValue = "", int? page = 1, int pageSize = 10)
	{
		if (page < 1)
		{
			return _responseServiceRepository.CustomBadRequestResponse("The page value cannot below 1", page);
		}
		var teachers = _teacherRepository.GetTeachers(filterValue, page ?? 0, pageSize);
		return _responseServiceRepository.CustomOkResponse("Data loaded successfully", teachers);
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
	public IActionResult Get(int id)
	{
		var teacher = _teacherRepository.GetTeacher(id);

		if (teacher == null)
		{
			return _responseServiceRepository.CustomNotFoundResponse("Teacher not found", teacher);
		}

		return _responseServiceRepository.CustomOkResponse("Data loaded successfully", teacher);
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
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))] 
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult Post([FromBody] CreateTeacherDTO createTeacherDTO)
	{
		var createdTeacherDTO = _teacherRepository.AddTeacher(createTeacherDTO);

		return _responseServiceRepository.CustomCreatedResponse("Teacher created successfully", createdTeacherDTO);
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
	public IActionResult Put(int id, [FromBody] CreateTeacherDTO TeacherDTO)
	{
		_teacherRepository.UpdateTeacher(id, TeacherDTO);

		return _responseServiceRepository.CustomNoContentResponse();
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
	public IActionResult Delete(int id)
	{
		_teacherRepository.DeleteTeacher(id);

		return _responseServiceRepository.CustomNoContentResponse();
	}
}
