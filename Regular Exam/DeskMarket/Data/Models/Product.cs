using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DeskMarket.Common.Constants;
using static DeskMarket.Common.ErrorMessages;

namespace DeskMarket.Data.Models
{
    public class Product
    {
        [Key]
        [Comment("Product Primary key")]
        public int Id { get; set; }

        [Required]
        [MaxLength(ProductNameMaxLength, ErrorMessage = ProductNameErrorMsg)]
        [Comment("Product Name")]
        public string ProductName { get; set; } = null!;

        [Required]
        [MaxLength(ProductDescriptionMaxLength, ErrorMessage = ProductDescriptionErrorMsg)]
        [Comment("Product Description")]
        public string Description { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), ProductPriceRangeStart, ProductPriceRangeEnd, ErrorMessage = ProductPriceRangeErrorMsg)]
        [Column(TypeName = PriceColumnType)]
        [Comment("Product Price")]
        public decimal Price { get; set; }

        [Comment("Product Image URL")]
        public string? ImageUrl { get; set; }

        [Required]
        [Comment("Product Seller Foreign Key")]
        public string SellerId { get; set; } = string.Empty;
        [ForeignKey(nameof(SellerId))]
        public IdentityUser Seller { get; set; } = null!;

        [Required]
        [Comment("Product Publishing Date")]
        public DateTime AddedOn { get; set; } = DateTime.Today;

        [Required]
        [Comment("Product Category Foreign Key")]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Comment("Soft Delete")]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<ProductClient> ProductsClients { get; set; } = new List<ProductClient>();

    }
}
