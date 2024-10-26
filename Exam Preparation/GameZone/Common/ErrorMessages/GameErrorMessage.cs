namespace GameZone.Common.ErrorMessages
{
    public static class GameErrorMessage
    {
        public const string TitleErrorMsg = "Title is required";
        public const string TitleMinLengthErrorMsg = "Title must be between 2 and 50 characters";
        public const string TitleMaxLengthErrorMsg = "Title must be between 2 and 50 characters";
        public const string DescriptionErrorMsg = "Description is required";
        public const string DescriptionMinLengthErrorMsg = "Description must be between 10 and 500 characters";
        public const string DescriptionMaxLengthErrorMsg = "Description must be between 10 and 500 characters";
        public const string PublisherErrorMsg = "Publisher Id is required";
        public const string GenreErrorMsg = "Game genre is required";
        public const string DateFormatErrorMsg = "Date must be in the format yyyy-MM-dd.";
    }
}
