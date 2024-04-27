using System;

namespace Blasterify.Client
{
    public class RentItem
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public Guid RentId { get; set; }

        public int RentDuration { get; set; }
    }
}