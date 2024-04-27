using System;

namespace Blasterify.Client.Models
{
    public class Rent
    {
        public Guid Id { get; set; }

        public DateTime RentDate { get; set; }

        public int ClientUserId { get; set; }
    }
}