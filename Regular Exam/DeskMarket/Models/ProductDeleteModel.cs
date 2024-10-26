﻿namespace DeskMarket.Models
{
    public class ProductDeleteModel
    {
        public int Id { get; set; }

        public required string ProductName { get; set; }

        public required string SellerId { get; set; }

        public required string Seller { get; set; }
    }
}