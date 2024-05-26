namespace Blasterify.Models.Model
{
    public class RentItemDetailModel
    {
        public int MovieId { get; set; }

        public int RentDuration { get; set; }

        public string Title { get; set; }

        public double Duration { get; set; }

        public string Description { get; set; }

        public string FirebasePosterId { get; set; }

        public double Price { get; set; }
    }
}
