using CatalogService.Models;

namespace CatalogService.Services
{
	public interface ICatalogService
	{
        // Category Services
        Task<List<Category>> GetCategoriesAsync(); // GET All Categories
        Task<Category> GetCategoryAsync(long id); // GET Single Category
        Task<Category> AddCategoryAsync(Category category); // POST New Category
        Task<Category> UpdateCategoryAsync(Category category); // PUT Category
        Task<(bool, string)> DeleteCategoryAsync(Category category); // DELETE Category

        // ProductItem Services
        Task<List<ProductItem>> GetProductItemsAsync(int? page, int? page_size, long? categoryid); // GET ProductItems filtered & paged
        Task<ProductItem> GetProductItemAsync(long id); // Get Single ProductItem
        Task<ProductItem> AddProductItemAsync(ProductItem productItem); // POST New ProductItem
        Task<ProductItem> UpdateProductItemAsync(ProductItem productItem); // PUT ProductItem
        Task<(bool, string)> DeleteProductItemAsync(ProductItem productItem); // DELETE ProductItem

    }
}
