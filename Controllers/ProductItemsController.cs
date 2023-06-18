using CatalogService.Models;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductItemsController : ControllerBase
	{
		private readonly ICatalogService _catalogService;

		public ProductItemsController(ICatalogService catalogService)
		{
			_catalogService = catalogService;
		}

		/// <summary>
		/// Get ProductItems.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET api/ProductItems?page=2_page_size=3_categoryid=2
		///
		/// </remarks>
		[HttpGet]
		public async Task<ActionResult> GetProductItems(int? page, int? page_size, long? categoryid)
		{
			var productItems = await _catalogService.GetProductItemsAsync(page, page_size, categoryid);
			if (productItems == null)
			{
				return StatusCode(StatusCodes.Status204NoContent, "ProductItems not found.");
			}

			return StatusCode(StatusCodes.Status200OK, productItems);
		}

		// GET: api/ProductItems/5
		/// <summary>
		/// Get ProductItem by ID.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     GET api/ProductItems/5
		///
		/// </remarks>
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductItem>> GetProductItem(long id)
		{
			ProductItem productItem = await _catalogService.GetProductItemAsync(id);

			if (productItem == null)
			{
				return StatusCode(StatusCodes.Status204NoContent, $"No productItem found for id: {id}");
			}

			return StatusCode(StatusCodes.Status200OK, productItem);
		}

		/// <summary>
		/// Add a ProductItem.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST api/ProductItem
		///     {
		///        "CategoryId" = 2,
		///        "Title" = "new Title",
		///        "Colour" = "Black"
		///     }
		///
		/// </remarks>
		[HttpPost]
		public async Task<ActionResult<ProductItem>> AddProductItem(ProductItem productItem)
		{
			var dbProductItem = await _catalogService.AddProductItemAsync(productItem);

			if (dbProductItem == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"{productItem.Title} could not be added.");
			}

			return CreatedAtAction("GetProductItem", new { id = productItem.Id }, productItem);
		}

		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		/// <summary>
		/// Update ProductItem.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     PUT api/ProductItems/5
		///     {
		///        "CategoryId" = 2,
		///        "Title" = "new Title",
		///        "Colour" = "Black"
		///     }
		///
		/// </remarks>
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProductItem(long id, ProductItem productItem)
		{
			if (id != productItem.Id)
			{
				return BadRequest();
			}

			var dbProductItem = await _catalogService.UpdateProductItemAsync(productItem);

			if (dbProductItem == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"{productItem.Title} could not be updated.");
			}

			return NoContent();
		}

		// DELETE: api/ProductItems/5
		/// <summary>
		/// Delete ProductItem.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     DELETE api/ProductItems/5
		///
		/// </remarks>
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProductItem(long id)
		{
			var productItem = await _catalogService.GetProductItemAsync(id);
			(bool status, string message) = await _catalogService.DeleteProductItemAsync(productItem);

			if (status == false)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, message);
			}

			return StatusCode(StatusCodes.Status200OK, productItem);
		}
	}
}
