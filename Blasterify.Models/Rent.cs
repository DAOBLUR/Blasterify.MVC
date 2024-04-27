using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blasterify.Models
{
    public class Rent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime BuyDate { get; set; }

        [Required]
        [ForeignKey("ClientUser")]
        public int ClientUserId { get; set; }
    }
}