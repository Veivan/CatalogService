using CatalogService.Models;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICatalogService _catalogService;

		public CategoriesController(ICatalogService catalogService)
		{
			_catalogService = catalogService;
		}

		/// <summary>
		/// Get categories.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET api/Categories/
		///
		/// </remarks>
		[HttpGet]
		public async Task<ActionResult> GetCategories()
		{
			var categories = await _catalogService.GetCategoriesAsync();

			if (categories == null)
			{
				return StatusCode(StatusCodes.Status204NoContent, "No categories in database");
			}

			return StatusCode(StatusCodes.Status200OK, categories);
		}

		/// <summary>
		/// Get category by ID.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET api/Categories/5
		///
		/// </remarks>
		[HttpGet("{id}")]
		public async Task<ActionResult<Category>> GetCategory(long id)
		{
			Category category = await _catalogService.GetCategoryAsync(id);

			if (category == null)
			{
				return StatusCode(StatusCodes.Status204NoContent, $"No Category found for id: {id}");
			}

			return StatusCode(StatusCodes.Status200OK, category);
		}

		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		/// <summary>
		/// Add category.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST api/Categories
		///     {
		///        "name": "Category #1"
		///     }
		///
		/// </remarks>
		[HttpPost]
		public async Task<ActionResult<Category>> AddCategory(Category category)
		{
			var dbcategory = await _catalogService.AddCategoryAsync(category);

			if (dbcategory == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"{category.Name} could not be added.");
			}

			return CreatedAtAction("GetCategory", new { id = category.Id }, category);
		}

		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		/// <summary>
		/// Update category.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     PUT api/Categories/1
		///     {
		///        "name": "Boots"
		///     }
		///
		/// </remarks>
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCategory(long id, Category category)
		{
			if (id != category.Id)
			{
				return BadRequest();
			}

			var dbcategory = await _catalogService.UpdateCategoryAsync(category);

			if (dbcategory == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"{category.Name} could not be updated");
			}

			return NoContent();
		}

		/// <summary>
		/// Delete category.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     DELETE api/Categories/1
		///
		/// </remarks>
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(long id)
		{
			var category = await _catalogService.GetCategoryAsync(id);
			if (category == null)
			{
				return StatusCode(StatusCodes.Status204NoContent, $"No Category found for id: {id}");
			}

			(bool status, string message) = await _catalogService.DeleteCategoryAsync(category);

			if (status == false)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, message);
			}

			return StatusCode(StatusCodes.Status200OK, category);
		}
	}
}
