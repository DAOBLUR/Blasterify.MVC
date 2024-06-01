using System;

namespace Blasterify.Models.Model
{
    public class CompleteRentRequest
    {
        public string Token { get; set; }
        public Guid RentId { get; set; }
    }
}