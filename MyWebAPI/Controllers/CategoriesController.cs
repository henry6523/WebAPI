using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using DataAccessLayer.Interfaces;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Repositories;
using DataAccessLayer.Helpers;
using ModelsLayer.DTO;
using ModelsLayer.Entity;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesRepository _categoriesRepository;
        private readonly IResponseServiceRepository _responseServiceRepository;

		public CategoriesController(
            ICategoriesRepository categoryRepository,
            IResponseServiceRepository responseServiceRepository
            )
		{
            _categoriesRepository = categoryRepository;
            _responseServiceRepository = responseServiceRepository;
		}
        public enum FilterType
        {
            CategoriesName
        }

        /// <summary>
        /// Show list of Category
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Returns the list of **Category** that have been assigned access control on the referenced resource.
        /// </remarks>
        /// <response code="200">Successfully returns a list of Category.</response>
        [HttpGet]
        [Authorize(Roles = "Reader")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoriesDTO>))]
        public IActionResult Gets([FromQuery] PaginationDTO paginationDTO)
        {
            var categories = _categoriesRepository.Gets(paginationDTO);
            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", categories);
        }

        /// <summary>
        /// Get a Category by CategoryId
        /// </summary>
        /// <param name="id">Input Category's Id to see **Category's info**</param>
        /// <returns></returns>
        /// <remarks>
        /// Retrieve *categories* by **Category Id** and can custom select *any category*.
        /// 
        /// **Note**: If you need to find a Category but do not know the Category's Name,
        /// you can search on the System through the **Category Id**
        /// </remarks>
        /// <response code="200">Information of Category</response>
        /// <response code="404">Category Id Not Found!!</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Reader")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriesDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var category = _categoriesRepository.Get(id);

            if (category == null)
            {
                return _responseServiceRepository.CustomNotFoundResponse("Category not found", id);
            }

            return _responseServiceRepository.CustomOkResponse("Data loaded successfully", category);
        }

        /// <summary>
        /// Create a Category
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Create a Category by input **Category Name**.
        /// </remarks>
        /// <response code="201">Successfully created a Category.</response>
		/// <response code="400">Category domain is not among the registered SSO 
		/// domains for this System!!</response>
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoriesDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] CreateCategoryDTO categoryDTO)
        {
            var createdCategoryDTO = _categoriesRepository.Add(categoryDTO);

            return _responseServiceRepository.CustomCreatedResponse("Category created successfully", createdCategoryDTO);
        }

        /// <summary>
        /// Update a Category by CategoryId
        /// </summary>
        /// <param name="id">Input CategoryId to **update** Category's info.</param>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Update the specified _category_ to the System by **Category Id**.
        /// </remarks>
        /// <response code="204">Category's Info updated successfully</response>
        /// <response code="400">CategoryId domain is not among the registered SSO 
        /// domains for this System!!</response>
        /// <response code="404">CategoryId Not Found!!</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] CreateCategoryDTO categoryDTO)
        {
            _categoriesRepository.Update(id, categoryDTO);

            return NoContent();
        }

        /// <summary>
        /// Delete a Category's Info by CategoryId
        /// </summary>
        /// <param name="id">Input CategoryId to **delete** Category's info.</param>
        /// <returns></returns>
        /// <remarks>
        /// **Note:** Removes the specified Category from the list by **CategoryId**.
        /// 
        /// </remarks>
        /// <response code="204">Category's Info deleted successfully</response>
        /// <response code="404">CategoryId Not Found!!</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var existingCategories = _categoriesRepository.Get(id);

            if (existingCategories == null)
                return _responseServiceRepository.CustomNotFoundResponse("Category not found", id);

            _categoriesRepository.Delete(id);

            return NoContent();
        }
    }
}
