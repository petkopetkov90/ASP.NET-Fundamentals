using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GameZone.Common.Constants.GenreConstant;
using static GameZone.Common.ErrorMessages.GenreErrorMessage;

namespace GameZone.Data.Models
{
    [Table("Genres")]
    public class Genre
    {
        [Key]
        [Comment("Primary Key")]
        public int Id { get; set; }

        [Required(ErrorMessage = NameErrorMsg)]
        [MaxLength(NameMaxLength, ErrorMessage = GenreMaxLengthErrorMsg)]
        [Comment("Genre name")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
