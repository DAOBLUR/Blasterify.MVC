using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Blasterify.Models.RentStatus;

namespace Services.Models
{
    public class Rent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(60)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(16)]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [ForeignKey("ClientUser")]
        public int ClientUserId { get; set; }

        [Required]
        [ForeignKey("RentStatus")]
        public int Status { get; set; }
    }
}