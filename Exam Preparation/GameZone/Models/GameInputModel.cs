using GameZone.Data.Models;
using System.ComponentModel.DataAnnotations;
using static GameZone.Common.Constants.GameConstant;
using static GameZone.Common.ErrorMessages.GameErrorMessage;

namespace GameZone.Models
{
    public class GameInputModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = TitleErrorMsg)]
        [MinLength(TitleMinLength, ErrorMessage = TitleMinLengthErrorMsg)]
        [MaxLength(TitleMaxLength, ErrorMessage = TitleMaxLengthErrorMsg)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = DescriptionErrorMsg)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthErrorMsg)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthErrorMsg)]
        public string Description { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        public string? PublisherId { get; set; }

        [RegularExpression(DateFormat, ErrorMessage = DateFormatErrorMsg)]
        public string ReleasedOn { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        [Required(ErrorMessage = GenreErrorMsg)]
        public int GenreId { get; set; }

        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    }
}
