using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static DeskMarket.Common.Constants;

namespace DeskMarket.Data.Models
{
    public class Category
    {
        [Key]
        [Comment("Category Primary Key")]
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        [Comment("Category Name")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
