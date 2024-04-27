using System;

namespace Blasterify.Client.Models
{
    public class PreRent
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public int ClientUserId { get; set; }
    }
}