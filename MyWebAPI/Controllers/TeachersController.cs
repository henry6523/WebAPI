using DataAccessLayer.Interfaces;
using ModelsLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.DTO;
using DataAccessLayer.Repositories;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TeachersController : ControllerBase
{
	private readonly ITeachersRepository _teachersRepository;
	private readonly IResponseServiceRepository _responseServiceRepository;

	public TeachersController(
		ITeachersRepository teacherRepository,
		IResponseServiceRepository responseServiceRepository
	)
	{
        _teachersRepository = teacherRepository;
		_responseServiceRepository = responseServiceRepository;
	}

	/// <summary>
	/// Show list of Teacher
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// Returns the list of **Teacher** that have been assigned access control on the referenced resource.
	/// </remarks>
	/// <response code="200">Successfully returns a list of Teacher.</response>
	[HttpGet]
	[Authorize(Roles = "Reader")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TeachersDTO>))]
	public IActionResult Gets([FromQuery] PaginationDTO paginationDTO)
	{
		var teachers = _teachersRepository.Gets(paginationDTO);
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
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeachersDTO))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult Get(int id)
	{
		var teacher = _teachersRepository.Get(id);

		if (teacher == null)
		{
			return _responseServiceRepository.CustomNotFoundResponse("Teacher not found", id);
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
		var createdTeacherDTO = _teachersRepository.Add(createTeacherDTO);

		return _responseServiceRepository.CustomCreatedResponse("Teacher created successfully", createdTeacherDTO);
	}

    /// <summary>
    /// Update a Teacher by TeacherId
    /// </summary>
    /// <param name="id">Input TeacherId to **update** Teacher's info.</param>
    /// <param name="TeacherDTO"></param>
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
	public IActionResult Update(int id, [FromBody] CreateTeacherDTO TeacherDTO)
	{
        _teachersRepository.Update(id, TeacherDTO);

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
	public IActionResult Delete(int id)
	{
        var existingTeachers = _teachersRepository.Get(id);

        if (existingTeachers == null)
            return _responseServiceRepository.CustomNotFoundResponse("Category not found", id);

        _teachersRepository.Delete(id);

        return NoContent();
    }
}
