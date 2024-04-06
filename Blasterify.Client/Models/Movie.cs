using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blasterify.Client.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public double Duration { get; set; }

        public string Description { get; set; }

        public DateTime PremiereDate { get; set; }

        public double Rate { get; set; }

        public string FirebasePosterId { get; set; }

        public double Price { get; set; }

        public bool IsFree { get; set; }
    }
}