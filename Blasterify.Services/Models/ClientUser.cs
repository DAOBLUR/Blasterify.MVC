using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blasterify.Services.Models
{
    public class ClientUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1001, int.MaxValue, ErrorMessage = "Error in Id Client User.")]
        public int Id { get; set; }

        [MinLength(36)]
        [MaxLength(64)]
        public string? YunoId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(16)]
        public string? CardNumber { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string? MerchantOrderId { get; set; }

        [Required]
        public bool IsConnected { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(35)]
        public string? Email { get; set; }

        [Required]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public DateTime SubscriptionDate { get; set; }

        [Required]
        public DateTime LastConnectionDate { get; set; }

        [Required]
        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }

        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
    }
}