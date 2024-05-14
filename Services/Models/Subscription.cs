using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blasterify.Services.Models
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        public double Price { get; set; }

        [Required]
        public string? Features { get; set; }
    }
}