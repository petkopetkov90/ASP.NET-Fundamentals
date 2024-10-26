namespace DeskMarket.Common
{
    public static class Constants
    {
        public const int CategoryNameMaxLength = 50;

        public const int ProductNameMinLength = 2;
        public const int ProductNameMaxLength = 50;
        public const int ProductDescriptionMinLength = 10;
        public const int ProductDescriptionMaxLength = 250;
        public const string ProductPriceRangeStart = "1.00";
        public const string ProductPriceRangeEnd = "3000.00";
        public const string PriceColumnType = "decimal(18,2)";
        public const string RegexDateFormat = @"^\d{2}-\d{2}-\d{4}$";
        public const string DateFormatString = "dd-MM-yyyy";

    }
}
