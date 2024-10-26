using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GameZone.Common.Constants.GameConstant;
using static GameZone.Common.ErrorMessages.GameErrorMessage;

namespace GameZone.Data.Models
{
    [Table("Games")]
    public class Game
    {
        [Key]
        [Comment("Primary Key")]
        public int Id { get; set; }

        [Required(ErrorMessage = TitleErrorMsg)]
        [MaxLength(TitleMaxLength, ErrorMessage = TitleMaxLengthErrorMsg)]
        [Comment("Game Title")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = DescriptionErrorMsg)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthErrorMsg)]
        [Comment("Game Description")]
        public string Description { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength)]
        [Comment("Game Image URL")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = PublisherErrorMsg)]
        [Comment("Game Publisher")]
        public string PublisherId { get; set; } = null!;
        [ForeignKey(nameof(PublisherId))]
        public IdentityUser Publisher { get; set; } = null!;

        [Comment("Game Releasing Date")]
        public DateTime ReleasedOn { get; set; }

        [Required(ErrorMessage = GenreErrorMsg)]
        [Comment("Game Genre")]
        public int GenreId { get; set; }
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        public virtual ICollection<GamerGame> GamersGames { get; set; } = new List<GamerGame>();
    }
}
