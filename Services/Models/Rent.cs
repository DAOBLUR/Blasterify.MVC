using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class Rent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime RentDate { get; set; }

        [Required]
        [ForeignKey("ClientUser")]
        public int ClientUserId { get; set; }


        [Required]
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
    }
}
