using Blasterify.Models.Response;

namespace Blasterify.Models.Model
{
    public class PreRentItemModel
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int RentDuration { get; set; }
        public string Title { get; set; }
        public string FirebasePosterId { get; set; }
        public double Price { get; set; }

        public PreRentItemModel()
        {
        }

        public PreRentItemModel(PreRentItemResponse preRentItemResponse)
        {
            Id = preRentItemResponse.Id;
            MovieId = preRentItemResponse.MovieId;
            RentDuration = preRentItemResponse.RentDuration < 1 ? 1 : preRentItemResponse.RentDuration;
            Title = preRentItemResponse.Title;
            FirebasePosterId = preRentItemResponse.FirebasePosterId;
            Price = preRentItemResponse.Price;
        }

        public PreRentItemModel(MovieModel movieModel)
        {
            Id = 0;
            MovieId = movieModel.Id;
            RentDuration = 1;
            Title = movieModel.Title;
            FirebasePosterId = movieModel.FirebasePosterId;
            Price = movieModel.Price;
        }
    }
}