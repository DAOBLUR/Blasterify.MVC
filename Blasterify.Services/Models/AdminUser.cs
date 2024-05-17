using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blasterify.Services.Models
{
    public class AdminUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string? Name { get; set; }

        [Required]
        public bool IsConnected { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(35)]
        public string? Email { get; set; }

        [Required]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public DateTime LastConnectionDate { get; set; }

        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
    }
}