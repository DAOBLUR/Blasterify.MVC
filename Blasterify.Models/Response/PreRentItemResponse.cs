namespace Blasterify.Models.Response
{
    public class PreRentItemResponse
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int RentDuration { get; set; }
        public string? Title { get; set; }
        public string? FirebasePosterId { get; set; }
        public double Price { get; set; }
    }
}