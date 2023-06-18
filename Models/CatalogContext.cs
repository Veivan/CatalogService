using Microsoft.EntityFrameworkCore;

namespace CatalogService.Models
{
	public class CatalogContext : DbContext
	{
		public CatalogContext(DbContextOptions<CatalogContext> options)
			: base(options)
		{
		//	Database.EnsureDeleted();
			Database.EnsureCreated();
		}

		public DbSet<Category> Categories { get; set; } 
		public DbSet<ProductItem> ProductItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			/* modelBuilder.Entity<ProductItem>()
				.HasOne(x => x.Category)
				.WithMany(x => x.ProductItems);
			//.HasForeignKey("CategoryId"); */

			modelBuilder.Entity<Category>()
				.HasMany(e => e.ProductItems)
				.WithOne()
				.HasForeignKey(e => e.CategoryId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			var categories = new List<Category>()
			{
				new Category { Id = 1, Name = "Adidas" },
				new Category { Id = 2, Name = "Nike" },
				new Category { Id = 3, Name = "Puma" }
			};

			var productItems = new List<ProductItem>
			{
				new ProductItem { Id = 1, CategoryId = 1, Title = "Classic 21", Colour = "White"},
				new ProductItem { Id = 2, CategoryId = 1, Title = "Retro 95", Colour = "White"},
				new ProductItem { Id = 3, CategoryId = 2, Title = "Air", Colour = "White"},
				new ProductItem { Id = 4, CategoryId = 2, Title = "FootB", Colour = "Black"},
				new ProductItem { Id = 5, CategoryId = 3, Title = "Run 101", Colour = "Black"},
				new ProductItem { Id = 6, CategoryId = 3, Title = "Pulman 7", Colour = "White"}
			};

			modelBuilder.Entity<Category>().HasData(categories);
			modelBuilder.Entity<ProductItem>().HasData(productItems);
		}
	}
}

