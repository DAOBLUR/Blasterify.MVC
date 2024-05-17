using Blasterify.Models.Model;

namespace Blasterify.Models.Request
{
    public class PreRentItemRequest
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int RentDuration { get; set; }
        public double Price { get; set; }

        public PreRentItemRequest() { }

        public PreRentItemRequest(PreRentItemModel preRentItemModel)
        {
            Id = preRentItemModel.Id;
            MovieId = preRentItemModel.MovieId;
            RentDuration = preRentItemModel.RentDuration;
            Price = preRentItemModel.Price;
        }
    }
}