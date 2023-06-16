using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models
{
	public class Category
	{
		[Key] 
		public long Id { get; set; }
		public string? Name { get; set; }

		public List<ProductItem>? ProductItems { get; set; }
	}
}
