using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CatalogService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductItemsController : ControllerBase
	{
		private readonly CatalogContext _context;

		public ProductItemsController(CatalogContext context)
		{
			_context = context;
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
		public async Task<ActionResult<IEnumerable<ProductItem>>> GetProductItems(int? page, int? page_size, long? categoryid)
		{
			if (_context.ProductItems == null)
			{
				return NotFound();
			}
			int rPageSize = (int)(page_size == null ? 30 : page_size);
			return await _context.ProductItems
				.Where(x => (categoryid == null) || x.CategoryId == categoryid)
				.Skip((int)(page == null? 0 : (page - 1) * rPageSize))
				.Take(rPageSize)
				.ToListAsync();
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
			if (_context.ProductItems == null)
			{
				return NotFound();
			}
			var productItem = await _context.ProductItems.FindAsync(id);

			if (productItem == null)
			{
				return NotFound();
			}

			return productItem;
		}

		// PUT: api/ProductItems/5
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
		public async Task<IActionResult> PutProductItem(long id, ProductItem productItem)
		{
			if (id != productItem.Id)
			{
				return BadRequest();
			}

			_context.Entry(productItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductItemExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
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
		public async Task<ActionResult<ProductItem>> PostProductItem(ProductItem productItem)
		{
			if (_context.ProductItems == null)
			{
				return Problem("Entity set 'CatalogContext.ProductItems'  is null.");
			}
			_context.ProductItems.Add(productItem);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetProductItem", new { id = productItem.Id }, productItem);
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
			if (_context.ProductItems == null)
			{
				return NotFound();
			}
			var productItem = await _context.ProductItems.FindAsync(id);
			if (productItem == null)
			{
				return NotFound();
			}

			_context.ProductItems.Remove(productItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ProductItemExists(long id)
		{
			return (_context.ProductItems?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
