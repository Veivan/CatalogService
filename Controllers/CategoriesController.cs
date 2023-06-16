using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly CatalogContext _context;

		public CategoriesController(CatalogContext context)
		{
			_context = context;
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
		public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
		{
			if (_context.Categories == null)
			{
				return NotFound();
			}
			return await _context.Categories.ToListAsync();
		}

		// GET: api/Categories/5
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
			if (_context.Categories == null)
			{
				return NotFound();
			}
			//var category = await _context.Categories.FindAsync(id);

			var category = await _context.Categories
				.Include(c => c.ProductItems)
				.FirstOrDefaultAsync(i => i.Id == id);
			if (category == null)
			{
				return NotFound();
			}

			return category;
		}

		// PUT: api/Categories/5
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

			_context.Entry(category).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CategoryExists(id))
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

		// POST: api/Categories
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
		public async Task<ActionResult<Category>> PostCategory(Category category)
		{
			if (_context.Categories == null)
			{
				return Problem("Entity set 'CatalogContext.Categories'  is null.");
			}
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetCategory", new { id = category.Id }, category);
		}

		// DELETE: api/Categories/5
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
			if (_context.Categories == null)
			{
				return NotFound();
			}
			var category = await _context.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CategoryExists(long id)
		{
			return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
