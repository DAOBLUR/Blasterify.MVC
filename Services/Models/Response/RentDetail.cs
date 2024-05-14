using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Blasterify.Client.Models;

namespace Services.Models.Response
{
    public class RentDetail
    {
        public Guid Id { get; set; }

        public DateTime RentDate { get; set; }

        public int ClientUserId { get; set; }

        public List<RentMovie>? RentMovies { get; set; }
    }
}