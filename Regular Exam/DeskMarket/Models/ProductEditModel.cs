using DeskMarket.Data.Models;
using System.ComponentModel.DataAnnotations;
using static DeskMarket.Common.Constants;
using static DeskMarket.Common.ErrorMessages;

namespace DeskMarket.Models
{
    public class ProductEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ProductNameRequiredErrorMsg)]
        [StringLength(ProductNameMaxLength, ErrorMessage = ProductNameErrorMsg, MinimumLength = ProductNameMinLength)]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = ProductDescriptionRequiredErrorMsg)]
        [StringLength(ProductDescriptionMaxLength, ErrorMessage = ProductDescriptionErrorMsg, MinimumLength = ProductDescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = ProductPriceRequiredErrorMsg)]
        [Range(typeof(decimal), ProductPriceRangeStart, ProductPriceRangeEnd, ErrorMessage = ProductPriceRangeErrorMsg)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = ProductDateRequiredErrorMsg)]
        [RegularExpression(RegexDateFormat, ErrorMessage = DateFormatErrorMsg)]
        public string AddedOn { get; set; } = DateTime.Now.ToString(DateFormatString);

        [Required]
        public string SellerId { get; set; } = null!;

        [Required(ErrorMessage = ProductCategoryRequiredErrorMsg)]
        [Range(1, int.MaxValue, ErrorMessage = ProductPriceRangeErrorMsg)]
        public int CategoryId { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
