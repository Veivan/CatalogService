using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models
{
	public class ProductItem
	{
		[Key] 
		public long Id { get; set; }

		public string? Title { get; set; }
		
		public string? Colour { get; set; }

		public long CategoryId { get; set; }

//		public Category? Category { get; set; }
	}
}