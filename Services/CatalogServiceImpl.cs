using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services
{
	public class CatalogServiceImpl : ICatalogService
	{
		private readonly CatalogContext _db;

		public CatalogServiceImpl(CatalogContext db)
		{
			_db = db;
		}

		#region Categories

		public async Task<List<Category>> GetCategoriesAsync()
		{
			try
			{
				return await _db.Categories.ToListAsync();
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<Category> GetCategoryAsync(long id)
		{
			try
			{
				return await _db.Categories
					.Include(c => c.ProductItems)
					.FirstOrDefaultAsync(i => i.Id == id);
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<Category> AddCategoryAsync(Category category)
		{
			try
			{
				await _db.Categories.AddAsync(category);
				await _db.SaveChangesAsync();
				return await _db.Categories.FindAsync(category.Id); // Auto ID from DB
			}
			catch (Exception ex)
			{
				return null; // An error occured
			}
		}

		public async Task<Category> UpdateCategoryAsync(Category category)
		{
			try
			{
				_db.Entry(category).State = EntityState.Modified;
				await _db.SaveChangesAsync();

				return category;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<(bool, string)> DeleteCategoryAsync(Category category)
		{
			try
			{
				var dbCategory = await _db.Categories.FindAsync(category.Id);

				if (dbCategory == null)
				{
					return (false, "Category could not be found");
				}

				_db.Categories.Remove(dbCategory);
				await _db.SaveChangesAsync();

				return (true, "Category got deleted.");
			}
			catch (Exception ex)
			{
				return (false, $"An error occured. Error Message: {ex.Message}");
			}
		}

		#endregion

		#region ProductItems

		public async Task<List<ProductItem>> GetProductItemsAsync(int? page, int? page_size, long? categoryid)
		{
			try
			{
				int rPageSize = (int)(page_size == null ? 30 : page_size);
				return await _db.ProductItems
					.Where(x => (categoryid == null) || x.CategoryId == categoryid)
					.Skip((int)(page == null ? 0 : (page - 1) * rPageSize))
					.Take(rPageSize)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<ProductItem> GetProductItemAsync(long id)
		{
			try
			{
				return await _db.ProductItems.FindAsync(id);
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<ProductItem> AddProductItemAsync(ProductItem productItem)
		{
			try
			{
				await _db.ProductItems.AddAsync(productItem);
				await _db.SaveChangesAsync();
				return await _db.ProductItems.FindAsync(productItem.Id); // Auto ID from DB
			}
			catch (Exception ex)
			{
				return null; // An error occured
			}
		}

		public async Task<ProductItem> UpdateProductItemAsync(ProductItem productItem)
		{
			try
			{
				_db.Entry(productItem).State = EntityState.Modified;
				await _db.SaveChangesAsync();

				return productItem;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<(bool, string)> DeleteProductItemAsync(ProductItem productItem)
		{
			try
			{
				var dbproductItem = await _db.ProductItems.FindAsync(productItem.Id);

				if (dbproductItem == null)
				{
					return (false, "ProductItem could not be found.");
				}

				_db.ProductItems.Remove(dbproductItem);
				await _db.SaveChangesAsync();

				return (true, "ProductItem got deleted.");
			}
			catch (Exception ex)
			{
				return (false, $"An error occured. Error Message: {ex.Message}");
			}
		}

		#endregion
	}
}
