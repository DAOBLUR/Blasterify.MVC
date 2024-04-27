using System;

namespace Blasterify.Client.Models
{
    public class PreRentItem
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public Guid RentId { get; set; }

        public int RentDuration { get; set; }
    }
}