using Blasterify.Client.Models;
using System;
using System.Collections.Generic;

namespace Blasterify.Client.Models
{
    public class RentDetail
    {
        public Guid Id { get; set; }

        public DateTime RentDate { get; set; }

        public int ClientUserId { get; set; }

        public List<Blasterify.Models.Model.PreRentModel> RentMovies { get; set; }
    }
}