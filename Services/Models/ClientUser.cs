using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        public string CardNumber { get; set; } = string.Empty;

        public bool Status { get; set; }

        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public DateTime SuscriptionDate { get; set; } = new DateTime();

        public Subscription Subscription { get; set; } = new Subscription();

        public Country Country { get; set; } = new Country();
        
    }
}