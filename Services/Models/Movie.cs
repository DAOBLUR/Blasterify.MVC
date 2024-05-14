using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string? Title { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public double Duration { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime PremiereDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(3, 2)")]
        public double Rate { get; set; }

        [Required]
        public string? FirebasePosterId { get; set; }

        [Required]
        [Column(TypeName = "decimal(4, 2)")]
        public double Price { get; set; }

        [Required]
        public bool IsFree { get; set; }

        [Required]
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
    }
}