namespace DeskMarket.Models
{
    public class ProductCartViewModel
    {
        public int Id { get; set; }

        public required string ProductName { get; set; }

        public required decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
