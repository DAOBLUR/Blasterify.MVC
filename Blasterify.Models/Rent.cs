using System;

namespace Blasterify.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public DateTime BuyDate { get; set; }
        public int ClientUserId { get; set; }
    }
}