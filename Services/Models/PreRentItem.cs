using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class PreRentItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        [Required]
        [ForeignKey("PreRent")]
        public Guid RentId { get; set; }

        [Required]
        public int RentDuration { get; set; }
    }
}