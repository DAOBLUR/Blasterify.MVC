namespace Blasterify.Services.Models.Response
{
    public class RentDetail
    {
        public Guid Id { get; set; }

        public DateTime RentDate { get; set; }

        public int ClientUserId { get; set; }

        public List<RentMovie>? RentMovies { get; set; }
    }
}