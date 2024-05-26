using Blasterify.Models.Model;
using System;
using System.Collections.Generic;

namespace Blasterify.Models.Request
{
    public class PreRentRequest
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CardNumber { get; set; }
        public int ClientUserId { get; set; }
        public List<PreRentItemRequest> PreRentItems { get; set; } = new List<PreRentItemRequest>();

        public PreRentRequest() { }

        public PreRentRequest(PreRentModel preRent)
        {
            Id = preRent.Id;
            Date = preRent.Date;
            Name = preRent.Name;
            Address = preRent.Address;
            CardNumber = preRent.CardNumber;
            ClientUserId = preRent.ClientUserId;

            foreach (var item in preRent.PreRentItems.Values)
            {
                PreRentItems.Add(new PreRentItemRequest(item));
            }
        }
    }
}