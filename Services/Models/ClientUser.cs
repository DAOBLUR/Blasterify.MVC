using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Models
{
    public class ClientUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(16)]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        public bool Status { get; set; }

        [Required]
        [MaxLength(35)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        [Required]
        public DateTime SuscriptionDate { get; set; }

        public DateTime LastConnectionDate { get; set; }

        [Required]
        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }
        
        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
    }
}