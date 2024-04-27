using System;
namespace Blasterify.Client.Models
{
    public class RentMovie
    {
        public int MovieId { get; set; }

        public int RentDuration { get; set; }

        public string Title { get; set; }

        public double Duration { get; set; }

        public string Description { get; set; }

        public string FirebasePosterId { get; set; }

        public double Price { get; set; }

        public RentMovie()
        {
        }

        public RentMovie(Movie movie)
        {
            MovieId = movie.Id;
            RentDuration = 1;
            Title = movie.Title;
            Duration = movie.Duration;
            Description = movie.Description;
            FirebasePosterId = movie.FirebasePosterId;
            Price = movie.Price;
        }
    }
}