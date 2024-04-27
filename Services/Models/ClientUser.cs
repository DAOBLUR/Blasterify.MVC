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
        public string Username { get; set; }

        [Required]
        [StringLength(16)]
        [MinLength(16)]
        [MaxLength(16)]
        public string CardNumber { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        [MaxLength(35)]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public DateTime SuscriptionDate { get; set; }

        [Required]
        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }

        /*
        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        */
    }
}