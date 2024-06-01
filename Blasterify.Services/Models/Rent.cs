using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blasterify.Services.Models
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
        public string? Name { get; set; }

        [Required]
        [MaxLength(60)]
        public string? Address { get; set; }

        [Required]
        [StringLength(16)]
        public string? CardNumber { get; set; }

        [Required]
        [MinLength(36)]
        [MaxLength(64)]
        public string? CheckoutSession { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        [Required]
        [ForeignKey("ClientUser")]
        public int ClientUserId { get; set; }

        [Required]
        [ForeignKey("RentStatus")]
        public int StatusId { get; set; }
    }
}