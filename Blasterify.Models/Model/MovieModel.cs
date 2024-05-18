using System;

namespace Blasterify.Models.Model
{
    public class MovieModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public double Duration { get; set; }

        public string Description { get; set; }

        public DateTime PremiereDate { get; set; }

        public int PremiereYear { get { return PremiereDate.Year; } }

        public double Rate { get; set; }

        public string FirebasePosterId { get; set; }

        public double Price { get; set; }

        public bool IsFree { get; set; }
    }
}