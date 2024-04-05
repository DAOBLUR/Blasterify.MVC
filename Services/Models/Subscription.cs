using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        public double Price { get; set; }

        [Required]
        public string Features { get; set; } = string.Empty;
    }
}